using UnityEngine;
using System.Collections;

public interface Actions {
	void Process ();
}

public class MovementObj : Actions {
	public Unit unit;
	public Vector2 destination;
	public GridManager gridManager;

	public void Process() {
		unit.animator.SetBool ("Run", true);
		if (Mathf.Abs(unit.transform.position.x - destination.x) < unit.transform.localScale.z * 0.1f) {
			if (Mathf.Abs(unit.transform.position.z - destination.y) < unit.transform.localScale.z * 0.1f) {
				unit.animator.SetBool ("Run", false);
				unit.actionQueue.RemoveAt (0);
				unit.active = false;
				gridManager.RemovePath ();
			} else {
				if (unit.transform.position.z -destination.y > unit.transform.localScale.z * 0.1f){
					UpdateUnitPosition (unit, "z", -1, 180);
				} else {
					UpdateUnitPosition (unit, "z", 1, 0);
				}
			}
		} else {
			if(unit.transform.position.x - destination.x > unit.transform.localScale.z * 0.1f) {
				UpdateUnitPosition (unit, "x", -1, 270);
			} else {
				UpdateUnitPosition (unit, "x", 1, 90);
			}
		}
	}

	void UpdateUnitPosition(Unit unit, string coord, int posneg, int direction) {
		Vector3 movement = Vector3.up;
		if (coord == "x") {
			movement = new Vector3 (
				unit.transform.position.x + unit.transform.localScale.z * 0.1f * posneg,
				unit.transform.position.y,
				unit.transform.position.z
			);
		} else {
			movement = new Vector3 (
				unit.transform.position.x,
				unit.transform.position.y,
				unit.transform.position.z + unit.transform.localScale.z * 0.1f * posneg
			);
		}
		unit.transform.position = movement;
		unit.transform.rotation = Quaternion.Euler (0, direction, 0);
	}
}

public class CombatObj : Actions {
	public UnitManager unitManager;

	public Unit aggressor;
	public Unit defender;

	public bool aggressorAggressing = true;
	public bool defenderDefending = false;
	public bool defenderDying = false;

	private Animator anim;
	private int attackHash = Animator.StringToHash("Melee Right Attack 03");
	private int damageHash = Animator.StringToHash("Take Damage");
	private int deathHash = Animator.StringToHash("Die");

	public void Process() {
		aggressor.animator.SetBool ("Melee Right Attack 03", true);
		if (defender == null) {
			aggressor.actionQueue.RemoveAt(0);
			return;
		}
		if (aggressor.animator.GetCurrentAnimatorStateInfo(0).shortNameHash == attackHash) {
			aggressorAggressing = false;
			defender.animator.SetBool("Take Damage", true);
			defenderDefending = true;
		}
		if(!defender.animator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(damageHash) && defenderDefending) {
			defenderDefending = false;
			defender.animator.SetBool ("Die", true);
			defenderDying = true;
		}
		if(!defender.animator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(deathHash) && defenderDying) {
			defenderDying = false;
			aggressor.actionQueue.RemoveAt(0);
			unitManager.RemoveUnit (defender);
		}
	}
}

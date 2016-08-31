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

	public bool firstTime = true;
	public bool defenderDying = false;

	public void Process() {

		if (firstTime) {
			aggressor.animator.SetBool ("Melee Right Attack 03", true);
			defender.animator.SetBool ("Take Damage", true);
			firstTime = false;
		}

		if (defender.currentHP != 0) {
			aggressor.actionQueue.RemoveAt(0);
		}

		if (
			defenderDying &&
			defender.animator.GetNextAnimatorStateInfo (0).IsName ("Idle") && 
			defender.animator.IsInTransition (0)
		) {
			unitManager.RemoveUnit (defender);
			aggressor.actionQueue.RemoveAt(0);
		}

		if (
			defender.currentHP == 0 && 
			defender.animator.GetNextAnimatorStateInfo (0).IsName ("Idle") &&
			defender.animator.IsInTransition (0)
		) {
			defender.animator.CrossFade ("Die", 1.0f);
			defenderDying = true;
		}
			
	}
}

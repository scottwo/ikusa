using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class UnitManager : MonoBehaviour {

	public Unit[] unitPrefabs;
	public GridManager gridManager;
	public PlayerManager playerManager;
	public TurnManager turnManager;
	public Unit selectedUnit;
	public Unit touchedUnit;
	public enum UnitType
	{
		melee, heavy_melee, ranged, heavy_ranged, mage, heavy_mage, buff_mage, heal_mage
	};

	public List<Unit> actionQueue = new List<Unit>();
	public List<Unit> units = new List<Unit>();

	void Start () {
		
	}
	
	void Update () {
		ProcessQueue ();
	}

	void ProcessQueue() {
		if(actionQueue.Count > 0) {
			if (actionQueue [0].actionQueue.Count > 0) {
				actionQueue [0].actionQueue [0].Process ();
			} else {
				actionQueue.Remove (actionQueue [0]);
			}
		}
	}

	public void createUnit(Node node, float scale, UnitType type, Player player) {
		Unit unit = null;
		switch (type) {
		case UnitType.melee:
			unit = Instantiate (unitPrefabs [0]);
			break;
		case UnitType.heavy_melee:
			unit = Instantiate (unitPrefabs [1]);
			break;
		case UnitType.ranged:
			unit = Instantiate (unitPrefabs [2]);
			break;
		case UnitType.heavy_ranged:
			unit = Instantiate (unitPrefabs [3]);
			break;
		case UnitType.mage:
			unit = Instantiate (unitPrefabs [4]);
			break;
		case UnitType.heavy_mage:
			unit = Instantiate (unitPrefabs [5]);
			break;
		case UnitType.buff_mage:
			unit = Instantiate (unitPrefabs [6]);
			break;
		case UnitType.heal_mage:
			unit = Instantiate (unitPrefabs [7]);
			break;
		}
		unit.transform.parent = this.transform;
		unit.transform.localScale = new Vector3 (
			unit.transform.localScale.x * scale,
			unit.transform.localScale.y * scale,
			unit.transform.localScale.z * scale
		);
		unit.transform.position = new Vector3(
			node.transform.position.x,
			node.transform.position.y + node.transform.localScale.y / 2,
			node.transform.position.z
		);
		unit.currentNode = node;
		node.currentUnit = unit;
		unit.player = player;
		unit.player.units.Add (unit);
		unit.playerManager = playerManager;
		unit.unitManager = this;
		unit.turnManager = turnManager;
		units.Add (unit);
	}

	public void MoveUnit(Unit unit, Node node) {
		if(node.currentUnit != null){
			if(node.currentUnit.player != unit.player) {
				if (gridManager.path != null && gridManager.path.Length > 1) {
					this.MoveUnit (unit, gridManager.path [gridManager.path.Length - 2]);
				}
				this.UnitCombat (unit, node.currentUnit);
			}
		}
		float destinationX = node.transform.position.x;
		float destinationZ = node.transform.position.z;
		MovementObj movementVector = new MovementObj ();
		movementVector.unit = unit;
		movementVector.destination = new Vector2 (destinationX, destinationZ);
		movementVector.gridManager = gridManager;
		unit.actionQueue.Add (movementVector);
		actionQueue.Add (unit);
		unit.currentNode = node;
		node.currentUnit = unit;
		DeselectUnit ();
    }

	public void MoveUnitByXZ(Unit unit, Vector3 position) {
		Node node = gridManager.findNodeByCordFloat (position);
		MoveUnit (unit, node);
		DeselectUnit ();
	}

	public void SelectUnit(Unit unit, GameObject controller) {
		selectedUnit = unit;
		unit.isSelected = true;
		unit.ShowSelectedIndicator ();
	}

	public void DeselectUnit() {
		if (selectedUnit != null) {
			selectedUnit.isSelected = false;
			selectedUnit.HideIndicator ();
			selectedUnit = null;
			gridManager.RemovePath ();
		}
	}

	public void RemoveUnit(Unit unit) {
		units.Remove (unit);
		if(selectedUnit == unit) {
			selectedUnit = null;
		}
		if(touchedUnit == unit) {
			touchedUnit = null;
		}
		if (actionQueue.Contains (unit)) {
			actionQueue.Remove (unit);
		}
		unit.currentNode.currentUnit = null;
		unit.player.units.Remove (unit);
		Destroy (unit.gameObject);
	}

	public void UnitCombat(Unit aggressor, Unit defender) {
		CombatObj combatObj = new CombatObj ();
		combatObj.aggressor = aggressor;
		combatObj.defender = defender;
		combatObj.unitManager = this;
		aggressor.actionQueue.Add (combatObj);

		if (!actionQueue.Contains (aggressor)) {
			actionQueue.Add (aggressor);
		} 

		aggressor.active = false;
		DeselectUnit ();
	}
}

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
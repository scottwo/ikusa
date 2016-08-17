using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class UnitManager : MonoBehaviour {

	public Unit[] unitPrefabs;
	public GridManager gridManager;
	public PlayerManager playerManager;
	public Unit selectedUnit;
	public Unit touchedUnit;
	public enum UnitType
	{
		soldier, assassin, wizard, brute
	};

	private List<Unit> actionQueue = new List<Unit>();
	private List<Unit> units = new List<Unit>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ProcessQueue ();
	}

	void ProcessQueue() {
		if(actionQueue.Count > 0) {
			for (int i = 0; i < actionQueue.Count; i++) {
				if (actionQueue [i].actionQueue.Count > 0) {
					actionQueue [i].actionQueue [0].Process ();
				} else {
					actionQueue.Remove (actionQueue [i]);
				}
			}
		}
	}

	public void createUnit(Node node, float scale, UnitType type, Player player) {
		Unit unit = null;
		switch (type) {
		case UnitType.soldier:
			unit = Instantiate (unitPrefabs [0]);
			break;
		case UnitType.wizard:
			unit = Instantiate (unitPrefabs [1]);
			break;
		case UnitType.assassin:
			unit = Instantiate (unitPrefabs [2]);
			break;
		case UnitType.brute:
			unit = Instantiate (unitPrefabs [3]);
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
		unit.playerManager = playerManager;
		unit.unitManager = this;
		units.Add (unit);
	}

	public void MoveUnit(Unit unit, Node node) {
		if(node.currentUnit != null){
			if(node.currentUnit.player != unit.player) {
				if(gridManager.path.Length > 1) {
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
		unit.animator.SetBool ("Run", true);
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

	private void UnitCombat(Unit aggressor, Unit defender) {
		CombatObj combatObj = new CombatObj ();
		combatObj.aggressor = aggressor;
		combatObj.defender = defender;
		combatObj.aggressorAggressing = true;
		combatObj.defenderDefending = false;
		combatObj.unitManager = this;
		aggressor.actionQueue.Add (combatObj);

		if (!actionQueue.Contains (aggressor)) {
			actionQueue.Add (aggressor);
		} 
		aggressor.animator.SetBool ("Melee Right Attack 03", true);
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
		if (Mathf.Abs(unit.transform.position.x - destination.x) < unit.transform.localScale.z * 0.1f) {
			if (Mathf.Abs(unit.transform.position.z - destination.y) < unit.transform.localScale.z * 0.1f) {
				unit.animator.SetBool ("Run", false);
				unit.actionQueue.Remove (this);
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

	public bool aggressorAggressing;
	public bool defenderDefending;
	public bool defenderDying;

	public void Process() {
		if (!aggressor.animator.GetBool ("Melee Right Attack 03") && aggressorAggressing) {
			aggressorAggressing = false;
			defender.animator.SetBool ("Take Damage", true);
			defenderDefending = true;
		}
		if(!defender.animator.GetBool ("Take Damage") && defenderDefending) {
			defenderDefending = false;
			defender.animator.SetBool ("Die", true);
			defenderDying = true;
		}
		if(!defender.animator.GetBool ("Die") && defenderDying) {
			defenderDying = false;
			aggressor.actionQueue.Remove (this);
			unitManager.RemoveUnit (defender);
		}
	}
}
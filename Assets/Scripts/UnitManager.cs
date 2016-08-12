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

	private List<Unit> units = new List<Unit>();
	private List<MovementObj> movingUnits = new List<MovementObj>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ProcessMovingUnits ();
	}

	void ProcessMovingUnits() {
		if (movingUnits.Count > 0) {
			for (int i = 0; i < movingUnits.Count; i++) {
				if (Mathf.Abs(movingUnits [i].unit.transform.position.x - movingUnits [i].destination.x) < movingUnits [i].unit.transform.localScale.z * 0.1f) {
					if (Mathf.Abs(movingUnits [i].unit.transform.position.z - movingUnits [i].destination.y) < movingUnits [i].unit.transform.localScale.z * 0.1f) {
						movingUnits [i].unit.animator.SetBool ("Run", false);
						movingUnits.Remove (movingUnits [i]);
						gridManager.RemovePath ();
					} else {
						if (movingUnits [i].unit.transform.position.z - movingUnits [i].destination.y > movingUnits [i].unit.transform.localScale.z * 0.1f){
							UpdateUnitMove (movingUnits[i].unit, "z", -1, 180);
						} else {
							UpdateUnitMove (movingUnits[i].unit, "z", 1, 0);
						}
					}
				} else {
					if(movingUnits [i].unit.transform.position.x - movingUnits [i].destination.x > movingUnits [i].unit.transform.localScale.z * 0.1f) {
						UpdateUnitMove (movingUnits[i].unit, "x", -1, 270);
					} else {
						UpdateUnitMove (movingUnits[i].unit, "x", 1, 90);
					}
				}
			}
		}
	}

	void UpdateUnitMove(Unit unit, string coord, int posneg, int direction) {
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
		movingUnits.Add (movementVector);
		unit.currentNode = node;
		unit.animator.SetBool ("Run", true);
		DeselectUnit ();
		node.currentUnit = unit;
    }

	public void MoveUnitByXZ(Unit unit, Vector3 position) {
		Node node = gridManager.findNodeByCordFloat (position);
		MoveUnit (unit, node);
		DeselectUnit ();
	}

	public void TouchUnit(Unit unit) {
		unit.isBeingTouched = true;
		unit.ShowTouchedIndicator ();
		touchedUnit = unit;
	}

	public void UntouchUnit(){
		if (touchedUnit != selectedUnit) {
			touchedUnit.HideIndicator ();
		}
		touchedUnit.isBeingTouched = false;
		touchedUnit = null;
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

	private void UnitCombat(Unit aggressor, Unit defender) {
		aggressor.animator.SetBool ("Melee Right Attack 03", true);
//		defender.animator.SetBool ("Take Damage", true);
		defender.currentNode.currentUnit = null;
		Destroy (defender);
		DeselectUnit ();
	}
}

public class MovementObj {
	public Unit unit;
	public Vector2 destination;
}
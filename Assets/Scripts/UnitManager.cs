using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class UnitManager : MonoBehaviour {

	public Unit[] unitPrefabs;
	public GridManager gridManager;
	public Unit selectedUnit;
	public Unit touchedUnit;

	private List<Unit> units = new List<Unit>();
	private List<MovementObj> movingUnits = new List<MovementObj>();
	private GameObject selectedController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ProcessMovingUnits ();

		if (selectedController != null) {
			Node node = gridManager.findNodeByCordFloat (selectedController.transform.position);
			if (!node.hoverOver) {
				node.hoverOver = true;
			}
		}
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

	public void createUnit(Node node, float scale) {
		Unit unit = Instantiate (unitPrefabs [0]);
		unit.transform.parent = gameObject.transform;
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
		units.Add (unit);
	}

	public void MoveUnit(Unit unit, Node node) {
		float destinationX = node.transform.position.x;
		float destinationZ = node.transform.position.z;
		MovementObj movementVector = new MovementObj ();
		movementVector.unit = unit;
		movementVector.destination = new Vector2 (destinationX, destinationZ);
		movingUnits.Add (movementVector);
		unit.currentNode = node;
		unit.animator.SetBool ("Run", true);
		DeselectUnit ();
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
		if (selectedUnit != touchedUnit) {
			touchedUnit.HideIndicator ();
		}
		touchedUnit.isBeingTouched = false;
		touchedUnit = null;
	}

	public void SelectUnit(Unit unit, GameObject controller) {
		selectedUnit = unit;
		unit.isSelected = true;
		selectedController = controller;
		unit.ShowSelectedIndicator ();
	}

	public void DeselectUnit() {
		if (selectedUnit != null) {
			selectedUnit.isSelected = false;
			selectedUnit.HideIndicator ();
			selectedUnit = null;
			selectedController = null;
			gridManager.RemovePath ();
		}
	}
}

public class MovementObj {
	public Unit unit;
	public Vector2 destination;
}
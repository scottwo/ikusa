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
		if (movingUnits.Count > 0) {
			for (int i = 0; i < movingUnits.Count; i++) {
				if (Mathf.Abs(movingUnits [i].unit.transform.position.x - movingUnits [i].destination.x) < movingUnits [i].unit.transform.localScale.z * 4) {
					if (Mathf.Abs(movingUnits [i].unit.transform.position.z - movingUnits [i].destination.y) < movingUnits [i].unit.transform.localScale.z * 4) {
						movingUnits.Remove (movingUnits [i]);
					} else {
						if (movingUnits [i].unit.transform.position.z > movingUnits [i].destination.y) {
							movingUnits [i].unit.transform.position = new Vector3(
								movingUnits [i].unit.transform.position.x,
								movingUnits [i].unit.transform.position.y,
								movingUnits [i].unit.transform.position.z - movingUnits [i].unit.transform.localScale.z * 4
							);
							movingUnits [i].unit.transform.rotation = Quaternion.Euler (0, 180, 0);
						} else {
							movingUnits [i].unit.transform.position = new Vector3(
								movingUnits [i].unit.transform.position.x,
								movingUnits [i].unit.transform.position.y,
								movingUnits [i].unit.transform.position.z + movingUnits [i].unit.transform.localScale.z * 4
							);
							movingUnits [i].unit.transform.rotation = Quaternion.Euler (0, 0, 0);
						}
					}
				} else {
					if(Mathf.Abs(movingUnits [i].unit.transform.position.x - movingUnits [i].destination.x) < movingUnits [i].unit.transform.localScale.z * 4) {
						movingUnits [i].unit.transform.position = new Vector3(
							movingUnits [i].unit.transform.position.x - movingUnits [i].unit.transform.localScale.z * 4,
							movingUnits [i].unit.transform.position.y,
							movingUnits [i].unit.transform.position.z
						);
						movingUnits [i].unit.transform.rotation = Quaternion.Euler (0, 270, 0);
					} else {
						movingUnits [i].unit.transform.position = new Vector3(
							movingUnits [i].unit.transform.position.x + movingUnits [i].unit.transform.localScale.z * 4,
							movingUnits [i].unit.transform.position.y,
							movingUnits [i].unit.transform.position.z
						);
						movingUnits [i].unit.transform.rotation = Quaternion.Euler (0, 90, 0);
					}
				}
			}
		}
		if (selectedController != null) {
//			Debug.Log ("x: " + selectedController.transform.position.x + ", z: " + selectedController.transform.position.z);
			Node node = gridManager.findNodeByCordFloat (selectedController.transform.position);
			node.hoverOver = true;
		}
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
		units.Add (unit);
	}

	public void MoveUnit(Unit unit, Node node) {
		float destinationX = node.transform.position.x;
		float destinationZ = node.transform.position.z;
		MovementObj movementVector = new MovementObj ();
		movementVector.unit = unit;
		movementVector.destination = new Vector2 (destinationX, destinationZ);
		movingUnits.Add (movementVector);
    }

	public void MoveUnitByXZ(Unit unit, Vector3 position) {
		Node node = gridManager.findNodeByCordFloat (position);
		MoveUnit (unit, node);
		DeselectUnit ();
	}

	public void SelectUnit(Unit unit, GameObject controller) {
		selectedUnit = unit;
		unit.isSelected = true;
		selectedController = controller;
	}

	public void DeselectUnit() {
		if (selectedUnit != null) {
			selectedUnit.isSelected = false;
			selectedUnit = null;
			selectedController = null;
		}
	}
}

public class MovementObj {
	public Unit unit;
	public Vector2 destination;
}
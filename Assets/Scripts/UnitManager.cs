using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {

	public Unit[] unitPrefabs;
	public GridManager gridManager;

	private List<Unit> units = new List<Unit>();
	private List<MovementObj> movingUnits = new List<MovementObj>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (movingUnits.Count > 0) {
			for (int i = 0; i < movingUnits.Count; i++) {
				if (movingUnits [i].unit.transform.position.x == movingUnits [i].destination.x) {
					if (movingUnits [i].unit.transform.position.z == movingUnits [i].destination.y) {
						movingUnits.Remove (movingUnits [i]);
					} else {
						if (movingUnits [i].unit.transform.position.x > movingUnits [i].destination.x) {
							movingUnits [i].unit.transform.position = new Vector3(
								movingUnits [i].unit.transform.position.x,
								movingUnits [i].unit.transform.position.y,
								movingUnits [i].unit.transform.position.z + 0.01f
							);
						} else {
							movingUnits [i].unit.transform.position = new Vector3(
								movingUnits [i].unit.transform.position.x,
								movingUnits [i].unit.transform.position.y,
								movingUnits [i].unit.transform.position.z + 0.01f
							);
						}
					}
				} else {
					if(movingUnits [i].unit.transform.position.x > movingUnits [i].destination.x) {
						movingUnits [i].unit.transform.position = new Vector3(
							movingUnits [i].unit.transform.position.x,
							movingUnits [i].unit.transform.position.y,
							movingUnits [i].unit.transform.position.z + 0.01f
						);
					} else {
						movingUnits [i].unit.transform.position = new Vector3(
							movingUnits [i].unit.transform.position.x,
							movingUnits [i].unit.transform.position.y,
							movingUnits [i].unit.transform.position.z + 0.01f
						);
					}
				}
			}
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
		MoveUnit(unit, gridManager.findNodeByCord(5, 5));
	}

	public void MoveUnit(Unit unit, Node node) {
		float destinationX = node.transform.position.x;
		float destinationZ = node.transform.position.z;
		MovementObj movementVector = new MovementObj ();
		movementVector.unit = unit;
		movementVector.destination = new Vector2 (destinationX, destinationZ);
		movingUnits.Add (movementVector);
    }
}

public class MovementObj {
	public Unit unit;
	public Vector2 destination;
}
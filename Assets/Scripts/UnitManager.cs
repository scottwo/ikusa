using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {

	public Unit[] unitPrefabs;

	private List<Unit> units = new List<Unit>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (units[0]);
		if (units[0]) {
			units[0].transform.position = new Vector3(
				units[0].transform.position.x,
				units[0].transform.position.y,
				units[0].transform.position.z + units[0].transform.localScale.z * 0.9f
			);
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
}

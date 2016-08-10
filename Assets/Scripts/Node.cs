using UnityEngine;
using System;
using System.Collections;

public class Node : MonoBehaviour {

	public Vector2 coords;
	public GridManager gridManager;
	public bool hoverOver;

	private GameObject sphere;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ShowIndicator() {
		sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		sphere.transform.position = new Vector3 (
			gameObject.transform.position.x,
			gameObject.transform.position.y + gameObject.transform.localScale.y * 2,
			gameObject.transform.position.z
		);
		sphere.transform.localScale = new Vector3 (
			gameObject.transform.localScale.y / 3,
			gameObject.transform.localScale.y / 3,
			gameObject.transform.localScale.y / 3
		);
		sphere.transform.parent = gameObject.transform;
	}

	public void HideIndicator() {
		Destroy (sphere);
		sphere = null;
	}
}

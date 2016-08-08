using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	public Vector2 coords;
	public GridManager gridManager;
	public bool hoverOver;

	private Material originalMaterial;

	// Use this for initialization
	void Start () {
		originalMaterial = gameObject.GetComponent<Renderer> ().material;	
	}
	
	// Update is called once per frame
	void Update () {
		if (hoverOver) {
			gameObject.GetComponent<Renderer> ().material = gridManager.selectedMaterial;
		} else {
			gameObject.GetComponent<Renderer> ().material = originalMaterial;
		}
	}
}

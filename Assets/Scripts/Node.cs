using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	public Vector2 coords;
	public GridManager gridManager;
	public bool hoverOver;

	private Material originalMaterial;
	private Material currentMaterial;
	private Renderer rendar;

	// Use this for initialization
	void Start () {
		rendar = gameObject.GetComponent<Renderer> ();
		originalMaterial = rendar.material;	
		currentMaterial = originalMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		if (hoverOver && currentMaterial == originalMaterial) {
			rendar.material = gridManager.selectedMaterial;
		} else if(!hoverOver && currentMaterial != originalMaterial) {
			rendar.material = originalMaterial;
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class Unit_Placement : MonoBehaviour {

	public Animator animator;
	public bool isBeingTouched = false;

	private GameObject cube;
	private Color cubeSelectedColor = Color.red;
	private Renderer cubeRendar;
	private Vector3 cubeRotation = new Vector3 (1.0f, 1.0f, 1.0f);

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		GetComponent<VRTK_InteractableObject>().InteractableObjectTouched += new InteractableObjectEventHandler(WasTouched);
		GetComponent<VRTK_InteractableObject>().InteractableObjectUntouched += new InteractableObjectEventHandler(WasUntouched);
	}

	private void WasTouched(object sender, InteractableObjectEventArgs e) {
		ShowTouchedIndicator ();
		isBeingTouched = true;
	}

	private void WasUntouched(object sender, InteractableObjectEventArgs e) {
		HideIndicator ();
		isBeingTouched = false;
	}

	public void ShowTouchedIndicator() {
		cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.localScale = new Vector3 (0.025f, 0.025f, 0.025f);
		cube.transform.parent = gameObject.transform;
		cube.transform.position = new Vector3 (
			gameObject.transform.position.x,
			gameObject.transform.position.y + gameObject.transform.localScale.y * 2.0f,
			gameObject.transform.position.z
		);
		cubeRendar = cube.GetComponent<Renderer> ();
	}

	public void HideIndicator() {
		if (cube != null) {
			Destroy (cube);
			cube = null;
			cubeRendar = null;
		}
	}
}

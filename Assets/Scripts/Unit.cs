using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public bool isBeingTouched;
	public bool isSelected;

	private GameObject cube;
	private Color cubeSelectedColor = Color.red;
	private Renderer cubeRendar;
	private Vector3 cubeRotation;

	// Use this for initialization
	void Start () {
		isBeingTouched = false;
		isSelected = false;
		cubeRotation = new Vector3 (1.0f, 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(cube != null) {
			cube.transform.Rotate (cubeRotation, Time.deltaTime * 120f);
		}
		//Create touched cube above if it doesn't exist and unit is being touched.
		if (isBeingTouched && cube == null) {
			cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube.transform.localScale = new Vector3 (0.025f, 0.025f, 0.025f);
			cube.transform.parent = gameObject.transform;
			cube.transform.position = new Vector3 (
				gameObject.transform.position.x,
				gameObject.transform.position.y + gameObject.transform.localScale.y * 120,
				gameObject.transform.position.z
			);
			cubeRendar = cube.GetComponent<Renderer> ();
		//If unit isn't being touched and it isn't selected and the cube is there, destroy it.
		} else if (!isBeingTouched && !isSelected && cube != null) {
			Destroy (cube);
			cube = null;
			cubeRendar = null;
		//If unit is still selected and the cube exists, turn it red.
		} else if (isSelected && cube != null) {
			if (cubeRendar.material.color != cubeSelectedColor) {
				cubeRendar.material.color = cubeSelectedColor;
			}
		}
	}
}

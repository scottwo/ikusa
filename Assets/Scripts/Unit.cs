﻿using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public bool isBeingTouched;
	public bool isSelected;
	public Node currentNode;
	public Animator animator;

	private GameObject cube;
	private Color cubeSelectedColor = Color.red;
	private Renderer cubeRendar;
	private Vector3 cubeRotation;

	// Use this for initialization
	void Start () {
		isBeingTouched = false;
		isSelected = false;
		cubeRotation = new Vector3 (1.0f, 1.0f, 1.0f);
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(cube != null) {
			cube.transform.Rotate (cubeRotation, Time.deltaTime * 120f);
		}
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

	public void ShowSelectedIndicator() {
		if (cubeRendar.material.color != cubeSelectedColor) {
			cubeRendar.material.color = cubeSelectedColor;
		}
	}

	public void HideIndicator() {
		Destroy (cube);
		cube = null;
		cubeRendar = null;
	}
}

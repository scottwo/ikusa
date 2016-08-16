﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class Unit : MonoBehaviour {

	public Unit() {}
	public UnitManager unitManager;
	public PlayerManager playerManager;
	public bool isBeingTouched;
	public bool isSelected;
	public Node currentNode;
	public Animator animator;
	public Player player;
	public List<Actions> actionQueue = new List<Actions>();

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
		GetComponent<VRTK_InteractableObject>().InteractableObjectTouched += new InteractableObjectEventHandler(WasTouched);
		GetComponent<VRTK_InteractableObject>().InteractableObjectUntouched += new InteractableObjectEventHandler(WasUntouched);
	}
	
	// Update is called once per frame
	void Update () {
		if(cube != null) {
			cube.transform.Rotate (cubeRotation, Time.deltaTime * 120f);
		}
	}

	private void WasTouched(object sender, InteractableObjectEventArgs e) {
		if(player == playerManager.currentPlayer && !isSelected) {
			ShowTouchedIndicator ();
		}
		isBeingTouched = true;
		unitManager.touchedUnit = this;
	}
		
	private void WasUntouched(object sender, InteractableObjectEventArgs e) {
		if(!isSelected) {
			HideIndicator ();
		}
		isBeingTouched = false;
		unitManager.touchedUnit = null;
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
		if (this.player == playerManager.currentPlayer && cubeRendar.material.color != cubeSelectedColor) {
			cubeRendar.material.color = cubeSelectedColor;
		}
	}

	public void HideIndicator() {
		if (cube != null) {
			Destroy (cube);
			cube = null;
			cubeRendar = null;
		}
	}
}

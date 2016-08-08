using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public bool isBeingTouched;
	public bool isSelected;

	// Use this for initialization
	void Start () {
		isBeingTouched = false;
		isSelected = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
}

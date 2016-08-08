using UnityEngine;
using System.Collections;
using VRTK;

public class ControllerInteract_Listener : MonoBehaviour {

	public UnitManager unitManager;

	// Use this for initialization
	void Start () {
		GetComponent<VRTK_InteractTouch>().ControllerTouchInteractableObject += new ObjectInteractEventHandler(SomethingWasTouched);
		GetComponent<VRTK_InteractTouch>().ControllerUntouchInteractableObject += new ObjectInteractEventHandler(SomethingWasUntouched);
		GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(TriggerWasPulled);
		GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(TriggerWasReleased);
		GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(TouchPadWasPressed);
//		GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);
//		GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
//		GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
//		GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
//		GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += new ControllerInteractionEventHandler(MenuButtonPressed);
//		GetComponent<VRTK_ControllerEvents>().ApplicationMenuReleased += new ControllerInteractionEventHandler(DoApplicationMenuReleased);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void SomethingWasTouched(object sender, ObjectInteractEventArgs e) {
		if(e.target.name.ToLower().Contains("hero")) {
			Unit unit = e.target.GetComponent<Unit> ();
			unit.isBeingTouched = true;
			unitManager.touchedUnit = unit;
		}
	}

	private void SomethingWasUntouched(object sender, ObjectInteractEventArgs e) {
		if(e.target.name.ToLower().Contains("hero")) {
			Unit unit = e.target.GetComponent<Unit> ();
			unit.isBeingTouched = false;
			unitManager.touchedUnit = null;
		}
	}

	private void TriggerWasPulled(object sender, ControllerInteractionEventArgs e) {
		if (unitManager.touchedUnit != null) {
			unitManager.SelectUnit (unitManager.touchedUnit, gameObject);
		} else {
			unitManager.DeselectUnit ();
		}
		Debug.Log (this.transform.position);
		Debug.Log (gameObject.transform.position);
	}

	private void TriggerWasReleased(object sender, ControllerInteractionEventArgs e) {

	}

	private void TouchPadWasTouched() {
		
	}

	private void TouchPadWasPressed(object sender, ControllerInteractionEventArgs e) {
		if (unitManager.selectedUnit != null) {
			unitManager.MoveUnitByXZ (unitManager.selectedUnit, this.transform.position);
		} 
	}

	private void MenuButtonPressed() {
		
	}
}

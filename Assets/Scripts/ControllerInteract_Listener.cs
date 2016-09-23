using UnityEngine;
using VRTK;

public class ControllerInteract_Listener : MonoBehaviour {

	public UnitManager unitManager;
	public GridManager gridManager;
	public PlayerManager playerManager;
	public TurnManager turnManager;

	public InteractableObject touchedObject;

	void Start () {
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

	void Update () {
		Node hoveringNode = gridManager.findNodeByCordFloat (this.transform.position);
		if (hoveringNode != null) {
			gridManager.hoveringNode = hoveringNode;
		} else {
			gridManager.hoveringNode = null;
		}
	}

	private void TriggerWasPulled(object sender, ControllerInteractionEventArgs e) {
		if (unitManager.touchedUnit != null) {
			unitManager.touchedUnit.TriggerWasPulled ();
		} else {
			unitManager.DeselectUnit ();
		}
		if (touchedObject != null) {
			touchedObject.WasTriggered ();
		}
	}

	private void TriggerWasReleased(object sender, ControllerInteractionEventArgs e) {

	}

	private void TouchPadWasTouched() {
		
	}

	private void TouchPadWasPressed(object sender, ControllerInteractionEventArgs e) {
		if (unitManager.selectedUnit != null) {
			unitManager.MoveUnit (unitManager.selectedUnit, gridManager.hoveringNode);
		} 
	}

	private void MenuButtonPressed() {
		
	}
}

public interface InteractableObject {
	void WasTriggered();
}
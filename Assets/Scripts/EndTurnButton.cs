using UnityEngine;
using System.Collections;
//using VRTK;

public class EndTurnButton : MonoBehaviour {

	public GameManager gameManager;
	public TurnManager turnManager;
//	public ControllerInteract_Listener listener;

	private bool untouched = true;

	//WARNING: Something is very wrong about this. Fix it. Seems to trigger twice with every touch.
	//Just test it and see what I mean.
	void Start () {
//		listener = gameManager.RightController.GetComponent<ControllerInteract_Listener> ();
//		GetComponent<VRTK_InteractableObject>().InteractableObjectTouched += new InteractableObjectEventHandler(WasTouched);
//		GetComponent<VRTK_InteractableObject>().InteractableObjectUntouched += new InteractableObjectEventHandler(WasUntouched);
	}
	
//	void WasTouched(object sender, InteractableObjectEventArgs e) {
//		if (untouched) {
//			listener.touchedObject = this;
//			untouched = false;
//		}
//	}
//
//	void WasUntouched(object sender, InteractableObjectEventArgs e) {
//		listener.touchedObject = null;
//		untouched = true;
//	}
		
	public void WasTriggered() {
		turnManager.NextTurn ();
	}
}

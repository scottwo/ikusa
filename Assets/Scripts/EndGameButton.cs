using UnityEngine;
using System.Collections;
using VRTK;

public class EndGameButton : MonoBehaviour, InteractableObject {

	public GameManager gameManager;
	public ControllerInteract_Listener listener;

	private bool untouched = true;

	void Start () {
		listener = gameManager.RightController.GetComponent<ControllerInteract_Listener> ();
		GetComponent<VRTK_InteractableObject>().InteractableObjectTouched += new InteractableObjectEventHandler(WasTouched);
		GetComponent<VRTK_InteractableObject>().InteractableObjectUntouched += new InteractableObjectEventHandler(WasUntouched);
	}

	void WasTouched(object sender, InteractableObjectEventArgs e) {
		if (untouched) {
			listener.touchedObject = this;
			untouched = false;
		}
	}

	void WasUntouched(object sender, InteractableObjectEventArgs e) {
		listener.touchedObject = null;
		untouched = true;
	}

	public void WasTriggered() {
		gameManager.ClearGame ();
	}
}

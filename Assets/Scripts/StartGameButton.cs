using UnityEngine;
using System.Collections;
using VRTK;

public class StartGameButton : MonoBehaviour {

	public ControllerInteract_Listener listener;
	public GameManager gameManager;

	private bool untouched = true;

	//WARNING: Something is very wrong about this. Fix it. Seems to trigger twice with every touch.
	//Just test it and see what I mean.
	void Start () {
		GetComponent<VRTK_InteractableObject>().InteractableObjectTouched += new InteractableObjectEventHandler(WasTouched);
		GetComponent<VRTK_InteractableObject>().InteractableObjectUntouched += new InteractableObjectEventHandler(WasUntouched);
	}
	
	void WasTouched(object sender, InteractableObjectEventArgs e) {
		if (untouched) {
			listener.touchedObject = gameObject;
			untouched = false;
		}
	}

	void WasUntouched(object sender, InteractableObjectEventArgs e) {
		listener.touchedObject = null;
		untouched = true;
	}

	public void WasTriggered() {
		gameManager.StartNewGame ();
	}
}

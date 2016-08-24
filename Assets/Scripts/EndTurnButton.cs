using UnityEngine;
using System.Collections;
using VRTK;

public class EndTurnButton : MonoBehaviour {

	public TurnManager turnManager;

	private bool untouched = true;

	//WARNING: Something is very wrong about this. Fix it.
	void Start () {
		GetComponent<VRTK_InteractableObject>().InteractableObjectTouched += new InteractableObjectEventHandler(WasTouched);
		GetComponent<VRTK_InteractableObject>().InteractableObjectUntouched += new InteractableObjectEventHandler(WasUntouched);
	}
	
	void WasTouched(object sender, InteractableObjectEventArgs e) {
		if (untouched) {
			turnManager.NextTurn ();
			untouched = false;
		}
	}

	void WasUntouched(object sender, InteractableObjectEventArgs e) {
		untouched = true;
	}
}

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GridManager gridManager;
	public TurnManager turnManager;
	public PlayerManager playerManager;
	public UnitManager unitManager;
	public MenuManager menuManager;
	public Game currentGame;
	public GridManager.Size mapSize;
	public GameObject RightController;
	public Material buttonSelectedMaterial;
	public bool gameInProgress = false;

	void Start() {
		gridManager.GenerateGrid (gridManager.CreateRandomGrid());
	}

	public void StartNewGame() {
		unitManager.ClearUnits ();
		gridManager.ClearGrid ();
		gridManager.mapSize = mapSize;
		gridManager.GenerateGrid (gridManager.CreateRandomGrid());
		InitiateUnitPlacement ();
		gameInProgress = true;
	}

	public void ResetCurrentGame() {
		
	}

	public void ClearGame() {
		gameInProgress = false;
		unitManager.ClearUnits ();
		gridManager.ClearGrid ();
		gridManager.GenerateGrid (gridManager.CreateRandomGrid());
	}

	public void EndGame(Player loser) {
		Debug.Log ("End game");
		gameInProgress = false;
		if (!loser.isUser) {
			// Create message saying congrats you won!
			ClearGame();
		} else {
			// Message is condescending and mean.
			StartNewGame();
		}
	}

	public void SetMapSize(GridManager.Size newMapSize, Menu_MapSize_Button button) {
		mapSize = newMapSize;
		button.GetComponent<Renderer> ().material = buttonSelectedMaterial;
		//Somehow remove the material from the other buttons. Need a reference to the buttons. MenuManager?
		//Doesn't seem to be working anyways.
	}
		
	void InitiateUnitPlacement() {
		int currency;
		switch (mapSize) {
		case GridManager.Size.small:
			currency = 1000;
			break;
		case GridManager.Size.medium:
			currency = 2000;
			break;
		case GridManager.Size.large:
			currency = 3000;
			break;
		case GridManager.Size.huge:
			currency = 5000;
			break;
		}
		gridManager.AddPlaceableUnits ();
		menuManager.AddUnitPlacementUI ();
	}
}

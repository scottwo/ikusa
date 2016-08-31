using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GridManager gridManager;
	public TurnManager turnManager;
	public PlayerManager playerManager;
	public UnitManager unitManager;
	public Game currentGame;
	public GridManager.Size mapSize;
	public GameObject RightController;
	public Material buttonSelectedMaterial;
	public bool gameInProgress = false;

	void Start() {
		gridManager.GenerateGrid ();
	}

	public void StartNewGame() {
		gameInProgress = true;
		unitManager.ClearUnits ();
		gridManager.ClearGrid ();
		gridManager.mapSize = mapSize;
		gridManager.GenerateGrid ();
		unitManager.createUnit (gridManager.findNodeByCord(0, 0), gridManager.unitScale, UnitManager.UnitType.melee, playerManager.playerList[0]);
		unitManager.createUnit (gridManager.findNodeByCord(1, 0), gridManager.unitScale, UnitManager.UnitType.heavy_melee, playerManager.playerList[1]);
		unitManager.createUnit (gridManager.findNodeByCord(2, 0), gridManager.unitScale, UnitManager.UnitType.ranged, playerManager.playerList[0]);
		unitManager.createUnit (gridManager.findNodeByCord(3, 0), gridManager.unitScale, UnitManager.UnitType.heavy_ranged, playerManager.playerList[1]);
		unitManager.createUnit (gridManager.findNodeByCord(4, 0), gridManager.unitScale, UnitManager.UnitType.mage, playerManager.playerList[0]);
		unitManager.createUnit (gridManager.findNodeByCord(5, 0), gridManager.unitScale, UnitManager.UnitType.heavy_mage, playerManager.playerList[1]);
		unitManager.createUnit (gridManager.findNodeByCord(6, 0), gridManager.unitScale, UnitManager.UnitType.buff_mage, playerManager.playerList[0]);
		unitManager.createUnit (gridManager.findNodeByCord(7, 0), gridManager.unitScale, UnitManager.UnitType.heal_mage, playerManager.playerList[1]);
		turnManager.StartFirstTurn ();
	}

	public void ResetCurrentGame() {
		
	}

	public void ClearGame() {
		gridManager.ClearGrid ();
		gridManager.GenerateGrid ();
		unitManager.ClearUnits ();
	}

	public void EndGame(Player loser) {
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
	}
		
}

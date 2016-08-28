using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GridManager gridManager;
	public TurnManager turnManager;
	public PlayerManager playerManager;
	public UnitManager unitManager;
	public Game currentGame;

	public GameObject RightController;

	void Start() {
		gridManager.GenerateGrid ();
	}

	public void StartNewGame() {
		unitManager.ClearUnits ();
		gridManager.ClearGrid ();
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
}

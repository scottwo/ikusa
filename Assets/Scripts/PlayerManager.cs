using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	public GridManager gridManager;
	public UnitManager unitManager;
	public TurnManager turnManager;
	public List<Player> playerList = new List<Player> ();
	public Player player;
	public int numOfPlayers;

	void Awake () {
		for(int i = 0; i < numOfPlayers; i++) {
			Player newPlayer = Instantiate (player);
			newPlayer.playerManager = this;
			newPlayer.id = i;
			playerList.Add (newPlayer);
		}
		playerList [0].isUser = true;
	}

	void Update() {
		
	}

	public void ProcessAITurn(Player player) {
		//Do the robot.
		//Cycle through each player unit.
		//Cycle through each non-player unit.
		//Move adjacent to and attack the nearest unit.
		//Figure out how to do it only one at a time. I even added an update function to see if i could use it.
		//So, see if you can use it.
		for (int i = 0; i < player.units.Count; i++) {
			Unit unit = player.units [i];
			//Create a list of enemies.
			List<Unit> enemies = new List<Unit> ();
			for (int j = 0; j < unitManager.units.Count; j++) {
				if (unitManager.units [j] != unit && unitManager.units [j].player != unit.player) {
					enemies.Add (unitManager.units[j]);
				}
			}
			//Find the closest enemy.
			if (enemies.Count > 0) {
				Unit closestEnemy = enemies [0];
				float closestDistance = Vector2.Distance (enemies [0].currentNode.coords, unit.currentNode.coords);
				for (int j = 0; j < enemies.Count; j++) {
					if (Vector2.Distance (enemies [j].currentNode.coords, unit.currentNode.coords) < closestDistance) {
						closestEnemy = enemies [j];
					}
				}
				Node nodeToMoveTo = closestEnemy.currentNode;
				int x = (int)unit.currentNode.coords.x - (int)closestEnemy.currentNode.coords.x;
//			int y = (int)unit.currentNode.coords.y - (int)closestEnemy.currentNode.coords.y;
				if (x >= 0) {
					nodeToMoveTo = gridManager.findNodeByCord ((int)closestEnemy.currentNode.coords.x + 1, (int)closestEnemy.currentNode.coords.y);
					if (nodeToMoveTo.currentUnit != null) {
						nodeToMoveTo = gridManager.findNodeByCord ((int)closestEnemy.currentNode.coords.x, (int)closestEnemy.currentNode.coords.y + 1);
					}
				} else {
					nodeToMoveTo = gridManager.findNodeByCord ((int)closestEnemy.currentNode.coords.x - 1, (int)closestEnemy.currentNode.coords.y);
					if (nodeToMoveTo.currentUnit != null) {
						nodeToMoveTo = gridManager.findNodeByCord ((int)closestEnemy.currentNode.coords.x, (int)closestEnemy.currentNode.coords.y + 1);
					}
				}
				//attack closest enemy.
//			unitManager.MoveUnit (unit, closestEnemy.currentNode);
				unitManager.MoveUnit (unit, nodeToMoveTo);
				unitManager.UnitCombat (unit, closestEnemy);
			} else {
				//No more enemies. Game is won. Baddies win.
				Debug.Log("Enemies won. You lost by literally doing nothing.");
				return;
			}
		}
	}

}

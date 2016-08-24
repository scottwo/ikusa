using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour {

	//What the TurnManager does:
	//Provides a place where player/unit/grid managers can go to see if they should perform certain actions.
	//Should have a public player for who the current player is. If the current player is the ai, then 
	//the user's actions are mostly disabled. They should still be able to touch/hover over units, though.
	//When it's the AI's turn, it should also let the AIManager know that it can do it's thing.
	//At the start of a turn, it should run the NewTurn method on the player whose turn it is.
	//It should record a record of each turn and the actions performed in it. Maybe it has a list of Actions
	//for each turn.
	public PlayerManager playerManager;
	public Player currentPlayer;

	void Start () {
	
	}

	void Update () {
		bool allInactive = true;
		for (int i = 0; i < currentPlayer.units.Count; i++) {
			if (currentPlayer.units [i].active) {
				allInactive = false;
			}
		}
		if (allInactive) {
			NextTurn ();
		}
	}

	public void NextTurn() {
		//clean up current player's units.
		for (int i = 0; i < currentPlayer.units.Count; i++) {
			currentPlayer.units [i].active = false;
			currentPlayer.units [i].currentActionPoints = 0;
			currentPlayer.units [i].currentMovementPoints = 0;
		}

		//Change player to next player in the list and setup their units/stough.
		int currentIndex = currentPlayer.id;
		int nextPlayerIndex = currentIndex + 1;
		if (nextPlayerIndex == playerManager.playerList.Count) {
			currentPlayer = playerManager.playerList [0];
		} else {
			currentPlayer = playerManager.playerList [nextPlayerIndex];
		}

		//Setup next player's units
		for (int i = 0; i < currentPlayer.units.Count; i++) {
			currentPlayer.units [i].active = true;
			currentPlayer.units [i].currentActionPoints = currentPlayer.units[i].maxActionPoints;
			currentPlayer.units [i].currentMovementPoints = currentPlayer.units[i].maxMovementPoints;
		}

		//If the next player is AI, let it robot around.
		if (!currentPlayer.isUser) {
			playerManager.ProcessAITurn (currentPlayer);
		}
	}

	public void StartFirstTurn() {
		currentPlayer = playerManager.playerList[0];
		for (int i = 0; i < currentPlayer.units.Count; i++) {
			currentPlayer.units [i].active = true;
			currentPlayer.units [i].currentActionPoints = currentPlayer.units[i].maxActionPoints;
			currentPlayer.units [i].currentMovementPoints = currentPlayer.units[i].maxMovementPoints;
		}
	}
}

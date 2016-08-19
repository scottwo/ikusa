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
	
	}

	public void NextTurn() {
		//clean up current player's units.
//		currentPlayer.units.ForEach(u => u.);
		//Change player to next player in the list and setup their units/stough.
		int currentIndex = playerManager.playerList.FindIndex (p => p.id == currentPlayer.id);
		if (currentIndex == playerManager.playerList.Count - 1) {
			currentPlayer = playerManager.playerList [0];
		} else {
			currentPlayer = playerManager.playerList [currentIndex++];
		}

	}
}

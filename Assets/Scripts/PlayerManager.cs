using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	public List<Player> playerList = new List<Player> ();
	public Player currentPlayer;
	public Player player;
	public int numOfPlayers;

	void Awake () {
		for(int i = 0; i < numOfPlayers; i++) {
			Player newPlayer = Instantiate (player);
			newPlayer.playerManager = this;
			newPlayer.id = i;
			playerList.Add (newPlayer);
		}
		playerList [0] = currentPlayer;
	}
}

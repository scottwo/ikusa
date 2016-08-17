using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	public List<Player> playerList = new List<Player> ();
	public Player currentPlayer;
	public Player player;
	public int numOfPlayers;

	void Awake () {
		for(int i =0; i < numOfPlayers; i++) {
			playerList.Add (Instantiate(player));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public Player[] playerList;
	public Player currentPlayer;
	public Player player;

	void Awake () {
		playerList = new Player[2];
		playerList[0] = Instantiate(player);
		playerList [0].isUser = true;
		playerList [0].id = 0;
		playerList [0].transform.parent = this.transform;
		playerList[1] = Instantiate(player);
		playerList [1].isUser = false;
		playerList [1].id = 1;
		playerList [1].transform.parent = this.transform;
		currentPlayer = playerList [0];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

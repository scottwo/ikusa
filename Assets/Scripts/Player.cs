using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public bool isUser;
	public int id;
	public List<Unit> units = new List<Unit>();
	public PlayerManager playerManager;

}

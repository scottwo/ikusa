using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public bool isUser;
	public int id;
	public List<Unit> units = new List<Unit>();
	public PlayerManager playerManager;

	public void NewTurn() {
		for (int i = 0; i < units.Count; i++) {
			units [i].active = true;
			units [i].currentActionPoints = units [i].maxActionPoints;
			units [i].currentMovementPoints = units [i].maxMovementPoints;
		}
	}

	public void EndTurn() {
		for (int i = 0; i < units.Count; i++) {
			units [i].active = false;
			units [i].currentActionPoints = 0;
			units [i].currentMovementPoints = 0;
		}
	}
}

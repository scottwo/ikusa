using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

	public UnitManager unitManager;
	public PlayerManager playerManager;
	public TurnManager turnManager;
	public Node cube;
	public Material[] materials;
	public Material selectedMaterial;
	public Vector3 initialPosition;
	public enum Size
	{
		small, medium, large, huge
	};
	public Size mapSize;
	public Node hoveringNode;
	public Node[] path;

	private Node[] grid;
	private float sideLength = 1.2f;
	private int xSize;
	private int zSize;
	private float scale = 0.025f;
	private float unitScale;

	void Start () {
		GenerateGrid ();
		unitManager.createUnit (grid[0], unitScale, UnitManager.UnitType.melee, playerManager.playerList[0]);
		unitManager.createUnit (grid[10], unitScale, UnitManager.UnitType.heavy_melee, playerManager.playerList[1]);
		unitManager.createUnit (grid[20], unitScale, UnitManager.UnitType.ranged, playerManager.playerList[0]);
		unitManager.createUnit (grid[30], unitScale, UnitManager.UnitType.heavy_ranged, playerManager.playerList[1]);
		unitManager.createUnit (grid[40], unitScale, UnitManager.UnitType.mage, playerManager.playerList[0]);
		unitManager.createUnit (grid[50], unitScale, UnitManager.UnitType.heavy_mage, playerManager.playerList[1]);
		unitManager.createUnit (grid[60], unitScale, UnitManager.UnitType.buff_mage, playerManager.playerList[0]);
		unitManager.createUnit (grid[61], unitScale, UnitManager.UnitType.heal_mage, playerManager.playerList[1]);
		turnManager.StartFirstTurn ();
	}

	void GenerateGrid() {
		CalculateScaleModifier ();
		grid = new Node[xSize * zSize];
		for(int i = 0, v = 0; i < zSize; i++) {
			for (int j = 0; j < xSize; j++, v++) {
				float randomY = Random.Range (100, 120);
				randomY /= 100;
				grid [v] = Instantiate (cube);
				grid [v].transform.position = new Vector3 (
					initialPosition.x + (j * scale), 
					initialPosition.y + (grid [v].transform.localScale.y * randomY / 2), 
					initialPosition.z + (i * scale)
				);
				grid [v].transform.localScale = new Vector3 (scale, grid [v].transform.localScale.y * randomY, scale);
				grid [v].coords = new Vector2 (j, i);
				grid [v].transform.parent = gameObject.transform;
				grid [v].gridManager = this;
				if (randomY < 1.05f) {
					grid [v].GetComponent<MeshRenderer> ().material = materials [2];
				} else if (randomY < 1.12f) {
					grid [v].GetComponent<MeshRenderer> ().material = materials [1];
				} else {
					grid [v].GetComponent<MeshRenderer> ().material = materials[0];
				}
			}
		}
	}

	void CalculateScaleModifier() {
		switch (mapSize) {
		case Size.small:
			scale = sideLength / 8;
			xSize = zSize = 8;
			unitScale = 2.0f;
			break;
		case Size.medium:
			scale = sideLength / 16;
			xSize = zSize = 16;
			unitScale = 1.0f;
			break;
		case Size.large:
			scale = sideLength / 24;
			xSize = zSize = 24;
			unitScale = 0.75f;
			break;
		case Size.huge:
			scale = sideLength / 32;
			xSize = zSize = 32;
			unitScale = 0.5f;
			break;
		default:
			scale = sideLength / 8;
			xSize = zSize = 8;
			unitScale = 1.0f;
			break;
		}
		initialPosition.x += scale / 2;
		initialPosition.z += scale / 2;
	}

	void Update () {
		//Only draw path if user is hovering over a node and has selected a unit.
		if (hoveringNode != null && unitManager.selectedUnit != null) {
			RemovePath ();
			DrawPath ();
		}
	}

	public void RemovePath() {
		if (path != null) {
			for (int k = 0; k < path.Length; k++) {
				path [k].HideIndicator ();
			}
		}
		path = null;
	}

	public void DrawPath() {
		Node start = unitManager.selectedUnit.currentNode;
		int xDiff = Mathf.Abs ((int)hoveringNode.coords.x - (int)start.coords.x);
		int zDiff = Mathf.Abs ((int)hoveringNode.coords.y - (int)start.coords.y);
		int xDirection = hoveringNode.coords.x - start.coords.x > 0 ? 1 : -1;
		int zDirection = hoveringNode.coords.y - start.coords.y > 0 ? 1 : -1;

		Node[] tempPath = new Node[xDiff + zDiff];
		for (int i = 1; i <= xDiff; i++) {
			Node foundNode = findNodeByCord ((int)start.coords.x + i * xDirection, (int)start.coords.y);
			int index = i - 1;
			tempPath [index] = foundNode;
		}
		for (int j = 1; j <= zDiff; j++) {
			Node foundNode = findNodeByCord ((int)start.coords.x + xDiff * xDirection, (int)start.coords.y + j * zDirection);
			int index = xDiff + j - 1;
			tempPath [index] = foundNode;
		}
		if (tempPath != path) {
			bool blocked = false;
			int blockedIndex = 0;
			for(int k = 0; k < tempPath.Length; k++) {
				if (tempPath [k].currentUnit != null) {
					//TODO: implement pathfinding around the obstacle/unit.
					blocked = true;
					blockedIndex = k;
					break;
				}
			}
			if (blocked) {
				path = new Node[blockedIndex + 1];
				for(int k = 0; k <= blockedIndex; k++) {
					path [k] = tempPath [k];
				}
			} else {
				path = tempPath;
				for (int k = 0; k < path.Length; k++) {
					path [k].ShowIndicator ();
				}
			}
		}
	}

	public Node findNodeByCord(int x, int z) {
		int index = (z * xSize) + x;
		if (index < grid.Length && index > -1) {
			Node foundNode = grid [index];
			return foundNode;
		} else {
			return grid [0];
		}
	}

    public Node findNodeByCordFloat(Vector3 pos)
    {
		float x = pos.x;
		float z = pos.z;
		float minX = initialPosition.x - (scale / 2);
		float minZ = initialPosition.z - (scale / 2);
		int xCoord = (int) (( x - minX) / scale) + 1;
		int zCoord = (int) (( z - minZ) / scale) + 1;
		Node foundNode = findNodeByCord(xCoord, zCoord);
        return foundNode;
    }

}

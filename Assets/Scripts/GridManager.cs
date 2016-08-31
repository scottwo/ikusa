using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public float unitScale;

	private Node[] grid;
	private float sideLength = 1.2f;
	private int xSize;
	private int zSize;
	private float scale = 0.025f;
	private Vector3 adjustedPosition = Vector3.up;

	void Start() {}

	public void GenerateGrid(List<GridItem> map) {
		grid = new Node[xSize * zSize];
		for (int i = 0; i < map.Count; i++) {
			//Instantiate each prefab here.
//			switch (map [i].type) {
//			case "grass":
//				grid[i] = Instantiate(grassCube);
//				break;
//			case "forest":
//				grid[i] = Instantiate(forestCube);
//				break;
//			case "mountain":
//				grid[i] = Instantiate(mountainCube);
//				break;
//			case "desert":
//				grid[i] = Instantiate(desertCube);
//				break;
//			}
			grid [i] = Instantiate (cube);
			grid [i].transform.position = map [i].position;
			grid [i].transform.localScale = map [i].scale;
			grid [i].coords = map [i].coords;
			grid [i].transform.parent = gameObject.transform;
			grid [i].gridManager = this;
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
		adjustedPosition.y = initialPosition.y;
		adjustedPosition.x = initialPosition.x + scale / 2;
		adjustedPosition.z = initialPosition.z + scale / 2;
	}

	public void ClearGrid() {
		for (int i = 0; i < grid.Length; i++) {
			Destroy (grid [i].gameObject);
		}
		grid = new Node[0];
		path = null;
		hoveringNode = null;
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
//			bool blocked = false;
//			int blockedIndex = 0;
//			for(int k = 0; k < tempPath.Length; k++) {
//				if (tempPath [k].currentUnit != null) {
//					//TODO: implement pathfinding around the obstacle/unit.
//					blocked = true;
//					blockedIndex = k;
//					break;
//				}
//			}
//			if (blocked) {
//				path = new Node[blockedIndex + 1];
//				for(int k = 0; k <= blockedIndex; k++) {
//					path [k] = tempPath [k];
//				}
//			} else {
				path = tempPath;
				for (int k = 0; k < path.Length; k++) {
					path [k].ShowIndicator ();
				}
//			}
		}
	}

	public Node findNodeByCord(int x, int z) {
		int index = (z * xSize) + x;
		if (grid != null && index < grid.Length && index > -1) {
			Node foundNode = grid [index];
			return foundNode;
		} else {
			return cube;
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

	public List<GridItem> CreateRandomGrid() {
		List<GridItem> map = new List<GridItem> ();
		CalculateScaleModifier ();
		for(int i = 0, v = 0; i < zSize; i++) {
			for (int j = 0; j < xSize; j++, v++) {
				float randomY = Random.Range (100, 110);
				randomY /= 100;
				GridItem newItem = new GridItem ();
				newItem.position = new Vector3 (
					adjustedPosition.x + (j * scale), 
					adjustedPosition.y + (cube.transform.localScale.y * randomY * 0.5f), 
					adjustedPosition.z + (i * scale)
				);
				newItem.scale = new Vector3 (scale, cube.transform.localScale.y * randomY, scale);
				newItem.coords = new Vector2 (j, i);
				//Determine type.
				map.Add(newItem);
			}
		}
		//Here's where we would add mountains/water features.
		return map;
	}
	public List<GridItem> LoadPremadeGrid(int gridNumber) {
		List<GridItem> map = new List<GridItem> ();
		return map;
	}

}

public class GridItem {
	public Vector3 scale;
	public Vector3 position;
	public Vector2 coords;
	public string type;
}

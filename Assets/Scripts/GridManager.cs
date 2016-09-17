using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour {

	public UnitManager unitManager;
	public PlayerManager playerManager;
	public TurnManager turnManager;
	public Node plainCube;
	public Node forestCube;
	public Node mountainCube;
	public Node waterCube;
	public Material[] materials;
	public Material selectedMaterial;
	public Vector3 initialPosition;
	public enum Size
	{
		small, medium, large, huge
	};
	public Size mapSize;
	public Node hoveringNode;
	public List<Node> path;
	public float unitScale;
	public float heightVariation;

	private Node[] grid;
	private float sideLength = 1.2f;
	private int xSize;
	private int zSize;
	private float scale = 0.025f;
	public Vector3 adjustedPosition = Vector3.up;
	private int dsSteps;

	void Start() {}

	public void GenerateGrid(List<GridItem> map) {
		grid = new Node[xSize * zSize];
		for (int i = 0; i < map.Count; i++) {
			//Instantiate each prefab here.
			switch (map [i].type) {
			case "water":
				grid[i] = Instantiate(waterCube);
				break;
			case "plain":
				grid[i] = Instantiate(plainCube);
				break;
			case "forest":
				grid[i] = Instantiate(forestCube);
				break;
			case "mountain":
				grid[i] = Instantiate(mountainCube);
				break;
			}
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
			scale = sideLength / 9;
			xSize = zSize = 9;
			unitScale = 2.0f;
			dsSteps = 3;
			break;
		case Size.medium:
			scale = sideLength / 17;
			xSize = zSize = 17;
			unitScale = 1.0f;
			dsSteps = 4;
			break;
		case Size.large:
			scale = sideLength / 33;
			xSize = zSize = 33;
			unitScale = 0.5f;
			dsSteps = 5;
			break;
		case Size.huge:
			scale = sideLength / 65;
			xSize = zSize = 65;
			unitScale = 0.25f;
			dsSteps = 6;
			break;
		default:
			scale = sideLength / 9;
			xSize = zSize = 9;
			unitScale = 1.0f;
			dsSteps = 3;
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
			for (int k = 0; k < path.Count; k++) {
				path [k].HideIndicator ();
			}
		}
		path = null;
	}

	public void DrawPath() {
		PathfindingUnit start = new PathfindingUnit ();
		start.current = unitManager.selectedUnit.currentNode;
		start.g = 0;
		List<Node> tempPath = FindPath (start, hoveringNode);
		if (tempPath != path) {
			path = tempPath;
			for (int k = 0; k < path.Count; k++) {
				path [k].ShowIndicator ();
			}
		}
	}

	public List<Node> FindPath(PathfindingUnit start, Node end) {
		List<Node> bestPath = new List<Node> ();
		PathfindingUnit nextBest = new PathfindingUnit ();
		if (start.current == end) {
			return bestPath;
		} else {
			List<Node> neighbors = FindNeighborNodes (start.current);
			nextBest.parent = start.current;
			nextBest.current = neighbors [0];
			nextBest.g = start.g + 1;
			nextBest.h = (int) Math.Abs(end.coords.x - neighbors[0].coords.x) + (int) Math.Abs(end.coords.y - neighbors[0].coords.y);
			nextBest.f = nextBest.g + nextBest.h;
			for (int i = 0; i < neighbors.Count; i++) {
				int g = start.g + 1;
				int h = (int) Math.Abs(end.coords.x - neighbors[i].coords.x) + (int) Math.Abs(end.coords.y - neighbors[i].coords.y);
				int f = g + h;
				if (f < nextBest.f) {
					nextBest.current = neighbors [i];
					nextBest.f = f;
					nextBest.g = g;
					nextBest.h = h;
				}
			}
			bestPath.Add (nextBest.current);
			return FindPath (nextBest, end);
		}

	}

	public List<Node> FindNeighborNodes(Node parent) {
		List<Node> tempNeighbors = new List<Node> ();
		List<Node> neighbors = new List<Node> ();
		tempNeighbors.Add (findNodeByCord((int)parent.coords.x, (int)parent.coords.y++));
		tempNeighbors.Add (findNodeByCord((int)parent.coords.x++, (int)parent.coords.y));
		tempNeighbors.Add (findNodeByCord((int)parent.coords.x, (int)parent.coords.y--));
		tempNeighbors.Add (findNodeByCord((int)parent.coords.x--, (int)parent.coords.y));
		for (int i = 0; i < tempNeighbors.Count; i++) {
			if (tempNeighbors [i].currentUnit == null) {
				neighbors.Add (tempNeighbors [i]);
			}
		}
		return neighbors;
	}

	public Node findNodeByCord(int x, int z) {
		int index = (z * xSize) + x;
		if (grid != null && index < grid.Length && index > -1) {
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
		int xCoord = (int) (( x - minX) / scale);
		int zCoord = (int) (( z - minZ) / scale);
		Node foundNode = findNodeByCord(xCoord, zCoord);
        return foundNode;
    }

	public List<GridItem> CreateRandomGrid() {
		List<GridItem> map = new List<GridItem> ();
		CalculateScaleModifier ();
		List<GridRow> heightMap = GenerateHeightMap ();

		//Simulate moisture/rain.
		//Random base amount for all points (Determines climate)
		//Added amounts for orthographic rain stuff. 
		//Randomly generate a wind direction.
		//Start from that direction and add moisture if the next node is higher than the current node
		//and add even more as the elevation increases.
		//Generate another heightmap for drainage.
		//Nodes can only keep up to 1x drainage. If they have more it has to flow to the neighbor node.
		//If node has double moisture than drainage, then it's river.
		//Nodes can only take 2x drainage. Anything over that overflows into neighbor nodes.
		//Loop through all nodes. If moisture is greater than drainage, then it flows.
		//Have it flow into lowest neighbor (Add lowest neighbor and overflow into queue to be processed).
		//If lowest neighbor is higher elevation, don't add unless double drainage.
		//If double drainage, becomes lake/pond. If more than double drainage, leaks into lowest neighbor even if higher.
		//Loop through queue until it's empty.

		//Over 2x drainage:
		//If there is a lower elevation neighbor node, send the overflow there.

		//If it's the lowest elevation, it's a pond that can grow into a lake. 
		//Water overflows to the lowest neighbor and the lake height raises to the neighbor's height.
		//If there's still overflow, it keeps overflowing into lowest height neighbor squares until
		//it reaches one that it doesn't reach 1x its drainage.
		GridItem highest;
		GridItem lowest;
		GridItem middle = new GridItem ();
		middle.position = adjustedPosition;
		highest = lowest = middle;
		for(int i = 0, v = 0; i < zSize; i++) {
			for (int j = 0; j < xSize; j++, v++) {
				GridItem newItem = new GridItem ();
				newItem.position = new Vector3 (
					adjustedPosition.x + (j * scale), 
					adjustedPosition.y + (heightMap[i].points[j] * 0.5f) - 0.05f, 
					adjustedPosition.z + (i * scale)
				);
				newItem.scale = new Vector3 (scale, heightMap[i].points[j], scale);
				newItem.coords = new Vector2 (j, i);
				//Determine type.
				if(newItem.position.y > highest.position.y) {
					highest = newItem;
				}
				if(newItem.position.y < lowest.position.y) {
					lowest = newItem;
				}
				map.Add(newItem);
			}
		}
		float mapScale = highest.scale.y - lowest.scale.y;
		float waterHeight = lowest.scale.y + (mapScale * 0.15f);
		float plainsHeight = lowest.scale.y + (mapScale * 0.65f);
		float forestHeight = lowest.scale.y + (mapScale * 0.8f);
		for(int i = 0, v = 0; i < zSize; i++) {
			for (int j = 0; j < xSize; j++, v++) {
				if (map [v].scale.y <= waterHeight) {
					map [v].type = "water";
					map [v].scale.y = waterHeight;
					map [v].position.y = adjustedPosition.y + (waterHeight * 0.5f) - 0.05f;
				} else if (map [v].scale.y <= plainsHeight) {
					map [v].type = "plain";
				} else if (map [v].scale.y <= forestHeight) {
					map [v].type = "forest";
				} else {
					map [v].type = "mountain";
				}
			}
		}
		//Here's where we would add mountains/water features.
		return map;
	}

	List<GridRow> GenerateHeightMap() {
		List<GridRow> gridPoints = new List<GridRow> ();
		float tallestPoint = adjustedPosition.y + (plainCube.transform.localScale.y * 0.5f);
		for (int i = 0; i < zSize; i++) {
			//Generate flat rows.
			gridPoints.Add(new GridRow());
			for (int j = 0; j < xSize; j++) {
				gridPoints [i].points.Add (adjustedPosition.y + (plainCube.transform.localScale.y * 0.5f));
			}
		}
		//Run diamondSquare as many times as we need to for the size of the map.
		for(int i = 0, v = 0; i < dsSteps; i++) {
			int between = (int)Mathf.Pow (2, dsSteps - i);
			for (int j = 0; j < Mathf.Pow (2, i); j++) {
				for (int k = 0; k < Mathf.Pow (2, i); k++, v+=5) {
					int middleX = Mathf.FloorToInt (xSize / Mathf.Pow(2, i + 1)) + between * j;
					int middleY = Mathf.FloorToInt (xSize / Mathf.Pow(2, i + 1)) + between * k;

					//Now, do square step on this x, y.
					float ne = gridPoints[middleX + (between / 2)].points[middleY + (between / 2)];
					float se = gridPoints[middleX + (between / 2)].points[middleY - (between / 2)];
					float sw = gridPoints[middleX - (between / 2)].points[middleY - (between / 2)];
					float nw = gridPoints[middleX - (between / 2)].points[middleY + (between / 2)];

					gridPoints[middleX].points[middleY] = ((ne + se + sw + nw) / 4) * RandomValue(between);

					//What are the diamond points for these middle points?
					gridPoints [middleX].points [middleY + (between / 2)] = ((nw + ne) / 2) * RandomValue (between / 2);
					gridPoints [middleX].points [middleY - (between / 2)] = ((sw + se) / 2) * RandomValue (between / 2);
					gridPoints [middleX + (between / 2)].points [middleY] = ((se + ne) / 2) * RandomValue (between / 2);
					gridPoints [middleX - (between / 2)].points [middleY] = ((nw + sw) / 2) * RandomValue (between / 2);
				}
			}
		}
		//Scale everything back down
		for (int j = 0; j < gridPoints.Count; j++) {
			for (int k = 0; k < gridPoints [j].points.Count; k++) {
				gridPoints [j].points [k] *= (plainCube.transform.localScale.y * 2.0f / tallestPoint);
			}
		}

		return gridPoints;
	}

	float RandomValue(int stepScale) {
		float randomValue = UnityEngine.Random.Range (100 - (stepScale * heightVariation), 100 + (stepScale * heightVariation));
		return randomValue / 100;
	}

	public List<GridItem> LoadPremadeGrid(int gridNumber) {
		List<GridItem> map = new List<GridItem> ();
		return map;
	}

}

public class GridRow {
	public List<float> points = new List<float> ();
}

public class GridItem {
	public Vector3 scale;
	public Vector3 position;
	public Vector2 coords;
	public string type;
}
	
public class PathfindingUnit {
	public int f;
	public int g;
	public int h;
	public Node parent;
	public Node current;
}

[System.Serializable]
public class Grid
{
	public List<GridItem> gridPoints;
	public GridManager.Size mapSize;
}
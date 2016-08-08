using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

	public UnitManager unitManager;
	public Node cube;
	public Material[] materials;
	public Material selectedMaterial;
	public Vector3 initialPosition;
	public enum Size
	{
		small, medium, large
	};
	public Size mapSize;

	private Node[] grid;
	private float sideLength = 1.2f;
	private int xSize;
	private int zSize;
	private float scale = 0.025f;
	private float unitScale;

	// Use this for initialization
	void Start () {
		GenerateGrid ();
		unitManager.createUnit (grid[0], unitScale);
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

	// Update is called once per frame
	void Update () {
	
	}

	public Node findNodeByCord(int x, int z) {
		int index = ((z - 1) * xSize) + x;
		if (index < grid.Length) {
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

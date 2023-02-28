using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BuildingSystem : MonoBehaviour {
	public static BuildingSystem current;

	public GridLayout gridLayout;

	private Grid grid;

	[SerializeField] Tilemap mainTilemap;
	[SerializeField] TileBase whiteTile;

	public GameObject prefab1;
	public GameObject prefab2;

	private PlaceableObject objectToPlace;
	

    #region Unity Methods

    private void Awake()
    {
		current = this;
		if (gridLayout != null)
        {
			grid = gridLayout.gameObject.GetComponent<Grid>();
		} else
        {
			Debug.Log("GridLayout missing.");
        }
		
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
			InitializeWithObject(prefab1);
        } else if (Input.GetKeyDown(KeyCode.B))
		{
			InitializeWithObject(prefab2);
		}

		if (!objectToPlace)
        {
			return;
        }
		if (Input.GetKeyDown(KeyCode.Return))
        {
			objectToPlace.Rotate();
        }
		else if (Input.GetKeyDown(KeyCode.Space))
        {
			if (CanBePlaced(objectToPlace))
            {
				objectToPlace.Place();
				Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
				TakeArea(start, objectToPlace.Size);
            } else
            {
				Destroy(objectToPlace.gameObject);
            }
        } else if (Input.GetKeyDown(KeyCode.Escape))
        {
			Destroy(objectToPlace.gameObject);
        }
	}
    #endregion

    #region Utils
    public static Vector3 GetMouseWorldPosition()
    {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
			return raycastHit.point;
        } else
        {
			return Vector3.zero;
        }
    }
	public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
		Vector3Int cellPos = gridLayout.WorldToCell(position);
		position = grid.GetCellCenterWorld(cellPos);
		return position;
    }
	
	public static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
		TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.y];

		int counter = 0;
		foreach (var v in area.allPositionsWithin)
        {
			Vector3Int pos = new Vector3Int(v.x, v.y, 0);
			array[counter] = tilemap.GetTile(pos);
			counter++;
        }
		return array;
    }
	#endregion

	#region Building Placement

	public void InitializeWithObject(GameObject prefab)
	{
		Vector3 position = SnapCoordinateToGrid(Vector3.zero);

		GameObject obj = Instantiate(prefab, position, Quaternion.identity);
		objectToPlace = obj.GetComponent<PlaceableObject>();
		obj.AddComponent<ObjectDrag>();

	}

	private bool CanBePlaced(PlaceableObject placeableObject)
    {
		BoundsInt area = new BoundsInt();
		area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
		area.size = placeableObject.Size;

		TileBase[] baseArray = GetTilesBlock(area, mainTilemap);

		foreach (var b in baseArray)
		{
			if (b == whiteTile)
			{
				return false;
			}
			
		}
		return true;
	}

	public void TakeArea(Vector3Int start, Vector3Int size)
    {
		mainTilemap.BoxFill(start, whiteTile, start.x, start.y, 
							start.x + size.x, start.y + size.y);

    }
	
	
	#endregion
}

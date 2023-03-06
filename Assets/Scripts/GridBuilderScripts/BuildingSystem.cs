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
	[SerializeField] TileBase highlighterTile;

	Vector3Int previousCell = Vector3Int.zero;
	bool highlighting = false;

	private GameManager gameManager;

	private PlaceableObject objectToPlace;

    private void OnEnable()
    {
		ItemDropHandler.objectDropEvent += OnDrop;
    }
    private void OnDisable()
    {
		ItemDropHandler.objectDropEvent -= OnDrop;
	}


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
    private void Start()
    {
		gameManager = GameManager.instance;
    }

    private void LateUpdate()
    {
		if (highlighting)
		{
			Vector3Int currentCell = gridLayout.WorldToCell(GetMouseWorldPosition());
			if (currentCell != previousCell)
            {
				mainTilemap.SetTile(currentCell, highlighterTile);
				mainTilemap.SetTile(previousCell, null);
				previousCell = currentCell;
            }
			
		}
		
	}
    private void Update()
    {
		
		//      if (Input.GetKeyDown(KeyCode.A))
		//      {
		//	InitializeWithObject(prefab1);
		//      } else if (Input.GetKeyDown(KeyCode.B))
		//{
		//	InitializeWithObject(prefab2);
		//}

		//if (!objectToPlace)
		//      {
		//	return;
		//      }
		//if (Input.GetKeyDown(KeyCode.Return))
		//      {
		//	objectToPlace.Rotate();
		//      }
		//else if (Input.GetKeyDown(KeyCode.Space))
		//      {
		//	if (CanBePlaced(objectToPlace))
		//          {
		//		objectToPlace.Place();
		//		Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
		//		TakeArea(start, objectToPlace.Size);
		//          } else
		//          {
		//		Destroy(objectToPlace.gameObject);
		//          }
		//      } else if (Input.GetKeyDown(KeyCode.Escape))
		//      {
		//	Destroy(objectToPlace.gameObject);
		//      }
	}
    #endregion

    #region Utils
    public static Vector3 GetMouseWorldPosition()
    {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 100f);
		foreach(RaycastHit hit in hits)
        {
			if (hit.collider.gameObject.CompareTag("Ground"))
			{
				return hit.point;
				
			}
			Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
		}
		return Vector3.zero;
		
		//if (hits.Length == 1)
  //      {
			
		//	return hits[0].point;
  //      } else
  //      {
		//	return Vector3.zero;
  //      }
    }
	public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
		Vector3Int cellPos = gridLayout.WorldToCell(position);
		position = grid.GetCellCenterWorld(cellPos);
		return position;
		
    }

    public void DrawHighlightTile(Vector3 position)
    {

		//highlighterPos = gridLayout.WorldToCell(position);
		//Debug.Log(highlighterPos);
		highlighting = true;
    }
	public void EraseHighlightTile()
    {
		highlighting = false;
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

	public void InitializeWithObject(GameObject prefab, Vector3 spawnPosition)
	{
		Vector3 position = SnapCoordinateToGrid(spawnPosition);

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
	
	public void OnDrop(int objectID)
    {
		PlacableObjectSO[] objectList = gameManager.levelData.objectList;

		foreach(PlacableObjectSO obj in objectList) {
			if (obj.objectID == objectID)
            {
				InitializeWithObject(obj.objectPrefab, GetMouseWorldPosition());
				break;
            }
        }
    }
	
	#endregion
}

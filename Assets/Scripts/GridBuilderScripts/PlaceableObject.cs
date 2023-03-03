using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaceableObject : MonoBehaviour {
	public bool Placed { get; private set; }
	public Vector3Int Size { get; private set; }

	private Vector3[] Vertices;

	private void GetColliderVertexPositionsLocal()
    {
		BoxCollider col = gameObject.GetComponent<BoxCollider>();

		Vertices = new Vector3[4];
		Vertices[0] = col.center + new Vector3(-col.size.x, -col.size.y, -col.size.z) * 0.5f;
		Vertices[1] = col.center + new Vector3(col.size.x, -col.size.y, -col.size.z) * 0.5f;
		Vertices[2] = col.center + new Vector3(col.size.x, -col.size.y, col.size.z) * 0.5f;
		Vertices[3] = col.center + new Vector3(-col.size.x, -col.size.y, col.size.z) * 0.5f;
	}
	private void CalculateSizeInCells()
    {
		Vector3Int[] vertices = new Vector3Int[Vertices.Length];

		for (int i = 0; i < vertices.Length; ++i)
        {
			Vector3 worldPosition = transform.TransformPoint(Vertices[i]);
			vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPosition);

        }

		Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x), 
										Math.Abs((vertices[0] - vertices[3]).y), 1);
    }

	public Vector3 GetStartPosition()
    {
		return transform.TransformPoint(Vertices[0]);
    }

    private void Start()
    {
		GetColliderVertexPositionsLocal();
		CalculateSizeInCells();

    }

	public virtual void Place()
    {
		ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
		Destroy(drag);
		Placed = true;

		//	Invoke events of placement

    }

	public void Rotate()
    {
		transform.Rotate(new Vector3(0, 45, 0));
		Size = new Vector3Int(Size.y, Size.x, 1);

		Vector3[] vertices = new Vector3[Vertices.Length];
		for (int i = 0; i < vertices.Length; ++i)
        {
			vertices[i] = Vertices[(i + 1) % vertices.Length];

        }
		Vertices = vertices;
    }
}

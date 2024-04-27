using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MovementGrid : MonoBehaviour
{
    GridPoints[,] totalGrid;

    [SerializeField] int width = 25;

    [SerializeField] int length = 25;

	//[SerializeField] int height = 2;

	[SerializeField] float gridPointSize = 1.0f;

	[SerializeField] LayerMask obstructionLayer;
	[SerializeField] LayerMask terrainLayer;
	

	private void Awake()
	{
		CreateGrid();
	}

	private void CheckForAvailableNodes()
	{
		for (int y = 0; y < width; y++)
		{
			for (int x = 0; x < length; x++)
			{
				Vector3 worldPosition = GetWorldPosition(x, y);
				bool passable = !Physics.CheckBox(worldPosition, Vector3.one / 2 * gridPointSize, Quaternion.identity, obstructionLayer);
				totalGrid[x, y].availablePoint = passable;
			}
		}
	}

	private void CreateGrid()
	{
		totalGrid = new GridPoints[length, width];

		for(int y = 0; y < width; y++)
		{
			for(int x = 0;x < length; x++)
			{
				totalGrid[x, y] = new GridPoints();
			}
		}

		CalculateElevation();
		CheckForAvailableNodes();
	}

	private void CalculateElevation()
	{
		for (int y = 0; y < width; y++)
		{
			for (int x = 0; x < length; x++)
			{
				Ray ray = new Ray(GetWorldPosition(x,y) + Vector3.up * 100f, Vector3.down);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, float.MaxValue, terrainLayer))
				{
					totalGrid[x, y].elevation = hit.point.y;
				}
			}
		}
	}

	public Vector2Int GetGridPosition(Vector3 worldPosition)
	{
		worldPosition -= transform.position;
		Vector2Int positionOnGrid = new Vector2Int((int)(worldPosition.x / gridPointSize), (int)(worldPosition.z / gridPointSize));

		return positionOnGrid;
	}

	private void OnDrawGizmos()
	{
		if (totalGrid == null)
		{
			for(int y = 0; y < width; y++) 
			{ 
				for (int x = 0; x < length; x++)
				{
					Vector3 pos = GetWorldPosition(x, y);
					Gizmos.DrawCube(pos, Vector3.one / 4);
				}
			}

		} else
		{
			for (int y = 0; y < width; y++)
			{
				for (int x = 0; x < length; x++)
				{
					Vector3 pos = GetWorldPosition(x, y, true);
					Gizmos.color = totalGrid[x, y].availablePoint ? Color.white : Color.red;
					Gizmos.DrawCube(pos, Vector3.one / 4);
				}
			}
		}
	}

	private Vector3 GetWorldPosition(int x, int y, bool elevation = false)
	{
		return new Vector3(transform.position.x + (x * gridPointSize), elevation== true ? totalGrid[x, y].elevation : 0f, transform.position.z + (y * gridPointSize));
	}

	internal void PlaceObject(Vector2Int positionOnGrid, GridEntity gridEntity)
	{
		if (BoundaryCheck(positionOnGrid) == true)
		{
			totalGrid[positionOnGrid.x, positionOnGrid.y].gridEntity = gridEntity;
		}
		else
		{
			Debug.Log("You are trying to get out of bounds!");
		}
		totalGrid[positionOnGrid.x, positionOnGrid.y].gridEntity = gridEntity;
	}

	internal GridEntity GetPlacedObject(Vector2Int gridPosition)
	{
		if (BoundaryCheck(gridPosition) == true)
		{

			GridEntity gridEntity = totalGrid[gridPosition.x, gridPosition.y].gridEntity;
			return gridEntity;
		}
		return null;
	}

	public bool BoundaryCheck(Vector2Int positionOnGrid)
	{
		if (positionOnGrid.x < 0 || positionOnGrid.x >= length)
		{
			return false;
		}
		if (positionOnGrid.y < 0 || positionOnGrid.y >= length)
		{
			return false;
		}
		return true;
	}
}

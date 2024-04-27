using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    [SerializeField] MovementGrid targetGrid;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	private void Initialize()
	{
        Vector2Int positionOnGrid = targetGrid.GetGridPosition(transform.position);
        targetGrid.PlaceObject(positionOnGrid, this);
	}
}

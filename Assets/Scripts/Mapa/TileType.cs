using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileType {
  public string name;
  public GameObject tileVisualPrefab;
  public bool isWalkable = true;
  public bool isWalkableU = true;
  public bool occupied = false;
  public bool isMobile = false;
  public float movementCost = 1;
}

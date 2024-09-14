using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Albañil{
  public int posicionX;
  public int posicionY;
  public static List<Vector3> listaDePosiblesObstaculos = new List<Vector3>{
    new Vector3(-1,0, 2),
    new Vector3( 1,0, 2),
    new Vector3(-1,0,-2),
    new Vector3( 1,0,-2),
    new Vector3(-2,0,-1),
    new Vector3(-2,0, 1),
    new Vector3( 2,0,-1),
    new Vector3( 2,0, 1)
  };

  public Albañil(int x, int y){
    this.posicionX = x;
    this.posicionY = y;
  }


}

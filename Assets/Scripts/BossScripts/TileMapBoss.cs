using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Documenta tu codigo animal.
 */
public class TileMapBoss : MonoBehaviour{

    public TileTypeBoss[] tileTypes;
    public int[,] tiles;
    Node[,] graph;
    public int mapSizeX = 9;
    public int mapSizeY = 9;

    void Start(){
        GenerateMapData();
        GenerateMapVisual();
    }

    void GenerateMapData(){
      //localizar los cuadros del mapar
      tiles = new int[mapSizeX, mapSizeY];
      int x,y;
      // Inicializar los cuadros
      for (x = 0; x < mapSizeX; x++){
        for (y = 0; y < mapSizeY; y++){
          tiles[x, y] = 0;
        }
      }

      //tiles[4,4] = 1;
      //tiles[3,4] = 1;
      //tiles[5,4] = 1;
      //tiles[4,5] = 1;
      //tiles[4,3] = 1;

    }

    void GenerateMapVisual(){
      for (int x = 0; x < mapSizeX; x++){
        for (int y = 0; y < mapSizeY; y++){
          TileTypeBoss tt = tileTypes[tiles[x, y]];
          GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);
        }
      }
    }

    public void updateTerreno(){
      GameObject[] enemies = GameObject.FindGameObjectsWithTag("Terreno");
      foreach(GameObject enemy in enemies){
        GameObject.Destroy(enemy);
      }
      GenerateMapVisual();
    }

    public bool canIWalkOnIt(int x, int y){
      if ((x > mapSizeX - 1 || x < 0) || (y > mapSizeY - 1 || y < 0)){
          return false;
      }
      return tileTypes[tiles[x,y]].isWalkable;
    }

    public Vector3 TileVector3(int x, int y){
        return new Vector3(x, y, 0);
    }

    public bool isDamageTile(int tileX , int tileY){
      if(tileTypes[tiles[tileX,tileY]].DamageTile){
        Debug.Log("Me lastimo el Tile");
        return true;
      }
      return false;
    }

    public Vector3 safeTile(int tileX , int tileY){
      PosibleTiles t = new PosibleTiles(TileVector3(tileX,tileY));
      foreach(Vector3 i in PosibleTiles.listaDePosiblesTiles){
        Vector3 nuevo = t.posicion + i;
        if ((nuevo.x > mapSizeX - 1 || nuevo.x < 0) || (nuevo.y > mapSizeY - 1 || nuevo.y < 0)){
          continue;
        }else if(tileTypes[tiles[(int)nuevo.x,(int)nuevo.y]].DamageTile){
          continue;
        }else{
          return nuevo;
        }
      }
      return t.posicion;
    }

    private class PosibleTiles{
      public Vector3 posicion;
      public static List<Vector3> listaDePosiblesTiles = new List<Vector3>{
        new Vector3(-1,0, -1.28f),
        new Vector3( 1,0, -1.28f),
        new Vector3( 0,1, -1.28f),
        new Vector3( 0,-1,-1.28f),
        new Vector3(-1,1, -1.28f),
        new Vector3(-1,-1,-1.28f),
        new Vector3( 1,1, -1.28f),
        new Vector3( 1,-1,-1.28f)
      };

      public PosibleTiles(Vector3 vect){
        this.posicion = vect;
      }
    }
}

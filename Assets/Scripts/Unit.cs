using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
  public int tileX; //Posicion X actual
  public int tileY;//Posicion Y actual
  public int tileXHome;//Posicion X de donde Spawnearon
  public int tileYHome;//Posicion Y de donde Spawnearon
  public TileMap map;// Referencia al mapa
  public GameObject target;//Referencia al player
  public TurnController controlador;//Referencia al controlador de turnoss


  //Inicializa el camino actual de la unidad.
  //Se usa en el algortimo GeneratePathTo en TileMap.
  public List<Node> currentPath = null;
  public bool pathFound = true;

  //VELOCIDAD DE LA ANIMACION DE MOVIMIENTO
  int speed = 5;

  // POR AHORA SE INICIALIZA EN 7 PARA QUE NO MARQUE ERROR
  public float remainingMovement;
  public float maxMovement;
  public bool unitTurn;// Si es el turno del unit
  public bool fusione = false;
  public bool scared = false;
  public int damage;

    void Update(){
        //Debug.Log (remainingMovement);
        if(unitTurn){
          if (currentPath != null) {
          int currentNode = 0;
          //Con motivos de Debug, vamos a mostrar el trayecto mediante una linea.
          while(currentNode < currentPath.Count - 1){//Terminamos 1 antes ya que revisamos el nodo actual y el siguiente.
            Vector3 start = map.TileCoordToWorldCoord(currentPath[currentNode].x , currentPath[currentNode].y)
              + new Vector3(0,0,-1f);//A�adimos esto para que la linea se muestre sobre el mapa.
            Vector3 end = map.TileCoordToWorldCoord(currentPath[currentNode+1].x, currentPath[currentNode+1].y) // Revisamos el siguiente nodo
              + new Vector3(0, 0, -1f);
            Debug.DrawLine(start,end,Color.red);
            currentNode++;
          }
            //Checamos si la animacion termino para seguir moviendonos
            if(transform.position == map.TileCoordToWorldCoord(tileX, tileY) ){
              //Debug.Log ("Ya llegue");
              MoveNextTile();
            }
          }
        }
        transform.position = Vector3.MoveTowards(transform.position, map.TileCoordToWorldCoord(tileX, tileY), Time.deltaTime * speed);
    }

    public void MoveNextTile(){
      if(remainingMovement > 0){
        float tmp = map.CostToEnterTile(currentPath[0].x,currentPath[0].y,currentPath[1].x,currentPath[1].y);
        bool mobile = map.canIMoveIt(currentPath[1].x,currentPath[1].y);
        if ( (remainingMovement - tmp) < 0){
          remainingMovement = 0;
          return;
        }else{
          if(mobile){
            map.kickObstacleU(currentPath[1].x,currentPath[1].y);
          }
          remainingMovement -= map.CostToEnterTile(currentPath[0].x,currentPath[0].y,currentPath[1].x,currentPath[1].y);
          map.iLeft(tileX,tileY,currentPath[1].x,currentPath[1].y);
          tileX = currentPath[1].x;
          tileY = currentPath[1].y;
          //Debug.Log ("Me movi");
          currentPath.RemoveAt(0);
          if(currentPath.Count == 1){
            currentPath = null;
          }
        }
      }
    }
}

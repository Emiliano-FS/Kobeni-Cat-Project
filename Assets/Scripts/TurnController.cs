using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{

    public List<GameObject> unitChicos = new List<GameObject>();
    public List<GameObject> unitMedianos = new List<GameObject>();
    public List<GameObject> unitGrandes = new List<GameObject>();
    public List<GameObject> unitsActivos = new List<GameObject>();
    public int currentTurn;//Turno Actual
    bool playerTurn;//True=Turno del jugador, false= turno del enemigo
    public Player playerObject;//Referencia de Player
    public TileMap map;

    //----------Cosas de la UI----------
    public Text turnCounter;//Texto de cuantos pasos nos queda
    public Image turnSwitch;//Imagen que indica que turno es.
    public Sprite pTurn;
    public Sprite eTurn;
    public Text turnIndicator;//Texto que nos dice cuantos turnos han pasado

    // Start is called before the first frame update
    void Start(){
        currentTurn = 1;

        //Inicializamos el jugador
        playerObject.tileX = (int)playerObject.transform.position.x;
        playerObject.tileY = (int)playerObject.transform.position.y;
        playerObject.map = this.map;


        //Tenemos que desactivar a todas las units por AHORA
        foreach(GameObject unidad in unitChicos){
          unidad.SetActive(false);
        }

        foreach(GameObject unidad in unitMedianos){
          unidad.SetActive(false);
        }

        foreach(GameObject unidad in unitGrandes){
          unidad.SetActive(false);
        }

        //Inicializamos la lista de Enemigos
      }


    // Update is called once per frame
    void Update(){
        //Muestra el numero de turnos que le quedan AL JUGADOR.
        turnIndicator.text = "Ronda " + currentTurn;
        turnCounter.text = playerObject.movesLeft.ToString();
        //Motivos de Debug, Reincia de forma automatica el turno del jugador dandole 8 turnos.
        //if (Input.GetKey("q")){
        //    Debug.Log("Reinciando las moves del jugador");
        //    playerObject.movesLeft = 8;
        //}

        if (playerObject.playerTurn == true){
            turnSwitchUI(true);
            if(playerObject.movesLeft <= 0){
              Debug.Log("Oh ow, se acabo el turno del jugador");
              playerObject.playerTurn = false;
              snpcSpawner();
              foreach(GameObject unit in unitsActivos){
                unit.GetComponent<Unit>().remainingMovement = unit.GetComponent<Unit>().maxMovement;
              }
            }
        }else{
            //SoundController.PlaySound("turnChange");
            Debug.Log("Es turno de los Units");
            turnSwitchUI(false);
            bool flag = false;
            foreach(GameObject unit in unitsActivos){
              if (unit.GetComponent<Unit>().remainingMovement <= 0 || !(unit.GetComponent<Unit>().pathFound)){
                continue;
              }else{
                flag = true;
                unit.GetComponent<Unit>().unitTurn = true;
              }
            }
            if(!flag){
              Debug.Log("Se acabo el turno de las Units");
              foreach(GameObject unit in unitsActivos){
                unit.GetComponent<Unit>().unitTurn = false;
              }
              playerObject.playerTurn = true;
              playerObject.movesLeft = 5;
              playerObject.rageMode = false;
              playerObject.rageModeController();
              playerObject.killer = false;
              currentTurn++;
            }
        }
    }

    public void snpcSpawner(){
      Debug.Log("Entre a crear unidades");
      if(currentTurn % 2 == 0) return;
      int snpcX = whereisPlayer();
      int dangerZone = howDangerous();
      monsterSpawner(snpcX,dangerZone);
    }


    public int whereisPlayer(){
      int playersX = playerObject.tileX;
      if(playersX <= 3) return 0;
      if(playersX <= 9) return 6;
      if(playersX <= 15) return 12;
      if(playersX <= 21) return 18;
      if(playersX <= 27) return 24;
      if(playersX <= 33) return 30;
      if(playersX <= 39) return 36;
      return 0;
    }

    public int howDangerous(){
      if(currentTurn <= 7) return 1;
      if(currentTurn <= 11) return 2;
      if(currentTurn > 14) return 3;
      return 0;
    }

    public void monsterSpawner(int snpcX, int dangerZone){
      List<GameObject> monsterList = new List<GameObject>();
      switch(dangerZone){
        case 1:
        monsterList = unitChicos;
        break;
        case 2:
        monsterList = unitMedianos;
        break;
        case 3:
        monsterList = unitGrandes;
        break;
      }
      Debug.Log("Cree Unidades");
      spawnUnit((float)snpcX,0f,monsterList);
      spawnUnit((float)snpcX,6f,monsterList);
    }

    public void spawnUnit(float snpcX , float snpcY, List<GameObject> list){
      GameObject a = list[0];
      unitsActivos.Add(a);
      a.transform.position = new Vector3(snpcX,snpcY, 0f);
      a.GetComponent<Unit>().tileX = (int)a.transform.position.x;
      a.GetComponent<Unit>().tileY = (int)a.transform.position.y;
      a.GetComponent<Unit>().tileXHome = (int)a.transform.position.x;
      a.GetComponent<Unit>().tileYHome = (int)a.transform.position.y;
      map.iLeft((int)a.transform.position.x,(int)a.transform.position.y,(int)a.transform.position.x,(int)a.transform.position.y);
      a.GetComponent<Unit>().map = this.map;
      a.SetActive(true);
      list.Remove(a);
    }

    //Si es el turno del jugador pon true.
    //Equis de no se me ocurrio otro nombre :^)
    public void turnSwitchUI(bool milhouse){
        if (milhouse) { turnSwitch.sprite = pTurn;}
        else { turnSwitch.sprite = eTurn; }
    }

}

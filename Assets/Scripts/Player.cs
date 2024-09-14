using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase que controla al jugador. Contiene su posicion X,y y una referencia al mapa.
public class Player : MonoBehaviour
{
    //------Manejo de la Grafica--------
    public int tileX; //Posicion X actual
    public int tileY;//Posicion Y actual
    public TileMap map;// Referencia al mapa

    //-----Elementos del Gameplay------
    public bool rageMode;//True= Power Up // False=Modo Normal
    public bool killer;//Ha matado a un NPC
    public int movesLeft; // Numero de movimientos que le quedan al personaje
    public bool playerTurn;// Si es el turno del jugador
    public bool inCutscene;//Si se esta desplegando una cutscene.
    public GameOver gameover;//Referencia a la pantalla de Game Over.

    //-------Elementos del Movimiento---------
    public float timer;// No quiero usar corrutinas lol
    Vector3 target ; // Esto es para moverlo con animacion
    int speed = 10; // Velocidad con la que avanza la animacion
    public bool stop = true;

    //----------Controladores de la barra de Vida-------
    public int maxHealth = 30;
    public int currentHealth;
    public HealthBar healthbar;//Referencia al objeto HealthBar

    //-------Controladores de Textura----
    public Material normal;
    public Material mad;
    public Material backdash;
    public Material fwddash;
    public Material Madbackdash;
    public Material Madfwddash;
    public Material MadKick;
    public Material kick;
    public Material smokeball1;
    public Material smokeball2;
    public Material smokeball3;

    // Start is called before the first frame update
    void Start()
    {
        rageMode = false;//Inicia en modo normal
        movesLeft = 5;
        playerTurn = true;
        timer = 0;
        target = transform.position;
        currentHealth = maxHealth;
        healthbar.setMaxHealth(maxHealth);
        inCutscene = false;
    }

    // Update is called once per frame
    //Mueve al personaje una casilla y actualiza su posicion
    void Update(){
        if (currentHealth <= 0)
        {
            movesLeft = 0;
            gameover.TriggerGameOver();
            Destroy(this);
        }
        //if (Input.GetKey("z")) { takeDamage(1); }
        if (playerTurn){
            // if (movesLeft == 0){ movesLeft = 8; }// Si ya es su turno y sus moves=0, le damos nuevas moves
            timer += Time.deltaTime;
            if (timer >= 0.3f && movesLeft > 0 && !inCutscene){// Pa que no vueles por el mapa, solo podras actuar cada 0.3 segundos.
                //Debug.Log("El jugador tiene "+ movesLeft+" pasos restantes");
                if (Input.GetKey("p")){
                    rageMode = !rageMode;
                    rageModeController();
                    timer = 0f;
                }
                if (Input.GetKey("w")){
                    //pos.z += speed * Time.deltaTime;
                    Vector3 tmp = transform.position + new Vector3(0, 1f, 0);
                    if (map.canIWalkOnIt((int)tmp.x,(int)tmp.y)){
                        timer = 0f;
                        StartCoroutine(spriteController(1));
                      target = tmp;
                      stop = false;
                      //Esta funcion que esta en TileMap hace que marque los tiles en donde esten para que se vea cuales estan ocupados
                      map.iLeft(tileX,tileY,tileX,tileY+1);
                      tileY++;
                      // Por motivos de Playtest, nunca baja los turnos, cuando quieras probar chido quita el comentario.
                      movesLeft--;
                    }else if(map.canIMoveIt((int)tmp.x,(int)tmp.y)){
                      Debug.Log("Entre a patear");
                        StartCoroutine(spriteController(3));
                        if (map.kickObstacle((int)tmp.x,(int)tmp.y, 0 , 1,this)) movesLeft --;
                      timer = 0f;
                    }else{
                      timer = 0f;
                    }

                }

                if (Input.GetKey("s")){
                    Vector3 tmp = transform.position + new Vector3(0, -1f, 0);
                    if (map.canIWalkOnIt((int)tmp.x,(int)tmp.y)){
                        timer = 0f;
                        StartCoroutine(spriteController(2));
                      target = tmp;
                      stop = false;
                      //Esta funcion que esta en TileMap hace que marque los tiles en donde esten para que se vea cuales estan ocupados
                      map.iLeft(tileX,tileY,tileX,tileY-1);
                      tileY --;
                      // Por motivos de Playtest, nunca baja los turnos, cuando quieras probar chido quita el comentario.
                      movesLeft--;
                    }else if(map.canIMoveIt((int)tmp.x,(int)tmp.y)){
                      Debug.Log("Entre a patear");
                        StartCoroutine(spriteController(3));
                        if (map.kickObstacle((int)tmp.x,(int)tmp.y, 0 , -1,this)) movesLeft --;
                      timer = 0f;
                    }else{
                      timer = 0f;
                    }

                }
                if (Input.GetKey("d")){
                    Vector3 tmp = transform.position + new Vector3(1f, 0, 0);
                    if (map.canIWalkOnIt((int)tmp.x,(int)tmp.y)){
                        timer = 0f;
                        StartCoroutine(spriteController(2));
                      target = tmp;
                      stop = false;
                      //Esta funcion que esta en TileMap hace que marque los tiles en donde esten para que se vea cuales estan ocupados
                      map.iLeft(tileX,tileY,tileX+1,tileY);
                      tileX ++;
                      // Por motivos de Playtest, nunca baja los turnos, cuando quieras probar chido quita el comentario.
                      movesLeft--;
                    }else if(map.canIMoveIt((int)tmp.x,(int)tmp.y)){
                      Debug.Log("Entre a patear");
                        StartCoroutine(spriteController(3));
                        if (map.kickObstacle((int)tmp.x,(int)tmp.y, 1 , 0,this)) movesLeft --;
                      timer = 0f;
                    }else{
                      timer = 0f;
                    }

                }
                if (Input.GetKey("a")){
                    Vector3 tmp = transform.position + new Vector3(-1f, 0, 0);
                    if (map.canIWalkOnIt((int)tmp.x,(int)tmp.y)){
                        timer = 0f;
                        StartCoroutine( spriteController(1));
                      target = tmp;
                      stop = false;
                      //Esta funcion que esta en TileMap hace que marque los tiles en donde esten para que se vea cuales estan ocupados
                      map.iLeft(tileX,tileY,tileX-1,tileY);
                      tileX--;
                      // Por motivos de Playtest, nunca baja los turnos, cuando quieras probar chido quita el comentario.
                      movesLeft--;
                    }else if(map.canIMoveIt((int)tmp.x,(int)tmp.y)){
                      Debug.Log("Entre a patear");
                        StartCoroutine(spriteController(3));
                        if (map.kickObstacle((int)tmp.x,(int)tmp.y, -1 , 0,this)) movesLeft --;
                      timer = 0f;
                    }else{
                      timer = 0f;
                    }
                }
            }
        }
        if(transform.position != target && !stop){
          transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        }else{
          stop = true;
        }
    }

    //Controlador del Power Up, por lo mientras lo mapeo a la tecla P
    //Cambia el color de material (por ahora)
    public void rageModeController(){
        //player.GetComponent<Player>();
        var playerColor = this.GetComponent<Renderer>();
        if (!rageMode){//Si tienes Power up
            //Call SetColor using the shader property name "_Color" and setting the color to red
            playerColor.material = normal;
        } else {// Si no tienes Powerup
            playerColor.material = mad;
        }

    }

    // Disminuye el currentHP y actualiza la vista de la Health Bar.
    public void takeDamage(int damage){
        currentHealth -= damage;
        healthbar.setHealth(currentHealth);
        StartCoroutine(spriteController(9));
    }

    //Cambia el sprite segun los datos que se pasen.
    //1: Izquierda o Arriba. 2: Derecha o Abajo
    //9 es la animacion de la smokeball
    IEnumerator spriteController(int input){
        var playerColor = this.GetComponent<Renderer>();
        if (rageMode){
            switch (input)
            {
                case 1:
                    Debug.Log("Entrando a Backdash");
                    playerColor.material = Madbackdash;
                    SoundController.PlaySound("dash");
                    transform.localScale += new Vector3(0.4f, -0.3f, 0f);
                    yield return new WaitForSeconds(0.2f);
                    transform.localScale -= new Vector3(0.4f, -0.3f, 0f);
                    playerColor.material = mad;
                    break;
                case 2:
                    Debug.Log("Entrando a FwdDash");
                    playerColor.material = Madfwddash;
                    SoundController.PlaySound("dash");
                    //transform.localScale += new Vector3(0.4f, -0.3f, 0f);
                    yield return new WaitForSeconds(0.2f);
                    //transform.localScale -= new Vector3(0.4f, -0.3f, 0f);
                    playerColor.material = mad;
                    break;
                case 3:
                    Debug.Log("Entrando a Kick");
                    playerColor.material = MadKick;
                    transform.localScale += new Vector3(0.61f, 0.2f, 0f);
                    yield return new WaitForSeconds(0.2f);
                    transform.localScale -= new Vector3(0.61f, 0.2f, 0f);
                    playerColor.material = mad;
                    break;
                case 9:
                    Debug.Log("Entrando a Bola de Humo");
                    transform.localScale += new Vector3(0.62f, 0.0f, 0f);
                    SoundController.PlaySound("smokeball");
                    playerColor.material = smokeball1;
                    yield return new WaitForSeconds(0.2f);
                    //transform.localScale -= new Vector3(0.4f, -0.3f, 0f);
                    timer = 0;//Aseguramos que no nos podemos mover mientras esta esta animacion
                    playerColor.material = smokeball2;
                    yield return new WaitForSeconds(0.2f);
                    timer = 0;
                    playerColor.material = smokeball3;
                    yield return new WaitForSeconds(0.2f);
                    timer = 0;
                    playerColor.material = smokeball1;
                    yield return new WaitForSeconds(0.2f);
                    timer = 0;
                    playerColor.material = smokeball2;
                    yield return new WaitForSeconds(0.2f);
                    timer = 0;
                    playerColor.material = smokeball3;
                    yield return new WaitForSeconds(0.2f);
                    transform.localScale += new Vector3(-0.62f, 0.0f, 0f);
                    playerColor.material = mad;
                    break;
            }
        }
        else {
            switch (input){
                case 1:
                    Debug.Log("Entrando a Backdash");
                    playerColor.material = backdash;
                    SoundController.PlaySound("dash");
                    transform.localScale += new Vector3(0.4f, -0.3f, 0f);
                    yield return new WaitForSeconds(0.2f);
                    transform.localScale -= new Vector3(0.4f, -0.3f, 0f);
                    playerColor.material = normal;
                    break;
                case 2:
                    Debug.Log("Entrando a FwdDash");
                    playerColor.material = fwddash;
                    SoundController.PlaySound("dash");
                    //transform.localScale += new Vector3(0.4f, -0.3f, 0f);
                    yield return new WaitForSeconds(0.2f);
                    //transform.localScale -= new Vector3(0.4f, -0.3f, 0f);
                    playerColor.material = normal;
                    break;
                case 3:
                    Debug.Log("Entrando a Kick");
                    playerColor.material = kick;
                    transform.localScale += new Vector3(0.61f, 0.2f, 0f);
                    yield return new WaitForSeconds(0.2f);
                    transform.localScale -= new Vector3(0.61f, 0.2f, 0f);
                    playerColor.material = normal;
                    break;
                case 9:
                    Debug.Log("Entrando a Bola de Humo");
                    transform.localScale += new Vector3(0.62f, 0.0f, 0f);
                    SoundController.PlaySound("smokeball");
                    playerColor.material = smokeball1;
                    yield return new WaitForSeconds(0.2f);
                    //transform.localScale -= new Vector3(0.4f, -0.3f, 0f);
                    timer = 0;//Aseguramos que no nos podemos mover mientras esta esta animacion
                    playerColor.material = smokeball2;
                    yield return new WaitForSeconds(0.2f);
                    timer = 0;
                    playerColor.material = smokeball3;
                    yield return new WaitForSeconds(0.2f);
                    timer = 0;
                    playerColor.material = smokeball1;
                    yield return new WaitForSeconds(0.2f);
                    timer = 0;
                    playerColor.material = smokeball2;
                    yield return new WaitForSeconds(0.2f);
                    timer = 0;
                    playerColor.material = smokeball3;
                    yield return new WaitForSeconds(0.2f);
                    transform.localScale += new Vector3(-0.62f, 0.0f, 0f);
                    playerColor.material = normal;
                    break;
            }
        }

    }


}

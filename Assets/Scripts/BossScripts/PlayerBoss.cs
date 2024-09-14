using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoss : MonoBehaviour{
    //------Manejo de la Grafica--------
    public int tileX; //Posicion X actual
    public int tileY;//Posicion Y actual
    public TileMapBoss map;// Referencia al mapa


    //-----Elementos del Gameplay------
    public bool rageMode;//True= Power Up // False=Modo Normal
    public bool asFullMeter;//True, tiene 100% meter, False en otro caso.
    public bool killer;//Ha matado a un NPC
    public bool inCutscene;
    public GameOver gameover;//Referencia a la pantalla de Game Over.

    //-------Elementos del Movimiento---------
    float timer;// No quiero usar corrutinas lol
    Vector3 target ; // Esto es para moverlo con animacion
    int speed = 10; // Velocidad con la que avanza la animacion
    public bool stop = true;
    bool ouch = false;

    //----------Controladores de la barra de Vida-------
    public int maxHealth = 30;
    public int currentHealth;
    public HealthBar healthbar;//Referencia al objeto HealthBar
    public float maxRage= 20.0f;
    public float currentRage;
    public RageController rageMeter;
    public Image rageFull;

    //-------Controladores de Textura----
    public Material normal;
    public Material mad;
    public Material backdash;
    public Material fwddash;
    public Material Madbackdash;
    public Material Madfwddash;
    public Material smokeball1;
    public Material smokeball2;
    public Material smokeball3;
    public Material normalAttack;
    public Material madAttack;
    public Material hurt;
    //-----------Hitboxes---------------
    public GameObject attack;
    public GameObject bigAttack;


    // Start is called before the first frame update
    void Start()
      {
          rageMode = false;//Inicia en modo normal
          timer = 0;
          inCutscene = false;
          target = transform.position;
          currentHealth = maxHealth;
          healthbar.setMaxHealth(maxHealth);
        currentRage = 0f;
        rageMeter.setMaxRage(maxRage);
        rageMeter.setRage(0f);
        rageFull.enabled = false;

      }

    // Update is called once per frame
    //Mueve al personaje una casilla y actualiza su posicion
    void Update(){
        if (currentHealth <= 0)
        {
            gameover.TriggerGameOver();
            Destroy(this);
        }
        timer += Time.deltaTime;
      CheckMeter();
            if (timer >= 0.3f && !inCutscene){// Pa que no vueles por el mapa, solo podras actuar cada 0.3 segundos.
                //Debug.Log("El jugador tiene "+ movesLeft+" pasos restantes");
                if (Input.GetKey("p")){
                  if (asFullMeter) {
                      rageMode = true;
                      rageModeController(true);
                      asFullMeter = false;
                      currentRage -= 0.01f;
                      timer = 0f;
                  }

                }
                if (Input.GetKey("w")&& !ouch){
                    //pos.z += speed * Time.deltaTime;
                    Vector3 tmp = transform.position + new Vector3(0, 1f, 0);
                    if (map.canIWalkOnIt((int)tmp.x,(int)tmp.y)){
                      timer = 0f;
                      StartCoroutine(spriteController(1));
                      target = tmp;
                      stop = false;
                      tileY++;
                    }else{
                      timer = 0f;
                    }

                }

                if (Input.GetKey("s")&& !ouch){
                    Vector3 tmp = transform.position + new Vector3(0, -1f, 0);
                    if (map.canIWalkOnIt((int)tmp.x,(int)tmp.y)){
                  timer = 0f;
                  StartCoroutine(spriteController(2));
                      target = tmp;
                      stop = false;
                      tileY --;
                      // Por motivos de Playtest, nunca baja los turnos, cuando quieras probar chido quita el comentario.
                    }else{
                      timer = 0f;
                    }

                }
                if (Input.GetKey("d")&& !ouch){
                    Vector3 tmp = transform.position + new Vector3(1f, 0, 0);
                    if (map.canIWalkOnIt((int)tmp.x,(int)tmp.y)){
                  timer = 0f;
                  StartCoroutine(spriteController(2));
                      target = tmp;
                      stop = false;
                      tileX ++;
                      // Por motivos de Playtest, nunca baja los turnos, cuando quieras probar chido quita el comentario.
                    }else{
                      timer = 0f;
                    }

                }
                if (Input.GetKey("a")&& !ouch){
                    Vector3 tmp = transform.position + new Vector3(-1f, 0, 0);
                    if (map.canIWalkOnIt((int)tmp.x,(int)tmp.y)){
                  timer = 0f;
                  StartCoroutine( spriteController(1));
                      target = tmp;
                      stop = false;
                      tileX--;
                      // Por motivos de Playtest, nunca baja los turnos, cuando quieras probar chido quita el comentario.
                    }else{
                      timer = 0f;
                    }
                }
          if (Input.GetKey("space"))
          {
              StartCoroutine(attackButton());
              timer = -0.6f; // Tiene un ligero cooldown.
          }
        }
        if(transform.position != target && !stop){
          transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        }else{
          stop = true;
          ouch = false;
          if(map.isDamageTile(tileX,tileY)){
            takeDamage(1);
            ouch = true;
            damageTileMover(tileX,tileY);
          }
        }
    }

    private void damageTileMover(int tileX,int tileY){
      Vector3 tileMover = map.safeTile(tileX,tileY);
      this.tileX = (int)tileMover.x;
      this.tileY = (int)tileMover.y;
      target = tileMover;
      stop = false;

    }

    //Controlador del Power Up, por lo mientras lo mapeo a la tecla P
    //Cambia el color de material (por ahora)
    public void rageModeController(bool inRage){
        var playerColor = this.GetComponent<Renderer>();
        if (!inRage){//Si tienes Power up
            playerColor.material = normal;
        } else {// Si no tienes Powerup
            playerColor.material = mad;
        }

    }

    // Disminuye el currentHP y actualiza la vista de la Health Bar.
    public void takeDamage(int damage){
      currentHealth -= damage;
      currentRage += damage;
      healthbar.setHealth(currentHealth);
      rageMeter.setRage(currentRage);
      StartCoroutine(spriteController(9));
      StartCoroutine(invulFrames());
    }

    //Checa todo lo relacionado al Meter. Se hace cada Update.
    public void CheckMeter()
    {
        if(currentRage > maxRage) { 
            currentRage = maxRage;
        }
        if (currentRage == maxRage && !asFullMeter) {
            asFullMeter = true;
            rageFull.enabled = true;
            SoundController.PlaySound("ready");
        }
        if (rageMode)
        {
            rageFull.enabled = false;
            currentRage -= Time.deltaTime * 2.5f;
            rageMeter.setRage(currentRage);
            if (currentRage <= 0)
            {
                rageModeController(false);
                rageMode = false;
                currentRage = 0;
                asFullMeter = false;
            }
        }
    }

    //Genera una hitbox frente al personaje. La Hitbox Dura 0.3 segundos.
    IEnumerator attackButton()
    {
        StartCoroutine(spriteController(3));
        if (rageMode)
        {
            GameObject bigsword = Instantiate(bigAttack, new Vector3(this.transform.position.x + 1.4f, this.transform.position.y, -1.28f), Quaternion.Euler(0f, 0, 0));
            yield return new WaitForSeconds(0.5f);
            Destroy(bigsword);
        }
        else
        {
            GameObject sword = Instantiate(attack, new Vector3(this.transform.position.x + 1.05f, this.transform.position.y, -1.28f), Quaternion.Euler(0f, 0, 0));
            yield return new WaitForSeconds(0.5f);
            Destroy(sword);
        }
    }

    //Esconde la Hitbox del jugador por 1 segundo.
    IEnumerator invulFrames()
    {

        transform.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        transform.GetComponent<Collider>().enabled = true;
    }

    //Cambia el sprite segun los datos que se pasen.
    //1: Izquierda o Arriba. 2: Derecha o Abajo 3:Ataque
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
                    Debug.Log("Atacando");
                    playerColor.material = madAttack;
                    SoundController.PlaySound("bigSword");
                    transform.localScale += new Vector3(2.52f, 0.53f, 0f);
                    transform.position += new Vector3(0.25f, 0.0f, -0.15f);
                    yield return new WaitForSeconds(0.5f);
                    transform.localScale -= new Vector3(2.52f, 0.53f, 0f);
                    transform.position -= new Vector3(0.25f, 0.0f, -0.15f);
                    playerColor.material = mad;
                    break;
                case 9:
                    //transform.localScale += new Vector3(0.62f, 0.0f, 0f);
                    SoundController.PlaySound("hurt");
                    playerColor.material = hurt;
                    yield return new WaitForSeconds(0.2f);
                    //transform.localScale += new Vector3(-0.62f, 0.0f, 0f);
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
                    Debug.Log("Atacando");
                    playerColor.material = normalAttack;
                    SoundController.PlaySound("sword");
                    transform.localScale += new Vector3(1.5f, 0.5f, 0f);
                    transform.position += new Vector3(0.0f,0.25f,-0.25f);
                    yield return new WaitForSeconds(0.5f);
                    transform.localScale -= new Vector3(1.5f, 0.5f, 0f);
                    transform.position -= new Vector3(0.0f, 0.25f,-0.25f);
                    playerColor.material = normal;
                    break;
                case 9:
                    transform.localScale += new Vector3(0.13f, 0.1f, 0f);
                    SoundController.PlaySound("hurt");
                    SoundController.PlaySound("hit");
                    playerColor.material = hurt;
                    yield return new WaitForSeconds(0.2f);
                    transform.localScale -= new Vector3(0.13f, 0.1f, 0f);
                    playerColor.material = normal;
                    break;
              }
          }

      }


  }

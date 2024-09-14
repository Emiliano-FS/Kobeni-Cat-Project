using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ColisionadorNPC : MonoBehaviour {

    public DialogUIController controlador;//Controlador del dialogo
    public Player player;//Info del player
    public TurnController controladorTurnos;
    public Image ready;//Imagen que sale cuando puedes interactuar con algo.
    public Text efecto;
    bool canTalk;

    void Start()
    {
        efecto.enabled = false;
        canTalk = false;
        changeUI();
    }

    void OnTriggerEnter(Collider collider) {
        Debug.Log("Entre al colisionador del NPC");
        if (collider.gameObject.tag == "Player")
        {
            canTalk = true;
            changeUI();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            canTalk =false;
            changeUI();
        }
    }

    void changeUI()
    {
        ready.enabled = canTalk;
    }

    void Update()
    {
        if (canTalk)
        {

            if (Input.GetKey("e"))//talvez hay que cambiarlo
            {
                StartCoroutine(CollisionSequence());
                canTalk = false;
                changeUI();
            }
        }
    }

    private IEnumerator CollisionSequence(){
      controlador.StartDialogue(transform.parent.gameObject.GetComponent<NPCGenerico>().guion);
      yield return new WaitUntil(() => player.inCutscene == false);
      Action(transform.parent.gameObject.GetComponent<NPCGenerico>());
    }

    public void Action(NPCGenerico npc){
      int ruleta = Random.Range(0,10);
      if(ruleta < npc.probabilidad){
        buenaFortuna();
      }else{
        malaFortuna();
      }
    }

    public void buenaFortuna(){
      int ruleta = Random.Range(0,13);
      if(ruleta < 4){
        Debug.Log("The Chariot");
        StartCoroutine(cartEffect("The Chariot \n Que nada se interponga en tu camino \n Activa Blood Rage"));
        player.rageMode = true;
        player.movesLeft += 5;
        player.rageModeController();
      }else if (ruleta < 8){
        Debug.Log("Strength");
        StartCoroutine(cartEffect("Strength \n Que tu fuerza te de virtud \n Vida y Movilidad Incrementada"));
        if(player.currentHealth != player.maxHealth){
          player.currentHealth += (player.currentHealth / 2);
          player.healthbar.setHealth(player.currentHealth);
        }
        player.movesLeft += 5;
      }else if (ruleta < 11){
        Debug.Log("The Stars");
        StartCoroutine(cartEffect("The Stars \n Que encuentres lo que deseas \n Movilidad Incrementada"));
        player.movesLeft += 12;
      }else if (ruleta < 13){
        Debug.Log("The Lovers");
        StartCoroutine(cartEffect("The Lovers \n Que prosperes y tengas buena salud \n Vida Restaurada"));
        player.currentHealth = player.maxHealth;
        player.healthbar.setHealth(player.currentHealth);
      }else if (ruleta == 13){
        Debug.Log("The World");
        StartCoroutine(cartEffect("The World \n Abre tus ojos y ve \n Los turnos se reinician"));
        controladorTurnos.currentTurn = 1;
        controladorTurnos.turnIndicator.text = "Turno " + controladorTurnos.currentTurn;
      }
    }

    public void malaFortuna(){
      int ruleta = Random.Range(0,13);
      //int ruleta = 12;
      if(ruleta < 4){
        Debug.Log("The Fool");
        StartCoroutine(cartEffect("The Fool \n Donde comienza el viaje \n Regresas al inicio"));
        player.tileX = 0;
        player.tileY = 3;
        player.transform.position = new Vector3(0f,3f,-1.28f);
      }else if (ruleta < 8){
        Debug.Log("The Hermit");
        StartCoroutine(cartEffect("The Hermit \n La soledad es el comienzo \n Movilidad Reducida"));
        player.movesLeft -= player.movesLeft/2;
      }else if (ruleta < 11){
        Debug.Log("The Devil");
        StartCoroutine(cartEffect("The Devil \n Nunca debiste venir Aqui \n Vida Reducida"));
        player.currentHealth -= player.currentHealth/2;
        player.healthbar.setHealth(player.currentHealth);
      }else if (ruleta < 13){
        Debug.Log("The Emperor");
        StartCoroutine(cartEffect("The Emperor \n ¡Desafíame! \n Los Turnos se incrementan"));
        controladorTurnos.currentTurn += 9;
        controladorTurnos.turnIndicator.text = "Turno " + controladorTurnos.currentTurn;
      }else if (ruleta == 13){
        Debug.Log("Death");
        StartCoroutine(cartEffect("Death \n Ya no hay esperanza \n Vida Drenada"));
        player.currentHealth = 1;
        player.healthbar.setHealth(player.currentHealth);
      }
    }

    private IEnumerator cartEffect(string cart){
      efecto.text = cart;
      efecto.enabled = true;
      yield return new WaitForSeconds(4);
      efecto.enabled = false;
    }

}

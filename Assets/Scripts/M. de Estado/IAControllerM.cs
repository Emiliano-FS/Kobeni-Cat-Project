using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAControllerM : MonoBehaviour{

    public TileMap map;
    Animator anim;

    //-------Controladores de Textura----
    public Material normal;
    public Material sad;

    string state = "IDLE";
    // Start is called before the first frame update
    void Start(){
      anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        var playerColor = this.gameObject.transform.GetChild(0).GetComponent<Renderer>();
        Vector3 direction = this.GetComponent<Unit>().target.transform.position - this.transform.position;
        if(direction.x < 9 && direction.x > -9){

          if(this.GetComponent<Unit>().target.GetComponent<Player>().rageMode == true){
            anim.SetBool("hasPower",true);
            state = "ESCONDIENDO";
                playerColor.material = sad;
            }
            else{
            anim.SetBool("hasPower",false);
            anim.SetTrigger("iSeeHim");
            state = "ATACAR";
                playerColor.material = normal;
            }
        }else{
          anim.SetTrigger("iDSeeHim");
          state = "IDLE";
        }
        if(state == "ATACAR"){
            playerColor.material = normal;
            //Debug.Log("Estoy en atacar");
            //Debug.Log("Posicion enemiga en x " + (int)this.GetComponent<Unit>().target.transform.position.x);
            //Debug.Log("Posicion enemiga en y " + (int)this.GetComponent<Unit>().target.transform.position.y);
            map.GeneratePathTo((int)this.GetComponent<Unit>().target.transform.position.x,(int)this.GetComponent<Unit>().target.transform.position.y,gameObject);
        }

        if(state == "ESCONDIENDO"){
          //Debug.Log("Estoy en Escondiendo");
          map.GeneratePathTo(this.GetComponent<Unit>().tileXHome,this.GetComponent<Unit>().tileYHome,gameObject);
          playerColor.material = sad;
        }
        if(state == "IDLE"){
          //Debug.Log("Estoy en Idle");
          this.GetComponent<Unit>().controlador.unitsActivos.Remove(gameObject);
          if(this.tag == "Grande") this.GetComponent<Unit>().controlador.unitGrandes.Add(gameObject);
          if(this.tag == "Mediano") this.GetComponent<Unit>().controlador.unitMedianos.Add(gameObject);
          if(this.tag == "Chicos") this.GetComponent<Unit>().controlador.unitChicos.Add(gameObject);
          gameObject.SetActive(false);
        }
      }
}

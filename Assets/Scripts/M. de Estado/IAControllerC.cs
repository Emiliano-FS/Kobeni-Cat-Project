using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAControllerC : MonoBehaviour{

    public TileMap map;
    Animator anim;
    GameObject support;

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
          if(this.GetComponent<Unit>().target.GetComponent<Player>().killer == true){
            anim.SetTrigger("killer");
            state = "BUSCANDO";
            Debug.Log("Lo asuste");
            this.GetComponent<Unit>().scared = true;
            if(!isThereSupport()){
              anim.SetTrigger("noSupport");
              state = "ESCONDIENDO";
                    playerColor.material = sad;
                }
                else{
              anim.SetTrigger("Support");
              state = "BUSCANDO";
                    playerColor.material = sad;
                }
          }else if(this.GetComponent<Unit>().target.GetComponent<Player>().rageMode == true){
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
          //Debug.Log("Estoy en atacar");
          map.GeneratePathTo((int)this.GetComponent<Unit>().target.transform.position.x,(int)this.GetComponent<Unit>().target.transform.position.y,gameObject);
            playerColor.material = normal;
        }
        if(state == "BUSCANDO"){
          map.GeneratePathTo((int)support.transform.position.x,(int)support.transform.position.y,gameObject);
            playerColor.material = sad;
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

    bool isThereSupport(){

      bool help = false;
      foreach(GameObject unidad in this.GetComponent<Unit>().controlador.unitsActivos){
      if(unidad != gameObject){
        Vector3 direction2 = unidad.transform.position - this.transform.position;
          if(direction2.x < 9 && direction2.x > -9){
            help = true;
            support = unidad;
            break;
          }
        }
      }
      return help;
    }
}

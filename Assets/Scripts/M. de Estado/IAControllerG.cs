using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAControllerG : MonoBehaviour{

    public TileMap map;
    Animator anim;

    string state = "IDLE";
    // Start is called before the first frame update
    void Start(){
      anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        Vector3 direction = this.GetComponent<Unit>().target.transform.position - this.transform.position;
        if(direction.x < 9 && direction.x > -9){
          anim.SetTrigger("iSeeHim");
          state = "ATACAR";
        }else{
          anim.SetTrigger("iDSeeHim");
          state = "IDLE";
        }
        if(state == "ATACAR"){
          //Debug.Log("Estoy en atacar");
          map.GeneratePathTo((int)this.GetComponent<Unit>().target.transform.position.x,(int)this.GetComponent<Unit>().target.transform.position.y,gameObject);
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

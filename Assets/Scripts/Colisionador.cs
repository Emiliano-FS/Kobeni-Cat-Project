using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisionador : MonoBehaviour{

  public TurnController controlador;//Referencia al controlador de turnoss

  void OnTriggerEnter(Collider collider){
    Debug.Log("Entre al colisionador");
    if(collider.gameObject.tag == "Player"){
        Debug.Log("Colisione con el Jugador");
        collider.gameObject.GetComponent<Player>().takeDamage(transform.parent.gameObject.GetComponent<Unit>().damage);
        collider.gameObject.GetComponent<Player>().killer = true;
        controlador.unitsActivos.Remove(transform.parent.gameObject);
        if(transform.parent.tag == "Grande") controlador.unitGrandes.Add(transform.parent.gameObject);
        if(transform.parent.tag == "Mediano") controlador.unitMedianos.Add(transform.parent.gameObject);
        if(transform.parent.tag == "Chico") controlador.unitChicos.Add(transform.parent.gameObject);
        transform.parent.gameObject.SetActive(false);
    }
    //Holly fuck, se ve horrible por eso lo voy a comentar
    if(collider.gameObject.tag == "Chico" && transform.parent.tag == "Chico"){
      if(collider.transform.parent.gameObject.GetComponent<Unit>().scared && transform.parent.gameObject.GetComponent<Unit>().scared){
        Debug.Log("Colisione con un Chico siendo chico, FUSION");
        //Removemos los 2 enemigos que colisionan de los activos
        controlador.unitsActivos.Remove(transform.parent.gameObject);
        //controlador.unitsActivos.Remove(collider.gameObject); Para no hacerlo 2 veces
        //Los regresamos a la lista de assets
        controlador.unitChicos.Add(transform.parent.gameObject);
        //controlador.unitChicos.Add(collider.gameObject); Para no hacerlo 2 veces
        //Si el otro ya fusiono ya no lo hacemos nosotros
        if(collider.transform.parent.gameObject.GetComponent<Unit>().fusione != true){
          //Sacamos un asset de tamaño mediano
          GameObject a = controlador.unitMedianos[0];
          //Lo agregamos a los activos
          controlador.unitsActivos.Add(a);
          //Le ponemos la posicion de uno de los que se fusionaron
          a.transform.position = new Vector3(collider.gameObject.transform.position.x,collider.gameObject.transform.position.y,0f);
          //Le ponemos a su script los tileX y tileY
          a.GetComponent<Unit>().tileX = (int)a.transform.position.x;
          a.GetComponent<Unit>().tileY = (int)a.transform.position.y;
          //Activamos el nuevo
          a.SetActive(true);
          //Quitamos el mediano de su lista de assets
          controlador.unitMedianos.Remove(a);
          transform.parent.gameObject.GetComponent<Unit>().fusione = true;
        }
        //Los desactivamos para que se desaparescan
        transform.parent.gameObject.SetActive(false);
        //collider.gameObject.SetActive(false);
      }
    }
  }
}

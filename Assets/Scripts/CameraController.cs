 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script que sigue al jugador
public class CameraController : MonoBehaviour {

    public Transform player;
    public float speed;//1.0f;//Velocidad a la que la camara corije su movimiento.
    public Vector3 offset;// = new Vector3(0f,1.27f,-2.88f);


    void LateUpdate(){
        Vector3 desiredPosition= new Vector3 (player.position.x , player.position.y+offset.y , offset.z);
        Vector3 smoothMovement = Vector3.Lerp(transform.position, desiredPosition, speed);
        transform.position = smoothMovement;

 //transform.LookAt(player);

    }

}

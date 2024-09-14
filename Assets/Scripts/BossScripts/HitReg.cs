using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReg : MonoBehaviour
{

    public int damage;

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Entre al colisionador de la bala");
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Colisione con el Jugador");
            collider.GetComponent<PlayerBoss>().takeDamage(damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{

    public PlayerBoss player;

    void Start()
    {
        Physics.IgnoreCollision(transform.GetComponent<Collider>(), player.transform.GetComponent<Collider>());
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Colisiono con "+ collider.name);
        if (player.rageMode)
        {
            if (collider.gameObject.tag == "Boss")
            {
                collider.GetComponent<Boss>().takeDamage(6);
                Debug.Log("Auchy Auch");
            }
        }
        else
        {
            if(collider.gameObject.tag == "Boss")
            {
                Debug.Log("Auchy Auch");
                collider.GetComponent<Boss>().takeDamage(3);
                player.currentRage += 1;
                player.rageMeter.setRage(player.currentRage);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public KadmonAI boss;//Esto no deberia de tenerlo. pero ok.
    public int currentHealth;
    public int maxHealth;
    public HealthBar healthbar;//Referencia al objeto HealthBar
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.setMaxHealth(maxHealth);
    }

    public void takeDamage(int dmg)
    {
        Debug.Log("Jefe Tomando da�o");
        currentHealth -=dmg;
        healthbar.setHealth(currentHealth);
        if(boss.recovery){
          boss.Vulnerable = true;
          boss.recovery = false;
          boss.healing = false;
        }
        StartCoroutine(invulFrames());
        StartCoroutine(boss.spriteController(3));
    }

    IEnumerator invulFrames()
    {
        transform.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        transform.GetComponent<Collider>().enabled = true;
    }
}

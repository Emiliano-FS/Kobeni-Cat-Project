using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    public int nextLevel;
    public Image black;
    public Animator anim;
    public Guion guion;
    public DialogUIController controlador;

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Yey! Gane el juego");
        if (collider.gameObject.tag == "Player")
        {
            StartCoroutine(FadeToNextLevel(collider.gameObject.GetComponent<Player>()));
           
        }
    }

    IEnumerator FadeToNextLevel(Player player)
    {
        controlador.StartDialogue(guion);
        yield return new WaitUntil(() => player.inCutscene == false);
        anim.SetBool("Fade", true);
        yield return new WaitUntil(()=> black.color.a==1);
        SceneManager.LoadScene(nextLevel);
    }

    //Has aqui el codigo que asigne la cutscene final.
}

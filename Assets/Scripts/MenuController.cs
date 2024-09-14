using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //Nos lleva a la escena principal
    public void buttonStart()
    {
        SceneManager.LoadScene(1);
    }

    //Nos lleva a la escena principal
    public void buttonCompendium()
    {
        SceneManager.LoadScene(2);
    }

    //Adivina que hace.
    public void buttonQuit()
    {
        Application.Quit();
    }
}

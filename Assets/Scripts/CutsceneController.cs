using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{

    public bool opening;
    public Guion guion;
    public DialogUIController controlador;

    // Si opening es true, el dialogo se iniciara cuando se inice la escena.
    void LateUpdate()
    {
        if (opening)
        {
            controlador.StartDialogue(guion);
            opening = false;
        }
    }
}

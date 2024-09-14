using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RageController : MonoBehaviour
{
    public Slider slider;

    //Modifica visualmente la barra de vida
    public void setRage(float rage)
    {
        slider.value = rage;
    }

    public void setMaxRage(float rage)
    {
        slider.maxValue = rage;
        slider.value = rage;
    }
}

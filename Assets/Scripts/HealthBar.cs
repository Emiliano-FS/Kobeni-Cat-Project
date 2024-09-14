 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Clase que maneja las barras de salud, tanto de enmigos como del jugador.
//Tambien maneja el movimiento del turn Counter y que cambie.
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image part1;//Parte Blanca del TurnCounter
    public Image part2;//Parte Negra del TurnCounter

    //Modifica visualmente la barra de vida
    public void setHealth(int health)
    {
        slider.value = health;
    }

    //Ajusta el nivel maximo de salud. Flexible para ser usado por cualquier entidad
    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    //Basicamente hace que gire la cosa el turn Counter
    void Update()
    {
        part1.transform.Rotate(0f, 0f,0.1f);
        part2.transform.Rotate(0f, 0f, 0.105f);
    }

}

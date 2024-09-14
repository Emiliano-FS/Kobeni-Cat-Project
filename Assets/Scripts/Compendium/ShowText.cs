using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public string text;
    public Text uiText;

    // When highlighted with mouse.
    void OnMouseOver()
    {
        uiText.text = text;
        Debug.Log("Highlight Button");
    }
}

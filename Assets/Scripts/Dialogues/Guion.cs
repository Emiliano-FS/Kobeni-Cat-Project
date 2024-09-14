using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guion : MonoBehaviour{
  public List<Dialogo> listaDialogos;
}

[System.Serializable]
public class Dialogo{
  public NPCGenerico npc;
  [TextArea(5,5)]
  public string dialogos;
}

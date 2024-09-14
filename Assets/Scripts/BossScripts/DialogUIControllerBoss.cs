using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUIControllerBoss : MonoBehaviour
{
  public PlayerBoss player;//Te valio verga lo que te dije :u
  private Queue<Dialogo> secuenciaDialogos;
  private int orden;
  float timer;

  public Text nameText;
  public Text dialogueText;
  public Image portrait;
  public Image ready;

  public Animator animator;
  // Start is called before the first frame update
  void Start(){
      secuenciaDialogos = new Queue<Dialogo>();
      timer = 0;
      ready.enabled = false;
  }

  void Update()//Para que cuando presiones Spacebar avance el dialogo.
  {
      timer += Time.deltaTime;
      if (timer >= 0.4f)
      {
          if (Input.GetKey("space"))
          {
              DisplayNextSentence();
              timer = 0f;
          }
      }
  }

  public void StartDialogue(Guion guion){
      player.inCutscene = true;
      orden = 1;
      animator.SetBool("isOpen",true);

      foreach (Dialogo s in guion.listaDialogos){
          secuenciaDialogos.Enqueue(s);
      }

      DisplayNextSentence() ;
  }

  public void DisplayNextSentence()
  {
      ready.enabled = false;
      if (secuenciaDialogos.Count == 0)
      {
          EndDialogue();
          return;
      }

      Dialogo nextDialogo = secuenciaDialogos.Dequeue();
      nameText.text = nextDialogo.npc.nombreNPC;
      portrait.sprite = nextDialogo.npc.spriteNPC;
      string sentence = nextDialogo.dialogos;

      //dialogueText.text = tmp; <-Texto inmediato
      StopAllCoroutines();
      orden ++;
      StartCoroutine(TypeSentences(sentence));
  }

  IEnumerator TypeSentences(string sentence)
  {
      player.inCutscene = true;
      dialogueText.text ="";
      foreach(char letter in sentence.ToCharArray())
      {
          dialogueText.text += letter;
          yield return null;
      }
      ready.enabled = true;
  }

  void EndDialogue()
  {
      animator.SetBool("isOpen", false);
      player.inCutscene = false;
  }
}

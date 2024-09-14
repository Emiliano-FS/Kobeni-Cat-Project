using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    //Lista de Audio Clips
    public static AudioClip dash;
    public static AudioClip hit1;
    public static AudioClip moveTable;
    public static AudioClip smokeball;
    public static AudioClip turnChange;
    public static AudioClip sword;
    public static AudioClip bigSword;
    public static AudioClip hurt;
    public static AudioClip heal;
    public static AudioClip ready;

    //Para Kadmon
    public static AudioClip beep1;
    public static AudioClip gun1;
    public static AudioClip reload;
    public static AudioClip warning;
    public static AudioClip holy;
    public static AudioClip soda;

    //Cosas para hacerlo funcionar
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        moveTable = Resources.Load<AudioClip>("MoveTable");
        dash = Resources.Load<AudioClip>("move");
        hit1 = Resources.Load<AudioClip>("hit1");
        smokeball = Resources.Load<AudioClip>("smokeball");
        turnChange = Resources.Load<AudioClip>("turnChange");
        beep1 = Resources.Load<AudioClip>("beep1");
        gun1 = Resources.Load<AudioClip>("gun1");
        reload = Resources.Load<AudioClip>("reload");
        sword = Resources.Load<AudioClip>("sword");
        bigSword = Resources.Load<AudioClip>("Bigsword");
        hurt = Resources.Load<AudioClip>("hurt");
        warning = Resources.Load<AudioClip>("Warning");
        holy = Resources.Load<AudioClip>("Holy");
        heal = Resources.Load<AudioClip>("heal");
        ready = Resources.Load<AudioClip>("Sword4");
        soda = Resources.Load<AudioClip>("soda");
        audioSrc = GetComponent<AudioSource>();

    }

    public static void PlaySound(string clip)
    {
        switch (clip) {
            case "dash":
                audioSrc.PlayOneShot(dash);
                break;
            case "moveTable":
                audioSrc.PlayOneShot(moveTable);
                break;
            case "hit1":
                audioSrc.PlayOneShot(hit1);
                break;
            case "smokeball":
                audioSrc.PlayOneShot(smokeball);
                break;
            case "turnChange":
                audioSrc.PlayOneShot(turnChange);
                break;
            case "beep1":
                audioSrc.PlayOneShot(beep1);
                break;
            case "gun1":
                audioSrc.PlayOneShot(gun1);
                break;
            case "reload":
                audioSrc.PlayOneShot(reload);
                break;
            case "sword":
                audioSrc.PlayOneShot(sword);
                break;
            case "bigSword":
                audioSrc.PlayOneShot(bigSword);
                break;
            case "hurt":
                audioSrc.PlayOneShot(hurt);
                break;
            case "warning":
                audioSrc.PlayOneShot(warning);
                break;
            case "holy":
                audioSrc.PlayOneShot(holy);
                break;
            case "heal":
                audioSrc.PlayOneShot(heal);
                break;
            case "soda":
                audioSrc.PlayOneShot(soda);
                break;
            case "ready":
                audioSrc.PlayOneShot(ready);
                break;
        }
    }
}

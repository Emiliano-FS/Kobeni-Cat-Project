using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryControl : MonoBehaviour
{
    public CompendiumController cc;
    public Image image;
    bool displaying;
    float timer;
    public Text textUI;

    //Lista de Sprites.
    public Sprite hellWiki;
    public Sprite portada1;
    public Sprite portada2;

    //Caly
    public Sprite wikiCaly;
    public Sprite calyFace;
    public Sprite calyConcept1;
    public Sprite calyConcept2;
    //Take
    public Sprite takeFace;
    public Sprite wikiTake;
    public Sprite TakeConcept;
    //Adam
    public Sprite AdamWiki;
    public Sprite AdamFace;
    public Sprite AdamConcept;
    RectTransform picture;
    //Unused
    public Sprite Unused1;
    public Sprite Unused2;
    public Sprite Unused3;
    public Sprite Unused4;
    public Sprite Unused5;

    Sprite[] tail;
    int pos;
    // Start is called before the first frame update
    void Start()
    {
        textUI.text = "";
        timer = 0;
        displaying = false;
        cc.ToggleMenu(true);
        image.enabled = false;
        picture = image.GetComponent<RectTransform>();
    }

    //True: Galeria, False: Main menu
    public void ToggleGallery(bool type)
    {
        if (type)
        {
            displaying = true;
            image.enabled = true;
            cc.ToggleMenu(false);
            textUI.text = "W,A,S,D: Mover     M=Zoom In     N=Zoom Out    B= Reiniciar     Spacebar= Siguiente    ESC= Regresar";
        }
        else
        {
            displaying = false;
            image.enabled = false;
            cc.ToggleMenu(true);
            textUI.text = "";
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (displaying){
            timer += Time.deltaTime;
            if (timer > 0.4f) {
                if (Input.GetKey("space"))
                {
                    resetPosition();
                    next();
                    timer = 0;
                }
                if (Input.GetKey(KeyCode.Escape))
                {
                    ToggleGallery(false);
                    timer = 0;
                }
            }
            if (Input.GetKey("a"))
            {
                picture.anchoredPosition = new Vector2(picture.anchoredPosition.x - 0.1f, picture.anchoredPosition.y);
            }
            if (Input.GetKey("d"))
            {
                picture.anchoredPosition = new Vector2(picture.anchoredPosition.x + 0.1f, picture.anchoredPosition.y);
            }
            if (Input.GetKey("s"))
            {
                picture.anchoredPosition = new Vector2(picture.anchoredPosition.x , picture.anchoredPosition.y - 0.1f);
            }
            if (Input.GetKey("w"))
            {
                picture.anchoredPosition = new Vector2(picture.anchoredPosition.x , picture.anchoredPosition.y + 0.1f);
            }
            if (Input.GetKey("m"))//Zoom IN
            {
                picture.sizeDelta = new Vector2(picture.sizeDelta.x+ 0.1f, picture.sizeDelta.y + 0.1f);
            }
            if (Input.GetKey("n"))//Zoom Out
            {
                picture.sizeDelta = new Vector2(picture.sizeDelta.x- 0.1f, picture.sizeDelta.y - 0.1f);
            }
            if (Input.GetKey("b"))
            {
                resetPosition();
            }
        }
    }

    public void activate(int art)
    {
        resetPosition();
        pos = 0;
        ToggleGallery(true);
        switch (art)
        {
            case 1:
                image.sprite = hellWiki;
                tail= new Sprite[] {hellWiki, portada1 , portada2};
                break;
            case 2:
                image.sprite = AdamWiki;
                tail = new Sprite[] { AdamWiki, AdamFace, AdamConcept };
                break;
            case 3:
                image.sprite = wikiCaly;
                tail = new Sprite[] { wikiCaly, calyFace, calyConcept1, calyConcept2 };
                break;
            case 4:
                image.sprite = Unused1;
                tail = new Sprite[] { Unused1, Unused2, Unused3, Unused4,Unused5 };
                break;
            default:
                image.sprite = wikiTake;
                tail = new Sprite[] { wikiTake, takeFace, TakeConcept };
                break;
        }
    }

    public void next()
    {
        if (tail.Length-1 < pos) { pos = 0; }
        image.sprite = tail[pos];
        pos += 1;
    }

    public void resetPosition()
    {
        picture.anchoredPosition = new Vector2(0,0);
        picture.sizeDelta = new Vector2(600,1000);
    }

}

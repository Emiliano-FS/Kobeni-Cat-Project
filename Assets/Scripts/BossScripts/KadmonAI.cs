using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Panda;

public class KadmonAI : MonoBehaviour
{
    //Cosas para dialogo
    public Guion guion;
    public DialogUIControllerBoss controlador;

    //Logica del Jefe
    float timer;
    public GameObject standarBullet;
    public PlayerBoss player;
    public TileMapBoss map;
    public bool Vulnerable = false;
    public bool recovery = false;
    public bool healing = false;
    public int dificulty = 1;

    //Ataque mapa
    private int lastPlayerX = 0;
    private int lastPlayerY = 0;
    private (int,int)[] shield = new (int,int)[]{(3,4),(5,4),(4,5),(4,3),(4,4)};


    //Sprites
    public Material normal;
    public Material reload;
    public Material attack;
    public Material hurt;
    public Material heal;
    public Material defeated;
    public GameObject reticule;
    public List<GameObject> reticulas;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        map.tiles[4,4] = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.inCutscene) {
            timer += Time.deltaTime;
            if (timer > 6.5f)
            {
                //StartCoroutine(trackPlayerShot()); //1.5 de timer
                //StartCoroutine(FanTheHammer()); //5.3 de timer
                //StartCoroutine(LuckyShot());//6.5 de timer
                timer = 0;
            }
        }
    }

    [Task]
    public bool Iniciar(){
      if(player.inCutscene){
        return false;
      }
      return true;
    }

    [Task]
    public void TrackPlayerReticule(){
      reticule.GetComponent<MeshRenderer>().enabled = true;
      SoundController.PlaySound("beep1");
      Vector3 playerPos = player.transform.position;
      reticule.transform.position = new Vector3(playerPos.x, playerPos.y, -1.5f);
      Task.current.Succeed();
    }

    [Task]
    public void ShootPlayer(){
      reticule.GetComponent<MeshRenderer>().enabled = false;
      StartCoroutine(normalShot(reticule.transform.position));
      Task.current.Succeed();
    }

    [Task]
    public void FanAiming(){
      GameObject cross = Instantiate(reticule, player.transform.position, Quaternion.Euler(35f, 0, 0));
      SoundController.PlaySound("beep1");
      cross.GetComponent<MeshRenderer>().enabled = true;
      reticulas.Add(cross);
      Task.current.Succeed();
    }

    [Task]
    public void FanShooting(){
      GameObject a = reticulas[0];
      a.GetComponent<MeshRenderer>().enabled = false;
      reticulas.Remove(a);
      StartCoroutine(normalShot(a.transform.position));
      Destroy(a);
      Task.current.Succeed();
    }

    [Task]
    public void ActivateShield(){
        StartCoroutine(spriteController(4));
        map.tiles[3,4] = 1;
      map.tiles[5,4] = 1;
      map.tiles[4,5] = 1;
      map.tiles[4,3] = 1;
      Vulnerable = false;
      map.updateTerreno();
      Task.current.Succeed();

    }

    [Task]
    public void DectivateShield(){
      map.tiles[3,4] = 0;
      map.tiles[5,4] = 0;
      map.tiles[4,5] = 0;
      map.tiles[4,3] = 0;
      map.updateTerreno();
      recovery = true;
      Task.current.Succeed();
    }

    [Task]
    public void WarningX(){
        SoundController.PlaySound("beep1");
        lastPlayerY = player.tileY;
      for(int i = 0 ; i < map.mapSizeX ; i ++ ){
        if(!(checkShield((i,lastPlayerY)))){
          map.tiles[i,lastPlayerY] = 2;
          map.updateTerreno();
        }
      }
      Task.current.Succeed();
    }

    [Task]
    public void WarningY(){
        SoundController.PlaySound("beep1");
        lastPlayerX = player.tileX;
      for(int i = 0 ; i < map.mapSizeY ; i ++ ){
        if(!(checkShield((lastPlayerX,i)))){
          map.tiles[lastPlayerX,i] = 2;
          map.updateTerreno();
        }
      }
      Task.current.Succeed();
    }

    [Task]
    public void DamageX(){
        SoundController.PlaySound("holy");
        for (int i = 0 ; i < map.mapSizeX ; i ++ ){
        if(!(checkShield((i,lastPlayerY)))){
          map.tiles[i,lastPlayerY] = 1;
          map.updateTerreno();
        }
      }
      Task.current.Succeed();
    }

    [Task]
    public void DamageY(){
        SoundController.PlaySound("holy");
        for (int i = 0 ; i < map.mapSizeY ; i ++ ){
        if(!(checkShield((lastPlayerX,i)))){
          map.tiles[lastPlayerX,i] = 1;
          map.updateTerreno();
        }
      }
      Task.current.Succeed();
    }

    [Task]
    public void Clean(){
      for(int i = 0 ; i < map.mapSizeY ; i ++ ){
        if(!(checkShield((lastPlayerX,i)))){
          map.tiles[lastPlayerX,i] = 0;
        }
      }
      for(int i = 0 ; i < map.mapSizeX ; i ++ ){
        if(!(checkShield((i,lastPlayerY)))){
          map.tiles[i,lastPlayerY] = 0;
        }
      }
      map.updateTerreno();
      Task.current.Succeed();
    }

    [Task]
    public void RecoveryMode(){
      recovery = true;
      healing = true;
      for(int i = 0 ; i < map.mapSizeX ; i ++ ){
        map.tiles[i,0] = 1;
        map.tiles[i,8] = 1;
        map.tiles[0,i] = 1;
        map.tiles[8,i] = 1;
      }

      for(int i = 2; i < 7 ; i++){
        map.tiles[2,i] = 1;
        map.tiles[6,i] = 1;
        map.tiles[i,2] = 1;
        map.tiles[i,6] = 1;
      }
      map.tiles[4,2] = 0;
      map.updateTerreno();
      Task.current.Succeed();
    }

    [Task]
    public void RecoveryModeReverse(){
      for(int i = 0 ; i < map.mapSizeX ; i ++ ){
        map.tiles[i,0] = 0;
        map.tiles[i,8] = 0;
        map.tiles[0,i] = 0;
        map.tiles[8,i] = 0;
      }

      for(int i = 2; i < 7 ; i++){
        map.tiles[2,i] = 0;
        map.tiles[6,i] = 0;
        map.tiles[i,2] = 0;
        map.tiles[i,6] = 0;
      }
      map.updateTerreno();
      Task.current.Succeed();
    }

    [Task]
    public void Reload(){
      Debug.Log("Recargando Arma");
      SoundController.PlaySound("reload");
      StartCoroutine(spriteController(1));
      Task.current.Succeed();
    }

    [Task]
    public bool BadHealhtStatus(){
      if(this.GetComponent<Boss>().currentHealth < 40 && !Vulnerable){
        return true;
      }else if(healing){
        return true;
      }else{
      return false;
      }
    }

    [Task]
    public bool DangerStatus(){
      if(!Vulnerable){
        return true;
      }
      return false;
    }

    [Task]
    public void UpdateHealth(){
        if(this.GetComponent<Boss>().currentHealth <= this.GetComponent<Boss>().maxHealth)
        {
          this.GetComponent<Boss>().currentHealth ++;
          this.GetComponent<Boss>().healthbar.setHealth(this.GetComponent<Boss>().currentHealth);
        }
        SoundController.PlaySound("heal");
        Task.current.Succeed();
    }

    [Task]
    public bool Dead(){
      if(this.GetComponent<Boss>().currentHealth <= 0){
        return true;
      }else{
        return false;
      }
    }

    [Task]
    public void Kill(){
      Debug.Log("Me mori");
      this.GetComponent<PandaBehaviour>().enabled = false;
      StartCoroutine(spriteController(5));
      StartCoroutine(BossDefeated());
      Task.current.Succeed();
    }

    private bool checkShield((int,int) numbers){
      if (shield.Contains(numbers)){
        return true;
      }
      return false;
    }

    [Task]
    public bool HardMode(){
      if(dificulty == 3){
        return true;
      }
      Task.current.Succeed();
      return false;
    }

    [Task]
    public bool EasyMode(){
      if(dificulty == 1){
        return true;
      }
      Task.current.Succeed();
      return false;
    }

    [Task]
    public bool MediumMode(){
      if(dificulty == 2){
        return true;
      }
      Task.current.Succeed();
      return false;
    }

    [Task]
    public void ChooseDificulty(){
      if(this.GetComponent<Boss>().currentHealth < 46 && this.GetComponent<Boss>().currentHealth >= 35){
        dificulty = 1;
      }else if(this.GetComponent<Boss>().currentHealth < 35 && this.GetComponent<Boss>().currentHealth >= 15){
        dificulty = 2;
      }else if(this.GetComponent<Boss>().currentHealth < 15 ){
        dificulty = 3;
      }
      Task.current.Succeed();
    }

    //genera un Objeto bala en el lugar puesto. La bala esta ahi .3 segundos.
    IEnumerator normalShot(Vector3 pos)
    {
        StartCoroutine(spriteController(2));
        Debug.Log("BulletShot");
        GameObject bullet = Instantiate(standarBullet, pos, Quaternion.Euler(-90f, 0, 0));
        SoundController.PlaySound("gun1");
        yield return new WaitForSeconds(0.3f);
        Destroy(bullet);

    }

    //Trackea al jugador y dispara. Dura 1.3 segundos. 1 segundo de tracking y .3 de bala
    //IEnumerator trackPlayerShot()
    //{
    //    reticule.GetComponent<MeshRenderer>().enabled = true;
    //    yield return new WaitForSeconds(0.2f);
    //    SoundController.PlaySound("beep1");
    //    Vector3 playerPos = player.transform.position;
    //    reticule.transform.position = new Vector3(playerPos.x, playerPos.y, -1.5f);
    //    yield return new WaitForSeconds(0.2f);
    //    playerPos = player.transform.position;
    //    reticule.transform.position = new Vector3(playerPos.x, playerPos.y, -1.5f);
    //    yield return new WaitForSeconds(0.2f);
    //    SoundController.PlaySound("beep1");
    //    playerPos = player.transform.position;
    //    reticule.transform.position = new Vector3(playerPos.x, playerPos.y, -1.5f);
    //    yield return new WaitForSeconds(0.4f);
    //    reticule.GetComponent<MeshRenderer>().enabled = false;
    //    StartCoroutine(normalShot(reticule.transform.position));
    //}
    ////Genera 4 miras en posiciones pasadas del jugador y dispara en cada una de ellas.
    ////Dura 4.7
    //IEnumerator FanTheHammer()
    //{
    //    GameObject crosshair1 = Instantiate(reticule, player.transform.position, Quaternion.Euler(35f, 0, 0));
    //    SoundController.PlaySound("beep1");
    //    yield return new WaitForSeconds(1.0f);
    //    GameObject crosshair2 = Instantiate(reticule, player.transform.position, Quaternion.Euler(35f, 0, 0));
    //    SoundController.PlaySound("beep1");
    //    yield return new WaitForSeconds(1.0f);
    //    GameObject crosshair3 = Instantiate(reticule, player.transform.position, Quaternion.Euler(35f, 0, 0));
    //    SoundController.PlaySound("beep1");
    //    yield return new WaitForSeconds(1.0f);
    //    GameObject crosshair4 = Instantiate(reticule, player.transform.position, Quaternion.Euler(35f, 0, 0));
    //    SoundController.PlaySound("beep1");
    //    yield return new WaitForSeconds(1.0f);
    //    StartCoroutine(normalShot(crosshair1.transform.position));
    //    yield return new WaitForSeconds(0.1f);
    //    Destroy(crosshair1);
    //    StartCoroutine(normalShot(crosshair2.transform.position));
    //    yield return new WaitForSeconds(0.1f);
    //    Destroy(crosshair2);
    //    StartCoroutine(normalShot(crosshair3.transform.position));
    //    yield return new WaitForSeconds(0.1f);
    //    Destroy(crosshair3);
    //    StartCoroutine(normalShot(crosshair4.transform.position));
    //    yield return new WaitForSeconds(0.1f);
    //    Destroy(crosshair4);

    //}

    ////Dispara 5 veces en la posicion del jugador con delay. Despues espera 3 segundos para recargar el arma.
    ////Dura 2 segundos, y despues 3 de recarga.
    //IEnumerator LuckyShot()
    //{
    //    reticule.GetComponent<MeshRenderer>().enabled = true;
    //    SoundController.PlaySound("beep1");
    //    Vector3 playerPos = player.transform.position;
    //    reticule.transform.position = new Vector3(playerPos.x, playerPos.y, -1.5f);
    //    yield return new WaitForSeconds(0.4f);
    //    StartCoroutine(normalShot(reticule.transform.position));
    //    SoundController.PlaySound("beep1");
    //    playerPos = player.transform.position;
    //    reticule.transform.position = new Vector3(playerPos.x, playerPos.y, -1.5f);
    //    yield return new WaitForSeconds(0.4f);
    //    StartCoroutine(normalShot(reticule.transform.position));
    //    SoundController.PlaySound("beep1");
    //    playerPos = player.transform.position;
    //    reticule.transform.position = new Vector3(playerPos.x, playerPos.y, -1.5f);
    //    yield return new WaitForSeconds(0.4f);
    //    StartCoroutine(normalShot(reticule.transform.position));
    //    SoundController.PlaySound("beep1");
    //    playerPos = player.transform.position;
    //    reticule.transform.position = new Vector3(playerPos.x, playerPos.y, -1.5f);
    //    yield return new WaitForSeconds(0.4f);
    //    StartCoroutine(normalShot(reticule.transform.position));
    //    SoundController.PlaySound("beep1");
    //    playerPos = player.transform.position;
    //    reticule.transform.position = new Vector3(playerPos.x, playerPos.y, -1.5f);
    //    yield return new WaitForSeconds(0.4f);
    //    reticule.GetComponent<MeshRenderer>().enabled = false;
    //    StartCoroutine(normalShot(reticule.transform.position));
    //    Debug.Log("Recargando Arma");
    //    SoundController.PlaySound("reload");
    //    StartCoroutine(spriteController(1));
    //    yield return new WaitForSeconds(3.0f);
    //}
    public IEnumerator BossDefeated()
    {
        controlador.StartDialogue(guion);
        yield return new WaitUntil(() => player.inCutscene == false);
        SceneManager.LoadScene(0);
    }

    public IEnumerator spriteController(int input)
    {
        var spriteColor = this.GetComponent<Renderer>();
        switch (input)
        {
            case 1:
                yield return new WaitForSeconds(0.3f);
                transform.localScale += new Vector3(0.0f, 0.0f, 0.05f);
                transform.position += new Vector3(0.0f, 0.0f, -0.25f);
                spriteColor.material = reload;
                yield return new WaitForSeconds(2.5f);
                transform.localScale -= new Vector3(0.0f, 0.0f, 0.05f);
                transform.position -= new Vector3(0.0f, 0.0f, -0.25f);
                spriteColor.material = normal;
                break;
            case 2:
                spriteColor.material = attack;
                yield return new WaitForSeconds(0.2f);
                spriteColor.material = normal;
                break;
            case 3:
                spriteColor.material = hurt;
                SoundController.PlaySound("hit1");
                yield return new WaitForSeconds(0.5f);
                spriteColor.material = normal;
                break;
            case 4:
                spriteColor.material = heal;
                SoundController.PlaySound("soda");
                yield return new WaitForSeconds(2.5f);
                spriteColor.material = normal;
                break;
            case 5:
                spriteColor.material = defeated;
                break;
        }
    }
}

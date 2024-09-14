using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * Documenta tu codigo animal.
 */
public class TileMap : MonoBehaviour{

    public TileType[] tileTypes;
    public NPCGenerico[] npcs;
    int[,] tiles;
    Node[,] graph;
    public List<Albañil> listaAlbañil = new List<Albañil>();
    //Guarda el camino actual para Dijkstra.
    //List<Node> currentPath = null;
    int mapSizeX = 40;
    int mapSizeY = 7;
    private int last = 999;

    void Start(){
        //Actualizando los atributos de la unidad actual.
        //selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        //selectedUnit.GetComponent<Unit>().tileY = (int)selectedUnit.transform.position.y;
        //selectedUnit.GetComponent<Unit>().map = this;
        GenerateMapData();
        GeneratePathfindingGraph();
        foreach(NPCGenerico unidad in npcs){
          unidad.enabled = false;
        }
        npcRulette(6f,3f);
        npcRulette(24f,3f);

        tiles[1,1] = 4;

        poblarObstaculos(25);
        bool mapready = false;
        //do{
          //poblarObstaculos(24);
        mapready = DijkstraForMap(0,3,39,3);
        //}while(!mapready);

        GenerateMapVisual();

    }

    void GenerateMapData(){
        //localizar los cuadros del mapar
        tiles = new int[mapSizeX, mapSizeY];
        int x,y;
        // Inicializar los cuadros
        for (x = 0; x < mapSizeX; x++){
            for (y = 0; y < mapSizeY; y++){
                tiles[x, y] = 0;
            }
        }
        for (x = 0; x < mapSizeX; x++){
          if((x % 6) != 0){
            tiles[x, 0] = 2;
            tiles[x, 6] = 2;
          }else{
            tiles[x, 0] = 4;
            tiles[x, 6] = 4;
          }
        }

        for(x = 0; x <= 6; x++){
          int var = 2 + (6 * x);
          tiles[var, 1] = 2;
          tiles[var, 5] = 2;
        }

        for(x = 0; x <= 6; x++){
          int var = 3 + (6 * x);
          tiles[var, 1] = 2;
          tiles[var, 5] = 2;
        }

        for(x = 0; x <= 5; x++){
          int var = 4 + (6 * x);
          tiles[var, 1] = 2;
          tiles[var, 5] = 2;
        }

        tiles[0,3] = 4;
        tiles[0,2] = 4;
        tiles[0,4] = 4;

        tiles[39,3] = 4;
        tiles[39,2] = 4;
        tiles[39,4] = 4;


    }

    void GenerateMapVisual(){
        for (int x = 0; x < mapSizeX; x++){
            for (int y = 0; y < mapSizeY; y++){
                TileType tt = tileTypes[tiles[x, y]];
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int y){
        return new Vector3(x, y, 0);
    }


    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY){
        TileType tt =  tileTypes[tiles[targetX,targetY]];
        if(UnitCanEnterTile(targetX, targetY) == false){
          return Mathf.Infinity;
        }
        float cost = tt.movementCost;
        if( sourceX!=targetX && sourceY!=targetY){
          cost += 0.001f;
        }
        return cost;
    }


    //Genera la grafica del mapa, y asigna los vecinos de cada casilla.
    public void GeneratePathfindingGraph(){
        graph = new Node[mapSizeX, mapSizeY];
        //Inicializamos los valores de nuestra grafuica
        for (int x = 0; x < mapSizeX; x++){
            for (int y = 0; y < mapSizeY; y++){
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }
        for (int x = 0; x < mapSizeX; x++){
            for (int y = 0; y < mapSizeY; y++){
                if (x > 0)
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                if (x < mapSizeX - 1)
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                if (y < mapSizeY - 1)
                    graph[x, y].neighbours.Add(graph[x, y + 1]);
            }
        }
    }

    /*
     * Metodo que checa si un Enemigo puede entrar al tile
     */
    public bool UnitCanEnterTile(int x, int y){
      if ((x > mapSizeX - 1 || x < 0) || (y > mapSizeY - 1 || y < 0)){
          return false;
      }
      return tileTypes[tiles[x,y]].isWalkableU;
    }
    /*
     * Metodo que checa si el Jugador puede entrar al tile
     */
    public bool canIWalkOnIt(int x, int y){
      if ((x > mapSizeX - 1 || x < 0) || (y > mapSizeY - 1 || y < 0)){
          return false;
      }
      return tileTypes[tiles[x,y]].isWalkable;
    }

    /*
     * Usa Dijkstra para encontra el camino dentre 2 puntos de nuestro mapa
     * OJO: Este solo se implementa para los minions, el jugador se mueve con un
     * contador y las flechas.
    */
    public void GeneratePathTo(int x, int y, GameObject selectedUnit){
        // Borramos el camino antiguo de la unidad
        selectedUnit.GetComponent<Unit>().currentPath = null;

        if(UnitCanEnterTile(x,y) == false){
          return;
        }

        Dictionary<Node, float> distance = new Dictionary<Node, float>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
        List<Node> unvisited = new List<Node>();
        Node source = graph[selectedUnit.GetComponent<Unit>().tileX, selectedUnit.GetComponent<Unit>().tileY];
        Node target = graph[x, y];
        distance[source] = 0;
        previous[source] = null;
        //Inicializa todo los nodos de la grafica.
        foreach (Node v in graph){
            if (v != source){
                distance[v] = Mathf.Infinity;
                previous[v] = null;
            }
            unvisited.Add(v);
        }
        while (unvisited.Count > 0){
            //El nodo u es el de menor peso minimo y sin visitarlo
            Node smallest = null;
            foreach (Node possibleU in unvisited){
                if (smallest == null || distance[possibleU] < distance[smallest]){
                    smallest = possibleU;
                }
            }
            //Si llegamos al target, salimos del loop while.
            if (smallest == target){
                break;
            }
            unvisited.Remove(smallest);
            foreach (Node v in smallest.neighbours){
                float alt = distance[smallest] + CostToEnterTile(smallest.x,smallest.y,v.x,v.y);
                if (alt < distance[v]){
                    distance[v] = alt;
                    previous[v] = smallest;
                }
            }
        }
        //Si llegamos aqui, entonces significa que encontramos el camino
        //optimo al target O no existe una ruta al target.
        if (previous[target] == null){
            Debug.Log("Oh ow! No hay ruta optima!");
            selectedUnit.GetComponent<Unit>().pathFound = false;
            return;
        }

        List<Node> currentPath = new List<Node>();
        Node curr = target;
        while (curr != null){
            currentPath.Add(curr);
            curr = previous[curr];
        }
        //Como el camino describe uan ruta del objetivo al nodo actual, hay que invertir el camnio.
        currentPath.Reverse();
        selectedUnit.GetComponent<Unit>().currentPath = currentPath;
        selectedUnit.GetComponent<Unit>().pathFound = true;
    }

    /*
     * Metodo principal de la accion de patear, este es el metodo que usa el jugador para poder patear obstaculos
     */
    public bool kickObstacle(int obsX , int obsY , int x , int y, Player play){
      TileType tt =  tileTypes[tiles[obsX,obsY]];

      if(tt.name == "Mesa" && play.rageMode){
        SoundController.PlaySound("hit1");
        tiles[obsX,obsY] = 0;
        updateTerreno();
        return true;
      }

      if(tt.name == "Kwmo"){
        SoundController.PlaySound("hit1");
        tiles[obsX,obsY] = 0;
        updateTerreno();
        return true;
      }
      if ((obsX+x > mapSizeX - 1 || obsX+x < 0) || (obsY+y > mapSizeY - 1 || obsY+y < 0)){
        return false;
      }
      if(tt.name == "Mesa"){
        TileType tt2 =  tileTypes[tiles[obsX+x,obsY+y]];
        if(tt2.isMobile == false && tt2.isWalkable == true && tt2.occupied == false){
          SoundController.PlaySound("moveTable");
          tiles[obsX,obsY] = 0;
          tiles[obsX+x,obsY+y] = 1;
          updateTerreno();
          return true;
        }
      }
      return false;
    }

    /*
     * Metodo que sirve para que los enemigos puedan patear al obstaculo "Borracho" que desaparece
     * Y es el unico que pueden interactuar ellos
     */
    public void kickObstacleU(int obsX , int obsY){
      TileType tt =  tileTypes[tiles[obsX,obsY]];
      if(tt.name == "Kwmo"){
            tiles[obsX,obsY] = 0;
        updateTerreno();
      }
    }

    /*
     * Metodo que sirve para checar si el jugador puede interactuar con el obstaculo, ya sea silla o "Borracho"
     */
    public bool canIMoveIt(int mtileX , int mtileY){
      if ((mtileX > mapSizeX - 1 || mtileX < 0) || (mtileY > mapSizeY - 1 || mtileY < 0)) return false;
      TileType tt =  tileTypes[tiles[mtileX,mtileY]];
      return tt.isMobile;
    }

    /*
     * Metodo Auxiliar que ayuda a la ilusion de movimiento cuando el jugador o el enemigo patea algo
     */
    void updateTerreno(){
      GameObject[] enemies = GameObject.FindGameObjectsWithTag("Terreno");
      foreach(GameObject enemy in enemies)
      GameObject.Destroy(enemy);
      GenerateMapVisual();
    }

    /*
     * Metodo que marca los tiles en donde hay ya sea un enemigo o donde esta parado el Jugador
     * Esto sirve para el arreglo de que se podia patear a una mesa a un enemigo
     */
    public void iLeft(int presentx, int presenty, int futurex , int futurey){
      tiles[presentx,presenty] = 0;
      tiles[futurex,futurey] = 4;
      updateTerreno();
    }

    public void poblarObstaculos(int numero){
      int count = numero;
      int intentos = 100;
      while (count > 0 && intentos > 0){
        int randomx = Random.Range(0,mapSizeX);
        int randomy = Random.Range(0,mapSizeY);
        TileType tt = tileTypes[tiles[randomx,randomy]];
        if(!(tt.isMobile) && !(tt.occupied)){
          //if(){} Este if es reservado para cuando cheque los spawners
          listaAlbañil.Add(new Albañil(randomx,randomy));
          count --;
        }
        intentos --;
      }
      foreach(Albañil albañil in listaAlbañil){
        tiles[albañil.posicionX,albañil.posicionY] = 1;
        aplicarObstaculos(albañil);
      }
    }

    public void aplicarObstaculos(Albañil albañil){
      foreach(Vector3 obstaculo in Albañil.listaDePosiblesObstaculos){
        int nuevoObstaculoX = albañil.posicionX + (int)obstaculo.x;
        int nuevoObstaculoY = albañil.posicionY + (int)obstaculo.z;
        if ((nuevoObstaculoX > mapSizeX - 1 || nuevoObstaculoX < 0) || (nuevoObstaculoY > mapSizeY - 1 || nuevoObstaculoY < 0)){
          continue;
        }
        TileType tt = tileTypes[tiles[nuevoObstaculoX,nuevoObstaculoY]];
        if(!(tt.isMobile) && !(tt.occupied)){
          escogerObstaculo(nuevoObstaculoX,nuevoObstaculoY);
        }
      }
    }

    public void escogerObstaculo(int x, int y){
      int chooser = Random.Range(0,100);
      if(chooser < 2){
        tiles[x,y] = 2;
      }else if(chooser < 10){
        tiles[x,y] = 1;
      }else if (chooser < 30){
        randomChungo(x,y);
      }else{
        tiles[x,y] = 0;
      }
    }

    public void randomChungo(int x, int y){
      int chooser = Random.Range(0,5);
      switch(chooser){
        case 0:
          tiles[x,y] = 5;
          break;
        case 1:
          tiles[x,y] = 6;
          break;
        case 2:
          tiles[x,y] = 7;
          break;
        case 3:
          tiles[x,y] = 8;
          break;
        case 4:
          tiles[x,y] = 3;
          break;
      }
    }

    public bool DijkstraForMap(int startX, int startY, int goalX, int goalY){
        Dictionary<Node, float> distance = new Dictionary<Node, float>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
        List<Node> unvisited = new List<Node>();
        Node source = graph[startX, startY];
        Node target = graph[goalX, goalY];
        distance[source] = 0;
        previous[source] = null;
        //Inicializa todo los nodos de la grafica.
        foreach (Node v in graph){
            if (v != source){
                distance[v] = Mathf.Infinity;
                previous[v] = null;
            }
            unvisited.Add(v);
        }
        while (unvisited.Count > 0){
            //El nodo u es el de menor peso minimo y sin visitarlo
            Node smallest = null;
            foreach (Node possibleU in unvisited){
                if (smallest == null || distance[possibleU] < distance[smallest]){
                    smallest = possibleU;
                }
            }
            //Si llegamos al target, salimos del loop while.
            if (smallest == target){
                break;
            }
            unvisited.Remove(smallest);
            foreach (Node v in smallest.neighbours){
                float alt = distance[smallest] + CostToEnterTile(smallest.x,smallest.y,v.x,v.y);
                if (alt < distance[v]){
                    distance[v] = alt;
                    previous[v] = smallest;
                }
            }
        }
        //Si llegamos aqui, entonces significa que encontramos el camino
        //optimo al target O no existe una ruta al target.
        if (previous[target] == null){
            Debug.Log("Oh ow! No hay ruta optima! Para el mapa");
            return false;
        }

        List<Node> currentPath = new List<Node>();
        Node curr = target;
        while (curr != null){
            currentPath.Add(curr);
            curr = previous[curr];
        }
        Debug.Log("Encontramos el camino");
        return true;
    }

    private void npcRulette(float x, float y){
      int npcType = Random.Range(0,110);
      if(npcType < 5 && last != 0){
        last = 0;
        Debug.Log("Monaguillo");
        spawnNPC(x,y,0);
      }else if(npcType < 10 && last != 1){
        last = 1;
        Debug.Log("Papa");
        spawnNPC(x,y,5);
      }else if(npcType < 20 && last != 2){
        last = 2;
        Debug.Log("SadBoi");
        spawnNPC(x,y,4);
      }else if(npcType < 50 && last != 3){
        last = 3;
        Debug.Log("Chaparrita");
        spawnNPC(x,y,3);
      }else if(npcType < 80 && last != 4){
        last = 4;
        Debug.Log("CEO");
        spawnNPC(x,y,2);
      }else if(npcType > 80 && last != 5){
        last = 5;
        Debug.Log("Asesino");
        spawnNPC(x,y,1);
      }else{
        npcRulette(x,y);
      }
    }

    private void spawnNPC(float x, float y, int i){
      NPCGenerico a = npcs[i];
      a.transform.position = new Vector3(x,y, 0f);
      iLeft((int)a.transform.position.x,(int)a.transform.position.y,(int)a.transform.position.x,(int)a.transform.position.y);
      a.enabled = true;
    }
}

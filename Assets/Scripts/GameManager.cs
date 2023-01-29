using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] private Player player;

    [SerializeField] private AudioSource backgroundMusic;

    [SerializeField] private List<GroundTile> groundTiles = new List<GroundTile>();

    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private List<MoveableTile> woodTiles = new List<MoveableTile>();
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private List<MoveableTile> rockTiles = new List<MoveableTile>();
    [SerializeField] private GameObject berryBushPrefab;
    [SerializeField] private List<MoveableTile> berryBushTiles = new List<MoveableTile>();
    [SerializeField] private GameObject berryPrefab;
    [SerializeField] private List<Tile> berryTiles = new List<Tile>();

    [SerializeField] private GameObject enemyPrefab;
    private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private int enemySpawnerCounter;
    private int enemySpawnCounter = 0;
    [SerializeField] private int enemyMoverCounter;
    private int enemyMoveCounter = 0;

    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private List<Tile> waterTiles = new List<Tile>();
    
    private void Awake() {
        backgroundMusic.Play();
        SpawnEnemy(4, 5);
        SpawnRock(2, 0);
        SpawnRock(-4, 0);
        SpawnRock(0, 2);
        SpawnRock(3, 4);
        SpawnRock(-3, 4);
        SpawnRock(5, -1);
        SpawnRock(-1, -3);
        SpawnBerryBush(5, 3);
        SpawnBerryBush(-6, 3);
        SpawnBerryBush(0, -6);
        SpawnWater(-1, 5);
        SpawnWater(-4, -3);
        SpawnWater(4, -4);
    }

    public void Gameover() {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void GameTrigger() {
        enemyMoveCounter += 1;
        if (enemyMoveCounter == enemyMoverCounter) {
            for (int i = 0; i < enemies.Count; i++) { enemies[i].Move(); }
            enemyMoveCounter = 0;
        }
        for (int i = 0; i < berryBushTiles.Count; i++) {
            if (Random.Range(0, 100) > 80) {
                int x = Random.Range(0, 2);
                int y = Random.Range(0, 2);
                if (Obstructed(berryBushTiles[i].x + x, berryBushTiles[i].y + y)) { continue; }
                if (IsMoveable(berryBushTiles[i].x + x, berryBushTiles[i].y + y)) { continue; }
                if (IsBerry(berryBushTiles[i].x + x, berryBushTiles[i].y + y)) { continue; }
                if (IsSaltWater(berryBushTiles[i].x + x, berryBushTiles[i].y + y)) { continue; }
                SpawnBerry(berryBushTiles[i].x + x, berryBushTiles[i].y + y);
            }
        }
        enemySpawnCounter += 1;
        if (enemySpawnCounter == enemySpawnerCounter) {
            Debug.Log("enemy spawn attempt");
            enemySpawnCounter = 0;
            int x = Random.Range(-8, 8);
            int y = Random.Range(-8, 8);
            if (Obstructed(x, y)) { return; }
            if (IsMoveable(x,  y)) { return; }
            if (IsBerry(x, y)) { return; }
            if (IsSaltWater(x, y)) { return; }
            if (x == player.x || y == player.y) { return; }
            SpawnEnemy(x, y);
        }
    }

    public bool IsSaltWater(int x, int y) {
        for (int i = 0; i < groundTiles.Count; i++) { if (x == groundTiles[i].x && y == groundTiles[i].y) { return false; } }
        return true;
    }

    public bool IsBerry(int x, int y) {
        for (int i = 0; i < berryTiles.Count; i++) { if (x == berryTiles[i].x && y == berryTiles[i].y) { return true; } }
        return false;
    }

    public void DestroyBerry(int x, int y) {
        int index = -1;
        for (int i = 0; i < berryTiles.Count; i++) {
            if (x == berryTiles[i].x && y == berryTiles[i].y) {
                index = i;
                break;
            }
        }
        if (index != -1) {
            Destroy(berryTiles[index].gameObject);
            berryTiles.RemoveAt(index);
        }
    }

    public bool IsWater(int x, int y) {
        for (int i = 0; i < waterTiles.Count; i++) { if (x == waterTiles[i].x && y == waterTiles[i].y) { return true; } }
        return false;
    }

    public bool IsEnemy(int x, int y) {
        for (int i = 0; i < enemies.Count; i++) { if (x == enemies[i].x && y == enemies[i].y) { return true; } }
        return false;
    }

    public bool Obstructed(int x, int y) {
        if (IsSaltWater(x, y)) { return true; }
        for (int i = 0; i < waterTiles.Count; i++) { if (x == waterTiles[i].x && y == waterTiles[i].y) { return true; } }
        return false;
    }

    public bool IsMoveable(int x, int y) {
        for (int i = 0; i < woodTiles.Count; i++) { if (x == woodTiles[i].x && y == woodTiles[i].y) { return true; } }
        for (int i = 0; i < rockTiles.Count; i++) { if (x == rockTiles[i].x && y == rockTiles[i].y) { return true; } }
        for (int i = 0; i < berryBushTiles.Count; i++) { if (x == berryBushTiles[i].x && y == berryBushTiles[i].y) { return true; } }
        return false;
    }

    public MoveableTile GetMoveableTile(int x, int y) {
        for (int i = 0; i < woodTiles.Count; i++) { if (x == woodTiles[i].x && y == woodTiles[i].y) { return woodTiles[i]; } }
        for (int i = 0; i < rockTiles.Count; i++) { if (x == rockTiles[i].x && y == rockTiles[i].y) { return rockTiles[i]; } }
        for (int i = 0; i < berryBushTiles.Count; i++) { if (x == berryBushTiles[i].x && y == berryBushTiles[i].y) { return berryBushTiles[i]; } }
        return null;
    }

    public void SpawnEnemy(int x, int y) {
        GameObject enemyObject = Instantiate(enemyPrefab, new Vector3(x, y, -2), Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemy.player = player;
        enemy.x = x;
        enemy.y = y;
        enemy.gameManager = this;
        enemies.Add(enemy);
    }

    public void SpawnWood(int x, int y) {
        GameObject woodObject = Instantiate(rockPrefab, new Vector2(x, y), Quaternion.identity);
        MoveableTile wood = woodObject.GetComponent<MoveableTile>();
        wood.x = x;
        wood.y = y;
        wood.gameManager = this;
        woodTiles.Add(wood);
    }

    public void SpawnRock(int x, int y) {
        GameObject rockObject = Instantiate(rockPrefab, new Vector2(x, y), Quaternion.identity);
        MoveableTile rock = rockObject.GetComponent<MoveableTile>();
        rock.x = x;
        rock.y = y;
        rock.gameManager = this;
        rockTiles.Add(rock);
    }

    public void SpawnBerryBush(int x, int y) {
        GameObject berryBushObject = Instantiate(berryBushPrefab, new Vector2(x, y), Quaternion.identity);
        MoveableTile berryBush = berryBushObject.GetComponent<MoveableTile>();
        berryBush.x = x;
        berryBush.y = y;
        berryBush.gameManager = this;
        berryBushTiles.Add(berryBush);
    }

    public void SpawnBerry(int x, int y) {
        GameObject berryObject = Instantiate(berryPrefab, new Vector2(x, y), Quaternion.identity);
        Tile berry = berryObject.GetComponent<Tile>();
        berry.x = x;
        berry.y = y;
        berryTiles.Add(berry);
    }

    public void SpawnWater(int x, int y) {
        GameObject waterObject = Instantiate(waterPrefab, new Vector2(x, y), Quaternion.identity);
        Tile water = waterObject.GetComponent<Tile>();
        water.x = x;
        water.y = y;
        waterTiles.Add(water);
    }
}

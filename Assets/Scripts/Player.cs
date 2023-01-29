using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioSource eatSFX;
    [SerializeField] private AudioSource drinkSFX;
    
    public int x;
    public int y;

    public int hunger; // 0-5
    private int hungerTick;
    public int hungerTickMax;
    public int thirst; // 0-5
    private int thirstTick;
    public int thirstTickMax;

    public GameObject playerObject;
    public GameObject hungerView;
    public GameObject[] hungerLevels;
    public GameObject thirstView;
    public GameObject[] thirstLevels;

    private void Awake() {
        x = 0;
        y = 0;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            Move(0, 1);
        } else if (Input.GetKeyDown(KeyCode.A)) {
            Move(-1, 0);
        } else if (Input.GetKeyDown(KeyCode.S)) {
            Move(0, -1);
        } else if (Input.GetKeyDown(KeyCode.D)) {
            Move(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            playerObject.SetActive(true);
            hungerView.SetActive(false);
            thirstView.SetActive(false);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            playerObject.SetActive(false);
            hungerView.SetActive(true);
            thirstView.SetActive(false);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            playerObject.SetActive(false);
            hungerView.SetActive(false);
            thirstView.SetActive(true);
        }
    }

    private void ChangeHunger(int amount) {
        hunger += amount;
        if (amount < 0) { eatSFX.Play(); }
        for (int i = 0; i < hungerLevels.Length; i++) {
            if (i == hunger) {
                hungerLevels[i].SetActive(true);
            } else {
                hungerLevels[i].SetActive(false);
            }
        }
    }

    private void ChangeThirst(int amount) {
        thirst += amount;
        if (amount < 0) { drinkSFX.Play(); }
        for (int i = 0; i < thirstLevels.Length; i++) {
            if (i == thirst) {
                thirstLevels[i].SetActive(true);
            } else {
                thirstLevels[i].SetActive(false);
            }
        }
    }

    private void Move(int x, int y) {
        gameManager.GameTrigger();
        if (gameManager.IsEnemy(this.x + x, this.y + y)) {
            Debug.Log("DIE OF ENEMY!");
            gameManager.Gameover();
        }
        if (gameManager.Obstructed(this.x + x, this.y + y)) {
            if (gameManager.IsWater(this.x + x, this.y + y)) { if (thirst > 0) { ChangeThirst(-1); thirstTick = 0; } }
            return;
        }
        if (gameManager.IsMoveable(this.x + x, this.y + y)) {
            MoveableTile tile = gameManager.GetMoveableTile(this.x + x, this.y + y);
            tile.Move(x, y);
        }
        this.x += x;
        this.y += y;
        transform.position = new Vector2(this.x, this.y);

        if (gameManager.IsBerry(this.x, this.y)) { if (hunger > 0) { ChangeHunger(-1); hungerTick = 0; } gameManager.DestroyBerry(this.x, this.y); }

        hungerTick += 1;
        if (hungerTick > hungerTickMax) { ChangeHunger(1); hungerTick = 0; }
        thirstTick += 1;
        if (thirstTick > thirstTickMax) { ChangeThirst(1); thirstTick = 0; }

        if (hunger > 5) { Debug.Log("DIE OF HUNGER!"); gameManager.Gameover(); }
        if (thirst > 5) { Debug.Log("DIE OF THIRST!"); gameManager.Gameover(); }
    }
}

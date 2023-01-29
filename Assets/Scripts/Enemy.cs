using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public GameManager gameManager;
    public Player player;
    public int x;
    public int y;

    public void Move() {
        if (x > player.x && Random.Range(0, 100) > 40) {
            if (gameManager.GetMoveableTile(x - 1, y) != null || gameManager.Obstructed(x - 1, y)) {} else {
                x -= 1;
                transform.position = new Vector3(this.x, this.y, -2);
                if (this.y == player.y && this.x == player.x) { Debug.Log("DIE OF ENEMY!"); gameManager.Gameover(); }
                return;
            }
        }
        
        if (x < player.x && Random.Range(0, 100) > 40) {
            if (gameManager.GetMoveableTile(x + 1, y) != null || gameManager.Obstructed(x + 1, y)) {} else {
                x += 1;
                transform.position = new Vector3(this.x, this.y, -2);
                if (this.y == player.y && this.x == player.x) { Debug.Log("DIE OF ENEMY!"); gameManager.Gameover(); }
                return;
            }
        }
        
        if (y > player.y && Random.Range(0, 100) > 40) {
            if (gameManager.GetMoveableTile(x, y - 1) != null || gameManager.Obstructed(x, y - 1)) {} else {
                y -= 1;
                transform.position = new Vector3(this.x, this.y, -2);
                if (this.y == player.y && this.x == player.x) { Debug.Log("DIE OF ENEMY!"); gameManager.Gameover(); }
                return;
            }
        }
        
        if (y < player.y && Random.Range(0, 100) > 40) {
            if (gameManager.GetMoveableTile(x, y + 1) != null || gameManager.Obstructed(x, y + 1)) {} else {
                y += 1;
                transform.position = new Vector3(this.x, this.y, -2);
                if (this.y == player.y && this.x == player.x) { Debug.Log("DIE OF ENEMY!"); gameManager.Gameover(); }
                return;
            }
        }
    }
}

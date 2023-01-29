using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableTile : MonoBehaviour {

    public GameManager gameManager;
    public int x;
    public int y;

    public void Move(int x, int y) {
        if (gameManager.IsMoveable(this.x + x, this.y + y)) {
            MoveableTile tile = gameManager.GetMoveableTile(this.x + x, this.y + y);
            tile.Move(x, y);
        }
        this.x += x;
        this.y += y;
        transform.position = new Vector2(this.x, this.y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour {
    public int x;
    public int y;

    private void Awake() {
        x = (int) transform.position.x;
        y = (int) transform.position.y;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public PlayerController player;
    public bool reverseTower;

    void OnMouseDown()
    {
        player.Pull(this, reverseTower);
    }

    void OnMouseUp()
    {
        player.StopPulling();
    }
}

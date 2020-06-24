using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBulletLeave : MonoBehaviour
{
    public GameObject player;
    PlayerController pl;

    void Start()
    {
        pl = player.GetComponent<PlayerController>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "enem_bullet")
        {
            pl.SetBoostPowerUpDurations(3);
        }
    }
}

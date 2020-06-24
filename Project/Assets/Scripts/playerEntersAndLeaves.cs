using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEntersAndLeaves : MonoBehaviour
{
    public GameController gameGontroller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameGontroller.ChangePlayerIsInRangeOfHome(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            gameGontroller.ChangePlayerIsInRangeOfHome(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpChunk : MonoBehaviour
{

    public GameController gameController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chunk")
        {
            Destroy(other.gameObject);
            gameController.AddGold(10);   
        }
        if (other.tag == "scrap_chunk")
        {
            Destroy(other.gameObject);
            gameController.AddScrap(10);
        }
    }
}

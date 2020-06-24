using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroySpawnChunks : MonoBehaviour
{
    [SerializeField] private GameObject chunk;
    [SerializeField] private Vector2 amountOfChunksMinMax;


    void OnDestroy()
    {
        int randint = (int)(Mathf.Floor(Random.Range(amountOfChunksMinMax.x, amountOfChunksMinMax.y)));
        for (int i = 0; i < randint; i++)
        {
            GameObject ch = Instantiate(chunk, transform.position, transform.rotation);
            Rigidbody rb = ch.GetComponent<Rigidbody>();
            rb.velocity = Random.onUnitSphere;
            print(rb.velocity);
        }
        
    }
}

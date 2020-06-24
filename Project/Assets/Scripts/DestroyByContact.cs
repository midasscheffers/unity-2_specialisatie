using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue = 10;
    private GameController gameController;


    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Station" || other.tag == "unDestroyable")
        {
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        
        if (other.tag == "Player")
        {
            gameController.GameOver();
            //Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
        }
        
         Destroy(other.gameObject);
         Destroy(gameObject);
        
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_shooter_behaviour : MonoBehaviour
{


    public Transform shotSpawn;
    public GameObject shot;
    public int shotReload = 300;
    public int maxShotReload = 300;
    public int shoot_dist = 30;
    public int min_dis_to_player = 10;

    public float speed = .1f;

    public GameController gameController;
    GameObject player;


    void Start()
    {
        if (gameController == null)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
        player = GameObject.Find("Player_ship");
    }

    private void OnDestroy()
    {
        gameController.AddScore(100);
    }

    void Update()
    {
        if (gameController.gameState == GameController.GameState.play)
        {

            // movement
            Transform targetTrans = player.transform;
            Vector3 offset = new Vector3(0, 0, 0);
            float followSpeed = speed;
            float rotationSpeed = 10.0f;

            if (Vector3.Distance(targetTrans.position, transform.position) > min_dis_to_player)
            {
                Vector3 disPos = targetTrans.position + (targetTrans.rotation * offset);
                transform.position = Vector3.Lerp(transform.position, disPos, followSpeed * Time.deltaTime);
            }
            
            Quaternion disRot = Quaternion.LookRotation(targetTrans.position - transform.position, targetTrans.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, disRot, rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(targetTrans.position, transform.position) < shoot_dist)
            {
                if (shotReload < 1)
                {
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                    shotReload = maxShotReload;
                }
                else
                {
                    shotReload -= 1;
                }
            }
        }
    }
}

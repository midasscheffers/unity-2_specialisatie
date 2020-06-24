using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_miner_behaviour : MonoBehaviour
{
    
    enum Miner_State
    {
        Looking_for_rock,
        Mining,
        Moving_to_rock
    }

    Miner_State state = Miner_State.Looking_for_rock;

    GameObject targetAstroid = null;
    int destroyTicks = 0;
    public GameController gameController;

    void Start()
    {
        if (gameController == null)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
    }

    private void OnDestroy()
    {
        gameController.AddScore(25);
    }

    void Update()
    {   
        if (gameController.gameState == GameController.GameState.play)
        {
            if (targetAstroid == null)
            {
                state = Miner_State.Looking_for_rock;
            }

            if (state == Miner_State.Looking_for_rock)
            {
                targetAstroid = GameObject.FindGameObjectWithTag("Astroid");
                state = Miner_State.Moving_to_rock;
            }

            else if (state == Miner_State.Mining)
            {
                destroyTicks += 1;
                if (destroyTicks > 240)
                {
                    Destroy(targetAstroid);
                    destroyTicks = 0;
                    state = Miner_State.Looking_for_rock;
                }
            }

            else if (state == Miner_State.Moving_to_rock)
            {
                Transform targetTrans = targetAstroid.transform;
                Vector3 offset = new Vector3(2, 0, 0);
                float followSpeed = .125f;
                float rotationSpeed = 10.0f;

                Vector3 disPos = targetTrans.position + (targetTrans.rotation * offset);
                transform.position = Vector3.Lerp(transform.position, disPos, followSpeed * Time.deltaTime);

                Quaternion disRot = Quaternion.LookRotation(targetTrans.position - transform.position, targetTrans.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, disRot, rotationSpeed * Time.deltaTime);
                Vector3 our_round = new Vector3(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y), Mathf.Floor(transform.position.z));
                Vector3 target_round = new Vector3(Mathf.Floor(targetTrans.position.x), Mathf.Floor(targetTrans.position.y), Mathf.Floor(targetTrans.position.z));
                if (our_round == target_round + offset)
                {
                    state = Miner_State.Mining;
                }
            }
        }
    }
}

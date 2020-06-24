using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTrans;


    public Vector3 offset;

    [SerializeField] private float followSpeed = 0.125f;
    [SerializeField] private float rotationSpeed = 10.0f;


    void Start()
    {
        offset = transform.position - playerTrans.position;
    }

    void FixedUpdate()
    {
        Vector3 disPos = playerTrans.position + (playerTrans.rotation * offset);
        transform.position = Vector3.Lerp(transform.position, disPos, followSpeed * Time.deltaTime);

        Quaternion disRot = Quaternion.LookRotation(playerTrans.position - transform.position, playerTrans.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, disRot, rotationSpeed * Time.deltaTime);
    }
}

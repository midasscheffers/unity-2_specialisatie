using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public GameController gameController;

    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float rotSpeed_accel;
    [SerializeField] private float maxRotSpeed_accel;
    [SerializeField] private int boostPower = 5;

    private Vector3 rotationLast;
    private Vector3 rotationDelta;
    private Vector3 rot_accel;
    private Vector3 rot_vel;

    private int boost_power_up_duration = 0;
    private int boost;

    [SerializeField] private int movment_mode = 0;

    [SerializeField] private bool use_rot_accel;

    [SerializeField] private GameObject shot;
    [SerializeField] private Transform shotSpawn;
    [SerializeField] private float shotWait = 0.7f;
    private float nextFire;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotationLast = transform.rotation.eulerAngles;
    }

    void Update()
    {
        if (gameController.gameState == GameController.GameState.play){

            // shooting
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + shotWait;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            }
            // movement
                // boost
            if (boost_power_up_duration < 1)
            {
                boost = 1;
            }
            else
            {
                boost = boostPower;
                boost_power_up_duration -= 1;
                print(boost_power_up_duration);
            }
                //rest of movement
            switch (movment_mode)
            {
                case 0:
                    rot_accel = (transform.up * Input.GetAxis("Horizontal") * rotSpeed_accel * Time.deltaTime + transform.right * Input.GetAxis("Pitch") * rotSpeed_accel * Time.deltaTime + transform.forward * Input.GetAxis("Roll") * rotSpeed_accel * Time.deltaTime);
                    rot_vel = rot_vel + rot_accel;
                    rot_vel = new Vector3(
                        Mathf.Clamp(rot_vel.x, -maxRotSpeed_accel, maxRotSpeed_accel),
                        Mathf.Clamp(rot_vel.y, -maxRotSpeed_accel, maxRotSpeed_accel),
                        Mathf.Clamp(rot_vel.z, -maxRotSpeed_accel, maxRotSpeed_accel)
                    );
                    rb.AddForce(transform.forward * Input.GetAxis("Jump") * speed * Time.deltaTime * boost);
                    break;
                case 1:
                    rot_vel = (transform.up * Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime + transform.right * Input.GetAxis("Pitch") * rotSpeed * Time.deltaTime + transform.forward * Input.GetAxis("Roll") * rotSpeed * Time.deltaTime);
                    rb.AddForce(transform.forward * Input.GetAxis("Jump") * speed * Time.deltaTime * boost);
                    break;
                case 2:
                    rot_vel = (transform.up * Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime + transform.right * Input.GetAxis("Pitch") * rotSpeed * Time.deltaTime + transform.forward * Input.GetAxis("Roll") * rotSpeed * Time.deltaTime);

                    if (Input.GetAxis("Jump") == 0)
                    {
                        rb.velocity = new Vector3(rb.velocity.x * .0001f, rb.velocity.y * .0001f, rb.velocity.z * .0001f);
                    }
                    else
                    {
                        rb.AddForce(transform.forward * Input.GetAxis("Jump") * speed * Time.deltaTime * boost);
                    }
                    break;
            }

            rb.angularVelocity = new Vector3(0, 0, 0);
            transform.Rotate(rot_vel, Space.World);

            rb.velocity = new Vector3(
                Mathf.Clamp(rb.velocity.x, -maxSpeed * boost, maxSpeed * boost),
                Mathf.Clamp(rb.velocity.y, -maxSpeed * boost, maxSpeed * boost),
                Mathf.Clamp(rb.velocity.z, -maxSpeed * boost, maxSpeed * boost)
            );
        }
    }

    public void ChangeMovementMode(int mode)
    {
        movment_mode = mode;
    }

    public void ChangeReloadTime(float change)
    {
        shotWait += change;
    }

    public void SetBoostPowerUpDurations(int sec)
    {
        boost_power_up_duration = 60 * sec;
    }
}

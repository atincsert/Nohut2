using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public static Action OnWaterTouch;
    public static Action OnPlatformCollision;
    public static Action OnDrown;

    [SerializeField] private float forwardSpeed;
    [SerializeField] private Vector3 specialDownspeedValues = new Vector3(0f, -100f, 10f);
    [SerializeField] private Vector3 specialUpSpeedValues = new Vector3(0f, 50f, 10f);
    [SerializeField] private float velocityThreshold;

    private Rigidbody rb;
    private Vector3 downVelocity;
    private Vector3 upVelocity;
    [HideInInspector] public static bool hasTouchedWater;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        hasTouchedWater = false;
        rb.AddForce(Vector3.forward * forwardSpeed, ForceMode.Impulse);
    }

    private void OnEnable()
    {
        BallMovement.OnPlatformCollision += Bounce;
        BallMovement.OnWaterTouch += Drown;
    }

    private void OnDisable()
    {
        BallMovement.OnPlatformCollision -= Bounce;        
        BallMovement.OnWaterTouch -= Drown;
    }

    private void FixedUpdate()
    {
        SpeedUpDownwardsWhenHoldTouch();
        CheckForVelocityReduceDownForce();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == 4)
    //    {
    //        //Game Over send an event to restart of game over screen etc.
    //        //DeathCondition();
    //        OnWaterTouch?.Invoke();
    //        Debug.Log($"{ collision.gameObject.name }");
    //    }
    //    if (collision.gameObject.layer == 6)
    //    {
    //        rb.velocity += Vector3.up;            
    //    }               
    //}

    //private static void DeathCondition()
    //{
    //    hasTouchedWater = true;
    //}

    private void SpeedUpDownwardsWhenHoldTouch()
    {
        if (Input.GetMouseButton(0))
        {
            downVelocity = rb.velocity;
            downVelocity += specialDownspeedValues * Time.deltaTime;
            rb.velocity = downVelocity;
        }

        downVelocity = rb.velocity;
    }

    private void CheckForVelocityReduceDownForce()
    {
        upVelocity = rb.velocity;
        if (upVelocity.y > velocityThreshold)
        {
            upVelocity -= specialUpSpeedValues * Time.deltaTime;
        }
        else
            upVelocity = rb.velocity;

        rb.velocity = upVelocity;
    }

    private void Bounce()
    {
        rb.velocity += Vector3.up;
    }

    private void Drown()
    {
        PointSystem.endZPos = (int)transform.position.z;
        BallMovement.OnDrown?.Invoke();
        gameObject.SetActive(false);
    }
}

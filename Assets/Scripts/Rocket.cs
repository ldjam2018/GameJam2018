using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float maxSpeed;
    public float maxForce;
    public float arrivalRadius;        
    public float pathRadius;
    public float explodeDistance = 1000f;
   
    private Rigidbody rigidbody;
    private GameObject enemyTarget;

    public GameObject explosion;

    // Use this for initialization
    void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }

    public void SetTarget(GameObject target)
    {
        this.enemyTarget = target;
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        Wander();

        Seek(enemyTarget.transform.position);
        AvoidObstacles();
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
        transform.LookAt(transform.position + rigidbody.velocity);
    }

    public void Seek(Vector3 target)
    {
        Vector3 desired = (target - transform.position);
        float squaredDistance = desired.sqrMagnitude;
        desired.Normalize();

        //slow the boid down as it approaches its destination
        if (squaredDistance < (arrivalRadius * arrivalRadius))
        {
            desired *= (Map(squaredDistance, 0, arrivalRadius, 0, maxForce));
        }
        else
        {
            desired *= maxSpeed;
        }
        
        if(Vector3.Distance(enemyTarget.transform.position, transform.position) < explodeDistance)
        {
            Explode();
        }

        Vector3 steer = Vector3.ClampMagnitude((desired - rigidbody.velocity), maxForce);
        rigidbody.AddForce(steer);
    }

     public void WanderSeek(Vector3 target)
    {
        Vector3 desired = (target - transform.position);
        float squaredDistance = desired.sqrMagnitude;

        desired.Normalize();
        //slow the object down as it approaches its destination
        if (squaredDistance < (arrivalRadius * arrivalRadius))
        {
            desired *= (Map(squaredDistance, 0, arrivalRadius, 0, maxForce));
        }
        else
        {
            desired *= maxSpeed;
        }

        Vector3 steer = Vector3.ClampMagnitude((desired - rigidbody.velocity), maxForce);


        rigidbody.AddForce(0.15f*steer);


    }  

    public void Wander()
    {
        Vector3 predictedPoint = PredictedLocation(100);
        //TODO this is not the exact behaviour you want. 
        Vector3 randomDirection = predictedPoint + (Random.onUnitSphere.normalized * 30);        
        WanderSeek(new Vector3(randomDirection.x, transform.position.y, randomDirection.z));
    }  


    //maps a value from one range to another
    private float Map(float value, float originalMin, float originalMax, float newMin, float newMax)
    {
        return (newMin + (value - originalMin) * (newMax - newMin) / (originalMax - originalMin));
    }

    private Vector3 PredictedLocation(float distance)
    {
        return transform.position + (rigidbody.velocity.normalized * distance);
    }

    //current vectors of up down left and right raycastes, which will look for a way around obstacles
    private Vector3 lookUp;
    private Vector3 lookDown;
    private Vector3 lookLeft;
    private Vector3 lookRight;

    //current up doen left values within vectors
    float up;
    float down;
    float left;
    float right;
    float z;
    float step = 0.4f;

    bool colliding;

    public void AvoidObstacles()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);
        RaycastHit hit;

        //test new func
        if (Physics.Raycast(transform.position, forward, out hit, 10) && hit.transform.tag != "CheckPoint")
            if (FindRoute() != Vector3.zero)
            {
                ObstacleSeek(FindRoute());
            }
    }

    public void ObstacleSeek(Vector3 target)
    {
        float maxSpeed = 75f;
        float maxForce = 75f;
        Vector3 desired = (target - transform.position);
        float squaredDistance = desired.sqrMagnitude;

        desired.Normalize();

        desired *= maxSpeed;

        Vector3 steer = Vector3.ClampMagnitude((desired -
        GetComponent<Rigidbody>().velocity), maxForce);

        //GetComponent<Rigidbody>().velocity = Vector3.zero;

        GetComponent<Rigidbody>().AddForce(steer*3);//should be 1 for birds
    }


    private Vector3 FindRoute()
    {

        if (!colliding)
        {
            up = 0;
            down = 0;
            left = 0;
            right = 0;
            z = 1;
            step = 0.3f;
            colliding = true;
        }
        while (colliding)

        {

            if (right <= 1) //while not looking directly up
            {
                lookRight = transform.TransformDirection(new Vector3(right += step, 0, z) * 25);
                // Debug.DrawRay(transform.position, lookRight, Color.blue);
                if (!(Physics.Raycast(transform.position, lookRight, 10)))
                {
                    colliding = false;
                    Debug.DrawRay(transform.position, lookRight, Color.blue);
                    return (transform.position + lookRight);
                }

            }

            if (left <= 1) //while not looking directly up
            {
                lookLeft = transform.TransformDirection(new Vector3(left -= step, 0, z) * 25);
                //   Debug.DrawRay(transform.position, lookLeft, Color.magenta);
                if (!(Physics.Raycast(transform.position, lookLeft, 10)))
                {
                    colliding = false;
                    Debug.DrawRay(transform.position, lookRight, Color.magenta);
                    return (transform.position + lookLeft);
                }


            }

            z -= step;
            if (z <= 0)
            {
                colliding = false;
            }
        }
        return Vector3.zero;
    }
}


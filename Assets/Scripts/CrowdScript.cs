using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MemberStates { Seek, Flee}
public class CrowdScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float turnSpeed;
    public Rigidbody rb;
    private float speedLimit;
    Raycaster caster;
    MemberStates state;
    public float maxAhead;
    public float maxAheadForce;

    public List<Vector3> Obstacles;
    public float obstacleAvoidance;


    public MemberStates State
    {
        get { return state; }
        set { state = value; }
    }

    void Start()
    {
        turnSpeed = 50;
        speedLimit = 10;
        rb = gameObject.GetComponent<Rigidbody>();
        caster = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Raycaster>();
        state = MemberStates.Seek;
        Obstacles = new List<Vector3>();
        maxAhead = 20;
        maxAheadForce = 120;
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Obstacles");
        for(int i = 0; i < obj.Length; i++)
        {
            Obstacles.Add(obj[i].transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case MemberStates.Seek:
                rb.AddForce(Seek(caster.HitPoint));
                break;
            case MemberStates.Flee:
                rb.AddForce(Flee(caster.HitPoint));
                break;

        }

        AvoidObstacles();
        if (rb.velocity.magnitude > speedLimit)
        {
            Vector3 newVelocity = rb.velocity;
            newVelocity.Normalize();
            rb.velocity = newVelocity * speedLimit;

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            state = MemberStates.Flee;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            state = MemberStates.Seek;
        }

    }
  

    public Vector3 Seek(Vector3 target)
    {
        Vector3 force = target - transform.position;
        force.Normalize();
        force *= turnSpeed;
        force.y = 0f;
        return force;
    }

    public Vector3 Flee(Vector3 target)
    {
        Vector3 force = target - transform.position;
        force.Normalize();
        force *= turnSpeed;
        force.y = 0;
        return -force;
    }

    public void AvoidObstacles()
    {

        Vector3 ahead = transform.position + rb.velocity;
        for(int i = 0; i < Obstacles.Count; i++)
        {
            if(Vector3.Distance(ahead, Obstacles[i]) < 15f)
            {
               
                rb.AddForce((ahead - Obstacles[i]).normalized * maxAheadForce);
            }

            
        }

    }
}

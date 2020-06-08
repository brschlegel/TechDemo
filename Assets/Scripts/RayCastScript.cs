using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RayCastScript : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera cam;
    private Ray mouseRay;
    private RaycastHit rayHit;
    private bool hit;
    public NavMeshAgent agent;

    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        agent = GameObject.Find("Agent").GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        hit = Physics.Raycast(mouseRay, out rayHit, 1000.0f);

        if(Input.GetMouseButtonDown(0))
        {
            if(hit)
            {
                agent.SetDestination(rayHit.point);
                
           
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Scripting.APIUpdating;
using System.Data;

public delegate void LogChange();

public class VertexManager : MonoBehaviour
{
    //The vertex manager is primarily worried about creating and moving vertices as well as adding them to the linked list

    public GameObject vertexPrefab;
    public GameObject linePrefab;
    private Raycaster cast;

    //Linked list stuff
    public Vertex head;
    public Vertex tail;
    public int count;
    //height at which the vertices are instantiated
    public float height;
  
    public LogChange logger;

    void Start()
    {
        cast = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Raycaster>();
        head = null;
        tail = null;
        height = 1;
        count = 0;
        logger = gameObject.GetComponent<UndoRedo>().logger;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //returning value is whether or not something has actually been created
    //Not a coroutine because I don't want things to be selected during creation process
    public bool Create()
    {
        //creating a new vertex
        if (Input.GetMouseButtonDown(0))
        {
            //Linked list
            Vector3 vectPosition = new Vector3(cast.HitPoint.x, height, cast.HitPoint.z);
           
            Vertex v = Instantiate(vertexPrefab, vectPosition, Quaternion.identity, transform).GetComponent<Vertex>();

            
            if (head == null)
            {
                v.inVertex = null;
                head = v;
                tail = head;
              

            }
            else
            {
                v.inVertex = tail;
                tail.OutVertex = v;
                tail = tail.OutVertex;
            }
            count++;

            if (v.inVertex != null)
            {
                //sets up a line between previous vertex and this one
                //The line is owned by the PREVIOUS vertex, and is parented under it in unity
                LineRenderer l = Instantiate(linePrefab, v.inVertex.gameObject.transform).GetComponent<LineRenderer>();
                //Lines are set to take in gradients, so you have to set start and end
                l.startColor = cast.CurrentColor;
                l.endColor = cast.CurrentColor;

                Vector3[] vertexPositions = { v.inVertex.gameObject.transform.position, v.transform.position };
                l.SetPositions(vertexPositions);
                //creates capsule, the capsule will be morphed to the line in UpdateConnections 
                v.inVertex.Capsule = l.gameObject.GetComponent<CapsuleCollider>();



            }
          
            UpdateConnections(v);
            logger();
            return true;

        }
        return false;
    }


    //Coroutine for moving vertices
    IEnumerator MoveVert(GameObject vMove)
    {
        while (!Input.GetMouseButtonUp(0))
        {
            Vector3 point = cast.HitPoint;
            vMove.transform.position = new Vector3(point.x, height, point.z);
            UpdateConnections(vMove.GetComponent<Vertex>());
            yield return null;
        }
        logger();
        StopCoroutine("MoveVert");

    }

    public void UpdateConnections(Vertex v)
    {
        if (v.inVertex != null && v.inVertex.Capsule != null)
        {
            //changes the line position, and moves the collider along with it
            Vector3[] vertexPositions = { v.inVertex.gameObject.transform.position, v.transform.position };
            LineRenderer l = v.inVertex.gameObject.transform.GetChild(0).GetComponent<LineRenderer>();
            l.SetPositions(vertexPositions);
            FitCapsuleToLine(v.inVertex.Capsule, l);
        }

        if (v.outVertex != null && v.Capsule != null)
        {
            Vector3[] vertexPositions = {  v.transform.position, v.outVertex.gameObject.transform.position };

            LineRenderer l = v.transform.GetChild(0).GetComponent<LineRenderer>();
            l.SetPositions(vertexPositions);

            FitCapsuleToLine(v.Capsule, l);

        }
    }

    public void RestoreConnections()
    {
        foreach(Vertex v in transform.GetComponentsInChildren<Vertex>())
        {
            UpdateConnections(v);
        }
    }

    public void Delete(GameObject toBeDeleted)
    {
        Vertex v = toBeDeleted.GetComponent<Vertex>();
        //deleting only one
        if(count <= 1)
        {
            head = null;
            tail = null;
        }
        //deleting first one
        else if(v.inVertex == null)
        {
            head = v.outVertex;
            head.inVertex = null;
        }
        //deleting last one
        else if(v.OutVertex == null)
        {
            tail = v.inVertex;
            tail.outVertex = null;
            DestroyImmediate(tail.transform.GetChild(0).gameObject);
        }
        else
        {
            v.inVertex.outVertex = v.outVertex;
            v.outVertex.inVertex = v.inVertex;
            UpdateConnections(v.InVertex);
        }
        count--;

       // toBeDeleted.transform.position = Vector3.zero;

        DestroyImmediate(toBeDeleted);
        logger();
        
       

    }
    public void FitCapsuleToLine(CapsuleCollider capsule, LineRenderer line)
    {
        Vector3[] positions = new Vector3[2];
        line.GetPositions(positions);

        Vector3 offset = (positions[1] - positions[0]) / 2;
        //moves center of capsule
        capsule.transform.position = positions[0] + offset;
        //Rotates capsule
        capsule.transform.LookAt(positions[1]);
        //Scales capsule
        capsule.height = (positions[0] - positions[1]).magnitude * 2;
    }

}

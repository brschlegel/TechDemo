using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{

    //Vertices worry about their connections, as well as moving themselves
    public float height;
    public bool selected;
    private Raycaster cast;

    public GameObject linePrefab;
    
    public Vertex outVertex;
    public Vertex inVertex;
    public List<LineRenderer> connections;
    CapsuleCollider capsule;
    #region Properties
    public bool Selected
    {
        get { return selected; }
        set { selected = value; }

    }

    public Vertex OutVertex
    {
        get { return outVertex; }
        set { outVertex = value; }
    }

    public Vertex InVertex
    {
        get { return inVertex; }
        set { inVertex = value; }
    }

    #endregion

    void Start()
    {
        cast = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Raycaster>();
        height = 1;
        if(inVertex != null)
        {
            //sets up a line between previous vertex and this one
            //The line is owned by the PREVIOUS vertex, and is parented under it in unity
            LineRenderer l = Instantiate(linePrefab, inVertex.gameObject.transform).GetComponent<LineRenderer>();
            //Lines are set to take in gradients, so you have to set start and end
            l.startColor = Raycaster.CurrentColor;
            l.endColor = Raycaster.CurrentColor;

            Vector3[] vertexPositions = { inVertex.gameObject.transform.position, gameObject.transform.position };
            l.SetPositions(vertexPositions);
            //creates capsule, the capsule will be morphed to the line in UpdateConnections 
            capsule = l.gameObject.GetComponent<CapsuleCollider>();
         }
    }

    // Update is called once per frame
    void Update()
    {
        if(selected)
        {
            Vector3 point = cast.HitPoint;
            transform.position = new Vector3(point.x, height, point.z);
            UpdateConnections();
            if(Input.GetMouseButtonUp(0))
            {
                selected = false;
            }
        }
    }

    public void UpdateConnections()
    {
        
        
        if(inVertex != null)
        {
            //changes teh line position, and moves the collider along with it
            Vector3[] vertexPositions = { inVertex.gameObject.transform.position, gameObject.transform.position };
            LineRenderer l = inVertex.gameObject.transform.GetChild(0).GetComponent<LineRenderer>();
            l.SetPositions(vertexPositions);
            Vector3 offset = (gameObject.transform.position - inVertex.gameObject.transform.position) / 2;
            
            capsule.transform.position = inVertex.transform.position + offset;
            
            capsule.transform.LookAt(gameObject.transform);
           
            capsule.height = (gameObject.transform.position - inVertex.transform.position).magnitude * 2;
        }

        if(outVertex != null)
        {
            Vector3[] vertexPositions = { outVertex.gameObject.transform.position, gameObject.transform.position };

            LineRenderer l = gameObject.transform.GetChild(0).GetComponent<LineRenderer>();
            l.SetPositions(vertexPositions);

            Vector3 offset = (outVertex.gameObject.transform.position - gameObject.transform.position) / 2;
            
            outVertex.capsule.transform.position = transform.position + offset;

            outVertex.capsule.transform.LookAt(outVertex.gameObject.transform);

            outVertex.capsule.height = (gameObject.transform.position - outVertex.transform.position).magnitude * 2;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{

    //Vertices are pretty much just a container
    public Vertex outVertex;
    public Vertex inVertex;

    CapsuleCollider capsule;


    #region Properties

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


    public CapsuleCollider Capsule
    {
        get { return capsule; }
        set { capsule = value; }
    }

    #endregion

    void Start()
    {

      

    }

    // Update is called once per frame
    void Update()
    {

    }
}
    
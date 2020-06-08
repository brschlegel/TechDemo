using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VertexManager : MonoBehaviour
{
    //The vertex manager is primarily worried about creating vertices and adding them to the linked list, as well as printing out the vertex positions

    // Start is called before the first frame update
    public GameObject vertexPrefab;
    private Raycaster cast;
    public Vertex head;
    public Vertex tail;
    public float height;

    string savePath = "Assets/test.yml";

    //This dictionary is for converting colors to strings for writing
    private Dictionary<Color, string> ColorDict;

    public bool creating;
    void Start()
    {
        creating = false;
        cast = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Raycaster>();
        head = null;
        tail = null;
        height = 1;

        //clear the file
        using (StreamWriter writer = new StreamWriter(savePath))
        {
            writer.Close();
        }
        ColorDict = new Dictionary<Color, string>();
        ColorDict.Add(Color.red, "Walk");
        ColorDict.Add(Color.green, "Crawl");
        ColorDict.Add(Color.blue, "Run");

    }

    // Update is called once per frame
    void Update()
    {
       
        if(creating && Input.GetMouseButtonDown(0))
        {
            //Linked list. Not sure if it needs to be doubly linked, but was simple enough to implement so I figured I would   
            Vertex v = Instantiate(vertexPrefab, new Vector3 (cast.HitPoint.x, height, cast.HitPoint.z), Quaternion.identity, transform).GetComponent<Vertex>();
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
            creating = false;
        }
    }

    

    public void CreateVertex()
    {
        creating = true;
        
    }

    public void WritePoints()
    {
        Vertex current = head;
        using (StreamWriter writer = new StreamWriter(savePath))
        { 
            //loops through linked list, printing associated action to the line the vertex owns, which is the line going out 
            //ie the last vertex has no line owned
            while (current.OutVertex != null)
            {
                writer.WriteLine("-" + ColorDict[current.transform.GetChild(0).GetComponent<LineRenderer>().startColor]);
                writer.WriteLine(" -" + current.transform.position.x);
                writer.WriteLine(" -" + current.transform.position.y);
                writer.WriteLine(" -" + current.transform.position.z);
                current = current.OutVertex;
            }
            //last vertex
            writer.WriteLine("-End");
            writer.WriteLine(" -" + current.transform.position.x);
            writer.WriteLine(" -" + current.transform.position.y);
            writer.WriteLine(" -" + current.transform.position.z);

        }
    }
}
    
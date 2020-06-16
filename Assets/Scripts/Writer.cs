using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Writer : MonoBehaviour
{
    //Writer takes in the vertices and writes to a YAML file
    string savePath = "Assets/test.yml";
    private VertexManager vm;

    private Dictionary<Color, string> ColorDict;
    void Start()
    {
        vm = GetComponent<VertexManager>();
        //clear the file;
        using (StreamWriter writer = new StreamWriter(savePath))
        {
            writer.Close();
        }
        ColorDict = new Dictionary<Color, string>();
        ColorDict.Add(Color.red, "Walk");
        ColorDict.Add(Color.green, "Crawl");
        ColorDict.Add(Color.blue, "Run");
    }

    public void WritePoints()
    {
        Vertex current = vm.head;
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

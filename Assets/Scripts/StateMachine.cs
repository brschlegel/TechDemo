using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIState{Creating, Selecting}
public class StateMachine : MonoBehaviour
{
    public VertexManager vertexManager;
    UIState state;
    
    // Start is called before the first frame update
    void Start()
    {
        state = UIState.Selecting;
        vertexManager = GameObject.Find("VertexManager").GetComponent<VertexManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

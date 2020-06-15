using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct VertexState
{
    List<Color> lineStates;
    List<Vector3> vertPositions;
    public VertexState(Vertex head)
    {
        vertPositions = new List<Vector3>();
        lineStates = new List<Color>();
        if (head != null)
        {
            Vertex current = head;
            

            while (current.outVertex != null)
            {

                vertPositions.Add(current.transform.position);
                lineStates.Add(current.transform.GetChild(0).GetComponent<LineRenderer>().startColor);

                current = current.outVertex;

            }

            vertPositions.Add(current.transform.position);

        }
    }

    #region Properties
    public List<Vector3> VertPositions
    {
        get { return vertPositions; }

    }
    public List<Color> LineStates
    {
        get { return lineStates; }

    }

    #endregion


}



public class UndoRedo : MonoBehaviour
{
    


    public Stack<VertexState> undoStack;
    public Stack<VertexState> redoStack;

    VertexState currentState;
    VertexManager vm;

    private GameObject vertexPrefab;
    public LogChange logger;

    void Awake()
    {
        logger = new LogChange(CreateVertexState);
    }
    // Start is called before the first frame update
    void Start()
    {
        undoStack = new Stack<VertexState>();
        redoStack = new Stack<VertexState>();

      
        vm = gameObject.GetComponent<VertexManager>();
        vertexPrefab = vm.vertexPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadVertexState(VertexState newState)
    {

        int numVert = newState.VertPositions.Count;
        //If a vertex was added or deleted, its easier to just rebuild all the vertices
        if (numVert > vm.count || numVert < vm.count)
        {
            RebuildVertices(newState);
        }
        else
        {
            Vertex current = vm.head;
            for (int i = 0; i < numVert; i++)
            {
                current.transform.position = newState.VertPositions[i];
                //current.UpdateConnections();
                if (i < numVert - 1)
                {
                    LineRenderer l = current.transform.GetChild(0).GetComponent<LineRenderer>();
                    l.startColor = newState.LineStates[i];
                    l.endColor = newState.LineStates[i];
                }
                current = current.OutVertex;
            }

            //update line colors
            current = vm.head;
            for (int i = 0; i < numVert - 1; i++)
            {


                LineRenderer l = current.transform.GetChild(0).GetComponent<LineRenderer>();
                l.startColor = newState.LineStates[i];
                l.endColor = newState.LineStates[i];

                current = current.OutVertex;
            }
        }

    }

    public void CreateVertexState()
    {

        undoStack.Push(currentState);
        redoStack.Clear();
        currentState = new VertexState(vm.head);
    }

    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            redoStack.Push(currentState);
            currentState = undoStack.Pop();
            LoadVertexState(currentState);
            vm.RestoreConnections();
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            undoStack.Push(currentState);
            currentState = redoStack.Pop();
            LoadVertexState(currentState);
            vm.RestoreConnections();
        }
    }

    public void RebuildVertices(VertexState newState)
    {
        //clear the linked list
        vm.head = null;
        vm.tail = null;
        vm.count = 0;

        //clear the actual scene
        foreach(Transform child in vm.transform)
        {
            Destroy(child.gameObject);
        }

        //recreate all of the vertices


        for (int i = 0; i < newState.VertPositions.Count; i++)
        {

            Vector3 vectPosition = newState.VertPositions[i];
            Vertex v = Instantiate(vertexPrefab, vectPosition, Quaternion.identity, transform).GetComponent<Vertex>();


            if (vm.head == null)
            {
                v.inVertex = null;
                vm.head = v;
                vm.tail = vm.head;


            }
            else
            {
                v.inVertex = vm.tail;
                vm.tail.OutVertex = v;
                vm.tail = vm.tail.OutVertex;
            }
            vm.count++;

            if (v.inVertex != null)
            {
                //sets up a line between previous vertex and this one
                //The line is owned by the PREVIOUS vertex, and is parented under it in unity
                LineRenderer l = Instantiate(vm.linePrefab, v.inVertex.gameObject.transform).GetComponent<LineRenderer>();
                //Lines are set to take in gradients, so you have to set start and end
                l.startColor = newState.LineStates[i - 1];
                l.endColor = newState.LineStates[i - 1];

                Vector3[] vertexPositions = { v.inVertex.gameObject.transform.position, v.transform.position };
                l.SetPositions(vertexPositions);
                //creates capsule, the capsule will be morphed to the line in UpdateConnections 
                v.inVertex.Capsule = l.gameObject.GetComponent<CapsuleCollider>();



            }
        }
        vm.RestoreConnections();


    }
}

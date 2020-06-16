using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIState { Creating, Selecting }
public class StateMachine : MonoBehaviour
{
    //State machine handles all of the input
    public VertexManager vertexManager;
    private Raycaster cast;
    public UIState state;
    public UndoRedo undo;

    void Start()
    {
        state = UIState.Selecting;
        vertexManager = GameObject.Find("VertexManager").GetComponent<VertexManager>();
        undo = vertexManager.GetComponent<UndoRedo>();
        
        cast = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Raycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case UIState.Creating:
                if (vertexManager.Create())
                {
                   state = UIState.Selecting;
                }
                
                break;
            case UIState.Selecting:
                if (cast.Hit)
                {
                    //Handling selection of different objects on screen
                    switch (cast.HitObject.tag)
                    {
                        case "Vertex":
                            if (Input.GetMouseButtonDown(0))
                            {
                                
                                vertexManager.StartCoroutine("MoveVert", cast.HitObject);
                            }
                            if(Input.GetKeyDown(KeyCode.Delete))
                            {
                                vertexManager.Delete(cast.HitObject);
                            }
                            break;
                        case "Lines":
                            if (Input.GetMouseButtonDown(0))
                            {
                                //change color to be the currently selected one
                                LineRenderer l = cast.HitObject.GetComponent<LineRenderer>();
                                l.startColor = cast.CurrentColor;
                                l.endColor = cast.CurrentColor;
                                undo.logger();
                            }
                            break;
                    }
                }

                break;
        }

        //Undo Redo, Debug keys, can be mapped to buttons on screen instead
        if( Input.GetKeyDown(KeyCode.Z) )
        {
            undo.Undo();
        }

        if(Input.GetKeyDown(KeyCode.Y))
        {
            undo.Redo();
        }
    }

    public void CreateButton()
    {
        state = UIState.Creating;
    }
}

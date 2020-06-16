using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycaster : MonoBehaviour
{
    //Raycaster handles all of the raycasting (go figure)
    private Camera cam;
    private Ray mouseRay;
    private RaycastHit rayHit;
    private bool hit;

    private Color currentColor;
    
    public Color CurrentColor
    {
        get
        {
            if(currentColor == null || currentColor == new Color(0,0,0,0))
            {
                int i = GameObject.Find("Dropdown").GetComponent<Dropdown>().value;

                switch (i)
                {
                    case 0:
                        return Color.red;

                    case 1:
                        return Color.blue;

                    case 2:
                        return Color.green;

                }

            }
            return currentColor;
        }

        set
        {
            currentColor = value;
        }
    }
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        hit = Physics.Raycast(mouseRay, out rayHit, 1000.0f);   
    }
    #region Properties

    public bool Hit
    {
        get { return hit; }
    }

    public GameObject HitObject
    {
        get { return rayHit.transform.gameObject; }
    }

    public Vector3 HitPoint
    {
        get { return rayHit.point; }
    }

    #endregion Properties
}

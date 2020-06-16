using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

public enum FixedAxis { X,Y,Z}
public class DragManager : MonoBehaviour
{
    // Start is called before the first frame update

    Raycaster cast;
    FixedAxis fixedAxis;
    public  GameObject xGameObj;
    public  GameObject yGameObj;
    public  GameObject zGameObj;
    public GameObject toBeMoved;
    void Start()
    {
        cast = xGameObj.GetComponent<Raycaster>();
        fixedAxis = FixedAxis.X;
        //xGameObj = GameObject.Find("XCam");
        //yGameObj = GameObject.Find("YCam");
        //zGameObj = GameObject.Find("ZCam");
        toBeMoved = null;

    }

    // Update is called once per frame
    void Update()
    {
        if(cast.Hit)
        {
            if(cast.HitObject.tag == "Draggable" && Input.GetMouseButtonDown(0))
            {
                 toBeMoved = cast.HitObject;
             
              
                
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            toBeMoved = null;
        }

        MoveObj(toBeMoved);
    }

   

    public void MoveObj(GameObject toBeMoved)
    {
        if(toBeMoved != null)
        {
            Vector3 point;

            switch (fixedAxis)
            {
                case FixedAxis.X:
                    point = new Vector3(toBeMoved.transform.position.x, cast.HitPoint.y, cast.HitPoint.z);
                    break;

                case FixedAxis.Y:
                    point = new Vector3(cast.HitPoint.x, toBeMoved.transform.position.y, cast.HitPoint.z);
                    break;

                case FixedAxis.Z:
                    point = new Vector3(cast.HitPoint.x, cast.HitPoint.y, toBeMoved.transform.position.z);
                    break;

                default:
                    point = toBeMoved.transform.position;
                    break;
            }

            if (point == Vector3.zero)
            {
                point = toBeMoved.transform.position;
            }
            toBeMoved.transform.position = point;
        }
    }

    public void XPress()
    {
        fixedAxis = FixedAxis.X;
        xGameObj.SetActive(true);
        yGameObj.SetActive(false);
        zGameObj.SetActive(false);

        cast = xGameObj.GetComponent<Raycaster>();
        
    }
    
    public void YPress()
    {
        fixedAxis = FixedAxis.Y;
        xGameObj.SetActive(false);
        yGameObj.SetActive(true);
        zGameObj.SetActive(false);

        cast = yGameObj.GetComponent<Raycaster>();
    }

    public void ZPress()
    {
        fixedAxis = FixedAxis.Z;
        xGameObj.SetActive(false);
        yGameObj.SetActive(false);
        zGameObj.SetActive(true);
        cast = zGameObj.GetComponent<Raycaster>();
    }
}

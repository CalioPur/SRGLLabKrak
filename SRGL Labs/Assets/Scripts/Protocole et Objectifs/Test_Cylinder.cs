using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Cylinder : MonoBehaviour
{
    //ObjectWasFilledEvent va aussi etre utilisé pour des evenements ne concernant pas ObjectWasFilled -> changer le noms dans les vrais classes
    public delegate void ObjectWasFilledEvent(Objective obj);
    public static event ObjectWasFilledEvent OnObjectWasFilledEvent;

    List<(string, int)> listOfContainedElements = new List<(string, int)>();

    public string extraElementName;
    public int extraElementQuantity;

    void Start()
    {
        //pour tests
        listOfContainedElements.Add(("Solution 1", 10));
        listOfContainedElements.Add(("Solution 2", 20));
        if(extraElementQuantity != 0)
        {
            listOfContainedElements.Add((extraElementName, extraElementQuantity));
        }

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name.Equals(this.name))
            {
                //print(hit);
                ObjectWasFilled();
            }
        }else if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name.Equals(this.name))
            {
                
                ObjectGotGrabbed();
            }
        }
    }

    void ObjectWasFilled()
    {
        ObjectiveContains objc = new ObjectiveContains(listOfContainedElements);
        ObjectiveContainsStrict objcs = new ObjectiveContainsStrict(listOfContainedElements);
        //print(objct==null);
        //print(OnObjectWasFilledEvent == null);
        if (OnObjectWasFilledEvent != null)
        {
            OnObjectWasFilledEvent(objc);
            OnObjectWasFilledEvent(objcs);
        }
        

    }

    void ObjectGotGrabbed()
    {
        ObjectiveGrabItem objgi = new ObjectiveGrabItem(this.tag);
        if (OnObjectWasFilledEvent != null)
        {
            OnObjectWasFilledEvent(objgi);
        }
    }
}

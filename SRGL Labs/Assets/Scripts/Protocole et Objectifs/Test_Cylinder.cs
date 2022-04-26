using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Cylinder : MonoBehaviour
{
    public delegate void ObjectWasFilledEvent(Objective obj);
    public static event ObjectWasFilledEvent OnObjectWasFilledEvent;

    List<(string, int)> listOfContainedElements = new List<(string, int)>();

    void Start()
    {
        listOfContainedElements.Add(("Solution 3", 10));
        listOfContainedElements.Add(("Solution 2", 20));
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //print("aaaaaaaaaa");
                ObjectWasFilled();
            }
        }
    }

    void ObjectWasFilled()
    {
        ObjectiveContains objct = new ObjectiveContains(listOfContainedElements);
        //print(objct==null);
        //print(OnObjectWasFilledEvent == null);
        if(OnObjectWasFilledEvent != null)
        {
            OnObjectWasFilledEvent(objct);
        }
        

    }
}

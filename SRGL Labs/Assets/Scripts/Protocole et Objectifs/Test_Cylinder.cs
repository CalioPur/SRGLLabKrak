using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Cylinder : MonoBehaviour
{
    List<(string, int)> listOfContainedElements = new List<(string, int)>();

    private void Start()
    {
        listOfContainedElements.Add(("Solution 1", 10));
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
                print("aaaaaaaaaa");
                ObjectWasFilled();
            }
        }
    }

    void ObjectWasFilled()
    {
        ObjectiveContains obj = new ObjectiveContains(listOfContainedElements);

    }
}

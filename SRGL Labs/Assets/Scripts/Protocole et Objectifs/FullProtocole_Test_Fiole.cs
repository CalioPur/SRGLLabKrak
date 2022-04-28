using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullProtocole_Test_Fiole : MonoBehaviour
{
    public delegate void ObjectHadSomethingHappen(Objective obj);
    public static event ObjectHadSomethingHappen ObjectHadSomethingHappenEvent;

    Dictionary<string,int> dictionaryOfContainedElements = new Dictionary<string,int>();

    public GameObject testLiquid;

    private void Start()
    {
        testLiquid.SetActive(false);
    }

    // Update is called once per frame
    //mouse button 0 -> interaction / prendre
    //mouse button 1 -> ajouter de l'eau distillée (100)
    //mouse button 3 -> empty bottle ?
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name.Equals(this.name))
            {
                ObjectGotGrabbed();
            }
                
        }
        else if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name.Equals(this.name))
            {
                if (this.dictionaryOfContainedElements.ContainsKey("eau distillée"))
                {

                    this.dictionaryOfContainedElements["eau distillée"] += 25;
                    Debug.Log("+25");
                }
                else
                {
                    this.dictionaryOfContainedElements.Add("eau distillée", 25);
                    Debug.Log("+25 (set)");
                }
                ObjectWasFilled();
                testLiquid.SetActive(true);
            }

            
        }
        else if (Input.GetMouseButtonDown(2))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name.Equals(this.name))
            {
                print(hit.transform.name);
                this.dictionaryOfContainedElements.Clear();
                testLiquid.SetActive(false);
            }
                
        }
    }

    void ObjectWasFilled()
    {
        ObjectiveContainsDictionary objcd = new ObjectiveContainsDictionary(this.dictionaryOfContainedElements,1) ;
        if (ObjectHadSomethingHappenEvent != null)
        {
            ObjectHadSomethingHappenEvent(objcd);
        }


    }

    void ObjectGotGrabbed()
    {
        ObjectiveGrabItem objgi = new ObjectiveGrabItem(this.tag);
        if (ObjectHadSomethingHappenEvent != null)
        {
            ObjectHadSomethingHappenEvent(objgi);
        }
    }
}

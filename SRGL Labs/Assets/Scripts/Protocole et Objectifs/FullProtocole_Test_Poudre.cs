using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullProtocole_Test_Poudre : MonoBehaviour
{
    public delegate void ObjectHadSomethingHappenPoudre(Objective obj);
    public static event ObjectHadSomethingHappenPoudre ObjectHadSomethingHappenEventPoudre;

    public delegate void ObjectIsGrabbed(GameObject gObj);
    public static event ObjectIsGrabbed ObjectIsGrabbedEvent;
    public static event ObjectIsGrabbed ObjectIsDroppedEvent;

    Dictionary<string, int> dictionaryOfContainedElements = new Dictionary<string, int>();

    public GameObject testLiquid;

    public bool isGrabbed = false;

    private void Start()
    {
        testLiquid.SetActive(false);
    }

    // Update is called once per frame
    //mouse button 0 -> interaction / prendre
    //mouse button 1 -> ajouter de la poudre (5)
    //mouse button 3 -> empty bottle ?
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name.Equals(this.name))
            {
                Debug.Log("Held");
                isGrabbed = !isGrabbed;
                ObjectGotGrabbed();
            }

        }
        else if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name.Equals(this.name))
            {
                if (this.dictionaryOfContainedElements.ContainsKey("poudre"))
                {

                    this.dictionaryOfContainedElements["poudre"] += 5;
                    Debug.Log("+5");
                }
                else
                {
                    this.dictionaryOfContainedElements.Add("poudre", 5);
                    Debug.Log("+5 (set)");
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
                Debug.Log("Cleared");
            }

        }
    }

    void ObjectWasFilled()
    {
        ObjectiveContainsDictionary objcd = new ObjectiveContainsDictionary(this.dictionaryOfContainedElements, 1);
        if (ObjectHadSomethingHappenEventPoudre != null)
        {
            ObjectHadSomethingHappenEventPoudre(objcd);
        }


    }

    void ObjectGotGrabbed()
    {
        ObjectiveGrabItem objgi = new ObjectiveGrabItem(this.tag);
        if (ObjectHadSomethingHappenEventPoudre != null)
        {
            ObjectHadSomethingHappenEventPoudre(objgi);
        }
        if (ObjectIsGrabbedEvent != null && isGrabbed)
        {
            ObjectIsGrabbedEvent(this.transform.gameObject);
        }else if (ObjectIsDroppedEvent != null && !isGrabbed)
        {
            ObjectIsDroppedEvent(this.transform.gameObject);

        }
    }
}

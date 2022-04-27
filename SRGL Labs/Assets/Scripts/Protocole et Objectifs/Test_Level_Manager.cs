using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Level_Manager : MonoBehaviour
{
    Protocole protocole = new Protocole();

    //Liste des toggles (ordonnés en fonction des objectifs)
    public List<GameObject> toggleList;

    private void OnEnable()
    {
        //subscribes to- TOUJOURS EN ONENABLE
        Test_Cylinder.OnObjectWasFilledEvent += this.protocole.checkIfOrderedObjectiveIsValidated;
        Protocole.OnObjectiveSuccessfullyCompletedEvent += toggleUpdate;
    }

    // Start is called before the first frame update
    void Start()
    {
        List<(string, int)> listOfContainedElements = new List<(string, int)>();
        listOfContainedElements.Add(("Solution 1", 10));
        listOfContainedElements.Add(("Solution 2", 20));


        ObjectiveContains obj1 = new ObjectiveContains(listOfContainedElements);
        this.protocole.listOfObjectives.Add(obj1);
        this.protocole.dictionaryOfObjectives.Add(obj1, false);

        ObjectiveGrabItem obj2 = new ObjectiveGrabItem("Cylinder");
        this.protocole.listOfObjectives.Add(obj2);
        this.protocole.dictionaryOfObjectives.Add(obj2, false);

        ObjectiveContainsStrict obj3 = new ObjectiveContainsStrict(listOfContainedElements);
        this.protocole.listOfObjectives.Add(obj3);
        this.protocole.dictionaryOfObjectives.Add(obj3, false);


    }

    void toggleUpdate()
    {
        //print(this.protocole.objectivesCounter);
        toggleList[this.protocole.objectivesCounter].GetComponent<Toggle>().isOn = true;
    }

    //loop pour subscribe (protocole's function) sur objets ?
}

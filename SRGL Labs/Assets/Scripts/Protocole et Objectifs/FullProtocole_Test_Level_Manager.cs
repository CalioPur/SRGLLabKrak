using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullProtocole_Test_Level_Manager : MonoBehaviour
{
    private Protocole protocole = new Protocole();

    //Liste des toggles (ordonnés en fonction des objectifs)
    public List<GameObject> toggleList;

    public GameObject isHolding =null;
    public Vector3 positionBeforeHeld;

    private void OnEnable()
    {
        FullProtocole_Test_Fiole.ObjectHadSomethingHappenEvent += this.protocole.checkIfOrderedObjectiveIsValidated;
        FullProtocole_Test_Poudre.ObjectHadSomethingHappenEventPoudre += this.protocole.checkIfOrderedObjectiveIsValidated;
        FullProtocole_Test_Poudre.ObjectIsDroppedEvent += this.DropObject;
        FullProtocole_Test_Poudre.ObjectIsGrabbedEvent += this.HoldObject;
        FullProtocole_Test_Fiole.ObjectIsDroppedEvent += this.DropObject;
        FullProtocole_Test_Fiole.ObjectIsGrabbedEvent += this.HoldObject;
        Protocole.OnObjectiveSuccessfullyCompletedEvent += toggleUpdate;
    }

    void toggleUpdate()
    {
        //print(this.protocole.objectivesCounter);
        toggleList[this.protocole.objectivesCounter].GetComponent<Toggle>().isOn = true;
    }

    private void Start()
    {
        Dictionary<string,int> d1 = new Dictionary<string, int>();
        d1.Add("eau distillée", 100);
        ObjectiveContainsDictionary obj1 = new ObjectiveContainsDictionary(d1,1) ;
        this.protocole.listOfObjectives.Add(obj1);
        this.protocole.dictionaryOfObjectives.Add(obj1, false);

        //faire une pesee -> test contains strict
        Dictionary<string, int> d2 = new Dictionary<string, int>();
        d2.Add("poudre", 25);
        ObjectiveContainsDictionary obj2 = new ObjectiveContainsDictionary(d2, 1);
        this.protocole.listOfObjectives.Add(obj2);
        this.protocole.dictionaryOfObjectives.Add(obj2, false);

        //test contains strict sur fiole (poudre)
        d1.Add("poudre", 25);
        ObjectiveContainsDictionary obj3 = new ObjectiveContainsDictionary(d1, 1);
        this.protocole.listOfObjectives.Add(obj3);
        this.protocole.dictionaryOfObjectives.Add(obj3, false);

        //test contains strict sur fiole (eau)
        d1["eau distillée"]+=50;
        ObjectiveContainsDictionary obj4 = new ObjectiveContainsDictionary(d1, 1);
        this.protocole.listOfObjectives.Add(obj4);
        this.protocole.dictionaryOfObjectives.Add(obj4, false);

        //?

        //test contains fiole strict (acide)
        d1.Add("acide", 25);
        ObjectiveContainsDictionary obj6 = new ObjectiveContainsDictionary(d1, 1);
        this.protocole.listOfObjectives.Add(obj6);
        this.protocole.dictionaryOfObjectives.Add(obj6, false);

        //
        d1["eau distillée"] += 350;
        ObjectiveContainsDictionary obj7 = new ObjectiveContainsDictionary(d1, 1);
        this.protocole.listOfObjectives.Add(obj7);
        this.protocole.dictionaryOfObjectives.Add(obj7, false);

        //end protocole

    }

    void HoldObject(GameObject gObj)
    {
        if (this.isHolding == null)
        {
            this.isHolding = gObj;
            this.positionBeforeHeld = gObj.transform.position;
            gObj.transform.position = this.transform.position;
        }
        
    }

    void DropObject(GameObject gObj)
    {
        if (this.isHolding != null)
        {
            this.isHolding.transform.position = positionBeforeHeld;
            this.isHolding = null;
        }
        
        
    }


    //add loop to set all toggle labels from text file + set all toggles to isOn= false + interactable off
}

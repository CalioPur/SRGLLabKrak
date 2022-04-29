using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullProtocole_Test_Level_Manager : MonoBehaviour
{
    private Protocole protocole = new Protocole();

    //Liste des toggles (ordonnés en fonction des objectifs)
    public List<GameObject> toggleList;

    private void OnEnable()
    {
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

        ObjectiveGrabItem obj2 = new ObjectiveGrabItem("Poudre_Test");
        this.protocole.listOfObjectives.Add(obj2);
        this.protocole.dictionaryOfObjectives.Add(obj2, false);

        
    }


    //add loop to set all toggle labels from text file + set all toggles to isOn= false + interactable off
}

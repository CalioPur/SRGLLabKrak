using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Level_Manager : MonoBehaviour
{
    Protocole protocole = new Protocole();

    

    // Start is called before the first frame update
    void Start()
    {
        List<(string, int)> listOfContainedElements = new List<(string, int)>();
        listOfContainedElements.Add(("Solution 1", 10));
        listOfContainedElements.Add(("Solution 2", 20));


        ObjectiveContains obj1 = new ObjectiveContains(listOfContainedElements);
        this.protocole.listOfObjectives.Add(obj1);
        this.protocole.dictionaryOfObjectives.Add(obj1, false);
    }


    //loop pour subscribe (protocole's function) sur objets ?
}

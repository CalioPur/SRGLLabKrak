using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScaleTest : MonoBehaviour
{

    //************************************************************* VARIABLES

    // gants :
    bool glovesOn = false;
    bool glovesAreUnclean = false;

    // objet tenu :
    bool isHolding = false;
    GameObject objectHeld = null;

    // zone / vue
    string area = "paillasse"; // -> change json as it is stated as "paillaisse"

    //player
    bool mouseEnabled = true;

    //hand
    public GameObject handPlacement;

    //************************************************************* FONCTIONS

    void Start()
    {
        //Call json
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseEnabled) //clique gauche
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) //si contact avec un objet
            {
                GameObject target = hit.transform.gameObject;

                if (!isHolding && target.CompareTag("container")) //si main vide et target est un container
                {
                    print("container");
                    HoldObject(target);
                }
                else if (isHolding && target.CompareTag("placeholder")) // si main non vide et target est un placeholder
                {
                    PlaceObject(target);
                }
                else if (isHolding && target.CompareTag("container")) //si main non vide et target est un container
                {
                    //check fill errors before filling -> prevents you from filling if error detected
                }

            }
        }
    }


    void HoldObject(GameObject target) // target is the object to hold
    {
        //check hold errors
        this.isHolding = true;
        this.objectHeld = target;

        
        target.LeanMove(handPlacement.transform.position, 0.5f).setEaseOutQuart();
        StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time 
    }

    void PlaceObject(GameObject target) // target is the placeholder selected
    {
        //check put errors
    }

    public IEnumerator TimeUntilMouseEnables(float seconds)
    {
        mouseEnabled = false;
        yield return new WaitForSeconds(seconds);
        mouseEnabled = true;
    }
}

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

    //poids (temp)
    public float weightGoal;
    

    //************************************************************* FONCTIONS

    void Start()
    {
        //Call json
        weightGoal = 20;
    }

    //MAIN
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseEnabled) //clique gauche
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) //si contact avec un objet
            {
                GameObject target = hit.transform.gameObject;

                if (!isHolding && (target.CompareTag("container") || target.CompareTag("holder"))) //si main vide et target est un container
                {
                    HoldObject(target);
                }
                else if (isHolding && target.CompareTag("placeholder") && objectHeld.CompareTag("container")) // si main non vide et target est un placeholder
                {
                    PlaceObject(target);
                }
                else if (isHolding && target.CompareTag("container")) //si main non vide et target est un container 
                {
                    if (!target.Equals(objectHeld)) //si target n'est pas l'objet tenu
                    {
                        //check fill errors before filling -> prevents you from filling if error detected
                        FillContainer(target);
                    }
                    else //si target est l'objet tenu
                    {
                        //mix ?
                    }

                }
                else if (isHolding && target.CompareTag("unmovable_holder") && objectHeld.CompareTag("holder")) //tool sur unmovable holder 
                {
                    FillHolder(target);
                }
        
            }
            else //pas de contact
            {
                if(isHolding && objectHeld.CompareTag("holder") && !objectHeld.GetComponent<HoldingTool>().isFull)
                {
                    ReturnTool();
                }
            }
        }
    }

    //OTHER

    void HoldObject(GameObject target) // target is the object to hold
    {
        //check hold errors

        this.isHolding = true;
        this.objectHeld = target;

        target.LeanMove(handPlacement.transform.position, 0.5f).setEaseOutQuart();
        StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time 

        //placeholder needs to appear - for containers only here
        if (target.CompareTag("container"))
        {
            target.GetComponent<ContainerObjectScript>().hiddenPlaceholder.SetActive(true);
        }
    }

    void PlaceObject(GameObject target) // target is the placeholder selected
    {
        //check put errors

        this.isHolding = false;
        this.objectHeld.GetComponent<ContainerObjectScript>().hiddenPlaceholder = target; // for containers only

        this.objectHeld.LeanMove(target.transform.position, 0.5f).setEaseOutQuart();
        StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time 

        this.objectHeld = null;

        //placeholder will dissappear
        target.SetActive(false);
    }

    void ReturnTool() 
    {
        this.isHolding = false;

        objectHeld.LeanMove(objectHeld.GetComponent<HoldingTool>().originalPlacement, 0.5f).setEaseOutQuart();
        StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time

        this.objectHeld = null;
    }

    void FillContainer(GameObject target) // target is the container (prelevement ici aussi)
    {

        Vector3 tempPosition = target.transform.position;
        tempPosition.y += 0.05f;

        if (objectHeld.CompareTag("container"))
        {
            //check fill errors
            foreach (KeyValuePair<string, float> pair in objectHeld.GetComponent<ContainerObjectScript>().elementsContained)
            {
                target.GetComponent<ContainerObjectScript>().FillObject(pair.Key,pair.Value,weightGoal);
            }

            objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
            StartCoroutine(TimeUntilMouseEnables(0.8f)); //animation time

        }
        else if (objectHeld.CompareTag("holder"))
        {
            if (objectHeld.GetComponent<HoldingTool>().isFull)
            {
                //check fill errors
                target.GetComponent<ContainerObjectScript>().FillObject(objectHeld.GetComponent<HoldingTool>().containsName, objectHeld.GetComponent<HoldingTool>().containsQuantity,weightGoal);

                objectHeld.GetComponent<HoldingTool>().EmptyObject();

                objectHeld.LeanMove(tempPosition, 0.5f).setEaseOutQuart();
                StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time
                LeanTween.delayedCall(0.5f, ReturnTool);
            }
            else //si holding tool vide, prelevement
            {
                //check prelevement errors

                if (target.GetComponent<ContainerObjectScript>().weight > 0) //prelevement seulement si poids pas nul
                {
                    target.GetComponent<ContainerObjectScript>().TakeFromObject(0f, weightGoal);

                    objectHeld.GetComponent<HoldingTool>().FillObject("", 0);
                }
                
                objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
                StartCoroutine(TimeUntilMouseEnables(0.8f)); //animation time
            }
            
        }
    }

    void FillHolder(GameObject target) //target is unmovable holder
    {
        if (objectHeld.GetComponent<HoldingTool>().isFull)
        {
            objectHeld.GetComponent<HoldingTool>().EmptyObject();

            Vector3 tempPosition = target.transform.position;
            tempPosition.y += 0.1f;

            objectHeld.LeanMove(tempPosition, 0.5f).setEaseOutQuart();
            StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time

            LeanTween.delayedCall(0.5f, ReturnTool);
        }
        else
        {
            //check erreurs prelevement

            Vector3 tempPosition = target.transform.position;
            tempPosition.y += 0.1f;

            objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
            
            StartCoroutine(TimeUntilMouseEnables(0.8f)); //animation time

            objectHeld.GetComponent<HoldingTool>().FillObject(target.GetComponent<HoldingTool>().containsName, target.GetComponent<HoldingTool>().containsQuantity);
        }
    }

    public IEnumerator TimeUntilMouseEnables(float seconds)
    {
        mouseEnabled = false;
        yield return new WaitForSeconds(seconds);
        mouseEnabled = true;
    }

}

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

    //protocole
    private Protocole protocole = new Protocole();

    //JSON
    public TextAsset jsonErrorFile;

    ErrorManager allPossibleErrors; //erreurs

    //scale open
    public bool isScaleOpen = false;

    //************************************************************* FONCTIONS

    void Start()
    {
        //Call json
        allPossibleErrors = this.protocole.DeserializeJSONErrors(jsonErrorFile);
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
                        //check fill errors before filling -> prevents you from filling if error detected (within fill container)
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
            if (target.GetComponent<ContainerObjectScript>().hiddenPlaceholder.name.Equals("Scale")) //si placeholder de la balance
            {
                target.GetComponent<ContainerObjectScript>().hiddenPlaceholder.GetComponent<Placeholderscripttest>().scaleText.text = "0.00g";
            }
        }
    }

    void PlaceObject(GameObject target) // target is the placeholder selected
    {
        //check put errors

        this.isHolding = false;
        this.objectHeld.GetComponent<ContainerObjectScript>().hiddenPlaceholder = target; // for containers only

        this.objectHeld.LeanMove(target.transform.position, 0.5f).setEaseOutQuart();
        StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time 

        if (target.name.Equals("Scale")) //si placeholder de la balance
        {
            target.GetComponent<Placeholderscripttest>().scaleText.text = string.Format("{0:0.00}g", objectHeld.GetComponent<ContainerObjectScript>().weight);
        }

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

        ContainerObjectScript targetScript = target.GetComponent<ContainerObjectScript>();

        if (objectHeld.CompareTag("container"))
        {
            //check fill errors
            foreach (KeyValuePair<string, float> pair in objectHeld.GetComponent<ContainerObjectScript>().elementsContained)
            {
                targetScript.FillObject(pair.Key,pair.Value,weightGoal);
            }

            objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
            StartCoroutine(TimeUntilMouseEnables(0.8f)); //animation time

        }
        else if (objectHeld.CompareTag("holder"))
        {
            HoldingTool holdingScript = objectHeld.GetComponent<HoldingTool>();
            if (holdingScript.isFull)
            {
                //check fill errors
                //********************************************************************************************************** WORKING HERE
                if (targetScript.elementsContained.Count >= 1)
                {
                    foreach (KeyValuePair<string, float> pair in targetScript.elementsContained)
                    {
                        ErrorFilling error = new ErrorFilling("", targetScript.containerName, pair.Key, holdingScript.containsName, targetScript.danger, targetScript.hiddenPlaceholder.GetComponent<Placeholderscripttest>().place, targetScript.fill, targetScript.wasMixed);
                        print(protocole.CheckFillErrors(error, allPossibleErrors.fill));
                        
                    }
                }
                else
                {
                    ErrorFilling error = new ErrorFilling("", targetScript.containerName, null, holdingScript.containsName, targetScript.danger, targetScript.hiddenPlaceholder.GetComponent<Placeholderscripttest>().place, targetScript.fill, targetScript.wasMixed);
                    print(protocole.CheckFillErrors(error, allPossibleErrors.fill));
                    
                }


                targetScript.FillObject(holdingScript.containsName, holdingScript.containsQuantity,weightGoal);

                holdingScript.EmptyObject();

                objectHeld.LeanMove(tempPosition, 0.5f).setEaseOutQuart();
                StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time
                LeanTween.delayedCall(0.5f, ReturnTool);
            }
            else //si holding tool vide, prelevement
            {
                //check prelevement errors

                if (targetScript.weight > 0) //prelevement seulement si poids pas nul
                {
                    targetScript.TakeFromObject(0f, weightGoal);

                    holdingScript.FillObject("", 0);
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

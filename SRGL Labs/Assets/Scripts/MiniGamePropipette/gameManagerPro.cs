using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class gameManagerPro : MonoBehaviour
{

    //held item
    [SerializeField]
    bool isHolding = false;
    [SerializeField]
    GameObject objectHeld = null;

    //my hand
    public GameObject myHand;

    //animation
    public float animationDuration;
    public AnimationCurve animationCurve;

    //random game objets
    public GameObject propipette;
    public GameObject Sphere;
    GameObject becher;
    GameObject erlen;

    //camera position
    Vector3 cameraBasePos;

    //position de base propipette
    Vector3 propBasePos;

    //mouse
    bool mouseEnabled = true;
    
   //************************************************************************************* FONCTIONS

    //START
    private void Start()
    {
        gameObject.GetComponent<LiquidGestion>().mat.SetFloat("_fill", 0);
        gameObject.GetComponent<LiquidGestion>().enabled = false;
        cameraBasePos = transform.position;
        propBasePos = propipette.transform.position;
        
    }

    //MAIN
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseEnabled)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) //SI JE CLIQUE SUR UN OBJET
            {
                if ((hit.collider.CompareTag("pipette")|| hit.collider.CompareTag("pipetteA")) && objectHeld == null) //si je clique sur la pipette sans rien avoir dans la main
                {
                    isHolding = true;
                    objectHeld = hit.collider.gameObject;
                    StopAllCoroutines();

                    Vector3 temp = new Vector3(myHand.transform.position.x, myHand.transform.position.y+ 2f, myHand.transform.position.z);
                    SmoothPos(objectHeld, temp);
                    
                }
                else if(hit.collider.CompareTag("pissette") && objectHeld == null)
                {
                    isHolding = true;
                    objectHeld = hit.collider.gameObject;
                    StopAllCoroutines();
                    SmoothPos(objectHeld, myHand.transform.position);
                }
                else if (hit.collider.CompareTag("propipette") && isHolding && objectHeld.CompareTag("pipette")) //si je clique sur la propipete en ayant la pipette dans la main
                {
                    isHolding = true;
                    StopAllCoroutines();
                    hit.collider.tag = "Untagged"; //empeche l'interaction futur avec
                    SmoothPos(propipette, new Vector3(myHand.transform.position.x, myHand.transform.position.y + 3, myHand.transform.position.z));
                    SmoothPos(gameObject,  new Vector3(myHand.transform.position.x, myHand.transform.position.y + 2f, myHand.transform.position.z - 3));
                    propipette.GetComponent<ScrollPipette>().enabled = true;
                    
                    
                }
                else if (hit.collider.CompareTag("becher") && isHolding && objectHeld.CompareTag("pipetteA") ) //si je clique sur le becher en ayant la pipettte associé a la propipette
                {
                    becher = hit.collider.gameObject;
                    gameObject.GetComponent<LiquidGestion>().enabled = true;
                    StartCoroutine(SmoothPos2times(objectHeld, objectHeld.transform.position, new Vector3(becher.transform.position.x, becher.transform.position.y+2f, becher.transform.position.z), new Vector3(becher.transform.position.x, becher.transform.position.y + 1.5f, becher.transform.position.z)));
                    SmoothPos(gameObject, new Vector3(becher.transform.position.x, becher.transform.position.y + 3, becher.transform.position.z-8));
                    becher.GetComponentInChildren<Camera>().enabled = true;
                    isHolding = false;
                }
                else if(hit.collider.CompareTag("erlenmeyer") && isHolding && objectHeld.CompareTag("pipetteA") && objectHeld.GetComponentInChildren<SphereColliderScript>().isFilled) //si je clique sur l'erlenmeyer alors que ma pipette est remplie
                {
                    GameObject erlen = hit.collider.gameObject;
                    StartCoroutine(PipDansErlen(objectHeld, erlen, objectHeld.transform.position, new Vector3(erlen.transform.position.x, erlen.transform.position.y + 4f, erlen.transform.position.z)));
                    SmoothPos(gameObject, new Vector3(erlen.transform.position.x, erlen.transform.position.y + 4, erlen.transform.position.z - 8));
                    isHolding = false;
                }
                else if(hit.collider.CompareTag("erlenmeyer") && isHolding && objectHeld.CompareTag("pissette"))
                {
                    erlen = hit.collider.gameObject;
                    erlen.GetComponent<LiquideGestionPissette>().enabled = true;
                    SmoothPos(objectHeld, new Vector3(erlen.transform.position.x+1.6f, erlen.transform.position.y + 1f, erlen.transform.position.z));
                    SmoothRotZ(objectHeld,21f);
                    SmoothPos(gameObject, new Vector3(erlen.transform.position.x, erlen.transform.position.y + 3, erlen.transform.position.z - 8));
                    isHolding = false;
                }
                else if (hit.collider.CompareTag("holder") && !isHolding) //si clique sur holder et main vide
                {
                    HoldObject(hit.transform.gameObject);
                }
                else if (isHolding && hit.collider.CompareTag("unmovable_holder") && objectHeld.CompareTag("holder")) //tool sur unmovable holder 
                {
                    FillHolder(hit.transform.gameObject);
                }
                else if (isHolding && hit.collider.CompareTag("container")) //si main non vide et target est un container 
                {
                    if (!hit.collider.Equals(objectHeld)) //si target n'est pas l'objet tenu
                    {
                        //check fill errors before filling -> prevents you from filling if error detected (within fill container)
                        FillContainer(hit.transform.gameObject);
                    }
                    else //si target est l'objet tenu
                    {
                        //mix ?
                    }

                }

            }
            else //SI JE CLIQUE DANS LE VIDE
            {
                if (objectHeld!=null) // SI JE TIENS QUELQUE CHOSE
                {

                    if (objectHeld.CompareTag("holder") && !objectHeld.GetComponent<HoldingTool>().isFull) //si je tiens un holder vide
                    {
                        ReturnTool();
                    }

                    else if (objectHeld.tag == "pipette") //si je clique dans le vide et que je tiens la pipette 
                    {

                        if (propipette.GetComponentInChildren<ScrollPipette>().bienPlace) //si la propipette est bien plaçé
                        {
                            propipette.transform.parent = objectHeld.transform; //on dit que son parent est la pipette
                            objectHeld.tag = "pipetteA";
                        }
                        else
                        {
                            SmoothPos(propipette, propBasePos); //sinon, on la replace au bon endroit
                            propipette.GetComponent<ScrollPipette>().enabled = false; //on désactive le script pour le reboot quand on le reprendra
                            Sphere.tag = "propipette"; //je remet son tag a propipette pour pouvoir interagir a nouveau avec
                        }
                        if (!isHolding) //permet de remplacer dans la main ou dans l'espace si on la tiens ou si on vient de faire une action
                        {
                            SmoothPos(objectHeld, myHand.transform.position);
                            SmoothPos(gameObject, cameraBasePos);
                            propipette.GetComponent<ScrollPipette>().ICanScroll = false;
                            isHolding = true;
                        }
                        else
                        {
                            SmoothPos(objectHeld, new Vector3(0, 5f, 0));
                            SmoothPos(gameObject, cameraBasePos);
                            propipette.GetComponent<ScrollPipette>().ICanScroll = false;

                            objectHeld = null;
                            isHolding = false;
                        }

                    }
                    else if (objectHeld.tag == "pipetteA") //si je clique dans le vide et que je tiens la pipette+propipette
                    {
                        if (propipette.GetComponentInChildren<ScrollPipette>().bienPlace)
                        {
                            if (!isHolding)
                            {
                                gameObject.GetComponent<LiquidGestion>().enabled = false;
                                StartCoroutine(SmoothPos2times(objectHeld, objectHeld.transform.position, new Vector3(objectHeld.transform.position.x, objectHeld.transform.position.y + 0.5f, objectHeld.transform.position.z), myHand.transform.position));
                                SmoothPos(gameObject, cameraBasePos);
                                if (becher != null)
                                {
                                    becher.GetComponentInChildren<Camera>().enabled = false;
                                }
                                isHolding = true;
                            }
                            else
                            {
                                SmoothPos(objectHeld, new Vector3(0, 3.3f, 0));
                                objectHeld = null;
                                isHolding = false;
                            }


                        }
                        else //si le niveau de liquide dépasse, on recomence tout
                        {

                            gameObject.GetComponent<LiquidGestion>().fillInput = 0;
                            gameObject.GetComponent<LiquidGestion>().mat.SetFloat("_fill", 0);
                            propipette.transform.parent = transform.parent;
                            SmoothPos(propipette, propBasePos);
                            SmoothPos(objectHeld, new Vector3(0, 5f, 0));
                            SmoothPos(gameObject, cameraBasePos);
                            gameObject.GetComponent<LiquidGestion>().enabled = false;
                            becher.GetComponentInChildren<Camera>().enabled = false;
                            propipette.GetComponent<ScrollPipette>().enabled = false;
                            objectHeld.tag = "pipette";
                            Sphere.tag = "propipette"; //je remet son tag a propipette pour pouvoir interagir a nouveau avec
                            objectHeld = null;
                            isHolding = false;

                        }
                        becher = null;
                    }
                    else if (objectHeld.tag == "pissette") //si je clique dans le vide alors que je tiens la pissette
                    {
                        if (isHolding)
                        {
                            SmoothPos(objectHeld, new Vector3(1.3f, 1.5f, 0));
                            objectHeld = null;
                            isHolding = false;
                        }
                        else
                        {
                            SmoothPos(objectHeld, myHand.transform.position);
                            SmoothPos(gameObject, cameraBasePos);
                            SmoothRotZ(objectHeld, 0);
                            erlen.GetComponent<LiquideGestionPissette>().enabled = false;
                            isHolding = true;
                            erlen = null;
                        }
                    }
                }
                
            }
            
        }
    }




    public void EnableMouse()
    {
        mouseEnabled = true;
    }

    void HoldObject(GameObject target) // target is the object to hold
    {
        //check hold errors

        this.isHolding = true;
        this.objectHeld = target;

        target.LeanMove(myHand.transform.position, 0.5f).setEaseOutQuart();

        mouseEnabled = false;
        LeanTween.delayedCall(0.5f, EnableMouse);
        
    }

    void ReturnTool()
    {
        this.isHolding = false;

        objectHeld.LeanMove(objectHeld.GetComponent<HoldingTool>().originalPlacement, 0.5f).setEaseOutQuart();
        mouseEnabled = false;
        LeanTween.delayedCall(0.5f, EnableMouse);

        this.objectHeld = null;
    }

    void FillHolder(GameObject target) //target is unmovable holder
    {
        if (objectHeld.GetComponent<HoldingTool>().isFull) //vider holder
        {
            objectHeld.GetComponent<HoldingTool>().EmptyObject();

            Vector3 tempPosition = target.transform.position;
            tempPosition.y += target.GetComponent<Collider>().bounds.size.y + 0.1f;

            objectHeld.LeanMove(tempPosition, 0.5f).setEaseOutQuart();
            mouseEnabled = false;
            LeanTween.delayedCall(0.5f, EnableMouse);

            LeanTween.delayedCall(0.5f, ReturnTool);
        }
        else //remplir holder
        {
            //check erreurs prelevement

            Vector3 tempPosition = target.transform.position;
            tempPosition.y += target.GetComponent<Collider>().bounds.size.y +0.1f;

            objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
            mouseEnabled = false;
            LeanTween.delayedCall(0.8f, EnableMouse);

            objectHeld.GetComponent<HoldingTool>().FillObject(target.GetComponent<HoldingTool>().containsName, target.GetComponent<HoldingTool>().containsQuantity);
        }
    }

    void FillContainer(GameObject target) // target is the container (prelevement ici aussi)
    {

        Vector3 tempPosition = target.transform.position;
        tempPosition.y += target.GetComponent<Collider>().bounds.size.y + 0.1f;

        ContainerObjectScript targetScript = target.GetComponent<ContainerObjectScript>();

        if (objectHeld.CompareTag("container")) //si on verse avec container
        {
            //check fill errors
            foreach (KeyValuePair<string, float> pair in objectHeld.GetComponent<ContainerObjectScript>().elementsContained)
            {
                targetScript.FillObject(pair.Key, pair.Value, objectHeld.GetComponent<ContainerObjectScript>().shaderFill);
            }

            objectHeld.GetComponent<ContainerObjectScript>().EmptyObject();

            objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
            mouseEnabled = false;
            LeanTween.delayedCall(0.8f, EnableMouse);

        }
        else if (objectHeld.CompareTag("holder")) //si on verse avec holder
        {
            HoldingTool holdingScript = objectHeld.GetComponent<HoldingTool>();
            if (holdingScript.isFull)
            {
                //check fill errors
                    /*foreach (KeyValuePair<string, float> pair in targetScript.elementsContained)
                    {
                        ErrorFilling error = new ErrorFilling("", targetScript.containerName, pair.Key, holdingScript.containsName, targetScript.danger, targetScript.hiddenPlaceholder.GetComponent<Placeholderscripttest>().place, targetScript.fill, targetScript.wasMixed);

                        bool results = protocole.CheckFillErrors(error, allPossibleErrors.fill);
                        print(results);

                        if (results && targetScript.hiddenPlaceholder.name.Equals("Scale")) //si erreur sur scale
                        {
                            isScaleBroken = true;
                            targetScript.hiddenPlaceholder.GetComponent<Placeholderscripttest>().scaleText.text = "0,00g";
                        }

                    }*/



                targetScript.FillObject(holdingScript.containsName, holdingScript.containsQuantity, holdingScript.fillingValue);

                holdingScript.EmptyObject();

                objectHeld.LeanMove(tempPosition, 0.5f).setEaseOutQuart();
                mouseEnabled = false;
                LeanTween.delayedCall(0.5f, EnableMouse);

                LeanTween.delayedCall(0.5f, ReturnTool);
            }
            

        }
    }


    //********************************************************* ANIMATION NEW
    public void SmoothPos(GameObject target, Vector3 goal)
    {
        mouseEnabled = false;
        target.LeanMove(goal, animationDuration).setEaseOutQuart();
        LeanTween.delayedCall(animationDuration, EnableMouse);
    }

    public void SmoothRotZ(GameObject target, float goal)
    {
        mouseEnabled = false;
        target.LeanRotateZ(goal, animationDuration/2);
        LeanTween.delayedCall(animationDuration/2, EnableMouse);
    }

    //********************************************************** ANIMATION (old)

    public IEnumerator SmoothPos2times(GameObject targetToMove, Vector3 a, Vector3 b, Vector3 c) //annimation de 2 deplacements
    {
        mouseEnabled = false;
        float startTime = Time.realtimeSinceStartup;
        float currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.position = Vector3.Lerp(a, b, animationCurve.Evaluate(currentTimePercentage));
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        }
        startTime = Time.realtimeSinceStartup;
        currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.position = Vector3.Lerp(b, c, animationCurve.Evaluate(currentTimePercentage));
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        }
        mouseEnabled = true;
    }

    public IEnumerator PipDansErlen(GameObject targetToMove,GameObject erlenmeyer, Vector3 a, Vector3 b) //annimation de deplacement pipette dans erlen
    {
        mouseEnabled = false;
        gameObject.GetComponent<LiquidGestion>().enabled = true;
        //erlenmeyer.GetComponentInChildren<Wobble>().enabled = false;
        float startTime = Time.realtimeSinceStartup;
        float currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f); //met la pipette au dessus de l'erlenmeyer
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.position = Vector3.Lerp(a, b, animationCurve.Evaluate(currentTimePercentage));
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        }
        startTime = Time.realtimeSinceStartup;
        currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f); //l'enfonce un peu
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.position = Vector3.Lerp(b, new Vector3(b.x, b.y - 0.5f, b.z), animationCurve.Evaluate(currentTimePercentage)) ;
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        }
        float fillValue = targetToMove.GetComponentInChildren<menisqueDisp>().mat.GetFloat("_fill");
        startTime = Time.realtimeSinceStartup;
        currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f); // vide le liquide
        Material erlenMat = erlenmeyer.GetComponent<GetMaterialScript>().mat;
        float fillErlen = erlenMat.GetFloat("_fill");
        
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.GetComponentInChildren<menisqueDisp>().mat.SetFloat("_fill", Mathf.Lerp(fillValue, 0, animationCurve.Evaluate(currentTimePercentage)));
            erlenMat.SetFloat("_fill", Mathf.Lerp(fillErlen, fillErlen+0.05f, animationCurve.Evaluate(currentTimePercentage)));
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        }
        
        gameObject.GetComponent<LiquidGestion>().fillInput = 0;


        startTime = Time.realtimeSinceStartup;
        currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f); //remontte un peu la pipette
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.position = Vector3.Lerp(new Vector3(b.x, b.y - 0.5f, b.z), b, animationCurve.Evaluate(currentTimePercentage));
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        }
        startTime = Time.realtimeSinceStartup;
        currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f); //remet la pipette a sa place
        SmoothPos(gameObject, cameraBasePos);
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.position = Vector3.Lerp(b, a, animationCurve.Evaluate(currentTimePercentage));
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        }
        gameObject.GetComponent<LiquidGestion>().enabled = false;
        //erlenmeyer.GetComponentInChildren<Wobble>().enabled = true;
        mouseEnabled = true;
        isHolding = true;
    }
}

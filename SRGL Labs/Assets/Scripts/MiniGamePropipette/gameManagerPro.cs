using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerPro : MonoBehaviour
{

    //held item
    bool isInHand = false;
    GameObject ItemIHold = null;

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
                if ((hit.collider.CompareTag("pipette")|| hit.collider.CompareTag("pipetteA")) && ItemIHold == null) //si je clique sur la pipette sans rien avoir dans la main
                {
                    isInHand = true;
                    ItemIHold = hit.collider.gameObject;
                    StopAllCoroutines();

                    Vector3 temp = new Vector3(myHand.transform.position.x, myHand.transform.position.y+ 2f, myHand.transform.position.z);
                    SmoothPos(ItemIHold, temp);
                    
                }
                else if(hit.collider.CompareTag("pissette") && ItemIHold == null)
                {
                    isInHand = true;
                    ItemIHold = hit.collider.gameObject;
                    StopAllCoroutines();
                    SmoothPos(ItemIHold, myHand.transform.position);
                }
                else if (hit.collider.CompareTag("propipette") && isInHand && ItemIHold.CompareTag("pipette")) //si je clique sur la propipete en ayant la pipette dans la main
                {
                    isInHand = true;
                    StopAllCoroutines();
                    hit.collider.tag = "Untagged"; //empeche l'interaction futur avec
                    SmoothPos(propipette, new Vector3(myHand.transform.position.x, myHand.transform.position.y + 3, myHand.transform.position.z));
                    SmoothPos(gameObject,  new Vector3(myHand.transform.position.x, myHand.transform.position.y + 2f, myHand.transform.position.z - 3));
                    propipette.GetComponent<ScrollPipette>().enabled = true;
                    
                    
                }
                else if (hit.collider.CompareTag("becher") && isInHand && ItemIHold.CompareTag("pipetteA") ) //si je clique sur le becher en ayant la pipettte associ� a la propipette
                {
                    becher = hit.collider.gameObject;
                    gameObject.GetComponent<LiquidGestion>().enabled = true;
                    StartCoroutine(SmoothPos2times(ItemIHold, ItemIHold.transform.position, new Vector3(becher.transform.position.x, becher.transform.position.y+2f, becher.transform.position.z), new Vector3(becher.transform.position.x, becher.transform.position.y + 1.5f, becher.transform.position.z)));
                    SmoothPos(gameObject, new Vector3(becher.transform.position.x, becher.transform.position.y + 3, becher.transform.position.z-8));
                    becher.GetComponentInChildren<Camera>().enabled = true;
                    isInHand = false;
                }
                else if(hit.collider.CompareTag("erlenmeyer") && isInHand && ItemIHold.CompareTag("pipetteA") && ItemIHold.GetComponentInChildren<SphereColliderScript>().isFilled) //si je clique sur l'erlenmeyer alors que ma pipette est remplie
                {
                    GameObject erlen = hit.collider.gameObject;
                    StartCoroutine(PipDansErlen(ItemIHold, erlen, ItemIHold.transform.position, new Vector3(erlen.transform.position.x, erlen.transform.position.y + 4f, erlen.transform.position.z)));
                    SmoothPos(gameObject, new Vector3(erlen.transform.position.x, erlen.transform.position.y + 4, erlen.transform.position.z - 8));
                    isInHand = false;
                }
                else if(hit.collider.CompareTag("erlenmeyer") && isInHand && ItemIHold.CompareTag("pissette"))
                {
                    erlen = hit.collider.gameObject;
                    erlen.GetComponent<LiquideGestionPissette>().enabled = true;
                    SmoothPos(ItemIHold, new Vector3(erlen.transform.position.x+1.6f, erlen.transform.position.y + 1f, erlen.transform.position.z));
                    SmoothRotZ(ItemIHold,21f);
                    SmoothPos(gameObject, new Vector3(erlen.transform.position.x, erlen.transform.position.y + 3, erlen.transform.position.z - 8));
                    isInHand = false;
                }
                else if (hit.collider.CompareTag("holder") && ItemIHold==null) //si clique sur holder et main vide
                {
                    HoldObject(hit.transform.gameObject);
                }

            }
            else //SI JE CLIQUE DANS LE VIDE
            {
                if (ItemIHold!=null) // SI JE TIENS QUELQUE CHOSE
                {
                    if (ItemIHold.tag == "pipette") //si je clique dans le vide et que je tiens la pipette 
                    {

                        if (propipette.GetComponentInChildren<ScrollPipette>().bienPlace) //si la propipette est bien pla��
                        {
                            propipette.transform.parent = ItemIHold.transform; //on dit que son parent est la pipette
                            ItemIHold.tag = "pipetteA";
                        }
                        else
                        {
                            SmoothPos(propipette, propBasePos); //sinon, on la replace au bon endroit
                            propipette.GetComponent<ScrollPipette>().enabled = false; //on d�sactive le script pour le reboot quand on le reprendra
                            Sphere.tag = "propipette"; //je remet son tag a propipette pour pouvoir interagir a nouveau avec
                        }
                        if (!isInHand) //permet de remplacer dans la main ou dans l'espace si on la tiens ou si on vient de faire une action
                        {
                            SmoothPos(ItemIHold, myHand.transform.position);
                            SmoothPos(gameObject, cameraBasePos);
                            propipette.GetComponent<ScrollPipette>().ICanScroll = false;
                            isInHand = true;
                        }
                        else
                        {
                            SmoothPos(ItemIHold, new Vector3(0, 5f, 0));
                            SmoothPos(gameObject, cameraBasePos);
                            propipette.GetComponent<ScrollPipette>().ICanScroll = false;

                            ItemIHold = null;
                            isInHand = false;
                        }

                    }
                    else if (ItemIHold.tag == "pipetteA") //si je clique dans le vide et que je tiens la pipette+propipette
                    {
                        if (propipette.GetComponentInChildren<ScrollPipette>().bienPlace)
                        {
                            if (!isInHand)
                            {
                                gameObject.GetComponent<LiquidGestion>().enabled = false;
                                StartCoroutine(SmoothPos2times(ItemIHold, ItemIHold.transform.position, new Vector3(ItemIHold.transform.position.x, ItemIHold.transform.position.y + 0.5f, ItemIHold.transform.position.z), myHand.transform.position));
                                SmoothPos(gameObject, cameraBasePos);
                                if (becher != null)
                                {
                                    becher.GetComponentInChildren<Camera>().enabled = false;
                                }
                                isInHand = true;
                            }
                            else
                            {
                                SmoothPos(ItemIHold, new Vector3(0, 3.3f, 0));
                                ItemIHold = null;
                                isInHand = false;
                            }


                        }
                        else //si le niveau de liquide d�passe, on recomence tout
                        {

                            gameObject.GetComponent<LiquidGestion>().fillInput = 0;
                            gameObject.GetComponent<LiquidGestion>().mat.SetFloat("_fill", 0);
                            propipette.transform.parent = transform.parent;
                            SmoothPos(propipette, propBasePos);
                            SmoothPos(ItemIHold, new Vector3(0, 5f, 0));
                            SmoothPos(gameObject, cameraBasePos);
                            gameObject.GetComponent<LiquidGestion>().enabled = false;
                            becher.GetComponentInChildren<Camera>().enabled = false;
                            propipette.GetComponent<ScrollPipette>().enabled = false;
                            ItemIHold.tag = "pipette";
                            Sphere.tag = "propipette"; //je remet son tag a propipette pour pouvoir interagir a nouveau avec
                            ItemIHold = null;
                            isInHand = false;

                        }
                        becher = null;
                    }
                    else if (ItemIHold.tag == "pissette") //si je clique dans le vide alors que je tiens la pissette
                    {
                        if (isInHand)
                        {
                            SmoothPos(ItemIHold, new Vector3(1.3f, 1.5f, 0));
                            ItemIHold = null;
                            isInHand = false;
                        }
                        else
                        {
                            SmoothPos(ItemIHold, myHand.transform.position);
                            SmoothPos(gameObject, cameraBasePos);
                            SmoothRotZ(ItemIHold, 0);
                            erlen.GetComponent<LiquideGestionPissette>().enabled = false;
                            isInHand = true;
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

        this.isInHand = true;
        this.ItemIHold = target;

        target.LeanMove(myHand.transform.position, 0.5f).setEaseOutQuart();

        mouseEnabled = false;
        LeanTween.delayedCall(0.5f, EnableMouse);
        
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
    /*public IEnumerator SmoothPos(GameObject targetToMove, Vector3 a, Vector3 b) //annimation de deplacement base
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
        mouseEnabled = true;
    }*/


    /*public IEnumerator SmoothRotX(GameObject targetToMove, float a, float b) //annimation de rotation base
    {
        mouseEnabled = false;
        float startTime = Time.realtimeSinceStartup;
        float currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / (animationDuration)) : (1.0f);
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.rotation = new Quaternion(Mathf.Lerp(a, b, animationCurve.Evaluate(currentTimePercentage)), targetToMove.transform.rotation.y, targetToMove.transform.rotation.z, targetToMove.transform.rotation.w);
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / (animationDuration)) : (1.0f);
            //print(currentTimePercentage);
        }
        mouseEnabled = true;
    }

    public IEnumerator SmoothRotY(GameObject targetToMove, float a, float b) //animation de rotation base
    {
        mouseEnabled = false;
        float startTime = Time.realtimeSinceStartup;
        float currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / (animationDuration)) : (1.0f);
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.rotation = new Quaternion(targetToMove.transform.rotation.x,Mathf.Lerp(a, b, animationCurve.Evaluate(currentTimePercentage)), targetToMove.transform.rotation.z, targetToMove.transform.rotation.w);
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / (animationDuration)) : (1.0f);
            //print(currentTimePercentage);
        }
        mouseEnabled = true;
    }*/

    /*public IEnumerator SmoothRotZ(GameObject targetToMove, float a, float b) //animation de rotation base
    {
        mouseEnabled = false;
        float startTime = Time.realtimeSinceStartup;
        float currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / (animationDuration)) : (1.0f);
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.rotation = new Quaternion(targetToMove.transform.rotation.x,targetToMove.transform.rotation.y,Mathf.Lerp(a, b, animationCurve.Evaluate(currentTimePercentage)), targetToMove.transform.rotation.w);
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / (animationDuration)) : (1.0f);
            //print(currentTimePercentage);
        }
        mouseEnabled = true;
    }*/

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
        isInHand = true;
    }
}

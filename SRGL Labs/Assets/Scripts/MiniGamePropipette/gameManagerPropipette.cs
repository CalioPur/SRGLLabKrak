using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerPropipette : MonoBehaviour
{

    GameObject ItemIHold = null;
    public GameObject myHand;
    public float animationDuration;
    public AnimationCurve animationCurve;
    public GameObject propipette;
    public GameObject Sphere;

    Vector3 cameraBasePos;
    Vector3 propBasePos;
    bool mouseEnabled = true;
    GameObject becher;

    private void Start()
    {
        gameObject.GetComponent<LiquidGestion>().mat.SetFloat("_fill", 0);
        gameObject.GetComponent<LiquidGestion>().enabled = false;
        cameraBasePos = transform.position;
        propBasePos = propipette.transform.position;
        
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseEnabled)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if ((hit.collider.CompareTag("pipette")|| hit.collider.CompareTag("pipetteA")) && ItemIHold == null) //si je clique sur la pipette sans rien avoir dans la main
                {
                    ItemIHold = hit.collider.gameObject;
                    StopAllCoroutines();
                    StartCoroutine(SmoothPos(ItemIHold, ItemIHold.transform.position, myHand.transform.position));
                    
                }
                if (hit.collider.CompareTag("propipette") && ItemIHold.CompareTag("pipette")) //si je clique sur la propipete en ayant la pipette dans la main
                {
                    StopAllCoroutines();
                    hit.collider.tag = "Untagged"; //empeche l'interaction futur avec
                    StartCoroutine(SmoothPos(propipette, propipette.transform.position, new Vector3(myHand.transform.position.x, myHand.transform.position.y + 3, myHand.transform.position.z)));
                    StartCoroutine(SmoothPos(gameObject, transform.position, new Vector3(myHand.transform.position.x, myHand.transform.position.y + 2f, myHand.transform.position.z - 3)));
                    propipette.GetComponent<ScrollPipette>().enabled = true;
                    
                    
                }
                if (hit.collider.CompareTag("becher") && ItemIHold.CompareTag("pipetteA")) //si je clique sur le becher en ayant la pipettte associé a la propipette
                {
                    becher = hit.collider.gameObject;
                    gameObject.GetComponent<LiquidGestion>().enabled = true;
                    StartCoroutine(SmoothPos(ItemIHold, ItemIHold.transform.position, new Vector3(becher.transform.position.x, becher.transform.position.y+1.5f, becher.transform.position.z)));
                    StartCoroutine(SmoothPos(gameObject, transform.position, new Vector3(becher.transform.position.x, becher.transform.position.y + 3, becher.transform.position.z-8)));
                    becher.GetComponentInChildren<Camera>().enabled = true;
                }
            }
            else if (ItemIHold.tag == "pipette") //si je clique dans le vide et que je tiens la pipette 
            { 
                if (propipette.GetComponentInChildren<ScrollPipette>().bienPlace) //si la propipette est bien plaçé
                {
                    propipette.transform.parent = ItemIHold.transform; //on dit que son parent est la pipette
                    ItemIHold.tag = "pipetteA";
                }
                else
                {
                    StartCoroutine(SmoothPos(propipette, propipette.transform.position, propBasePos)); //sinon, on la replace au bon endroit
                    propipette.GetComponent<ScrollPipette>().enabled = false; //on désactive le script pour le reboot quand on le reprendra
                    Sphere.tag = "propipette"; //je remet son tag a propipette pour pouvoir interagir a nouveau avec
                }
                StartCoroutine(SmoothPos(ItemIHold, ItemIHold.transform.position, new Vector3(0, ItemIHold.transform.position.y, 0)));
                StartCoroutine(SmoothPos(gameObject, transform.position, cameraBasePos));
                propipette.GetComponent<ScrollPipette>().ICanScroll = false;
                
                ItemIHold = null;
            }
            else if (ItemIHold.tag == "pipetteA") //si je clique dans le vide et que je tiens la pipette+propipette
            {
                if (propipette.GetComponentInChildren<ScrollPipette>().bienPlace)
                {
                    gameObject.GetComponent<LiquidGestion>().enabled = false;
                    StartCoroutine(SmoothPos(ItemIHold, ItemIHold.transform.position, new Vector3(0, ItemIHold.transform.position.y, 0)));
                    StartCoroutine(SmoothPos(gameObject, transform.position, cameraBasePos));
                    becher.GetComponentInChildren<Camera>().enabled = false;
                    ItemIHold = null;
                }
                else //si le niveau de liquide dépasse, on recomence tout
                {

                    gameObject.GetComponent<LiquidGestion>().fillInput = 0;
                    gameObject.GetComponent<LiquidGestion>().mat.SetFloat("_fill", 0);
                    propipette.transform.parent = transform.parent;
                    StartCoroutine(SmoothPos(propipette, propipette.transform.position, propBasePos));
                    StartCoroutine(SmoothPos(ItemIHold, ItemIHold.transform.position, new Vector3(0, ItemIHold.transform.position.y, 0)));
                    StartCoroutine(SmoothPos(gameObject, transform.position, cameraBasePos));
                    gameObject.GetComponent<LiquidGestion>().enabled = false;
                    becher.GetComponentInChildren<Camera>().enabled = false;
                    propipette.GetComponent<ScrollPipette>().enabled = false;
                    ItemIHold.tag = "pipette";
                    Sphere.tag = "propipette"; //je remet son tag a propipette pour pouvoir interagir a nouveau avec
                    ItemIHold = null;

                }
            }
        }
    }
    public IEnumerator SmoothPos(GameObject targetToMove, Vector3 a, Vector3 b) //annimation de deplacement
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
    }
}

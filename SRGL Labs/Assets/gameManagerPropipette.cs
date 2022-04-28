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

    private void Awake()
    {
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
                if (hit.collider.CompareTag("pipette") && ItemIHold == null) //si je clique sur la pipette sans rien avoir dans la main
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
            }
            else if (ItemIHold != null) //si je clique dans le vide je replace tout a sa place
            {
                if (propipette.GetComponentInChildren<ScrollPipette>().bienPlace) //si la propipette est bien plaçé
                {
                    propipette.transform.parent = ItemIHold.transform; //on dit que son parent est la pipette
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

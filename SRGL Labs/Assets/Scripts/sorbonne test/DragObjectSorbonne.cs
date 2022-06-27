using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjectSorbonne : MonoBehaviour
{
    //*********************************************************** VARIABLES

    public delegate void PositionSet(int levelOfSecurity);
    public static event PositionSet OnPositionSet;

    public GameObject levelManager;
    bool isInSorbonne;

    bool wasMovingWindow = false;

    //*********************************************************** FONCTIONS
    // Start is called before the first frame update
    void Start()
    {
        isInSorbonne = levelManager.GetComponent<lvlmanagersorb>().isInSorbonne;
        transform.position = new Vector3(-0.03f, 5, -9.63f);
        //print(transform.position);
    }

    private Vector3 GetMousePosition() //position souris dans le jeu
    {
        Vector3 mousePosition = Input.mousePosition; //position of mouse in pixels
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; //test(objet) position in pixels
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //convertion to position in game 

        return mousePosition;
    }

    private void OnMouseDrag() //drag
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!isInSorbonne && Physics.Raycast(ray, out hit) && hit.transform.name.Equals("Vitre")) //si vitre
        {
            Vector3 mousePosition = GetMousePosition();
            transform.position = new Vector3(transform.position.x, mousePosition.y, transform.position.z);

            //attention pas aller trop loin
            if (transform.position.y < 5)
            {
                transform.position = new Vector3(transform.position.x, 5, transform.position.z);
            }
            else if (transform.position.y > 9)
            {
                transform.position = new Vector3(transform.position.x, 9, transform.position.z);
            }

        }
    }

    private void OnMouseUp() //lacher
    {
        float wantedPosition = 7.22f; //position souhaitée
        if (transform.position.y >= wantedPosition-0.1 && transform.position.y <= wantedPosition+0.1) //si bien placé
        {
            OnPositionSet(3); //message ok (niveau 3 placeholder)
        }
        else //si mal placé
        {
            OnPositionSet(1); //message pas ok (niveau 1 placeholder)
        }
    }
}

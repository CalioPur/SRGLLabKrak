using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lvlmanagersorb : MonoBehaviour
{
    //************************************************************* VARIABLES

    //position pour camera en mode sorbonne proche -> x0 y9.69 z-14.6 angle x17.479

    // gants :
    bool glovesOn = false;
    bool glovesAreUnclean = false;

    // objet tenu :
    bool isHolding = false;
    GameObject objectHeld = null;

    // zone / vue
    string area = "sorbonne";
    

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

    //sorbonne
    //si en zone sorbonne (variable area), check :
    public bool isInSorbonne = false;

    public List<GameObject> sorbonnePlaceholders;

    //camera
    public GameObject myCamera;

    //vitre sorbonne
    public GameObject sorbonneWindow;

    //scale open
    /*public bool isScaleOpen = false;

    public GameObject scalePlaceholder;

    public bool isScaleBroken = false;

    public GameObject scaleDoorUp;*/

    //************************************************************* FONCTIONS

    // Start is called before the first frame update
    void Start()
    {

        DragObjectSorbonne.OnPositionSet += SetPlaceHoldersLevel;

        foreach (GameObject obj in sorbonnePlaceholders)
        {
            obj.SetActive(false);
            Placeholderscripttest temp = obj.GetComponent<Placeholderscripttest>();
            temp.isReachable = false;
            temp.place = 1;
        }

        sorbonneWindow.GetComponent<DragObjectSorbonne>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInSorbonne) //si en vue sorbonne
        {
            sorbonneWindow.GetComponent<DragObjectSorbonne>().enabled = false;
        }
        else //si pas en vue sorbonne
        {
            if (Input.GetMouseButtonDown(0) && mouseEnabled)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) //si contact avec un objet
                {
                    GameObject target = hit.transform.gameObject;

                    if (target.name.Equals("Sorbonne"))
                    {
                        isInSorbonne = true;
                        Vector3 temp = new Vector3(0,9.69f,-14.6f);
                        myCamera.LeanMove(temp, 0.5f);
                        myCamera.LeanRotateX(17.479f, 0.5f);
                        
                    }
                }
            }
        }
    }

    void SetPlaceHoldersLevel(int level)
    {
        foreach (GameObject obj in sorbonnePlaceholders)
        {
            Placeholderscripttest temp = obj.GetComponent<Placeholderscripttest>();
            temp.place = level;
        }
    }

    
}

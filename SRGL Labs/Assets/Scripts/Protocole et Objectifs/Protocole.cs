using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocole
{
    
    //Events -> notification pour les toggles 
    public delegate void ObjectiveSuccessfullyCompleted();
    public static event ObjectiveSuccessfullyCompleted OnObjectiveSuccessfullyCompletedEvent;
    //On peut deplacer la gestion des toggles ici si on a le meme nombre de toggles et d'objectifs (toggleList, voir Test_Level_Manager) (?)

    //Dictionnaire des objectifs à faire+ un bool associé à chaque objectif pour savoir si il a été accompli 
    public Dictionary<Objective,bool> dictionaryOfObjectives = new Dictionary<Objective, bool>();
    //remplacer par liste ?
    //Liste des objectifs dans l'ordre
    public List<Objective> listOfObjectives = new List<Objective>();

    //Nb d'objectifs effectués (<= nb objectifs dans la liste)
    public int objectivesCounter =0;

    //************************************************************
    //variables pour detection des erreurs

    //events -> pour activation popup
    public delegate void ErrorDetected(string message);
    public static event ErrorDetected OnErrorDetectedEvent;

    //liste des erreurs + le nombre de fois qu'elles ont été faites
    public Dictionary<Error, int> dictionaryOfErrors = new Dictionary<Error, int>();


    //Regarde si l'action effectuée est l'objectif à faire
    //Si oui, update du dictionnaire + incrementation compteur
    //Seulement pour objectifs ordonnés
    public void checkIfOrderedObjectiveIsValidated(Objective obj)
    {
        //Debug.Log(objectivesCounter);
        //Debug.Log(listOfObjectives[objectivesCounter].Evaluate(obj));
        //Enlever premiere condition dans premier if si terminer le protocole empeche les interactions plus tard qui causent des index out of range
        if (objectivesCounter < listOfObjectives.Count && listOfObjectives[objectivesCounter].Evaluate(obj))
        {
            //Do something - Notifier les toggles
            Debug.Log("Notify toggles");
            if (OnObjectiveSuccessfullyCompletedEvent != null)
            {
                OnObjectiveSuccessfullyCompletedEvent();
            }
            
            dictionaryOfObjectives[obj] = true;
            objectivesCounter++;

            if (objectivesCounter == listOfObjectives.Count)
            {
                //Do something - Indiquer que le protocole est terminé
                Debug.Log("End of protocole");
            }
        }
    }

    //******************************************************************************
    //Fonction pour detection des erreurs 

   public bool checkIfErrorWasDoneFill(Error error, List<ErrorFilling> listOfErrors,int nbOfElements) // MAY BE REMOVED
    {
        bool flag = false;
        foreach(ErrorFilling errorFilling in listOfErrors)
        {
            if((!errorFilling.pouredIn.Equals("acide"))||(errorFilling.pouredIn.Equals("acide") && nbOfElements == 1))
            {
                if (errorFilling.EvaluateError(error))
                {
                    if (dictionaryOfErrors.ContainsKey(errorFilling))
                    {
                        dictionaryOfErrors[errorFilling] += 1;
                    }
                    else
                    {
                        dictionaryOfErrors.Add(errorFilling, 1);
                    }

                    if (OnErrorDetectedEvent != null)
                    {
                        OnErrorDetectedEvent(errorFilling.ErrorMessage());
                    }
                    flag = true;
                }
            }
            
        }
        return flag;
    }
    public bool CheckFillErrors(Error error, List<ErrorFilling> listOfErrors) //if true -> il y a eu une erreur
    {
        bool flag = false;
        foreach (ErrorFilling errorFilling in listOfErrors)
        {
            if (errorFilling.EvaluateError(error))
            {
                flag = true;
            }
        }

        return flag;
    }



    //****************************************************************************

    //-> deserialisation du fichier json contenant les etapes du protocole

    public Objective DeserializeJSONProtocole()
    {
        string str = "{\"dictionaryOfElementsAndQuantityRequired\":{\"poudre\" : 20 }}"; /*"{\"tagOrNameOfObject\" : \"Fiole\"}";*/
        //Debug.Log(str);
        return Newtonsoft.Json.JsonConvert.DeserializeObject<ObjectiveContainsDictionary>(str);
    }

    //-> deserialisation du fichier json contenant les erreurs possibles
    public ErrorManager DeserializeJSONErrors(TextAsset json)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorManager>(json.text);
    }
}

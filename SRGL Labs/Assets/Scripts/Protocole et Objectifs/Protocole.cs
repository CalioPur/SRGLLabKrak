using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocole
{
    //Dictionnaire des objectifs à faire+ un bool associé à chaque objectif pour savoir si il a été accompli 
    public Dictionary<Objective,bool> dictionaryOfObjectives;
    //remplacer par liste ?
    //Liste des objectifs dans l'ordre
    public List<Objective> listOfObjectives;

    //Nb d'objectifs effectués (<= nb objectifs dans la liste)
    public int objectivesCounter =0;


    //Regarde si l'action effectuée est l'objectif à faire
    //Si oui, update du dictionnaire + incrementation compteur
    //Seulement pour objectifs ordonnés
    public void checkIfOrderedObjectiveIsValidated(Objective obj)
    {
        //verif
        Debug.Log(listOfObjectives[objectivesCounter].Evaluate(obj));
        if (listOfObjectives[objectivesCounter].Evaluate(obj))
        {
            //Do something - Notifier les toggles
            Debug.Log("Notify toggles");
            dictionaryOfObjectives[obj] = true;
            objectivesCounter++;
            if (objectivesCounter == listOfObjectives.Count)
            {
                //Do something - Indiquer que le protocole est terminé
            }
        }
    }

    //AJOUTER SUBSCRIBE FUNCTION POUR OBJETS ?

}

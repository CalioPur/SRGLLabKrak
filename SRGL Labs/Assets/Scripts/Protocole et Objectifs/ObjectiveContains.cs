using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveContains : Objective
{
    public string container;
    public Dictionary<string, int> dictionaryOfElementsAndQuantityRequired;
    public bool wasMixed;
    public int order;

    public ObjectiveContains(string name, Dictionary<string, int> dictionary, bool mix, int nb)
    {
        this.dictionaryOfElementsAndQuantityRequired = dictionary;
        this.container = name;
        this.wasMixed = mix;
        this.order = nb;
    }

    public override bool Evaluate(Objective obj)
    {
        //add ignore a specific element if needed

        //verification du dictionnaire (stricte)
        ObjectiveContains temp = (ObjectiveContains)obj;

        bool flag = temp.dictionaryOfElementsAndQuantityRequired.Count == this.dictionaryOfElementsAndQuantityRequired.Count;

        if (flag)
        {
            foreach (KeyValuePair<string, int> pair in this.dictionaryOfElementsAndQuantityRequired)
            {

                if (!(temp.dictionaryOfElementsAndQuantityRequired.ContainsKey(pair.Key)) || !(temp.dictionaryOfElementsAndQuantityRequired[pair.Key] == pair.Value))
                {
                    flag = false;
                    break;
                }
            }
        }

        //verification du reste
        return flag && this.container.Equals(temp.container) && this.wasMixed==temp.wasMixed;
    }
}

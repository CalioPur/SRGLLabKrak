using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupScript : MonoBehaviour
{

    public TMP_Text popupText;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    public void Open(string message)
    {
        popupText.text = message;
        transform.LeanScale(new Vector3(1, 1, 0), 0.8f);
    }

    public void Close()
    {
        transform.LeanScale(Vector3.zero, 0.8f).setEaseInBack();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public GameObject m_StartButton;
    public GameObject m_Screen;

    void Start()
    {
        m_StartButton.SetActive(false);
        m_Screen.SetActive(true);
    }
}

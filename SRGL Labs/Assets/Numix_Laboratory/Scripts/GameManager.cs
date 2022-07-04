using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject m_StartButton;
    public GameObject m_Screen;
         
    void Start()
    {
        m_StartButton.SetActive(true);
        m_Screen.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    public GameObject m_Panel;
    public GameObject[] m_Pages;

    void Start()
    {
        
        m_Panel.SetActive(false);
        for (int i=0; i<m_Pages.Length; i++)
        {
            m_Pages[i].SetActive(i == 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerPopupTest1 : MonoBehaviour
{
    public int maxMessages = 10;

    public GameObject chatPanel, panel;

    [SerializeField]
    List<Message1> messageList = new List<Message1>();

    //list of messages 

    

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SendMessageHere("this is a very long text a bip bop wow aaaaaaaaaaaaaaaa aaaaaaaajdjdjbjzfjzfzjfeej mrrrrrrrrrrrrrrrheghvzejvghgvhzehvggrhvgej wowoowowowoowowowow huehrueh hsdhzuhehjhdhdh aaaaaaaaaaaaaaaaaaaa");
        }
    }

    public void SendMessageHere(string text)
    {
        if(messageList.Count >= maxMessages)
        {
            //destroy object 
            Destroy(messageList[0].panelInMessage.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message1 newMessage = new Message1();
        newMessage.text = text;

        GameObject newText = Instantiate(panel, chatPanel.transform);

        newMessage.panelInMessage = newText.gameObject;

        newMessage.panelInMessage.GetComponentInChildren<TMP_Text>().text = newMessage.text;

        messageList.Add(newMessage);
    }
}

[System.Serializable]
public class Message1
{
    public string text;

    //TMP_Text ?
    //public TMP_Text textObject;

    public GameObject panelInMessage;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerPopupTest : MonoBehaviour
{
    public int maxMessages = 10;

    public GameObject textObject;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SendMessageHere("hello world !");
        }
    }

    public void SendMessageHere(string text)
    {
        if(messageList.Count >= maxMessages)
        {
            messageList.Remove(messageList[0]);
        }
        Message newMessage = new Message();
        newMessage.text = text;

        messageList.Add(newMessage);
    }
}

[System.Serializable]
public class Message
{
    public string text;
}
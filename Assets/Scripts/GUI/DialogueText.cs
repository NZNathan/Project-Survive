using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueText : MonoBehaviour {

    public Text textbox; 
    private int letterCount = 0;
    private float textDelay = 0.05f;

    public void writeMessage(string message)
    {
        StartCoroutine(writeText(message));
    }

    IEnumerator writeText(string message)
    {
        textbox.text = "\"";

        while(letterCount < message.Length)
        {
            textbox.text += message[letterCount];
            yield return new WaitForSeconds(textDelay);
            letterCount++;
        }

        textbox.text += "\"";
        letterCount = 0;
    }
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    private Text textbox;
    private string sentence;

    private Transform target;

    [Header("Floating Text Variables")]
    public static float textOffset = 0.55f;
    public float textDuration { get; private set; }

    public void Start()
    {
        textbox = GetComponent<Text>();
    }

    /// <summary>
    /// Creates a text line above the targets head for a duration based on the length of the string
    /// </summary>
    public void setup(GameObject target, string sentence)
    {
        this.sentence = sentence;
        textbox.text = sentence;

        textDuration = 0.4f + (sentence.Length * 0.1f);

        this.target = target.transform;

        Invoke("turnoff", textDuration);
    }

    /// <summary>
    /// Clears the text and target, and turns the text object off
    /// </summary>
    public void turnoff()
    {
        //Cancel invoke incase this method was called before the invoke called it
        CancelInvoke();

        textbox.text = "";
        target = null;

        this.gameObject.SetActive(false);
    }

    public GameObject getTarget()
    {
        return target.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector2 textPos = new Vector2(target.position.x, target.position.y + textOffset);

            transform.position = CameraFollow.cam.WorldToScreenPoint(textPos);
        }
        else
        {
            CancelInvoke();
            turnoff();
        }
    }
}

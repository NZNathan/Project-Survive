using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    private Text textbox;
    private string sentence;

    private Transform target;

    [Header("Floating Text Variables")]
    public static float textOffset = 0.35f;
    private float textDuration = 2f;

    public void Start()
    {
        textbox = GetComponent<Text>();
    }

    public void setup(GameObject target, string sentence)
    {
        this.sentence = sentence;
        textbox.text = sentence;

        textDuration = 0.4f + (sentence.Length * 0.1f);

        this.target = target.transform;

        Invoke("turnoff", textDuration);
    }

    public void turnoff()
    {
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

            transform.position = Camera.main.WorldToScreenPoint(textPos);
        }
    }
}

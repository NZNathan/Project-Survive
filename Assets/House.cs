using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

    public GameObject interior;
    public GameObject exterior;

    public GameObject enterTrigger;
    public GameObject exitTrigger;

    private bool inside = false;
    private bool pause = false;

    private void Start()
    {
        interior.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player" && !pause)
        {
            //Turn off/on the exterior and the trigger to enter the house
            exterior.SetActive(inside);
            enterTrigger.SetActive(inside);

            inside = !inside;

            //Turn off/on the interior and the trigger to exit the house
            interior.SetActive(inside);
            exitTrigger.SetActive(inside);

            //So trigger can't trigger instantly again
            pause = true;

            StartCoroutine("pauseAfterTrigger");
        }

    }

    IEnumerator pauseAfterTrigger()
    {
        yield return new WaitForSeconds(0.1f);

        pause = false;
    }

}

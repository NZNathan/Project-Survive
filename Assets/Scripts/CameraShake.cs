using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [Header("Shake Variables")]
    public float shake = 0.0f;
    public float shakeAmount = 0.0f;
    public float decreaseFactor = 1.0f;


    void Update()
    {
        if (shake > 0)
        {
            Debug.Log("Shake");
            transform.localPosition = Random.insideUnitSphere * (shakeAmount/2);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            shake -= Time.deltaTime * decreaseFactor;

        }
        else if (shake != 0.0f)
        {
            shake = 0.0f;
            transform.localPosition = new Vector3(0, 0, -10);
        }
    }

}

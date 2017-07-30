using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public float backgroundSize;
    public float parallaxSpeed;

    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 10f;
    private int leftIndex;
    private int rightIndex;
    private float yValue;

    private float lastCameraX;

	// Use this for initialization
	void Start ()
    {
        cameraTransform = Camera.main.transform;
        layers = new Transform[transform.childCount];

        lastCameraX = cameraTransform.position.x;

        for (int i = 0; i < layers.Length; i++)
            layers[i] = transform.GetChild(i);

        yValue = layers[0].transform.position.y;
        leftIndex = 0;
        rightIndex = layers.Length-1;
    }

    void scrollLeft()
    {

        layers[rightIndex].position = new Vector2(layers[leftIndex].position.x - backgroundSize, yValue);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;
    }

    void scrollRight()
    {

        layers[leftIndex].position = new Vector2(layers[rightIndex].position.x + backgroundSize, yValue);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
            leftIndex = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float deltaX = cameraTransform.position.x - lastCameraX;
        transform.position += Vector3.right * (deltaX * parallaxSpeed);

        lastCameraX = cameraTransform.position.x;

        if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone)){
            scrollLeft();
        }

        if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone)){
            scrollRight();
        }
    }
}

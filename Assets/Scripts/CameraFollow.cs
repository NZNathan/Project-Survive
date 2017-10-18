using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    //Components
    public Transform target;
    public static Camera cam;

    public bool onTitle = false;

    //Raycast Variables
    private LayerMask blockMask; //Layer that all attackable objects are on
    public static float ObjectFade = 0.5f;
    private GameObject fadedObject;

    //Zoom Variables
    private bool zooming = false;
    private float currentZoom;
    public static float defaultZoom = 5f;
    public static float revengeZoom = 1f;

    //Revenge Variables  - Extract out into different class?
    [Header("Revenge Variables")]
    public GameObject cutsceneBars;
    public Text revengeText;
    public Text revengeTextName;

    //Smooth Variables
    public float smoothSpeed = 0.125f;
    public Vector3 offest;

    //Camera Edges
    //Y values to stop camera moving to high or low
    public float yUpperClamp = 0.27f;
    public float yLowerClamp = 0.26f;
    //Is the screen locked in place
    public static bool screenLocked = false;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        blockMask = LayerMask.GetMask("Sprite");
        defaultZoom = cam.orthographicSize;
        currentZoom = defaultZoom;

        if(target != null)
            transform.position = target.transform.position + offest;

        InvokeRepeating("checkPlayerObscured", 1, 0.1f);
    }

    public void setLockScreen(Transform target)
    {
        screenLocked = true;
        transform.position = new Vector2(target.position.x, transform.position.y);
    }

    void checkPlayerObscured()
    {
        if (Player.instance != null && !Player.instance.isInMenu())
        {
            RaycastHit2D hit = Physics2D.Raycast(Player.instance.transform.position, cam.transform.forward, 500, blockMask);
            if (hit.collider != null)
            {
                GameObject objectHit = hit.transform.gameObject;
                if (objectHit != fadedObject)
                {

                    if (objectHit.GetComponent<SpriteRenderer>().sortingOrder > Player.instance.GetComponentInChildren<SpriteRenderer>().sortingOrder)
                        objectHit.GetComponent<SpriteObject>().setTransparency(ObjectFade);

                    if (fadedObject != null)
                        fadedObject.GetComponent<SpriteObject>().setTransparency(1);

                    fadedObject = objectHit;
                }
                return;
            }

            if (fadedObject != null)
            {
                fadedObject.GetComponent<SpriteObject>().setTransparency(1);
                fadedObject = null;
            }
        }
    }

        void smoothZoom()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, revengeZoom, (4f / Time.timeScale) * Time.deltaTime);
    }

    public void resetCamera(Transform target)
    {
        zooming = false;
        currentZoom = defaultZoom;
        cam.orthographicSize = defaultZoom;
        this.target = target;

        transform.position = target.position + offest + new Vector3(0, 1, 0); //Add the extra vector at the end or it resets camera too low on the y axis

        cutsceneBars.SetActive(false);
    }

    public void setZoom(float zoom, Transform newTarget)
    {
        zooming = true;
        currentZoom = zoom;
        target = newTarget;

        cutsceneBars.SetActive(true);

        Enemy enemy = newTarget.GetComponent<Enemy>();

        //Setup text phrase
        if(enemy.isBoss)
            revengeText.text = "BOSS:";
        else
            revengeText.text = "New Revenge Target:";

        revengeTextName.text = enemy.firstName + " " + enemy.lastName;
    }

    bool cameraAtMapEdge(Vector2 nextPos)
    {
        if (onTitle)
            return true;

        if (screenLocked)
            return false;

        //At Right edge of map
        if (nextPos.x > LandscapeGen.rightEdge)
        {
            transform.position = new Vector2(LandscapeGen.rightEdge, nextPos.y);
            return true;
        }
        //At Left edge of map
        if (nextPos.x < LandscapeGen.leftEdge)
        {
            transform.position = new Vector2(LandscapeGen.leftEdge, nextPos.y);
            return true;
        }

        //Not at an edge
        return false;
    }

    //Always match the update of the camera to the update of the player (possibly lateupdate to update)
    void FixedUpdate()
    {
        if (currentZoom != cam.orthographicSize && zooming)
            smoothZoom();

        if (target != null)
        {
            Vector3 desiredPos = target.position + offest;

            //Clamp Y value if out of bounds
            if(desiredPos.y > yUpperClamp && !zooming)
                desiredPos = new Vector3(desiredPos.x, yUpperClamp, 0);
            else if (desiredPos.y < yLowerClamp && !zooming)
                desiredPos = new Vector3(desiredPos.x, yLowerClamp, 0);

            if (cameraAtMapEdge(desiredPos) || screenLocked)
                desiredPos = new Vector2(transform.position.x, desiredPos.y);

            Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);

            transform.position = smoothPos + offest;
        }
    }
}

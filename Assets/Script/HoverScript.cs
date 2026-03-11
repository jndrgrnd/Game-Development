using NUnit.Framework.Constraints;
using System;
using TMPro;
using UnityEditor.Toolbars;
using UnityEngine;

public class HoverScript : MonoBehaviour
{
    Vector3 mousePosition;
    RaycastHit2D raycastHit2d;
    Transform prevHoverObject, nextHoverObject;
    private Vector3 cameraTargetPos;
    public GameObject camera, hitboxes, 
        rightHitbox, leftHitbox;

    public float speedCam = 11;
    private float speedCamToLeft = 17;
    private bool hasMoved = false;
    public float tolerance = 0.01f;

    public int currentPos = 1;
    public int toPosition = 1;

    private void Start()
    {
        if (currentPos == 1)
        {
            camera.transform.position = new Vector3(0, camera.transform.position.y, camera.transform.position.z);
        }
    }

    void Update()
    {
        mousePosition = Input.mousePosition;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        prevHoverObject = nextHoverObject;

        raycastHit2d = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        nextHoverObject = raycastHit2d ? raycastHit2d.collider.transform : null;

        Debug.Log($"Current Position: {currentPos}");
        Debug.Log($"Heading to: {toPosition}");
        Debug.Log($"Hover Object: {nextHoverObject}");

        // Move the Camera to the Right
        if (currentPos == 1)
        {
            if (nextHoverObject != null && nextHoverObject.name == "RightObject")
            {
                hasMoved = true;
                toPosition = 2;

                rightHitbox.SetActive(false);
            }
            else if (nextHoverObject != null && nextHoverObject.name == "LeftObject")
            {
                hasMoved = true;
                toPosition = 0;

                leftHitbox.SetActive(false);
            }

            if (hasMoved && (toPosition == 2 || !isObjectActive(rightHitbox)))
            {
                if (!isObjectActive(leftHitbox))
                {
                    leftHitbox.SetActive(true);
                }

                Debug.Log("Moving Camera to the Right Pane");
                cameraTargetPos = new Vector3(8.64f, camera.transform.position.y, camera.transform.position.z);

                camera.transform.position =
                    Vector3.MoveTowards(
                        camera.transform.position,
                        cameraTargetPos,
                        speedCam * Time.deltaTime
                    );

                if (isCameraAtClosePosition())
                {
                    Debug.Log("Camera and Hitboxes has reached the target right pane position.");

                    currentPos = 2;
                    Debug.Log($"Current Position at {currentPos}");
                }

                if (isCameraAtExactPosition()) { hasMoved = false; }
            }
            else if (hasMoved && (toPosition == 0 || !isObjectActive(leftHitbox)))
            {
                if (!isObjectActive(rightHitbox))
                {
                    rightHitbox.SetActive(true);
                }

                Debug.Log("Moving Camera to the Left Pane");
                cameraTargetPos = new Vector3(-17.64f, camera.transform.position.y, camera.transform.position.z);

                camera.transform.position =
                    Vector3.MoveTowards(
                        camera.transform.position,
                        cameraTargetPos,
                        speedCamToLeft * Time.deltaTime
                    );

                if (isCameraAtClosePosition())
                {
                    Debug.Log("Camera and Hitboxes has reached the target left pane rotation.");

                    currentPos = 0;
                    Debug.Log($"Current Position at {currentPos}");
                }

                if (isCameraAtExactPosition()) { hasMoved = false; }
            }
        }
        else if (currentPos == 0  || (!isObjectActive(leftHitbox) && isObjectActive(rightHitbox)))
        {
            if (nextHoverObject != null && nextHoverObject.name == "RightObject")
            {
                hasMoved = true;
                toPosition = 1;

                leftHitbox.SetActive(true);
            }

            if (hasMoved && (toPosition == 1 && isObjectActive(rightHitbox)))
            {
                if (!isObjectActive(rightHitbox))
                {
                    rightHitbox.SetActive(true);
                }

                Debug.Log("Moving Camera to the Center Pane");
                cameraTargetPos = new Vector3(0f, camera.transform.position.y, camera.transform.position.z);

                camera.transform.position =
                    Vector3.MoveTowards(
                        camera.transform.position,
                        cameraTargetPos,
                        speedCamToLeft * Time.deltaTime
                    );

                if (isCameraAtClosePosition())
                {
                    Debug.Log("Camera and Hitboxes has reached the original position.");

                    currentPos = 1;
                    Debug.Log($"Current Position at {currentPos}");
                }

                if (isCameraAtExactPosition()) { hasMoved = false; }
            }
        }
        // After Disabling the Right Hover, Move the Camera to the Left
        else if (currentPos == 2 || (isObjectActive(leftHitbox) && !isObjectActive(rightHitbox)))
        {
            if (nextHoverObject != null && nextHoverObject.name == "LeftObject")
            { 
                hasMoved = true;
                toPosition = 1;

                rightHitbox.SetActive(true);
            }

            if (hasMoved && (toPosition == 1 && isObjectActive(leftHitbox)))
            {
                if (!isObjectActive(leftHitbox))
                {
                    leftHitbox.SetActive(true);
                }

                Debug.Log("Moving Camera to the Center Pane");
                cameraTargetPos = new Vector3(0f, camera.transform.position.y, camera.transform.position.z);

                camera.transform.position =
                    Vector3.MoveTowards(
                        camera.transform.position,
                        cameraTargetPos,
                        speedCam * Time.deltaTime
                    );

                if (isCameraAtClosePosition())
                {
                    Debug.Log("Camera and Hitboxes has reached the original position.");

                    currentPos = 1;
                    Debug.Log($"Current Position at {currentPos}");
                }

                if (isCameraAtExactPosition()) { hasMoved = false; }
            }
        }
    }


    bool isCameraAtClosePosition()
    { return Vector3.Distance(camera.transform.position, cameraTargetPos) < tolerance; }

    bool isCameraAtExactPosition()
    { return camera.transform.position == cameraTargetPos; }

    bool isObjectActive(GameObject obj)
    { return obj.activeSelf; }
}

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
    Quaternion cameraTargetRotation;
    private Vector3 cameraTargetPos;
    public GameObject camera, hitboxes, 
        rightHitbox, leftHitbox;

    public float speedCam, rotationSpeed;
    private bool hasMoved = false;
    public float tolerance = 0.01f;

    public int currentPos;
    public int toPosition;

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
        Debug.Log($"Hover Object: {nextHoverObject}");

        // Move the Camera to the Right
        if (currentPos == 1)
        {
            if (nextHoverObject != null && nextHoverObject.name == "RightObject")
            {
                hasMoved = true;
                toPosition = 2;
            }
            else if (nextHoverObject != null && nextHoverObject.name == "LeftObject")
            {
                hasMoved = true;
                toPosition = 0;
            }

            if (hasMoved && toPosition == 2)
            {
                rightHitbox.SetActive(false);
                Debug.Log("Right Hitbox has been turn off");

                cameraTargetPos = new Vector3(8.64f, camera.transform.position.y, camera.transform.position.z);

                camera.transform.position =
                    Vector3.MoveTowards(
                        camera.transform.position,
                        cameraTargetPos,
                        speedCam * Time.deltaTime
                    );

                if (isCameraAtTargetPosition())
                {
                    Debug.Log("Camera and Hitboxes has reached the target right pane position.");

                    currentPos = 2;
                    Debug.Log($"Current Position at {currentPos}");

                    hasMoved = false;
                }
            }
            else if (hasMoved && toPosition == 0)
            {
                leftHitbox.SetActive(false);
                Debug.Log("Left Hitbox has been turn off");

                cameraTargetRotation = Quaternion.Euler(camera.transform.rotation.x, -82.041f, camera.transform.rotation.z);

                camera.transform.rotation = 
                    Quaternion.RotateTowards(
                        camera.transform.rotation,
                        cameraTargetRotation,
                        rotationSpeed * Time.deltaTime
                    );

                if (isCameraAtTargetRotation())
                {
                    Debug.Log("Camera and Hitboxes has reached the target left pane rotation.");

                    currentPos = 0;
                    Debug.Log($"Current Position at {currentPos}");

                    hasMoved = false;
                }
            }
        }
        // After Disabling the Right Hover, Move the Camera to the Left
        else if (currentPos == 2)
        {
            if (nextHoverObject != null && nextHoverObject.name == "LeftObject")
            {
                hasMoved = true;

                rightHitbox.SetActive(true);
                Debug.Log("Right Hitbox has been turn on");
            }

            if (hasMoved)
            {
                cameraTargetPos = new Vector3(0f, camera.transform.position.y, camera.transform.position.z);

                camera.transform.position =
                    Vector3.MoveTowards(
                        camera.transform.position,
                        cameraTargetPos,
                        speedCam * Time.deltaTime
                    );

                if (isCameraAtTargetPosition())
                {
                    Debug.Log("Camera and Hitboxes has reached the original position.");

                    currentPos = 1;
                    Debug.Log($"Current Position at {currentPos}");

                    hasMoved = false;
                }
            }
        }
        else if (currentPos == 0)
        {
            if (nextHoverObject != null && nextHoverObject.name == "RightObject")
            {
                hasMoved = true;

                leftHitbox.SetActive(true);
                Debug.Log("Left Hitbox has been turn on");
            }

            if (hasMoved)
            {
                cameraTargetRotation = Quaternion.Euler(camera.transform.rotation.x, 0f, camera.transform.rotation.z);

                camera.transform.rotation =
                    Quaternion.RotateTowards(
                        camera.transform.rotation,
                        cameraTargetRotation,
                        rotationSpeed * Time.deltaTime
                    );

                if (isCameraAtTargetRotation())
                {
                    Debug.Log("Camera and Hitboxes has reached the original position.");

                    currentPos = 1;
                    Debug.Log($"Current Position at {currentPos}");

                    hasMoved = false;
                }
            }
        }
    }


    bool isCameraAtTargetPosition()
    {
        return Vector3.Distance(camera.transform.position, cameraTargetPos) < tolerance;
    }

    bool isCameraAtTargetRotation()
    {
        return Quaternion.Angle(camera.transform.rotation, cameraTargetRotation) < tolerance;
    }

}

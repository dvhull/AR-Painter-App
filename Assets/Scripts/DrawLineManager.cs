using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
//using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using System;

public class DrawLineManager : MonoBehaviour
{
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;
    // note: 1f = 1 meter in real life
    [SerializeField]
    private float startWidth; 
    [SerializeField]
    private float endWidth;


    private LineRenderer currentLine;
    private int currentLinePointNumber;

    private bool painting;

    void Start()
    {
        currentLinePointNumber = 0;
        painting = false;
    }

    void Update()
    {

        if (Input.touchCount > 0)
        {
            // just pressed down finger
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                StartNewLine();
                SetLineWidth();
                SetLineColor();

                // start painting
                painting = true;
            }

            // just lifted finger up
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // stop painting
                painting = false;
            }

            // painting
            else if (painting)
            {
                UpdateLine(CalculateFingerPosition());
            }

        }
    }

    private void StartNewLine()
    {
        GameObject emptyGameObject = new GameObject();
        currentLine = emptyGameObject.AddComponent<LineRenderer>();
        currentLinePointNumber = 0;
    }

    private void SetLineWidth()
    {
        currentLine.startWidth = startWidth;
        currentLine.endWidth = endWidth;
    }

    private void SetLineColor()
    {
        currentLine.material = new Material(Shader.Find("Sprites/Default"));
        currentLine.startColor = startColor;
        currentLine.endColor = endColor;
    }

    // this works by raycasting from a finger to the plane in front of the camera
    // and returns the hit position in worldspace
    private Vector3 CalculateFingerPosition()
    {
        Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        
        // Layer 8 is reserved for the plane in front of the camera
        if (Physics.Raycast(rayOrigin, out hitInfo, 1 << 8))
        {
            return hitInfo.point;
        }

        else
        {
            Debug.Log("Broken");
            return new Vector3(0, 0, 0);
        }
    }

    private void UpdateLine(Vector3 position)
    {
        currentLine.positionCount = currentLinePointNumber + 1;
        currentLine.SetPosition(currentLinePointNumber, position);
        currentLinePointNumber++;
    }

}

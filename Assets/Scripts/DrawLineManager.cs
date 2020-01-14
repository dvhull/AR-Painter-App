using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;
using System;

public class DrawLineManager : MonoBehaviour
{
    // line settings (can be adjusted within inspector)
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;
    [SerializeField]
    private float startWidth; // 1f = 1 meter in real life
    [SerializeField]
    private float endWidth;

    // current line
    private LineRenderer currentLine;
    private int currentLinePointNumber;
    
    // whether a line is currently being drawn
    private bool isPainting;

    void Start()
    {
        currentLinePointNumber = 0;
        isPainting = false;
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
                isPainting = true;
            }

            // just lifted finger up
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // stop painting
                isPainting = false;
            }

            // painting
            else if (isPainting)
            {
                // update line segment given the position of the phone and finger
                UpdateLine(CalculateFingerPosition()); 
            }
        }
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
    
    private void StartNewLine()
    {
        GameObject emptyGameObject = new GameObject();
        currentLine = emptyGameObject.AddComponent<LineRenderer>();
        currentLinePointNumber = 0;
    }

    // raycast from the finger on the screen to the plane in front of the camera
    // and return the hit position in worldspace 
    private Vector3 CalculateFingerPosition()
    {
        Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        
        // layer 8 is reserved for the plane in front of the camera
        if (Physics.Raycast(rayOrigin, out hitInfo, 1 << 8))
        {
            return hitInfo.point;
        }
        
        // raycast missed plane 
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

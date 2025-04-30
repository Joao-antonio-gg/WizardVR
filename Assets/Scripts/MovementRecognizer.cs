using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using PDollarGestureRecognizer;

public class MovementRecognizer : MonoBehaviour
{
    public XRNode inputSource; // The XRNode to track (LeftHand or RightHand)
    public InputHelpers.Button inputButton; // The button to check for input
    public float inputThreshold = 0.1f; // The threshold for button press detection
    public Transform movementSource; // The transform to track the movement of the wand

    public float newPositionThreshold = 0.05f; // The threshold for position change detection
    public GameObject debugCubePrefab; // Prefab for the debug cube

    private bool isMoving = false; // Flag to indicate if the user is making a wand movement
    private List<Vector3> positionList = new List<Vector3>(); // List to store the positions of the wand
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreshold);

        //Starting wand movement
        if (isPressed && !isMoving)
        {
            StartMovement();
        }
        //Ending wand movement
        else if (!isPressed && isMoving)
        {
            EndMovement();
        }
        //Updating wand movement
        else if (isPressed && isMoving)
        {
            UpdateMovement();
        }
    }

    void StartMovement()
    {
        Debug.Log("Wand movement started");
        isMoving = true;
        positionList.Clear(); // Clear the list to start fresh
        positionList.Add(movementSource.position); // Add the initial position of the wand
        if (debugCubePrefab)
        {
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity),3); // Instantiate the debug cube at the wand's position
        }
    }
    void EndMovement()
    {
        Debug.Log("Wand movement ended");
        isMoving = false;

        //create a gesture from the position list
    }
    void UpdateMovement()
    {
        Debug.Log("Wand is moving");
        // Store the current position of the wand in the list
        Vector3 lastPosition = positionList[positionList.Count - 1];
        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThreshold)
        {
            positionList.Add(movementSource.position);
            if (debugCubePrefab)
            {
                Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity),3); // Instantiate the debug cube at the wand's position
            }
        }
        
    }
}

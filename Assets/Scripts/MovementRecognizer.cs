using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;

public class MovementRecognizer : MonoBehaviour
{
    public XRNode inputSource; // The XRNode to track (LeftHand or RightHand)
    public InputHelpers.Button inputButton; // The button to check for input
    public float inputThreshold = 0.1f; // The threshold for button press detection
    public Transform movementSource; // The transform to track the movement of the wand

    public float newPositionThreshold = 0.05f; // The threshold for position change detection
    public GameObject debugCubePrefab; // Prefab for the debug cube
    public bool creationMode = true; // Flag to indicate if the gesture is being created
    public string newGestureName; // Name of the gesture to be created

    public float gestureRecognitionThreshold = 0.8f; // Threshold for gesture 
    
    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { } // Custom Unity event for string parameters
    public UnityStringEvent OnGestureRecognized; // Event to be triggered when a gesture is recognized

    private List<Gesture> trainingSet = new List<Gesture>(); // List to store the training set of gestures
    private bool isMoving = false; // Flag to indicate if the user is making a wand movement
    private List<Vector3> positionList = new List<Vector3>(); // List to store the positions of the wand
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml"); // Get all gesture files in the persistent data path
        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreshold);
       // Debug.Log("Is pressed" + isPressed);

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
        Point[] pointArray = new Point[positionList.Count];
        for (int i = 0; i < positionList.Count; i++)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0); // Create a new Point object for each position
        }
        Gesture newGesture = new Gesture(pointArray); // Create a new gesture with the points
        if (creationMode)
        {
            newGesture.Name = newGestureName; // Set the name of the gesture
            trainingSet.Add(newGesture); // Add the new gesture to the training set

            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml"; // Define the file path for saving the gesture
            GestureIO.WriteGesture(pointArray, newGestureName, fileName); // Save the gesture to a file
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray()); // Classify the gesture
            Debug.Log("Gesture recognized: " + result.GestureClass); // Log the recognized gesture class
            if (result.Score > gestureRecognitionThreshold) // Check if the score is above the threshold
            {
                OnGestureRecognized.Invoke(result.GestureClass); // Invoke the event with the recognized gesture class
            }
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{

    public GameObject thrownObject;

    public LineRenderer lineRenderer;
    [Min(3)] // ensuring that value is at least 3
    private int lineSegments = 30;
    [SerializeField, Min(1)] // makes private variable visible for testing, ensuring that value is at least 1
    private float timeOfTheFlight = 2;

    public float trajectoryDepthLimit = 0f; //line isn't drawn below this point

    public float defaultAngle = 30;
    public float defaultMagnitude = 7;
    public float angleControlScale = 20;
    public float magControlScale = 400;

    [SerializeField]
    private float angle = 30;
    [SerializeField]
    private float magnitude = 10;

    // mouse drag information
    private Vector3 mouseStartPosition;
    private bool isDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        mouseStartPosition = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            angle = defaultAngle + (Input.mousePosition - mouseStartPosition).y / angleControlScale;
            magnitude = defaultMagnitude + (Input.mousePosition - mouseStartPosition).y / magControlScale;
        }
            

        Vector3 startPoint = transform.position;
        Vector3 startVelocity = GetStartVelocity(angle, magnitude);

        // Draw trajectory on game board
        ShowTrajectoryLine(startPoint, startVelocity);

        // if clicked
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            mouseStartPosition = Input.mousePosition; // Store the starting position
            isDragging = true;
        }

        // Detect mouse release (end of the drag)
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;

            // Calculate and print the extent of the drag
            Vector3 dragExtent = Input.mousePosition - mouseStartPosition;
            // only care about y dimension
            //Debug.Log("Drag extent: " + dragExtent);
            //Debug.Log("Drag distance: " + dragExtent.magnitude); // Prints the length of the drag

            // if right click, throw bomb
            CreateBomb(startPoint, startVelocity);
        }
    }

    Vector3 GetStartVelocity(float angle, float magnitude)
    {
        Vector3 forward = transform.forward;
        Vector3 forwardOnXZPlane = new Vector3(forward.x, 0, forward.z).normalized;

        float yComponent = Mathf.Sin(Mathf.Deg2Rad * angle);
        float horizontalMagnitude = Mathf.Cos(Mathf.Deg2Rad * angle);

        Vector3 velocity = forwardOnXZPlane * horizontalMagnitude;
        velocity.y = yComponent;
        velocity *= magnitude;
        return velocity;
    }

    void CreateBomb(Vector3 startPoint, Vector3 startVelocity)
    {
        Rigidbody rb = Instantiate(thrownObject, startPoint, Quaternion.identity).GetComponent<Rigidbody>();
        rb.velocity = startVelocity;
    }

    public void ShowTrajectoryLine(Vector3 startpoint, Vector3 startVelocity)
    {
        /*https://www.youtube.com/watch?v=U3hovyIWBLk*/

        //The more points we add the smoother the line will be
        float timeStep = timeOfTheFlight / lineSegments;

        Vector3[] lineRendererPoints = CalculateTrajectoryLine(startpoint, startVelocity, timeStep);

        //for (int i = 0; i < lineRendererPoints.Length; i++)
        //    Debug.Log(lineRendererPoints[i].ToString());

        lineRenderer.positionCount = lineRendererPoints.Length;
        lineRenderer.SetPositions(lineRendererPoints);
    }

    private Vector3[] CalculateTrajectoryLine(Vector3 startpoint, Vector3 startVelocity, float timestep)
    {
        List<Vector3> lineRendererPoints = new List<Vector3>();
        lineRendererPoints.Add(startpoint);

        for (int i = 1; i < lineSegments; i++)
        {
            float timeOffset = timestep * i;

            Vector3 progressBeforeGravity = startVelocity * timeOffset;
            Vector3 gravityOffset = Vector3.up * -0.5f * Physics.gravity.y * timeOffset * timeOffset;
            Vector3 newPosition = startpoint + progressBeforeGravity - gravityOffset;
            if (newPosition.y < trajectoryDepthLimit) break;
            else lineRendererPoints.Add(newPosition);

        }
        return lineRendererPoints.ToArray();

    }
}

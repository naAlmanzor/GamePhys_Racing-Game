using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    // public Transform[] GameManager.instance.checkpoints;
    public float speed = 10.0f;
    public float obstacleDistance = 3.0f;
    public float obstacleAvoidanceSpeed = 5.0f;
    public LayerMask obstacleLayerMask;
    public bool visualizeSpherecast;

    private int currentCheckpointIndex = 0;
    private bool obstacleInPath = false;

    void Update()
    {
        // Calculate the direction to the current checkpoint
        Vector3 direction = (GameManager.instance.checkpoints[currentCheckpointIndex].transform.position - transform.position).normalized;

        // Calculate the angle between the car's forward direction and the direction to the checkpoint
        float angle = Vector3.SignedAngle(transform.right, direction, Vector3.up);

        if (!obstacleInPath)
        {
            // Rotate the car towards the checkpoint with a maximum rotation speed
            float maxRotationSpeed = 180.0f;
            float rotationSpeed = Mathf.Clamp(angle, -maxRotationSpeed, maxRotationSpeed);
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Check if the car is close enough to the current checkpoint to start moving towards the next one
            float distanceToCheckpoint = Vector3.Distance(transform.position, GameManager.instance.checkpoints[currentCheckpointIndex].transform.position);
            float checkpointRange = 10.0f;
            if (distanceToCheckpoint < checkpointRange)
            {
                // Switch to the next checkpoint
                currentCheckpointIndex = (currentCheckpointIndex + 1) % GameManager.instance.checkpoints.Length;
            }

            // Move the car towards the current checkpoint
            transform.position += transform.right * speed * Time.deltaTime;

            /// Check for obstacles
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 0.5f, transform.right, out hit, obstacleDistance, obstacleLayerMask, QueryTriggerInteraction.Ignore))
            {
                // Avoid obstacle by turning left or right
                if (hit.collider.CompareTag("Car"))
                {
                    // Turn more realistically towards the other car
                    CarAI otherCar = hit.collider.GetComponent<CarAI>();
                    float otherCarAngle = Vector3.SignedAngle(transform.right, hit.transform.right, Vector3.up);
                    float avoidanceAngle = otherCarAngle > 0 ? -90.0f : 90.0f;
                    avoidanceAngle *= Mathf.Clamp01(Mathf.Abs(otherCarAngle) / 90.0f) * 2.0f;
                    avoidanceAngle += Random.Range(-15.0f, 15.0f);
                    transform.Rotate(Vector3.up, avoidanceAngle);
                }
                else
                {
                    // Turn randomly if the obstacle is not another car
                    obstacleInPath = true;
                    transform.Rotate(Vector3.up, Random.Range(-45f, 45f));
                }
            
                // Ignore collisions between the car's collider and the obstacle
                Physics.IgnoreCollision(GetComponent<Collider>(), hit.collider);
            }
        }
        else
        {
            // Avoid obstacle by moving sideways at a slower speed
            transform.Translate(Vector3.forward * obstacleAvoidanceSpeed * Time.deltaTime);

            // Check if obstacle is cleared
            RaycastHit hit;
            if (!Physics.SphereCast(transform.position, 3f, transform.right, out hit, obstacleDistance, obstacleLayerMask))
            {
                // Obstacle is cleared, resume original path
                obstacleInPath = false;
            }
        }

        // Visualize the spherecast if requested
        if (visualizeSpherecast)
        {
            Debug.DrawRay(transform.position, transform.right * obstacleDistance, Color.red);
            Debug.DrawRay(transform.position, (Quaternion.AngleAxis(45, Vector3.up) * transform.right) * obstacleDistance, Color.red);
            Debug.DrawRay(transform.position, (Quaternion.AngleAxis(-45, Vector3.up) * transform.right) * obstacleDistance, Color.red);
        }
    }
}

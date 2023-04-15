using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 10f; // car's speed
    public float rotationSpeed = 100f; // car's rotation speed
    public int currentCheckpointIndex = 0; // current checkpoint index
    private float horizontalInput; // user's input for left/right arrow keys
    private float verticalInput; // user's input for up/down arrow keys

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // get user's input for left/right arrow keys
        verticalInput = Input.GetAxis("Vertical"); // get user's input for up/down arrow keys

        if(currentCheckpointIndex == 5) {
            currentCheckpointIndex = 0;
        }
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed * verticalInput); // move the car forward/backward
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * horizontalInput); // rotate the car left/right
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            currentCheckpointIndex = other.GetComponent<Checkpoint>().index;
        }
    }
}

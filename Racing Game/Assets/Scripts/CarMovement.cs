using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 10f; // car's speed
    public float rotationSpeed = 100f; // car's rotation speed
    private float horizontalInput; // user's input for left/right arrow keys
    private float verticalInput; // user's input for up/down arrow keys
    public List<Checkpoint> passedCheckpoints = new List<Checkpoint>(); // checkpoints the car has passed
    public int currentCheckpointIndex = 0;
    public int pastCheckpointIndex = 0;

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // get user's input for left/right arrow keys
        verticalInput = Input.GetAxis("Vertical"); // get user's input for up/down arrow keys
        // Debug.Log("Past: " + pastCheckpointIndex + "\nCurrent: " + currentCheckpointIndex);
        if(verticalInput != 0) {
            if(!AudioManager.instance.isPlaying("Rev")) {
                AudioManager.instance.Play("Rev");
            }
        } else {
            AudioManager.instance.Stop("Rev");
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
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();

            pastCheckpointIndex = currentCheckpointIndex;
            currentCheckpointIndex = checkpoint.index;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.transform.CompareTag("Obstacle")) {
            AudioManager.instance.Play("Crash");
        }    
    }
}


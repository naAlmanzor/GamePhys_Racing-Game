using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class CarMovement : NetworkBehaviour
{
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isAccelerating, _isTurning;

    public float speed = 10f; // car's speed
    public float rotationSpeed = 100f; // car's rotation speed
    private float horizontalInput; // user's input for left/right arrow keys
    private float verticalInput; // user's input for up/down arrow keys
    public List<Checkpoint> passedCheckpoints = new List<Checkpoint>(); // checkpoints the car has passed
    public int currentCheckpointIndex = 0;
    public int pastCheckpointIndex = 0;

    private void Awake() {
        _controller = GetComponent<CharacterController>();
    }

    private void Update() {
        horizontalInput = Input.GetAxis("Horizontal"); // get user's input for left/right arrow keys
        verticalInput = Input.GetAxis("Vertical"); // get user's input for up/down arrow keys

        if(horizontalInput != 0)
        {
            _isTurning = true;
        }

        if(verticalInput != 0)
        {
            _isAccelerating = true;
        }

        transform.Translate(Vector3.right * Time.deltaTime * speed * verticalInput); // move the car forward/backward
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * horizontalInput); // rotate the car left/right
    }

    public override void FixedUpdateNetwork()
    {
        if(!HasStateAuthority)
        {
            return;
        }

        Vector3 move = Runner.DeltaTime * speed * new Vector3(horizontalInput, 0f, verticalInput);
        _controller.Move(move + _velocity * Runner.DeltaTime);
    }

    // void FixedUpdate()
    // {
    //     transform.Translate(Vector3.right * Time.deltaTime * speed * verticalInput); // move the car forward/backward
    //     transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * horizontalInput); // rotate the car left/right
    // }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Checkpoint"))
    //     {
    //         Checkpoint checkpoint = other.GetComponent<Checkpoint>();

    //         pastCheckpointIndex = currentCheckpointIndex;
    //         currentCheckpointIndex = checkpoint.index;
    //     }
    // }

    // private void OnCollisionEnter(Collision other) {
    //     if(other.transform.CompareTag("Obstacle")) {
    //         AudioManager.instance.Play("Crash");
    //     }    
    // }
}


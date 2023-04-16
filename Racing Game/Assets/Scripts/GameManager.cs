using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CarMovement playerCar;
    public CarAI[] aiCars;
    public Checkpoint[] checkpoints;
    public int totalLaps = 3;
    public Text lapCountText;
    public Text positionText;

    private int lapCount = 0;
    private int playerPosition = 1;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Set lap count UI text
        lapCountText.text = "Lap " + (lapCount + 1) + "/" + totalLaps;
    }

    void Update()
    {
        // Update player lap count
        if (playerCar.currentCheckpointIndex == 5)
        {
            lapCount++;
            playerCar.currentCheckpointIndex = 0;

            // Check if player finished the race
            if (lapCount >= totalLaps)
            {
                Debug.Log("Player finished the race!");

                // Disable player car movement
                playerCar.enabled = false;
            }
            else
            {
                // Set lap count UI text
                lapCountText.text = "Lap " + (lapCount + 1) + "/" + totalLaps;
            }
        }

        // Update player race position
        int aiAheadCount = 0;
        foreach (CarAI aiCar in aiCars)
        {
            if (playerCar.transform.position.z < aiCar.transform.position.z)
            {
                aiAheadCount++;
            }
        }
        playerPosition = aiCars.Length + 1 - aiAheadCount;
        positionText.text = playerPosition.ToString();
    }
}

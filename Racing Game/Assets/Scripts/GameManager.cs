using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public CarMovement playerCar;
    public CarAI[] aiCars;
    public Checkpoint[] checkpoints;
    public int totalLaps = 3;
    public TextMeshProUGUI lapCountText;
    public TextMeshProUGUI positionText;

    private int lapCount = 0;
    private int playerPosition = 1;

    public static GameManager instance;

    public bool hasFired;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        Checkpoint.LapEvent += AddLap;
    }

    private void OnDisable() {
        Checkpoint.LapEvent -= AddLap;
    }

    void Start()
    {
        // Set lap count UI text
        lapCountText.SetText((lapCount + 1).ToString());
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "GameScene") {
            Debug.Log(lapCount);

            if (lapCount >= totalLaps)
            {
                SceneManager.LoadScene("MainMenu");
            }

            lapCountText.SetText((lapCount + 1).ToString());

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
            positionText.SetText(playerPosition.ToString());
        }

        if(Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void AddLap() {
        lapCount++;
    }
}

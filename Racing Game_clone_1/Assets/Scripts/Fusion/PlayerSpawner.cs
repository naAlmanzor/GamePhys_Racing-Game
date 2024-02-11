using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    private bool spawned;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Debug.Log("Joined");
        }
    }

    private void Update()
    {
        if(!spawned)
        {    
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                Runner.Spawn(PlayerPrefab, new Vector3(0, 4, 0), Quaternion.identity);
                spawned = true;
            }
            else
            {
                spawned = false;
            }
        }
    }
}
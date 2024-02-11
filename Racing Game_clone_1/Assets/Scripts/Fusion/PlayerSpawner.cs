using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    public GameObject PlayerPrefab;
    private bool spawned;
    private PlayerRef localPlayerRef;
    private bool LoadInvoked;
    public GameModeHandler gameModeHandler;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            localPlayerRef = player;
            Debug.Log($"{localPlayerRef} joined");

            LoadInvoked = false;
        }
    }

    public async void PlayerLeft(PlayerRef player)
    {
        Debug.Log($"{localPlayerRef} left");
        if(!LoadInvoked) await RpcLoadMenuScene();
    }

    

    private void Update()
    {
        if(!spawned)
        {    
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                Debug.Log("Spawned Car");
                // if(!Runner) Runner.Spawn(PlayerPrefab, new Vector3(0, 6, 0), Quaternion.identity);
                spawned = true;
            }
            else
            {
                spawned = false;
            }
        }
    }

    private async Task RpcLoadMenuScene() {
        if(!SceneManager.GetSceneByBuildIndex(0).isLoaded) await Runner.LoadScene(SceneRef.FromIndex(0), LoadSceneMode.Additive);
        if(SceneManager.GetSceneByBuildIndex(1).isLoaded) await Runner.UnloadScene(SceneRef.FromIndex(1));
        LoadInvoked = true;

        await gameModeHandler.DisconnectFromRunner();
    }
}
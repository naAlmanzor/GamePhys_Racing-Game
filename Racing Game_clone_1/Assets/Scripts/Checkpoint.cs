using System;
using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int index;
    public static event Action LapEvent;
    public bool lapAdded;
    public bool hasFired;

    private void OnTriggerEnter(Collider other) {
        // if(other.CompareTag("Player")) {
        //     if(GameManager.instance.playerCar.pastCheckpointIndex == 3 && GameManager.instance.playerCar.currentCheckpointIndex == 4) {
            
        //         LapEvent.Invoke();
        //     }
        // }
    }

}

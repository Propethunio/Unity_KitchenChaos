using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : CounterMaster {

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    [SerializeField] private float spawnPlateTimerMax = 4f;

    private int PlatesSpawnedAmount;
    private int PlatesSpawnedAmountMax = 4;
    private float spawnPlateTimer;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax) {
            spawnPlateTimer = 0f;
            if(PlatesSpawnedAmount < PlatesSpawnedAmountMax) {
                PlatesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        if(!player.HasKitchenObject()) {
            if(PlatesSpawnedAmount > 0) {
                spawnPlateTimer = 0f;
                PlatesSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
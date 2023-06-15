using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : CounterMaster {

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    [SerializeField] private float spawnPlateTimer = 4f;

    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;
    private float timer;

    private void Awake() {
        timer = spawnPlateTimer;
    }

    private void Update() {
        timer -= Time.deltaTime;
        if(timer < 0) {
            timer = spawnPlateTimer;
            if(GameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax) {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        if(!player.HasKitchenObject()) {
            if(platesSpawnedAmount > 0) {
                spawnPlateTimer = 0f;
                platesSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
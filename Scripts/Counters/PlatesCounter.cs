using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter {

    public event EventHandler OnPlateSpawn;
    public event EventHandler OnPlateRemoved;
    
    [SerializeField] private float spawnPlateTimeMax = 4f;
    [SerializeField] private int plateSpawnedAmounteMax = 4;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private int plateSpawnedAmount;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimeMax) {
            
            spawnPlateTimer = 0;

            if (plateSpawnedAmount < plateSpawnedAmounteMax) {
                plateSpawnedAmount++;

                OnPlateSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            if (plateSpawnedAmount > 0) {
                plateSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public int GetPlatesSpawnedAmount() { return plateSpawnedAmount; }
}

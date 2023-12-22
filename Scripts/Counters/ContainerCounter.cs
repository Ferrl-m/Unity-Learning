using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnOpenClose;
    
    public override void Interact(Player player) {
        if(!player.HasKitchenObject()) {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnOpenClose?.Invoke(this, EventArgs.Empty);
        }
    }
}

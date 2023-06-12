using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : CounterMaster {

    public event EventHandler OnPlayerGrabbedObject;

    public KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {
        if(!player.HasKitchenObject()) {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
} 
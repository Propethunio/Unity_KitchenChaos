using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : CounterMaster {

    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData() {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(Player player) {
        if(player.HasKitchenObject()) {
            player.GetKitchenObject().DestrySelf();
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
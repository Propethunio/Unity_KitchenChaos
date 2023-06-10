using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : CounterMaster {

    public override void Interact(Player player) {
        if(player.HasKitchenObject()) {
            player.GetKitchenObject().DestrySelf();
        }
    }
}
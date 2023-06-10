using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : CounterMaster {

    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject()) {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    Invoke(nameof(DestroyKitchenObject), 3f);
                }
            }
        }
    }

    private void DestroyKitchenObject() {
        if(HasKitchenObject()) {
            GetKitchenObject().DestrySelf();
        }
    }
}
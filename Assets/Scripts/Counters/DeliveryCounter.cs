using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : CounterMaster {

    private PlateKitchenObject currPlateKitchenObject;

    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject()) {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    currPlateKitchenObject = plateKitchenObject;
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    Invoke(nameof(TryDeliverRecipe), 3f);
                }
            }
        }
    }

    private void TryDeliverRecipe() {
        if(HasKitchenObject()) {
            DeliveryManager.Instance.DeliverRecipe(currPlateKitchenObject);
            GetKitchenObject().DestrySelf();
        }
    }
}
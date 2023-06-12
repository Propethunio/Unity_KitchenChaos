using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : CounterMaster, IHasProgress {

    public static event EventHandler OnAnyCut;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        } else {
            if(!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
                cuttingProgress = 0;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0
                });
            } else {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestrySelf();
                        cuttingProgress = 0;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0
                        });
                    }
                } else {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestrySelf();
                        }
                    }
                }
            }
        }
    }

    public override void InteractAlternate(Player player) {
        if(HasKitchenObject()) {
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            if(outputKitchenObjectSO) {
                cuttingProgress++;
                OnCut?.Invoke(this, EventArgs.Empty);
                OnAnyCut?.Invoke(this, EventArgs.Empty);
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });
                if(cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                    GetKitchenObject().DestrySelf();
                    KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
                }
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if(cuttingRecipeSO) {
            return cuttingRecipeSO.output;
        }
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if(cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

    public KitchenObjectSO GetInputForOutput (KitchenObjectSO outputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithOutput(outputKitchenObjectSO);
        if(cuttingRecipeSO) {
            return cuttingRecipeSO.input;
        }
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithOutput(KitchenObjectSO outputKitchenObjectSO) {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if(cuttingRecipeSO.output == outputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
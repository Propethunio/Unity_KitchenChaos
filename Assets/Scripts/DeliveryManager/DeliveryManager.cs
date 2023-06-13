using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public event EventHandler OnRecipeChanged;
    public event EventHandler<OnRecipeDeliveryEventArgs> OnRecipeSuccess;
    public event EventHandler<OnRecipeDeliveryEventArgs> OnRecipeFailed;

    public class OnRecipeDeliveryEventArgs : EventArgs {
        public Transform tranform;
    }

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private List<RecipeSO> recipeSOList;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax;
    private int waitingRecipesMax = 5;
    private int successfullRecipesAmount;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
        spawnRecipeTimerMax = 3f;
    }

    private void Update() {
        spawnRecipeTimer += Time.deltaTime;
        if(spawnRecipeTimer >= spawnRecipeTimerMax) {
            spawnRecipeTimer = 0f;
            spawnRecipeTimerMax = UnityEngine.Random.Range(10f, 22f);
            if(waitingRecipeSOList.Count < waitingRecipesMax) {
                RecipeSO newWaitingRecipeSO = recipeSOList[UnityEngine.Random.Range(0, recipeSOList.Count)];
                waitingRecipeSOList.Add(newWaitingRecipeSO);
                OnRecipeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject, Transform transform) {
        foreach(RecipeSO waitingRecipeSO in waitingRecipeSOList) {
            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                bool plateContentMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    bool ingredientFound = false;
                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        if(plateKitchenObjectSO == recipeKitchenObjectSO) {
                            ingredientFound = true; break;
                        }
                    }
                    if(!ingredientFound) {
                        plateContentMatchesRecipe = false;
                        break;
                    }
                }
                if(plateContentMatchesRecipe) {
                    successfullRecipesAmount++;
                    waitingRecipeSOList.Remove(waitingRecipeSO);
                    OnRecipeChanged.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess.Invoke(this, new OnRecipeDeliveryEventArgs {
                        tranform = transform
                    });
                    return;
                }
            }
        }
        OnRecipeFailed.Invoke(this, new OnRecipeDeliveryEventArgs {
            tranform = transform
        });
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingRecipeSOList;
    }

    public int GetSuccesfullRecipesAmount() {
        return successfullRecipesAmount;
    }
}
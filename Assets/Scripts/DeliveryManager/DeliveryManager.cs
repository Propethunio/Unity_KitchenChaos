using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public event EventHandler OnRecipeChanged;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private List<RecipeSO> recipeSOList;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax;
    private int waitingRecipesMax = 5;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
        spawnRecipeTimerMax = UnityEngine.Random.Range(5f,12f);
    }

    private void Update() {
        spawnRecipeTimer += Time.deltaTime;
        if (spawnRecipeTimer >= spawnRecipeTimerMax) {
            spawnRecipeTimer = 0f;
            spawnRecipeTimerMax = UnityEngine.Random.Range(5f, 12f);
            if(waitingRecipeSOList.Count < waitingRecipesMax) {
                RecipeSO newWaitingRecipeSO = recipeSOList[UnityEngine.Random.Range(0, recipeSOList.Count)];
                waitingRecipeSOList.Add(newWaitingRecipeSO);
                OnRecipeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) { 
        foreach (RecipeSO waitingRecipeSO in waitingRecipeSOList) {
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    bool ingredientFound = false;
                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                            ingredientFound = true; break;
                        }
                    }
                    if(!ingredientFound) {
                        plateContentMatchesRecipe = false;
                        break;
                    }
                }
                if (plateContentMatchesRecipe) {
                    waitingRecipeSOList.Remove(waitingRecipeSO);
                    OnRecipeChanged.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        print("Bad Ricepie!");
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingRecipeSOList;
    } 
}
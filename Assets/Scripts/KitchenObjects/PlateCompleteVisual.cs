using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {

    [Serializable] public struct KitchenObjectSOGameObject {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSOGameObject> kitchenObjectSOGameObjectList;

    private void Start () {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        foreach(KitchenObjectSOGameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList) {
            if(kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO) {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
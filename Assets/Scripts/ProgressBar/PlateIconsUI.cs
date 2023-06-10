using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour {

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private GameObject iconTemplate;

    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach (Transform child in transform) {
            if(child.gameObject == iconTemplate) continue;
                Destroy(child.gameObject);
        }
        foreach(KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            GameObject icon = Instantiate(iconTemplate, transform);
            icon.SetActive(true);
            icon.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
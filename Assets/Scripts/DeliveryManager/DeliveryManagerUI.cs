using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {

    [SerializeField] private GameObject container;
    [SerializeField] private GameObject recipeTemplate;

    private void Start() {
        DeliveryManager.Instance.OnRecipeChanged += DeliveryManager_OnRecipeChanged;
    }

    private void DeliveryManager_OnRecipeChanged(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach(Transform child in container.transform) {
            if(child.gameObject == recipeTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList()) {
            GameObject recipe = Instantiate(recipeTemplate, container.transform);
            recipe.SetActive(true);
            recipe.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
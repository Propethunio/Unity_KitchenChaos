using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour {

    [SerializeField] private TMP_Text recipeNameText;
    [SerializeField] private GameObject iconsContainer;
    [SerializeField] private GameObject iconTemplate;

    public void SetRecipeSO(RecipeSO recipeSO) {
        recipeNameText.text = recipeSO.recipeName;
        foreach(Transform child in iconsContainer.transform) {
            if(child.gameObject == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList) {
            GameObject icon = Instantiate(iconTemplate, iconsContainer.transform);
            icon.SetActive(true);
            icon.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
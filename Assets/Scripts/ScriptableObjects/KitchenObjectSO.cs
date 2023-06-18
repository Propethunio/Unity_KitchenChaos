using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject {

    public enum OutputOfRecipeType {
        None,
        Cut,
        Fry
    }

    public GameObject prefab;
    public Sprite sprite;
    public string objectName;
    public OutputOfRecipeType outputOfRecipeType;
}
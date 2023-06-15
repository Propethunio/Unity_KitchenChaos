using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    [SerializeField] private TMP_Text recipeDeliveredText;

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e) {
        if(GameManager.Instance.IsGameOver()) {
            recipeDeliveredText.text = DeliveryManager.Instance.GetSuccesfullRecipesAmount().ToString();
            Show();
            SelectButton();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void MainMenuClick() {
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void TryAgainClick() {
        Loader.Load(Loader.Scene.Game);
    }

    private void SelectButton() {
        gameObject.GetComponentInChildren<Button>().Select();
    }
}
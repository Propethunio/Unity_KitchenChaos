using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour {

    [SerializeField] private GameObject optionsUI;

    private bool isActive;

    private void Start() {
        GameManager.Instance.OnGamePausedToggle += GameManager_OnGamePausedToggle;
        Hide();
    }

    private void GameManager_OnGamePausedToggle(object sender, System.EventArgs e) {
        if(isActive) {
            Hide();
        } else {
            Show();
        }
    }

    public void Show() {
        isActive = true;
        gameObject.SetActive(true);
        gameObject.GetComponentInChildren<Button>().Select();
    }

    public void Hide() {
        isActive = false;
        optionsUI.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Resume() {
        GameManager.Instance.TogglePauseGame();
    }

    public void LoadMainMenu() {
        Loader.Load(Loader.Scene.MainMenu);
    }
}
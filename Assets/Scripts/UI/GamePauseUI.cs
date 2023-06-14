using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseUI : MonoBehaviour {

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

    private void Show() {
        isActive = true;
        gameObject.SetActive(true);
    }

    private void Hide() {
        isActive = false;
        gameObject.SetActive(false);
    }

    public void Resume() {
        GameManager.Instance.TogglePauseGame();
    }

    public void LoadMainMenu() {
        Loader.Load(Loader.Scene.MainMenu);
    }
}
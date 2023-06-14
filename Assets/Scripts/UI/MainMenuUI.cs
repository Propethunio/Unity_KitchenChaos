using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {

    private void Start() {
        Time.timeScale = 1f;
    }

    public void PlayClick() {
        Loader.Load(Loader.Scene.Game);
    }

    public void QuitClick() {
        Application.Quit();
    }
}
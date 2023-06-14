using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    public void PlayClick() {
        Loader.Load(Loader.Scene.Game);
    }

    public void QuitClick() {
        Application.Quit();
    }
}
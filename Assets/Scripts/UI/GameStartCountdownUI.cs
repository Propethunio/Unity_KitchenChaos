using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour {

    [SerializeField] private TMP_Text countdownText;

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e) {
        if(GameManager.Instance.IsCountdownActive()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Update() {
        countdownText.text = Mathf.Ceil(GameManager.Instance.GetTimedownTimer()).ToString();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
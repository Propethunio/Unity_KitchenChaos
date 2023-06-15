using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour {

    private const string NumberPop = "NumberPop";

    [SerializeField] private TMP_Text countdownText;

    private Animator animator;
    private int previousCountdownNumber;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

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
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetTimedownTimer());
        countdownText.text = countdownNumber.ToString();
        if(countdownNumber != previousCountdownNumber) {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NumberPop);
            SoundManager.Instance.PlayCountdownSound();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
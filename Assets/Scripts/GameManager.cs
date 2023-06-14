using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePausedToggle;

    [SerializeField] private float gameTime = 90f;

    private bool isGamePaused;

    private enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    private State state;
    private float waitingTimer = 1f;

    private void Awake() {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start() {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        TogglePauseGame();
    }

    private void Update() {
        switch(state) {
            case State.WaitingToStart:
                waitingTimer -= Time.deltaTime;
                if(waitingTimer <= 0f) {
                    state = State.CountdownToStart;
                    waitingTimer = 3f;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                waitingTimer -= Time.deltaTime;
                if(waitingTimer <= 0f) {
                    state = State.GamePlaying;
                    waitingTimer = gameTime;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                waitingTimer -= Time.deltaTime;
                if(waitingTimer <= 0f) {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public void TogglePauseGame() {
        isGamePaused = !isGamePaused;
        OnGamePausedToggle?.Invoke(this, EventArgs.Empty);
        if(isGamePaused) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }

    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsCountdownActive() {
        return state == State.CountdownToStart;
    }

    public float GetTimedownTimer() {
        return waitingTimer;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    public float GetPlayTimeNormalized() {
        return waitingTimer / gameTime;
    }
}
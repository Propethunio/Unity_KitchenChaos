using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAltButton;
    [SerializeField] private Button gamepadPauseButton;
    [SerializeField] private TMP_Text soundEffectText;
    [SerializeField] private TMP_Text musicText;
    [SerializeField] private TMP_Text moveUpText;
    [SerializeField] private TMP_Text moveDownText;
    [SerializeField] private TMP_Text moveLeftText;
    [SerializeField] private TMP_Text moveRightText;
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private TMP_Text interactAltText;
    [SerializeField] private TMP_Text pauseText;
    [SerializeField] private TMP_Text gamepadInteractText;
    [SerializeField] private TMP_Text gamepadInteractAltText;
    [SerializeField] private TMP_Text gamepadPauseText;
    [SerializeField] private GameObject rebindPop;

    private void Awake() {
        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateEffectsVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateMusicVisual();
        });
        moveUpButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.MoveUp);
        });
        moveDownButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.MoveDown);
        });
        moveLeftButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.MoveLeft);
        });
        moveRightButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.MoveRight);
        });
        interactButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Interact);
        });
        interactAltButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.InteractAlt);
        });
        pauseButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Pause);
        });
        gamepadInteractButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.GamepadInteract);
        });
        gamepadInteractAltButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.GamepadInteractAlt);
        });
        gamepadPauseButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.GamepadPause);
        });
    }

    private void Start() {
        UpdateEffectsVisual();
        UpdateMusicVisual();
        UpdateBindingVisual();
    }

    private void UpdateEffectsVisual() {
        soundEffectText.text = Mathf.Round(SoundManager.Instance.GetVolume() * 10f).ToString();
    }

    private void UpdateMusicVisual() {
        musicText.text = Mathf.Round(MusicManager.Instance.GetVolume() * 10f).ToString();
    }

    private void UpdateBindingVisual() {
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamepadInteract);
        gamepadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamepadInteractAlt);
        gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamepadPause);
    }

    private void ShowRebindPop() {
        rebindPop.SetActive(true);
    }

    private void HideRebindPop() {
        rebindPop.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding) {
        ShowRebindPop();
        GameInput.Instance.RebindBinding(binding, () => {
            UpdateBindingVisual();
            HideRebindPop();
        });
    }

    public void SelectButton() {
        gameObject.GetComponentInChildren<Button>().Select();
    }
}
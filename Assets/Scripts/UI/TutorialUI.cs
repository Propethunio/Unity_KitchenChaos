using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour {

    [SerializeField] private TMP_Text moveUpText;
    [SerializeField] private TMP_Text moveDownText;
    [SerializeField] private TMP_Text moveLeftText;
    [SerializeField] private TMP_Text moveRightText;
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private TMP_Text interactAltText;
    [SerializeField] private TMP_Text pauseText;
    [SerializeField] private TMP_Text interactGamepadText;
    [SerializeField] private TMP_Text interactAltGamepadText;
    [SerializeField] private TMP_Text pauseGamepadText;

    private void Start() {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        UpdateVisual();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if(GameManager.Instance.IsCountdownActive()) {
            gameObject.SetActive(false);
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        interactGamepadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamepadInteract);
        interactAltGamepadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamepadInteractAlt);
        pauseGamepadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamepadPause);
    }
}
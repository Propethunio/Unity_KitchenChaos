using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private const string playerPrefsSoundVolume = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipsRefsSO audioClipsRefsSO;

    private float volume = .7f;

    private void Awake() {
        Instance = this;
        volume = PlayerPrefs.GetFloat(playerPrefsSoundVolume, volume);
    }

    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        CounterMaster.OnAnyObjectPlacedHere += CounterMaster_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e) {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySoundFromArray(audioClipsRefsSO.objectDrop, trashCounter.transform.position);
    }

    private void CounterMaster_OnAnyObjectPlacedHere(object sender, System.EventArgs e) {
        CounterMaster counterMaster = sender as CounterMaster;
        PlaySoundFromArray(audioClipsRefsSO.objectDrop, counterMaster.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e) {
        PlaySoundFromArray(audioClipsRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySoundFromArray(audioClipsRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, DeliveryManager.OnRecipeDeliveryEventArgs e) {
        PlaySoundFromArray(audioClipsRefsSO.deliveryFail, e.tranform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, DeliveryManager.OnRecipeDeliveryEventArgs e) {
        PlaySoundFromArray(audioClipsRefsSO.deliverySuccess, e.tranform.position);
    }

    private void PlaySoundFromArray(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    public void PlayFootstepSound(Vector3 position, float volume) {
        PlaySoundFromArray(audioClipsRefsSO.footstep, position, volume);
    }

    public void PlayCountdownSound() {
        PlaySoundFromArray(audioClipsRefsSO.warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position) {
        PlaySoundFromArray(audioClipsRefsSO.warning, position);
    }

    public void ChangeVolume() {
        volume += .1f;
        if(volume > 1f) {
            volume = 0f;
        }
        PlayerPrefs.SetFloat(playerPrefsSoundVolume, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }
}
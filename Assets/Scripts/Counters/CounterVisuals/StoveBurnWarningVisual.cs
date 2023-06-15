using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningVisual : MonoBehaviour {

    [SerializeField] StoveCounter stoveCounter;

    private void Start() {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        gameObject.SetActive(false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        float burnShowPrgoressAmount = .5f;
        bool show = stoveCounter.isFried() && e.progressNormalized > burnShowPrgoressAmount;
        if(show) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnBarVisual : MonoBehaviour {

    private const string isFlashing = "isFlashing";

    [SerializeField] StoveCounter stoveCounter;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.SetBool(isFlashing, false);
    }

    private void Start() {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        float burnShowPrgoressAmount = .5f;
        bool show = stoveCounter.isFried() && e.progressNormalized > burnShowPrgoressAmount;
        animator.SetBool(isFlashing, show);
    }
}
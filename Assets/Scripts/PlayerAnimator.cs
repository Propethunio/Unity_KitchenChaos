using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    [SerializeField] Player player;
    Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        animator.SetBool("isWalking", player.IsWalking());
    }
}
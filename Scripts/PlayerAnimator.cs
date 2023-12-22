using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    private const string IS_WALKING = "IsWalking";
    private const string GRAB = "Grab";

    [SerializeField] private Player player;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();

    }

    private void Start() {
        player.OnPlayerGrab += Player_OnPlayerGrab;
    }

    private void Player_OnPlayerGrab(object sender, System.EventArgs e) {
        animator.SetTrigger(GRAB);
    }

    private void Update() {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}


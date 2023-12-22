using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterAnimator : MonoBehaviour {


    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField] private ContainerCounter containerCounter;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        containerCounter.OnOpenClose += ContainerCounter_OnOpenClose;
    }

    private void ContainerCounter_OnOpenClose(object sender, System.EventArgs e) {
        animator.SetTrigger(OPEN_CLOSE);
    }
}

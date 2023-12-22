using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStartCountdownUI : MonoBehaviour {

    private const string ON_COUNTDOWN = "OnCountdown";

    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator animator;
    private int previousCountdownNumber = 0;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        GameManager.Instance.OnStageChanges += GameManager_OnStageChanges;

        Hide();
    }

    private void Update() {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        if (countdownNumber != previousCountdownNumber) {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(ON_COUNTDOWN);
            SoundManager.Instance.PlayCountdownSound();
        }
    }

    private void GameManager_OnStageChanges(object sender, System.EventArgs e) {
        if(GameManager.Instance.IsCountdownToStartActive()) {
            Show();
        } else Hide();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}

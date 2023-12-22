using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI recipesAmountText;

    private void Start() {
        GameManager.Instance.OnStageChanges += GameManager_OnStageChanges;

        Hide();
    }

    private void Update() {
        
    }

    private void GameManager_OnStageChanges(object sender, System.EventArgs e) {
        if(GameManager.Instance.IsGameOver()) {
            recipesAmountText.text = DeliveryManager.Instance.GetSuccessfullRecipesAmount().ToString();

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

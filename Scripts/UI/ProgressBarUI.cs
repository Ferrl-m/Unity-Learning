using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

    [SerializeField] private Image barImage;
    [SerializeField] private GameObject hasProgressGameObject;

    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null ) {
            Debug.LogError("Game Object " + hasProgressGameObject + " does not have implementations of IHasprogress!");
        }

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangeddEventArgs e) {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 1f) {
            Hide();
        } else { Show(); }
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    
    private void Hide() {
        gameObject.SetActive(false);
    }


}

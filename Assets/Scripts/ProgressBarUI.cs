using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
  [SerializeField] private Image progressBarImage;
    [SerializeField] private GameObject hasProgressGameObject;
     private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if(hasProgress==null)
            Debug.LogError("Game Object " + hasProgressGameObject +" does not have a component that implements IHasProgress");
        progressBarImage.fillAmount = 0;
        hasProgress.OnProgressChanged += hasProgress_OnProgressChanged;
        gameObject.SetActive(false);
    }

    private void hasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        progressBarImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
            Hide();
        else
            Show();

    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}

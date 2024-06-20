using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI countdownText;





    private void Start() {
        Hide();
        KitchenGameManager.Instance.OnGameStateChanged += KitchenGameManager_OnGameStateChanged;
    }

    private void Update() {
        if(KitchenGameManager.Instance.IsCountdownToStartActive()) {
            countdownText.text = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer()).ToString();
        }
    }
    private void KitchenGameManager_OnGameStateChanged(object sender, System.EventArgs e) {
        if (KitchenGameManager.Instance.IsCountdownToStartActive()) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}

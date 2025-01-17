using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {
    public static OptionsUI Instance { get; private set; }
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI soundsEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Button closeButton;

    private void Awake() {
        Instance = this;
        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(Hide);
    }
    private void Start() {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        UpdateVisual();
        Hide();
    }

    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void UpdateVisual() {
        soundsEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
    }
    public void Show()
    {
    gameObject.SetActive(true);
    }
    public void Hide()
    {
    gameObject.SetActive(false);
    }

}

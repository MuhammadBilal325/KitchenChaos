using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour {
    [SerializeField] private Image timerImage;


    private void Update() {
        if (KitchenGameManager.Instance.IsGamePlaying()) {
            timerImage.fillAmount = KitchenGameManager.Instance.GetPlayingTimerNormalized();
        }
    }
    

   
}

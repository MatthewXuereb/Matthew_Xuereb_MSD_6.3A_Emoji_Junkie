using EmojiJunkie.Timer;
using UnityEngine;

namespace EmojiJunkie
{
    public class Surrender : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;

        [SerializeField] private CountdownTimer _countdownTimer;

        public void ShowGameOverPanel()
        {
            _countdownTimer.EndTimer();
            _gameOverPanel.SetActive(true);
        }
    }
}

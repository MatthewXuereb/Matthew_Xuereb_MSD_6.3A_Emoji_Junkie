using EmojiJunkie.Data;
using EmojiJunkie.Dev;
using UnityEngine;

namespace EmojiJunkie.UI
{
    public class Surrender : MonoBehaviour
    {
        [Header("Scripts")]
        [SerializeField] private CountdownTimer _countdownTimer;
        [SerializeField] private GameSceneManager _gameSceneManager;

        [Header("Panels")]
        [SerializeField] private GameObject gameOverPanel;

        public void EndGame()
        {
            _countdownTimer.EndTimer();
            gameOverPanel.SetActive(true);

            if (GameData.activePlayer == 0) _gameSceneManager.gameOverPanelWinnerText.text = "Player 2 Won";
            else _gameSceneManager.gameOverPanelWinnerText.text = "Player 1 Won";
        }
    }
}

using EmojiJunkie.Data;
using TMPro;
using UnityEngine;

namespace EmojiJunkie.UI
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _player1Score;
        [SerializeField] private TextMeshProUGUI _player2Score;

        void Update()
        {
            _player1Score.text = "Player 1: " + GameData.player1Score.ToString();
            _player2Score.text = "Player 2: " + GameData.player2Score.ToString();
        }
    }
}

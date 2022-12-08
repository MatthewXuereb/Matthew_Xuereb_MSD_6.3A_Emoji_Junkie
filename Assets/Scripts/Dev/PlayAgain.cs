using UnityEngine;

namespace EmojiJunkie
{
    public class PlayAgain : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;

        public void Replay()
        {
            _gameOverPanel.SetActive(false);
        }
    }
}

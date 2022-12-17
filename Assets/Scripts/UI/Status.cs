using EmojiJunkie.Data;
using TMPro;
using UnityEngine;

namespace EmojiJunkie
{
    public class Status : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _statusText;

        void Update()
        {
            if (GameData.activePlayer == 0) _statusText.text = "Player 1 Turn";
            else _statusText.text = "Player 2 Turn";
        }
    }
}

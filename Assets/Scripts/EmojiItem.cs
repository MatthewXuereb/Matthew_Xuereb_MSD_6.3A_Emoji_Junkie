using UnityEngine;

namespace EmojiJunkie
{
    [CreateAssetMenu(fileName = "Emoji Item", menuName = "Objects/Emoji Item")]
    public class EmojiItem : ScriptableObject
    {
        public int id;

        public string name;

        public Sprite sprite;
    }
}

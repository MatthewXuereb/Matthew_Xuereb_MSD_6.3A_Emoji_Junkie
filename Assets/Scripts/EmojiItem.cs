using UnityEngine;

namespace EmojiJunkie
{
    [CreateAssetMenu(fileName = "Emoji Item", menuName = "Objects/Emoji Item")]
    public class EmojiItem : ScriptableObject
    {
        public string name;

        public int index;

        public Sprite sprite;
    }
}

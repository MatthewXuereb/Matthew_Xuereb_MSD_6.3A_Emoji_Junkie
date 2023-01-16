using UnityEngine;

namespace EmojiJunkie
{
    [CreateAssetMenu(fileName = "Emoji List Object", menuName = "Objects/Emoji List Object")]
    public class EmojiList : ScriptableObject
    {
        public EmojiItem[] emojis;
    }
}

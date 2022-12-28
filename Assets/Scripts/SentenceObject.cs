using UnityEngine;

namespace EmojiJunkie
{
    [CreateAssetMenu(fileName = "Sentence Object", menuName = "Objects/Sentence Object")]
    public class SentenceObject : ScriptableObject
    {
        public EmojiItem[] items;
    }
}

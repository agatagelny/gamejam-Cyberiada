using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Dialogue/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    [System.Serializable]
    public struct CharacterEntry
    {
        public string name;
        public Sprite portrait;
        public Sprite background;
    }

    public List<CharacterEntry> characters;

    public Sprite GetSprite(string characterName)
    {
        var entry = characters.FirstOrDefault(c => c.name.Equals(characterName, System.StringComparison.OrdinalIgnoreCase));
        return entry.portrait;
    }

    public Sprite GetBackground(string characterName)
    {
        var entry = characters.FirstOrDefault(c => c.name.Equals(characterName, System.StringComparison.OrdinalIgnoreCase));
        return entry.background;
    }
}
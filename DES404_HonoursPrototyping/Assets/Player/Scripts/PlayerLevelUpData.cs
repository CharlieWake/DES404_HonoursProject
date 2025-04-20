using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Up Data", menuName = "Player/Level Up Data")]
public class PlayerLevelUpData : ScriptableObject
{
    public List<LevelInfo> levels = new List<LevelInfo>();

    [System.Serializable]
    public class LevelInfo
    {
        public float experienceToLevelUp;
        public float maxHealth;
        public int actionsPerTurn;
    }

    public int maxLevel => levels.Count;

    public LevelInfo GetLevelInfo(int level)
    {
        if (level < 1 || level > levels.Count)
        {
            Debug.Log("Level is outwith bounds");
            return null;
        }

        return levels[level - 1];
    }
}

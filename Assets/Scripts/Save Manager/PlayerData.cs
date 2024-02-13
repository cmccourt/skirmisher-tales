using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public bool frozenUnlocked;
    public bool heavyUnlocked;

    public PlayerData(GameManager player)
    {
        level = player.level;
        frozenUnlocked = player.frozenUnlocked;
        heavyUnlocked = player.heavyUnlocked;
        
    }
}

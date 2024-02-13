using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : BaseManager
{
    protected override void EndGame()
    {
        levelMan.ChangeLevelStatus();
    }
}

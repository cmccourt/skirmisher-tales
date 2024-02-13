using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : BaseManager
{
    protected override void EndGame()
    {
        levelMan.GameOver();
    }

}

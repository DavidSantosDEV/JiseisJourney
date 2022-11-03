using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectible
{
    protected override void OnPickedUp()
    {
        GameManager.Instance?.AddCollectedCoins(1);
        base.OnPickedUp();
    }
}

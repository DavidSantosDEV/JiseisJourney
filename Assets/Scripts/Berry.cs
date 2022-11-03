using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry : Collectible
{
    protected override void OnPickedUp()
    {
        GameManager.Instance.AddCollectedBerries(1);
        base.OnPickedUp();
    }
}

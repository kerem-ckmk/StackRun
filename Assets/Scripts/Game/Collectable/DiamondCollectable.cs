using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondCollectable : CollectableBase
{
    public override int Price()
    {
        return GameConfigs.Instance.DiamondPrice;
    }
    public override void OnRotateAnimation()
    {

    }
}

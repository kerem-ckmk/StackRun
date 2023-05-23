using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCollectable : CollectableBase
{
    public override int Price()
    {
        return GameConfigs.Instance.GoldPrice;
    }

    public override void OnRotateAnimation()
    {

    }

}

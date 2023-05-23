using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollectable : CollectableBase
{
    public override int Price()
    {
        return GameConfigs.Instance.StarPrice;
    }

    public override void OnRotateAnimation()
    {

    }

}

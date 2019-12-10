using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLightningReaction : LightningReaction
{
    public override void React()
    {
        Debug.Log("I am being hit by lightning");
    }
}

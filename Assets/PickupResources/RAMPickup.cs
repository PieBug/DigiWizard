using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAMPickup : Pickup
{
    public override bool CanObtain(PlayerController player)
    {
        return player.GetComponent<MagicSystem>().ramAmount != 100;
    }

    public override void Obtain(PlayerController player)
    {
        base.Obtain(player);
        player.GetComponent<MagicSystem>().ramAmount += 25;
    }
}

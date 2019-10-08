using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    public override bool CanObtain(PlayerController player)
    {
        return player.GetComponent<PlayerHealthAndDeathManager>().currentHealth != 100;
    }

    public override void Obtain(PlayerController player)
    {
        base.Obtain(player);
        player.GetComponent<PlayerHealthAndDeathManager>().HealPlayer(25);
    }
}

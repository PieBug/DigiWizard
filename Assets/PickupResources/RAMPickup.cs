using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAMPickup : Pickup
{
    public int ramAmt = 25; // Amount that will be added when player picks up the RamPickup

    public override bool CanObtain(PlayerController player)
    {
        return player.GetComponent<MagicSystem>().ramAmount != 100;   // Will only get player if the ram amount isn't already full
    }

    public override void Obtain(PlayerController player)
    {
        //if (player.GetComponent<MagicSystem>().penaltyRunning == true)  // Checking to see if the penalty coroutine is running
        //{
        //    player.GetComponent<MagicSystem>().CancelPenaltyCoroutine();  // If it is running, cancel the penalty.
        //    //print("Calling to cancel penalty!");
        //    base.Obtain(player);
        //    player.GetComponent<MagicSystem>().AddRam(ramAmt);
        //}
        //else
        //{
            base.Obtain(player);
            player.GetComponent<MagicSystem>().AddRam(ramAmt);   // Using the AddRam function from the Magic System, add the ram amount value
        //}

    }
}


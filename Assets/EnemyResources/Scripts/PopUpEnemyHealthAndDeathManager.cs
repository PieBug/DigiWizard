using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpEnemyHealthAndDeathManager : EnemyHealthAndDeathManager
{
    public PopUpAdAI popUp;

    public override void DamageEnemy(int damageAmount)
    {
        base.DamageEnemy(damageAmount);
        popUp.Alert();
    }
    protected override IEnumerator Death()
    {
        popUp.animator.SetTrigger("death");
        //slug.attackSphere.gameObject.SetActive(false);
        popUp.state = PopUpAdAI.State.dead;
        popUp.StopAllCoroutines();
        popUp.audioSource2D.PlayOneShot(popUp.attributes.deathClip);
        yield return new WaitForSeconds(1f);
        //Play particle effect
        //Play dissappear noise
        //transform.Find("Slug").gameObject.SetActive(false);
        //GetComponent<Collider>().enabled = false;
        GameObject pickup = Instantiate(popUp.attributes.normalDrop, transform.position, Quaternion.identity);
        pickup.GetComponent<Pickup>().MakeTemporary(3f);
        Destroy(gameObject, 5f);
    }
}

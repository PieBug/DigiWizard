using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemyHealthAndDeathManager : EnemyHealthAndDeathManager
{
    public SpiderAI spider;
    public override void DamageEnemy(int damageAmount)
    {
        base.DamageEnemy(damageAmount);
        spider.Alert();
    }
    protected override IEnumerator Death()
    {
        spider.animator.SetTrigger("death");
        spider.state = SpiderAI.State.dead;
        spider.StopAllCoroutines();
        spider.audioSource2D.PlayOneShot(spider.attributes.deathClip);
        yield return new WaitForSeconds(1f);
        //Play particle effect
        //Play dissappear noise
        transform.Find("Spider").gameObject.SetActive(false);
        GameObject pickup = Instantiate(spider.attributes.normalDrop, transform.position, Quaternion.identity);
        pickup.GetComponent<Pickup>().MakeTemporary(3f);
        Destroy(gameObject, 5f);
    }
}

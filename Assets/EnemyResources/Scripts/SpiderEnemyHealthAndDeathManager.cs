using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemyHealthAndDeathManager : EnemyHealthAndDeathManager
{
    public GameObject mesh;
    public override void DamageEnemy(int damageAmount)
    {
        base.DamageEnemy(damageAmount);
        ai.Alert();
    }
    protected override IEnumerator Death()
    {
        SpiderAI spider = (SpiderAI)ai;
        spider.animator.SetTrigger("death");
        spider.state = SpiderAI.State.dead;
        spider.StopAllCoroutines();
        spider.attackSphere.gameObject.SetActive(false);
        spider.audioSource2D.PlayOneShot(spider.attributes.deathClip);
        yield return new WaitForSeconds(1f);
        //Play particle effect
        //Play dissappear noise
        mesh.SetActive(false);
        GameObject pickup = Instantiate(spider.attributes.normalDrop, transform.position, Quaternion.identity);
        pickup.GetComponent<Pickup>().MakeTemporary(3f);
        Destroy(transform.parent.gameObject, 5f);
    }
}

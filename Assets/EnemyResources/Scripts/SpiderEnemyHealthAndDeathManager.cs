using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemyHealthAndDeathManager : EnemyHealthAndDeathManager
{
    protected override IEnumerator Death()
    {
        SpiderAI spider = GetComponent<SpiderAI>();
        spider.animator.SetTrigger("death");
        spider.attackSphere.gameObject.SetActive(false);
        spider.state = SpiderAI.State.dead;
        spider.StopAllCoroutines();
        spider.audioSource2D.PlayOneShot(spider.attributes.deathClip);
        yield return new WaitForSeconds(1f);
        //Play particle effect
        //Play dissappear noise
        transform.Find("Spider").gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
        GameObject pickup = Instantiate(spider.attributes.normalDrop, transform.position, Quaternion.identity);
        pickup.GetComponent<Pickup>().MakeTemporary(3f);
        Destroy(gameObject, 5f);
    }
}

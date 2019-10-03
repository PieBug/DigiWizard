using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemyHealthAndDeathManager : EnemyHealthAndDeathManager
{
    public override void Kill()
    {
        SpiderAI spider = GetComponentInParent<SpiderAI>();
        spider.animator.SetTrigger("death");
        spider.attackSphere.gameObject.SetActive(false);
        spider.state = SpiderAI.State.dead;
        spider.StopAllCoroutines();
        spider.audioSource2D.PlayOneShot(spider.attributes.deathClip);
    }
}

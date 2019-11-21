using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugEnemyHealthAndDeathManager : EnemyHealthAndDeathManager
{
    public GameObject mesh;
    public override void DamageEnemy(int damageAmount)
    {
        base.DamageEnemy(damageAmount);
        SlugEnemyAttributes attributes = ((SlugAI)ai).attributes;
        attributes.distanceBias = Mathf.Min(attributes.distanceBias * 1.1f, 1f);
        attributes.dot1Bias = Mathf.Max(attributes.dot1Bias * 0.5f, attributes.distanceBias * 0.5f);
        ai.Alert();
    }
    protected override IEnumerator Death()
    {
        //Upcast to slug ai
        SlugAI slug = (SlugAI)ai;
        slug.animator.SetTrigger("death");
        //slug.attackSphere.gameObject.SetActive(false);
        slug.state = SlugAI.State.dead;
        slug.StopAllCoroutines();
        slug.audioSource2D.PlayOneShot(slug.attributes.deathClip);
        yield return new WaitForSeconds(1f);
        //Play particle effect
        //Play dissappear noise
        mesh.SetActive(false);
        GameObject pickup = Instantiate(slug.attributes.normalDrop, transform.position, Quaternion.identity);
        pickup.GetComponent<Pickup>().MakeTemporary(3f);
        Destroy(gameObject, 5f);
    }
}

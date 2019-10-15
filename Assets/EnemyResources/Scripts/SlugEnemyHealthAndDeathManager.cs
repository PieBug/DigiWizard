using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugEnemyHealthAndDeathManager : EnemyHealthAndDeathManager
{
    protected override IEnumerator Death()
    {
        SlugAI slug = GetComponent<SlugAI>();
        slug.animator.SetTrigger("death");
        //slug.attackSphere.gameObject.SetActive(false);
        slug.state = SlugAI.State.dead;
        slug.StopAllCoroutines();
        slug.audioSource2D.PlayOneShot(slug.attributes.deathClip);
        yield return new WaitForSeconds(1f);
        //Play particle effect
        //Play dissappear noise
        transform.Find("Slug").gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
        GameObject pickup = Instantiate(slug.attributes.normalDrop, transform.position, Quaternion.identity);
        pickup.GetComponent<Pickup>().MakeTemporary(3f);
        Destroy(gameObject, 5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Ability
{
    public override void Use(Vector2 spawnPos, Vector2 direction)
    {
        player.TakeDamage(-1 * m_Info.power);
        StartCoroutine("lagDestroy");

    }


    IEnumerator lagDestroy(){
        yield return new WaitForSeconds(m_Info.lagDestroyTime);

        Destroy(gameObject);


    }
}

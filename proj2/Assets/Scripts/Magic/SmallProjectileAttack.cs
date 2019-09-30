using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallProjectileAttack : Ability
{
    [SerializeField]
    Sprite left;
    
    [SerializeField]
    Sprite right;

    [SerializeField]
    Sprite up;
    
    [SerializeField]
    Sprite down;

    Sprite explosion;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    Vector2 curr_velocity;

    bool created;
    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        created = false;

    }



    public override void Use(Vector2 spawnPos, Vector2 direction)
    {
        float test_velocity = 10;
        Debug.Log("DIRECTION FOR EXPLOSION" + direction.x +  " " + direction.y);
        //logic for which direction travelling/sprite rendering/hitbox loading
        if (direction == Vector2.up) {
            spriteRenderer.sprite = up;
            curr_velocity = direction * test_velocity;
        }
        else if (direction == Vector2.down) {
            spriteRenderer.sprite = down;
            curr_velocity = direction * test_velocity;


        } else if (direction == Vector2.left) {
            spriteRenderer.sprite = left;
            curr_velocity = direction * test_velocity;


        } else {
            spriteRenderer.sprite = right;
            curr_velocity = direction * test_velocity;
        }
        created = true;

    }

    void Update(){
        if (created){
            rb.velocity = curr_velocity;
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {

        if (other.transform.CompareTag("Wall") || other.CompareTag("TrashMan")){

            RaycastHit2D[] hits;
            float newLength = m_Info.Range;
            hits = Physics2D.CircleCastAll(transform.position, 1.0f, Vector2.zero, m_Info.Range);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("TrashMan"))
                {
                    hit.collider.GetComponent<TrashMan>().TakeDamage(m_Info.power);
                }
            }
            Destroy(gameObject);
            //explosion goes here
        }
    }
}

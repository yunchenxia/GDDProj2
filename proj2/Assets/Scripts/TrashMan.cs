using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMan : MonoBehaviour
{
    #region TrashManVar
    public float move_speed;
    public float maxHealth;
    [SerializeField]
    [Tooltip("how long before man can move again after being hit")]
    public float hitstun;
    public float damage;

    [SerializeField]
    [Tooltip("time interval between throwing?")]
    public float throwing_interval;





    public Rigidbody2D enemyRB;
    float currHealth;
    
    //after being hit, cannot move or attack
    bool stunned;
    float stun_timer;
    
    //in a state of throwing, cannot move
    public bool throwing;
    
    //only one player!
    public static Unicorn player;
    public static Spawner spawn_point;

    private Animator anim;
    #endregion
   

    private void Awake(){
        enemyRB = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
        player = GameObject.Find("Unicorn").transform.GetComponent<Unicorn>();
        spawn_point = GameObject.Find("Spawner").transform.GetComponent<Spawner>();
        anim = GetComponent<Animator>();


        stunned = false;
        stun_timer = 0;

        throwing = false;

    }

    // Update is called once per frame
    void Update()
    {
        enemyRB.velocity = Vector2.zero;
        
        // when stunned timer reaches 0 can move/attack again!        
        if (stun_timer <= 0){
            stunned = false;
           
            if (!throwing) {
                Move();

            }
        }

        stun_timer -= Time.deltaTime;
        anim.SetBool("throwing", throwing);
        anim.SetBool("stunned", stunned);
    }


    void Move(){
        //left
        Vector2 movementVector = Vector2.left;
        enemyRB.velocity = movementVector * move_speed;
    }

    public void TakeDamage(float value){

        currHealth -= value;
        Debug.Log("enemy health is "+ currHealth.ToString());
               
        if (currHealth <= 0) {
            //dead
            Die();
        }

        
        if (!stunned) {
            //handles stunning
            Debug.Log("Trashman Stunned!");
            stunned = true;
            //stop moving
            enemyRB.velocity = Vector2.zero;
        }
        stun_timer = hitstun;

    }

    public void StartThrowing(){
        //stop in place
        throwing = true;
        enemyRB.velocity = Vector2.zero;

        StartCoroutine("ThrowLoop");

    }

    IEnumerator ThrowLoop(){
        while(true){
            if (player){
                player.TakeDamage(damage);
            }
            yield return new WaitForSeconds(throwing_interval);


        }
    }

    private void Die() {
        spawn_point.num_enemy -= 1;
        Debug.Log("trashman DIED");
        Destroy(this.gameObject);
    }


}

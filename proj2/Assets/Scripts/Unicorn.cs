using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unicorn : MonoBehaviour
{
    #region UnicornVar

    [SerializeField]
    [Tooltip("To update UIs")]
    private HUDController m_HUD;

    [SerializeField]
    [Tooltip("how long before uni can punch again")]
    public float punch_lag;
    public float movespeed;

    public float damage;
    public float maxHealth;

    [SerializeField]
    [Tooltip("how big is the punch hitbox")] 
    public Vector2 hitbox_size = Vector2.one;

    [SerializeField]
    [Tooltip("animation delay of punch")] 
    public float punch_anim_delay = 0.1f;

    public float healCooldown;
    public float attackCooldown;

    
    Rigidbody2D playerRB;

    public Vector2 curr_dir;

    float currHealth;
    float punch_timer;
    float x_input;
    float y_input;

    //used for stopping movement/animation
    bool Attacking;

    [SerializeField]
    [Tooltip("List of attacks that the unicorn can do")]
    private PlayerAttackInfo[] m_Attacks;


    [SerializeField]
    [Tooltip("Animator")]
    private Animator anim;

    private Background background;

    private GameObject ProjSpawner;


    //used to fix animation glitch with background, i.e. 3 => >= 0.73
    private int curr_percentile;

    #endregion
    


    #region unity_func
    void Awake()
    {
        background =  GameObject.Find("Background").transform.GetComponent<Background>();
        playerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        punch_timer = 0;
        currHealth = maxHealth;
        curr_percentile = 3;

        ProjSpawner = GameObject.Find("ProjectileSpawner");

        
        for (int i = 0; i < m_Attacks.Length; i++)
        {
            m_Attacks[i].Cooldown = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if already attacking do nothing else this frame
        if (Attacking){
            return;
        }

        //access input values
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");
        Move();

        //check for attack executed
        bool punch_pressed = Input.GetKeyDown(KeyCode.Z);
        if (punch_pressed && punch_timer <= 0) {
            Punch();
        }

        //decrement all attack timers
        punch_timer -= Time.deltaTime;


        //Use Magic Attacks
        for (int i = 0; i < m_Attacks.Length; i++)
        {
            PlayerAttackInfo attack = m_Attacks[i];

            if (attack.IsReady())
            {
                if (i == 0)
                {
                    m_HUD.UpdateAttackCooldown(1.0f);
                } else if (i == 1)
                {
                    m_HUD.UpdateHealCooldown(1.0f);
                }
                if (Input.GetButtonDown(attack.Button))
                {
                    attack.ResetCooldown();
                    StartCoroutine(UseAttack(attack));
                    break;
                }
            }
            else if (attack.Cooldown > 0)
            {
                attack.Cooldown -= Time.deltaTime;
                if (i == 0)
                {
                    m_HUD.UpdateAttackCooldown(1.0f * (attackCooldown - attack.Cooldown) / attackCooldown);
                }
                else if (i == 1)
                
                {
                    m_HUD.UpdateHealCooldown(1.0f * (10 - attack.Cooldown) / healCooldown);
                }
            }


        }

    }

    #endregion

    #region my_func
    //allows diagonal movement

    void Move(){
        if (x_input > 0  && y_input == 0) {
            curr_dir = Vector2.right;
        }
        else if (x_input < 0 && y_input == 0) {
            curr_dir = Vector2.left;

        } else if (y_input < 0 && x_input == 0) {
            curr_dir = Vector2.down;
        } else if (y_input > 0 && x_input == 0) {
            curr_dir = Vector2.up;

        }

        anim.SetFloat("DirX", curr_dir.x);
        anim.SetFloat("DirY", curr_dir.y);
        ProjSpawner.transform.position = transform.position + 0.50f * new Vector3(curr_dir.x, curr_dir.y, 0);
        Vector2 movementVector = new Vector2(x_input, y_input);
        playerRB.velocity = movementVector * movespeed;


    }


    void Punch(){
        Debug.Log("punching now");
        Debug.Log(curr_dir);

        //handles attack/calculates hitbox
        StartCoroutine(PunchRoutine());

        punch_timer = punch_lag;
    }

    //Punch hitbox
    IEnumerator PunchRoutine(){
        
        //pause character during attack
        Attacking = true;
        playerRB.velocity = Vector2.zero;
        //update animator
        anim.SetBool("Attacking", Attacking);    
        
        //create hitbox
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position + curr_dir, hitbox_size, 0, Vector2.zero, 0);
        Debug.Log("cast hitbox");

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("TrashMan")) {
                hit.transform.GetComponent<TrashMan>().TakeDamage(damage);
                //attack one
                break;
            }
        }
        yield return new WaitForSeconds(punch_anim_delay);
        //reenable movement
        Attacking = false;
        //update animator
        anim.SetBool("Attacking", Attacking);
    }    

    public void TakeDamage(float value){
        
        currHealth -= value;
        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }


        float percentage = 1.0f * currHealth / maxHealth;

        //update curr_percentile if wrong
        //if cp = 3. then 3 =< (0.75/0.25) AND 4 >= (1/0.25)
        if (!(curr_percentile <= (percentage / 0.25)) && (curr_percentile + 1 >= (percentage / 0.25))) {
            curr_percentile = Mathf.FloorToInt(percentage / 0.25f);
            background.UpdateBackground(percentage);

        }

        m_HUD.UpdateHealth(percentage);
        Debug.Log("unicorn health is "+ currHealth.ToString());
        if (currHealth <= 0) {
            //dead
            Die();
        }
    
    }

    private void Die() {
        Debug.Log("unicorn DIED");
        Destroy(this.gameObject);
    }

    #endregion


    #region Attack Methods
    private IEnumerator UseAttack(PlayerAttackInfo attack)
    {
        //pause character during attack
        Attacking = true;
        playerRB.velocity = Vector2.zero;
        //update animator
        anim.SetBool("Attacking", Attacking);   




        GameObject go = Instantiate(attack.AbilityGO, playerRB.position + curr_dir, Quaternion.identity);
        go.GetComponent<Ability>().Use(playerRB.position + curr_dir, curr_dir);


        yield return new WaitForSeconds(0.3f);
        
        //reenable movement
        Attacking = false;
        //update animator
        anim.SetBool("Attacking", Attacking);

    }

    #endregion

}


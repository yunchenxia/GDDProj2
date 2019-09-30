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
    [Tooltip("how long before uni can punch again")] 
    public Vector2 hitbox_size = Vector2.one;



    Rigidbody2D playerRB;

    Vector2 curr_dir;

    float currHealth;
    float punch_timer;
    float x_input;
    float y_input;

    bool Attacking;

    [SerializeField]
    [Tooltip("List of attacks that the unicorn can do")]
    private PlayerAttackInfo[] m_Attacks;

    #endregion
    


    #region unity_func
    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        punch_timer = 0;
        currHealth = maxHealth;

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
                if (Input.GetButtonDown(attack.Button))
                {
                    //p_FrozenTimer = attack.FrozenTime;
                    //DecreaseHealth(attack.HealthCost);
                    StartCoroutine(UseAttack(attack));
                    //UseAttacks(attack);
                    break;
                }
            }
            else if (attack.Cooldown > 0)
            {
                attack.Cooldown -= Time.deltaTime;
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
        yield return new WaitForSeconds(.1f);
        //reenable movement
        Attacking = false;
    }    

    public void TakeDamage(float value){

        currHealth -= value;

        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }

        float percentage = 1.0f * currHealth / maxHealth;
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

    public float getcurHP() {
        return currHealth;
    }
    #endregion


    #region Attack Methods
    private IEnumerator UseAttack(PlayerAttackInfo attack)
    {

        //cc_Rb.rotation = Quaternion.Euler(0, m_CameraTransform.eulerAngles.y, 0);
        //Quaternion new_Rotation = cc_Rb.rotation;
        //cr_Anim.SetTrigger(attack.TriggerName);
        //IEnumerator toColor = ChangeColor(attack.AbilityColor, 10);
        //StartCoroutine(toColor);
        //yield return new WaitForSeconds(attack.WindUpTime);

        //Vector3 offset = transform.forward * attack.Offset.z + transform.right * attack.Offset.x + transform.up * attack.Offset.y;
        Vector3 spawnPos = transform.position;// + offset;
        /*if (attack.Bomb)
        {
            spawnPos = new Vector3(transform.position.x, 40, transform.position.z);
            Quaternion current_Rotation = attack.AbilityGO.transform.rotation;
            new_Rotation = new Quaternion(-current_Rotation.x, current_Rotation.y, current_Rotation.z, current_Rotation.w);

        }*/
        GameObject go = Instantiate(attack.AbilityGO, spawnPos, transform.rotation);
        go.GetComponent<Ability>().Use(spawnPos);

        //StopCoroutine(toColor);
        //StartCoroutine(ChangeColor(p_DefaultColor, 50));
        yield return new WaitForSeconds(attack.Cooldown);

        attack.ResetCooldown();
    }



    #endregion

}


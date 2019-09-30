using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Rigidbody2D p_Rb;
    public float speed = 20.0f;
    public Vector3 startingPos;
    [SerializeField]
    private Unicorn unicorn;
    public Vector2 dir;

    [SerializeField]
    private Ability attack;

    // Start is called before the first frame update
    void Start()
    {
        dir = unicorn.curr_dir;
        p_Rb = gameObject.GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        p_Rb.AddForce(dir * transform.up * speed);
        p_Rb.velocity = dir * transform.up * speed;

    }

    // Update is called once per frame
    void Update()
    {
        if ((Mathf.Abs(transform.position.x - startingPos.x) > attack.m_Info.Range) || Mathf.Abs(transform.position.y - startingPos.y) > attack.m_Info.Range)
        {
            Destroy(gameObject);
        }
        
    }
}

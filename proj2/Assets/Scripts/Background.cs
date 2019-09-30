using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Unicorn player;
    Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.Find("Unicorn").transform.GetComponent<Unicorn>();
    }

    public void UpdateBackground(float percentage)
    {
        anim.SetFloat("health", percentage);
    }
}

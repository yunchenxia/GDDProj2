using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    public AbilityInfo m_Info;
    #endregion

    public Unicorn player;

    #region Intialization
    private void Awake()
    {
        player = GameObject.Find("Unicorn").transform.GetComponent<Unicorn>();
        
    }
    #endregion

    #region Use Methods
    public abstract void Use(Vector2 spawnPos, Vector2 direction);
    #endregion



}

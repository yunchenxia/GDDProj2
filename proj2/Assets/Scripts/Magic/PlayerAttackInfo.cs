using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackInfo
{
    #region Editor Variables
    [SerializeField]
    private string m_Name;
    public string AttackName
    {
        get
        {
            return m_Name;
        }
    }

    [SerializeField]
    private string m_Button;
    public string Button
    {
        get
        {
            return m_Button;
        }
    }

    [SerializeField]
    private string m_TriggerName;
    public string TriggerName
    {
        get
        {
            return m_TriggerName;
        }
    }

    [SerializeField]
    private GameObject m_AbilityGO;
    public GameObject AbilityGO
    {
        get
        {
            return m_AbilityGO;
        }
    }

    [SerializeField]
    private Vector2 m_offset;
    public Vector2 Offset
    {
        get
        {
            return m_offset;
        }
    }

    [SerializeField]
    private float m_WindUpTime;
    public float WindUpTime
    {
        get
        {
            return m_WindUpTime;
        }
    }

    [SerializeField]
    private float m_FrozenTime;
    public float FrozenTime
    {
        get
        {
            return m_FrozenTime;
        }
    }

    [SerializeField]
    private float m_Cooldown;

    [SerializeField]
    private int m_ManaCost;
    public int ManaCost
    {
        get
        {
            return m_ManaCost;
        }
    }

    [SerializeField]
    private bool m_Projectile;
    public bool Projectile
    {
        get
        {
            return m_Projectile;
        }
    }

    #endregion

    #region Public Variables
    public float Cooldown
    {
        get;
        set;
    }
    #endregion

    #region Cooldown Methods
    public void ResetCooldown()
    {
        Cooldown = m_Cooldown;
    }
    
    public bool IsReady()
    {
        return Cooldown <= 0;
    }
    #endregion


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityInfo
{
    #region Editor Variables
    [SerializeField]
    private int m_Power;

    public int power
    {
        get
        {
            return m_Power;
        }
    }

    [SerializeField]
    private float m_Range;
    public float Range
    {
        get
        {
            return m_Range;
        }
    }
    #endregion

    public float lagDestroyTime;

}

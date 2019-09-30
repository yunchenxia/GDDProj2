using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The health bar of the player")]
    private RectTransform m_HealthBar;

    [SerializeField]
    [Tooltip("The text of wave timer")]
    private Text m_waveTimer;

    [SerializeField]
    [Tooltip("The text of wave counter")]
    private Text m_waveCounter;

    [SerializeField]
    [Tooltip("The cooldown timer bar of missle attack")]
    private RectTransform m_AttackCoolDownBar;

    [SerializeField]
    [Tooltip("The cooldown timer bar of heal")]
    private RectTransform m_HealCoolDownBar;

    #endregion

    #region Private Variables
    //To keep track of the original width
    private float p_HealthBarOrigWidth;

    private float p_AttackBarOrigWidth;

    private float p_HealBarOrigWidth;
    #endregion

    #region Initialization
    private void Awake()
    {
        p_HealthBarOrigWidth = m_HealthBar.sizeDelta.x;
        p_AttackBarOrigWidth = m_AttackCoolDownBar.sizeDelta.x;
        p_HealBarOrigWidth = m_HealCoolDownBar.sizeDelta.x;
    }
    #endregion


    #region Update Health Bar
    public void UpdateHealth(float percent)
    {
        m_HealthBar.sizeDelta = new Vector2(p_HealthBarOrigWidth * percent, m_HealthBar.sizeDelta.y);
    }

    #endregion

    #region Wave Related Updates
    public void UpdateWaveTimer(float timer)
    {
        int fullTime = (int)timer;
        m_waveTimer.text = fullTime.ToString() + "s";
    }

    public void UpdateWaveCounter(int counter)
    {
        m_waveCounter.text = "Wave: " + counter.ToString();
    }

    #endregion

    #region Cooldown Related Udates

    public void UpdateAttackCooldown(float percent)
    {
        m_AttackCoolDownBar.sizeDelta = new Vector2(p_AttackBarOrigWidth * percent, m_AttackCoolDownBar.sizeDelta.y);
    }

    public void UpdateHealCooldown(float percent)
    {
        m_HealCoolDownBar.sizeDelta = new Vector2(p_HealBarOrigWidth * percent, m_HealCoolDownBar.sizeDelta.y);
    }

    #endregion
}

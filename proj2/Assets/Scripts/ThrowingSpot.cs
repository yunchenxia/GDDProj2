using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingSpot : MonoBehaviour
{
    #region ThrowingVar

    #endregion
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("TrashMan"))
        {
            TrashMan enemy = coll.transform.GetComponent<TrashMan>();            
            enemy.StartThrowing();

        }
    }



}

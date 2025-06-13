using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltClearer : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Car"))
        {
            return;
        }

        UIWarmDNA.Instance.SongMiner(UIType.Win);
        RWarmDNA.Instance.WarmRemove();
        Debug.Log("=============   发生碰撞   =============");
    }

}

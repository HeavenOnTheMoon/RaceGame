using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RephraseStubble : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("Road")]    public SpriteRenderer Belt;
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Car"))
        {
            return;
        }

        Belt.sortingOrder = 3;
        Debug.Log("------------------�ر�ת��-----------------------");
    }
}

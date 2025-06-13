using System.Collections;
using System.Collections.Generic;   
using UnityEngine;

public class RephraseBoy : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("roadType")]    public RoadType PrayBoom;
[UnityEngine.Serialization.FormerlySerializedAs("ColloderObject")]
    public List<GameObject> WithdrawMexico;
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Car"))
        {
            return;
        }
        RWarmDNA.Instance.ToSongGnaw = false;
        if (PrayBoom == RoadType.OverLap)
        {
            RWarmDNA.Instance.ClearerDanceSmell(true);
        }
        RWarmDNA.Instance.ClearerBoySmell();
        RWarmDNA.Instance.Handle.SeldomB = null;
        RWarmDNA.Instance.Handle.Birth += 0.05f;
        RWarmDNA.Instance.Handle.ToAheadBelt = false;
        /*for (int i = 0; i < ColloderObject.Count; i++)
        {
            ColloderObject[i].SetActive(false);
        }*/
    }
}

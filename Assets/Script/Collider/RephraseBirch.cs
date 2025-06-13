using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RephraseBirch : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("PointObj")]    public GameObject GraceOdd;
[UnityEngine.Serialization.FormerlySerializedAs("roadType")]
    public RoadType PrayBoom;
[UnityEngine.Serialization.FormerlySerializedAs("isClockWise")]
    //�Ƿ�Ϊ˳ʱ��
    public bool SoGuildHeat;
[UnityEngine.Serialization.FormerlySerializedAs("list_head")]
    public List<GameObject> Keep_Rend;
[UnityEngine.Serialization.FormerlySerializedAs("list_tail")]    public List<GameObject> Keep_Burn;

    private void Start()
    {
        if (PrayBoom == RoadType.OverLap)
        {
            IncomeRephraseGlance(Keep_Rend, true);
            IncomeRephraseGlance(Keep_Burn, false);
        }
        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        
        if (!collision.gameObject.CompareTag("Car"))
        {
            return;
        }
        RWarmDNA.Instance.ToSongGnaw = true;
        if (GraceOdd != null)
        {
            RWarmDNA.Instance.ClearerGraceMexicoSmell(GraceOdd);
        }

        RWarmDNA.Instance.ClearerGuildHeatSmell(SoGuildHeat);
        if (PrayBoom == RoadType.OverLap)
        {
            IncomeRephraseGlance(Keep_Rend, false);
            IncomeRephraseGlance(Keep_Burn, true);
        }
       
    }

    public void AvantRephrase()
    {
        
    }

    public void IncomeRephraseGlance(List<GameObject> list,bool isEnable)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(isEnable);
        }
    }
}

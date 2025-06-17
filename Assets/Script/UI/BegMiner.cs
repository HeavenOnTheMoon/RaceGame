using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BegMiner : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("Text_Coin")]    public Text Cost_Diet;
[UnityEngine.Serialization.FormerlySerializedAs("AdButton")]    public Button HePhrase;
[UnityEngine.Serialization.FormerlySerializedAs("GetButton")]    public Button CarPhrase;
    public GameObject AdTitle;
    void Start()
    {
        HePhrase.onClick.AddListener(()=> {
            A_ADManager.Instance.playRewardVideo((ok)=> {
                if (ok)
                {
                    UIWarmDNA.Instance.ThoseMiner(UIType.Win);
                    RWarmDNA.Instance.IncomeDiet((RWarmDNA.Instance.WarmAlarmRecoil * 10 * 2));
                    GrowMiner.Instance.WarmAcid();
                }
                else
                {
                    AdTitle.SetActive(true);
                }
                
            });
        });

        CarPhrase.onClick.AddListener(()=> {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            UIWarmDNA.Instance.ThoseMiner(UIType.Win);
            RWarmDNA.Instance.IncomeDiet((RWarmDNA.Instance.WarmAlarmRecoil * 10));
            GrowMiner.Instance.WarmAcid();
        });
    }

    private void OnEnable()
    {
        Cost_Diet.text = (RWarmDNA.Instance.WarmAlarmRecoil * 10).ToString();
    }
}

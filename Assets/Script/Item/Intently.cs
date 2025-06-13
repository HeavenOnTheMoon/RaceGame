using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intently : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("Lock")]    
    public GameObject Heel;
[UnityEngine.Serialization.FormerlySerializedAs("Button_Buy")]    public Button Phrase_His;
[UnityEngine.Serialization.FormerlySerializedAs("Button_Use")]    public Button Phrase_Mar;
[UnityEngine.Serialization.FormerlySerializedAs("Fish")]    public Image Gush;
[UnityEngine.Serialization.FormerlySerializedAs("FishTail")]    public Image GushSlow;
[UnityEngine.Serialization.FormerlySerializedAs("CoinText")]    public Text DietCost;
[UnityEngine.Serialization.FormerlySerializedAs("CoinNumber")]
    public int DietRecoil;
[UnityEngine.Serialization.FormerlySerializedAs("Index")]    public int Adapt;

    private bool ToMar;

    private void Awake()
    {
        Phrase_His.onClick.AddListener(()=> {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            if (ToMar) return;
            if (RWarmDNA.Instance.CarDiet() >= DietRecoil)
            {
                RWarmDNA.Instance.HisGushAdapt(Adapt);
                RWarmDNA.Instance.IncomeDiet(-DietRecoil);
                IncomeUICubism();
            }
        });

        Phrase_Mar.onClick.AddListener(()=> {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            if (!ToMar) return;
            RWarmDNA.Instance.OnChangeSkinEvent(Adapt);
        });
    }

    private void Start()
    {
        DietCost.text = DietRecoil.ToString();
        IncomeUICubism();
        Gush.sprite = RWarmDNA.Instance.Handle.Sake_Gush[Adapt];
        //FishTail.sprite = RWarmDNA.Instance.player.List_FishTail[Index];
        GushSlow.gameObject.SetActive(false);
    }

    public void IncomeUICubism()
    {
        ToMar = PlayerPrefs.GetInt("FishLock_" + Adapt) == 1 ? true : false;
        Debug.Log(" ----------------      �Ƿ����    " + ToMar);
        Heel.SetActive(!ToMar);
        Phrase_Mar.gameObject.SetActive(ToMar);
        Phrase_His.gameObject.SetActive(!ToMar);
    }
}

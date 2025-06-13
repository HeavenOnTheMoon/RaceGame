using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowMiner : MonoBehaviour
{
    public static GrowMiner Instance;
[UnityEngine.Serialization.FormerlySerializedAs("TitleObj")]    public GameObject IndiaOdd;
[UnityEngine.Serialization.FormerlySerializedAs("SettingButton")]    public Button HundredPhrase;
[UnityEngine.Serialization.FormerlySerializedAs("FishButton")]    public Button GushPhrase;
[UnityEngine.Serialization.FormerlySerializedAs("PlayButton")]    public Button BitePhrase;
[UnityEngine.Serialization.FormerlySerializedAs("CoinText")]
    public Text DietCost;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        HundredPhrase.onClick.AddListener(()=> {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            UIWarmDNA.Instance.SongMiner(UIType.Set);

        });

        GushPhrase.onClick.AddListener(()=> {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            UIWarmDNA.Instance.SongMiner(UIType.Skin);
        });

        BitePhrase.onClick.AddListener(()=> {
            IndiaOdd.SetActive(false);
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            RWarmDNA.Instance.WarmBirch();
        });
        RWarmDNA.Instance.OnCoinEvent += OnCoinTextUpdate;
        OnCoinTextUpdate();

    }

    private void OnCoinTextUpdate()
    {
        DietCost.text = PlayerPrefs.GetInt("Coin").ToString();
    }

    public void WarmAcid()
    {
        
        IndiaOdd.SetActive(true);
        RWarmDNA.Instance.WarmAcid();
        //StartCoroutine(nameof(GameStart));
    }

    IEnumerator WarmBirch()
    {
        yield return new WaitForSeconds(1f);
        IndiaOdd.SetActive(false);
        RWarmDNA.Instance.WarmBirch();
    }
}

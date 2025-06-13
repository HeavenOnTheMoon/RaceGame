using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MistMiner : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("Slider")]    public Image Charge;
[UnityEngine.Serialization.FormerlySerializedAs("SliderText")]    public Text ChargeCost;
    void Start()
    {
        Charge.fillAmount = 0;
        ChargeCost.text = "0%";

        PlayerPrefs.SetInt("Coin", 50000);
        //A_AudioManager.Instance.PlayMusic("Bg");
    }

    // Update is called once per frame
    void Update()
    {
        Charge.fillAmount += Time.deltaTime / 3f;
        ChargeCost.text = (int)(Charge.fillAmount * 100) + "%";
        if (Charge.fillAmount >= 1)
        {
            //UIWarmDNA.Instance.ClosePanel(UIType.Load);
            Application.targetFrameRate = 60;
            UIWarmDNA.Instance.SongMiner(UIType.Main);
            GrowMiner.Instance.WarmAcid();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatMiner : MonoBehaviour
{
    private float Birth;
    [UnityEngine.Serialization.FormerlySerializedAs("Button_Close")]    public Button Phrase_Those;
    void Start()
    {
        Phrase_Those.onClick.AddListener(()=> {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            UIWarmDNA.Instance.SongMiner(UIType.Main);
        });
    }

    public void OnEnable()
    {
        Birth = RWarmDNA.Instance.Handle.Birth;
        RWarmDNA.Instance.Handle.Birth = 0;
    }

    public void OnDisable()
    {
        RWarmDNA.Instance.Handle.Birth = Birth;
    }

}

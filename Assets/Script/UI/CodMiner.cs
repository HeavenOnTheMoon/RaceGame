using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodMiner : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("MusicBtn")]    public Button StoreCry;
[UnityEngine.Serialization.FormerlySerializedAs("MusicBtnOff")]    public Button StoreCryHot;
[UnityEngine.Serialization.FormerlySerializedAs("SoundBtn")]    public Button BoundCry;
[UnityEngine.Serialization.FormerlySerializedAs("SoundBtnOff")]    public Button BoundCryHot;
[UnityEngine.Serialization.FormerlySerializedAs("ReplayBtn")]
    public Button SparseCry;
[UnityEngine.Serialization.FormerlySerializedAs("BackBtn")]    public Button TermCry;
[UnityEngine.Serialization.FormerlySerializedAs("CloseBtn")]    public Button ThoseCry;

    private float Birth;
    void Start()
    {
        Application.targetFrameRate = 60;

        StoreCry.gameObject.SetActive(PlayerPrefs.GetInt("Music", 1) == 1);
        StoreCryHot.gameObject.SetActive(PlayerPrefs.GetInt("Music", 1) != 1);
        BoundCry.gameObject.SetActive(PlayerPrefs.GetInt("Sound", 1) == 1);
        BoundCryHot.gameObject.SetActive(PlayerPrefs.GetInt("Sound", 1) != 1);

        StoreCry.onClick.AddListener(()=> {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            Store();
        });
        StoreCryHot.onClick.AddListener(() => {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            Store();
        });

        BoundCry.onClick.AddListener(()=> {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            Bound();
        });
        BoundCryHot.onClick.AddListener(() => {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            Bound();
        });
        ThoseCry.onClick.AddListener(()=> {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            UIWarmDNA.Instance.SongMiner(UIType.Main);
        });
        TermCry.onClick.AddListener(()=> {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            UIWarmDNA.Instance.SongMiner(UIType.Main);
        });

        SparseCry.onClick.AddListener(()=> {
            A_AudioManager.Instance.PlaySound("ButtonMusic");
            UIWarmDNA.Instance.SongMiner(UIType.Main);
            RWarmDNA.Instance.WarmRemove();
            GrowMiner.Instance.WarmAcid();
        });
    }
    public void Store()
    {
        int music = PlayerPrefs.GetInt("Music", 1);
        music = music == 1 ? 0 : 1;
        PlayerPrefs.SetInt("Music", music);
        StoreCry.gameObject.SetActive(music == 1);
        StoreCryHot.gameObject.SetActive(music != 1);
        A_AudioManager.Instance.ToggleMusic();
    }
    public void Bound()
    {
        int sound = PlayerPrefs.GetInt("Sound", 1);
        sound = sound == 1 ? 0 : 1;
        PlayerPrefs.SetInt("Sound", sound);
        BoundCry.gameObject.SetActive(sound == 1); 
        BoundCryHot.gameObject.SetActive(sound != 1); 
        A_AudioManager.Instance.ToggleSound();
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

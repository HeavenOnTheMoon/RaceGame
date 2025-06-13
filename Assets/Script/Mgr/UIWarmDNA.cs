using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWarmDNA : MonoBehaviour
{
    public static UIWarmDNA Instance;
[UnityEngine.Serialization.FormerlySerializedAs("List_UI")]
    public List<GameObject> Sake_UI;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    public void SongMiner(UIType type)
    {
        int index = (int)type;
        if (index >= Sake_UI.Count  || index < 0 ) return;
        for (int i = 0; i < Sake_UI.Count; i++)
        {
            if (type == UIType.Main)
            {
                Sake_UI[i].SetActive(index == i);
            }
            else
            {
                Sake_UI[i].SetActive((index == i || (UIType)i == UIType.Main));
            }
        }
    }

    public void ThoseMiner(UIType type)
    {
        int index = (int)type;
        if (index >= Sake_UI.Count || index < 0) return;
        Sake_UI[index].SetActive(false);
    }

    
}

public enum UIType
{
    Load,Set,Skin,Win,Main
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RWarmDNA : MonoBehaviour
{
    public static RWarmDNA Instance;
[UnityEngine.Serialization.FormerlySerializedAs("player")]
    public ActionKick Handle;
[UnityEngine.Serialization.FormerlySerializedAs("IsOpenTurn")]
    public bool ToSongGnaw= false;

    private int BeltEight= 4;
[UnityEngine.Serialization.FormerlySerializedAs("GameLevelNumber")]
    public int WarmAlarmRecoil;
[UnityEngine.Serialization.FormerlySerializedAs("GameLevelAllNumber")]
    public int WarmAlarmAieRecoil;
[UnityEngine.Serialization.FormerlySerializedAs("AllCoinNumber")]
    public int AieDietRecoil;

    private Vector3 BoyBeltEra= Vector3.zero;
[UnityEngine.Serialization.FormerlySerializedAs("List_LineRoad")]
    public List<GameObject> Sake_PorkBelt;
[UnityEngine.Serialization.FormerlySerializedAs("List_LeftRoad")]    public List<GameObject> Sake_AlsoBelt;
[UnityEngine.Serialization.FormerlySerializedAs("List_RightRoad")]    public List<GameObject> Sake_GlialBelt;
[UnityEngine.Serialization.FormerlySerializedAs("List_DownRoad")]    public List<GameObject> Sake_LoamBelt;
[UnityEngine.Serialization.FormerlySerializedAs("List_UpRoad")]    public List<GameObject> Sake_DyBelt;
[UnityEngine.Serialization.FormerlySerializedAs("RoadParent")]
    public GameObject BeltChaste;
[UnityEngine.Serialization.FormerlySerializedAs("EndPointVec")]    /// <summary>
    /// 最后一个点的位置
    /// </summary>
    public Vector3 BoyGraceGel;
[UnityEngine.Serialization.FormerlySerializedAs("List_TileMap")]    //四个路面
    public List<GameObject> Sake_RoleSty;
    //位置
    private List<Vector3> Sake_RoleStyEra;

    private List<GameObject> Sake_Belt= new List<GameObject>();

    public event Action<GameObject> OnPointEvent;

    public event Action<int> OnChangeSkin;

    public event Action<bool> OnClockWiseEvent;

    public event Action<bool> OnChangeOrderEvent;

    public event Action OnEndEvent;

    public event Action OnCoinEvent;


   
    // 触发事件的方法
    public void ClearerGraceMexicoSmell(GameObject message)
    {
        OnPointEvent?.Invoke(message);
    }


    public void ClearerGuildHeatSmell(bool isClock)
    {
        OnClockWiseEvent?.Invoke(isClock);
    }

    public void ClearerDanceSmell(bool isOrder)
    {
        OnChangeOrderEvent?.Invoke(isOrder);
    }

    public void OnChangeSkinEvent(int skin)
    {
        OnChangeSkin?.Invoke(skin);
    }
    public void ClearerBoySmell()
    {
        OnEndEvent?.Invoke();
    }

    public void OnChangeCoinEvent()
    {
        OnCoinEvent?.Invoke();
    }

    private void Awake()
    {
        Instance = this;

    }

    void Start()
    {
        Sake_RoleStyEra = new List<Vector3>() { Vector3.zero,new Vector3(0,20,0),new Vector3(-20,0,0),new Vector3(-20,20,0)};
        //RoadInit();
        OnEndEvent += OnEndChangeRoad;
        HisGushAdapt(0);
    }

    #region 公共方法
    //路初始化
    public void BeltAcid()
    {
        for (int i = 0; i < Sake_RoleSty.Count; i++)
        {
            Sake_RoleSty[i].transform.localPosition = Sake_RoleStyEra[i];
        }
        BoyGraceGel = Vector3.down;
        for (int i = 0; i < BeltEight; i++)
        {
            GameObject road = CarBeltMexico(i);
            Sake_Belt.Add(road);
        }
    }


    public GameObject CarBeltMexico(int i)
    {
        
        GameObject randomRoad = i == 0 || i == 1 ? Sake_PorkBelt[0] : CarThirstBelt(BoyGraceGel);
        GameObject road = Instantiate(randomRoad);
        road.transform.SetParent(BeltChaste.transform);
        if (i != 0 && i != 1)
        {
            // 获取上一个道路的终点在世界空间中的位置
            Vector3 lastEndPointWorldPos = Sake_Belt[Sake_Belt.Count - 1].GetComponent<BeltArid>().BoyGrace.transform.position;
            // 将新道路的起点移动到上一个道路的终点位置
            road.transform.position = lastEndPointWorldPos;
        }
        else
        {
            if (i == 0)
            {
                road.transform.position = new Vector3(0,-4,0);
            }

            if (i == 1)
            {
                road.transform.position = Vector3.zero;
            }
        }
        var item = road.GetComponent<BeltArid>();
        BoyGraceGel = item.Little;
        if (item.JarCost != null)
        {
            WarmAlarmAieRecoil++;
            item.JarCost?.CodAdapt(WarmAlarmAieRecoil);
        }
        
        return road;
    }


    public void WarmAcid()
    {
        WarmAlarmAieRecoil = 0;
        Handle.Birth = 0;
        UpgradeBelt();
        BeltAcid();
        Handle.transform.localPosition = Vector3.zero;
        Handle.transform.localRotation = Quaternion.identity;
        Handle.PutWarmMexico.localPosition = Vector3.zero;
        Handle.PutWarmMexico.localRotation = Quaternion.identity;
    }
    public void WarmBirch()
    {
        Handle.transform.localPosition = Vector3.zero;
        Handle.transform.localRotation = Quaternion.identity;
        Handle.PutWarmMexico.localPosition = Vector3.zero;
        Handle.PutWarmMexico.localRotation = Quaternion.identity;
        Handle.Birth = 2;
        WarmAlarmRecoil = 0;
        
    }

    public void WarmRemove()
    {
        Handle.Birth = 0;
        ToSongGnaw = false;
        RWarmDNA.Instance.Handle.ToAheadBelt = false;
    }

    public int CarDiet()
    {
        return PlayerPrefs.GetInt("Coin");
    }

    public void IncomeDiet(int coin)
    {
        int all = coin + PlayerPrefs.GetInt("Coin");
        PlayerPrefs.SetInt("Coin", all);
        AieDietRecoil = all; 
        OnChangeCoinEvent();
    }

    public void HisGushAdapt(int index)
    {
        PlayerPrefs.SetInt("FishLock_" + index, 1);
    }

    #endregion

    #region 私有方法
    private void OnEndChangeRoad()
    {
        if (Sake_Belt.Count > 5)
        {
            Destroy(Sake_Belt[0].gameObject);
            Sake_Belt.RemoveAt(0);
        }
        GameObject road = CarBeltMexico(2);
        Sake_Belt.Add(road);
        for (int i = 0; i < Sake_Belt.Count; i++)
        {
            Sake_Belt[i].GetComponent<BeltArid>().RephraseOdd?.SetActive(i <= 3 && i > 0);
        }
    }

    private void UpgradeBelt()
    {
        for (int i = 0; i < Sake_Belt.Count; i++)
        {
            Destroy(Sake_Belt[i].gameObject);
        }
        Sake_Belt.Clear();
    }


    #endregion



    public GameObject CarThirstBelt(Vector3 pos)
    {
        // 随机数用于决定是否从直线道路中选择
        float randomValue = Random.value;
        GameObject selectedRoad = null;

        if (randomValue <= 0.2f && Sake_PorkBelt.Count > 1)
        {
            // 20%的概率从直线道路中选择
            if (pos == Vector3.up || pos == Vector3.down)
            {
                selectedRoad = Sake_PorkBelt[1];
            }
            else // Vector3.left 或 Vector3.right
            {
                selectedRoad = Sake_PorkBelt[0];
            }
        }
        else
        {
            // 80%的概率从对应方向的道路中选择
            List<GameObject> targetList = null;

            // 根据方向选择相反方向的道路列表
            if (pos == Vector3.up)
            {
                targetList = Sake_DyBelt;
            }
            else if (pos == Vector3.down)
            {
                targetList = Sake_LoamBelt;
            }
            else if (pos == Vector3.right)
            {
                targetList = Sake_GlialBelt;
            }
            else if (pos == Vector3.left)
            {
                targetList = Sake_AlsoBelt;
            }

            if (targetList != null && targetList.Count > 0)
            {
                int index = 0;
                int randomIndex = Random.Range(0, 100);
                if (randomIndex <= 5)
                {
                    index = 0;
                }
                else if (randomIndex > 5 && randomIndex <= 60)
                {
                    index = 1;
                }
                else if (randomIndex > 60 && randomIndex <= 90)
                {
                    index = 2;
                }
                else
                {
                    index = 3;
                }
                if (Sake_Belt.Count > 0  && Sake_Belt[Sake_Belt.Count - 1].GetComponent<BeltArid>().PrayBoom == RoadType.U)
                {
                    index = 1;
                }

                if (Sake_Belt.Count > 0 && Sake_Belt[Sake_Belt.Count - 1].GetComponent<BeltArid>().PrayBoom == RoadType.OverLap)
                {
                    index = 1;
                }

                if (Sake_Belt.Count > 0 && Sake_Belt[Sake_Belt.Count - 1].GetComponent<BeltArid>().PrayBoom == RoadType.Orthogonal)
                {
                    index = 0;
                }
                selectedRoad = targetList[index];
            }
        }

        return selectedRoad;
    }


   
}
public enum RoadType
{
    Line,Orthogonal,OverLap,U
}


using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionKick : MonoBehaviour
{
    [Header("基础参数")]
[UnityEngine.Serialization.FormerlySerializedAs("objectB")]    public Transform SeldomB;          // 物体B的Transform
[UnityEngine.Serialization.FormerlySerializedAs("speed")]    public float Birth= 5f;          // 移动速度
[UnityEngine.Serialization.FormerlySerializedAs("turnSpeed")]    public float LossBulge= 90f;    // 旋转速度（改为每秒360度，即1圈）
[UnityEngine.Serialization.FormerlySerializedAs("turnSpeedMultiplier")]    public float LossBulgeThroughout= 2f; // 转弯速度倍数
[UnityEngine.Serialization.FormerlySerializedAs("normalizeAngleThreshold")]    public float CultivateBeastRehydrate= 45f; // 归位角度阈值

    [Header("漂移参数")]
[UnityEngine.Serialization.FormerlySerializedAs("carGameObject")]    public Transform PutWarmMexico;    // 赛车子物体
[UnityEngine.Serialization.FormerlySerializedAs("maxDriftAngle")]    public float OakThumbBeast= 30f; // 最大漂移角度
[UnityEngine.Serialization.FormerlySerializedAs("driftSpeed")]    public float AloudBulge= 8f;     // 漂移速度
[UnityEngine.Serialization.FormerlySerializedAs("returnSpeed")]    public float MortalBulge= 5f;    // 回正速度
[UnityEngine.Serialization.FormerlySerializedAs("overshootAngle")]    public float OriginateBeast= 15f;// 过冲角度
[UnityEngine.Serialization.FormerlySerializedAs("oscillationSpeed")]    public float UnpublishedBulge= 5f;// 摆动速度
[UnityEngine.Serialization.FormerlySerializedAs("oscillationDecay")]    public float UnpublishedForay= 3f;// 摆动衰减速度

    [Header("轮胎痕迹参数")]
[UnityEngine.Serialization.FormerlySerializedAs("tireTrailMaterial")]    public Material LuckHumidBluebird;
[UnityEngine.Serialization.FormerlySerializedAs("trailSpawnInterval")]    public float DriveWristHandmade= 0.01f;  // 更高的生成频率
[UnityEngine.Serialization.FormerlySerializedAs("leftWheel")]    public Transform RoadValve;
[UnityEngine.Serialization.FormerlySerializedAs("rightWheel")]    public Transform MercyValve;
[UnityEngine.Serialization.FormerlySerializedAs("trailLength")]    public float DrivePointe= 0.2f;         // 更短的痕迹长度
[UnityEngine.Serialization.FormerlySerializedAs("pointsPerTrail")]    public int RelictRimHumid= 3;           // 每个痕迹的点数，使痕迹更平滑

    private bool SoImmerse= false;   // 是否正在转弯
    private bool SoUpGnawContest= false; // 是否在一次完整的转弯会话中
    private bool SoImplySeaweed= false; // 鼠标按下状态
    private bool Incentive;           // 顺时针旋转
    private Vector2 Reject;           // 转弯圆心
    private float radius;             // 转弯半径
    private float EndlessBulge;       // 角速度
    private Vector2 OutcropChitin;    // 初始点到圆心的向量
    private bool BetrayCommunity= false; // 是否需要归位
    private bool NowAlsoGnaw= false;  // 新增：标记是否已离开转弯状态

    // 漂移相关变量
    private float SandbarThumbBeast= 0f;
    private float ArcticThumbBeast= 0f;
    private float AloudTame= 0f;
    private bool SoSteamship= false;
    private float MortalBirchBeast= 0f;
    private bool SoThumbCovering= true;
    private const float STABILITY_THRESHOLD= 0.01f; // 稳定性阈值

    private List<GarbHumid> LuckValley= new List<GarbHumid>();
    private float PartHumidTame;
    private GameObject DemandWholesome;
    private Vector3 PartAlsoEra;
    private Vector3 PartGlialEra;
    private bool SoPartyHumid= true;
[UnityEngine.Serialization.FormerlySerializedAs("List_Fish")]
    public List<Sprite> Sake_Gush;
[UnityEngine.Serialization.FormerlySerializedAs("List_FishTail")]    public List<Sprite> Sake_GushSlow;
[UnityEngine.Serialization.FormerlySerializedAs("List_FishSpine")]    public List<SkeletonAnimation> Sake_GushHeavy;
[UnityEngine.Serialization.FormerlySerializedAs("FishSprite")]    public SpriteRenderer GushAfford;
[UnityEngine.Serialization.FormerlySerializedAs("FishTailSprite")]    public SpriteRenderer GushSlowAfford;
[UnityEngine.Serialization.FormerlySerializedAs("IsLeaveRoad")]
    //左侧逆时针，右侧顺时针

    public bool ToAheadBelt= false;

    void Start()
    {
        OnChangeSkin(0);
        if (PutWarmMexico == null)
        {
            PutWarmMexico = transform.Find("CarGameObject");
            if (PutWarmMexico == null)
            {
                Debug.LogError("找不到CarGameObject子物体！");
                return;
            }
        }

        // 创建轮胎痕迹容器
        DemandWholesome = new GameObject("TireTrails");
        DemandWholesome.transform.parent = transform.parent; // 将容器放在玩家同级

        // 如果没有指定轮子位置，创建默认位置（调整轮子间距）
        if (RoadValve == null)
        {
            RoadValve = TingleValveGrace("LeftWheel", new Vector3(-0.3f, 0, 0)); // 减小轮子间距
        }
        if (MercyValve == null)
        {
            MercyValve = TingleValveGrace("RightWheel", new Vector3(0.3f, 0, 0)); // 减小轮子间距
        }

        // 如果没有指定材质，创建默认材质
        if (LuckHumidBluebird == null)
        {
            LuckHumidBluebird = new Material(Shader.Find("Sprites/Default"))
            {
                color = new Color(0f, 0.85f, 1f, 1f)
            };
            LuckHumidBluebird.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            LuckHumidBluebird.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        }

        RWarmDNA.Instance.OnPointEvent += OnChangePoint;

        RWarmDNA.Instance.OnClockWiseEvent += OnChangeClockWise;

        RWarmDNA.Instance.OnChangeOrderEvent += OnChangerOrder;

        RWarmDNA.Instance.OnChangeSkin += OnChangeSkin;
    }


 
    private void OnChangeSkin(int obj)
    {
        Debug.Log("===================    Index    " + obj + "        Sake_GushHeavy.Count      " + Sake_GushHeavy.Count);

        /*FishSprite.sprite = List_Fish[obj];
        FishTailSprite.sprite = List_FishTail[obj];*/
        for (int i = 0; i < Sake_GushHeavy.Count; i++)
        {
            Sake_GushHeavy[i].gameObject.SetActive(i == obj);
        }
    }

    private void OnChangerOrder(bool obj)
    {
        //carGameObject.GetComponent<SpriteRenderer>().sortingOrder = obj ? 5 : 3;
    }

    private void OnChangeClockWise(bool obj)
    {
        Incentive = obj;
    }

    private void OnChangePoint(GameObject obj)
    {
        SeldomB = obj.transform;

        // 清理所有轮胎痕迹
        foreach (var trail in LuckValley)
        {
            if (trail.YorkAnything != null)
            {
                Destroy(trail.YorkAnything.gameObject);
            }
        }
        LuckValley.Clear();
        SoPartyHumid = true;
    }

    Transform TingleValveGrace(string name, Vector3 localPosition)
    {
        GameObject wheel = new GameObject(name);
        wheel.transform.parent = PutWarmMexico;
        wheel.transform.localPosition = localPosition;
        return wheel.transform;
    }

    void Update()
    {
        RationAware();
        CubismPlethora();
        CubismHutThumb();
        CubismGarbValley();
    }

    void RationAware()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ToAheadBelt = true;
        }

        if (Input.GetMouseButton(0) && RWarmDNA.Instance.ToSongGnaw && ToAheadBelt  && SeldomB != null)
        {
            if (!SoImplySeaweed)
            {
                SoImplySeaweed = true;
                if (!SoUpGnawContest)
                {
                    BirchSheGnawContest();
                    
                }
                else
                {
                    MusicianImmerse();
                }
            }
            SoImmerse = true;
            // 开始生成轮胎痕迹
            SoThumbCovering = false;
        }
        else
        {
            if (SoImplySeaweed && SeldomB != null)
            {
                SoImplySeaweed = false;
                DukeImmerse();
                // 停止生成新的轮胎痕迹，但保留现有的
                SoThumbCovering = true;
                SoPartyHumid = true; // 重置第一次轨迹标记，为下次点击做准备
            }
        }

        // 只在完全结束转弯时检查角度矫正
        if (!RWarmDNA.Instance.ToSongGnaw && SoUpGnawContest)
        {
            
            Debug.Log("结束转弯,进行归位");
            BoyGnawContest();
            AvantSapNonfunctional();
        }
    }

    void CubismPlethora()
    {
        // 始终保持移动
        transform.position += transform.up * Birth * Time.deltaTime;

        if (SoImmerse && SeldomB != null)
        {
            
            SunriseImmerse();
        }
        else if (BetrayCommunity)
        {
           
            CommunityParental();
        }
    }

    void BirchSheGnawContest()
    {
        SoUpGnawContest = true;
        EpidermisGnawGermanium();
        ClassroomGnawHinterland();
        BirchImmerse();
    }

    void MusicianImmerse()
    {
        BirchImmerse();
    }

    void EpidermisGnawGermanium()
    {
        //clockwise = transform.position.x < objectB.transform.position.x;
    }

    void ClassroomGnawHinterland()
    {
        radius = Vector2.Distance(transform.position, SeldomB.position);
        Reject = SeldomB.transform.position;
        OutcropChitin = (Vector2)transform.position - Reject;
        
        // 增加角速度
        EndlessBulge = (Birth / radius) * Mathf.Rad2Deg * LossBulgeThroughout;
        
        // 确保最小角速度不会太小
        EndlessBulge = Mathf.Max(EndlessBulge, LossBulge);
    }

    void BirchImmerse()
    {
        SoImmerse = true;
        OutcropChitin = (Vector2)transform.position - Reject;
    }

    void SunriseImmerse()
    {
        if (!SoImmerse) return;

        // 增加每帧的旋转角度
        float deltaAngle = EndlessBulge * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0, 0, deltaAngle * (Incentive ? -1 : 1));
        Vector2 newVec = rotation * OutcropChitin;

        // 计算新位置
        Vector2 newPosition = Reject + newVec;
        
        // 使用更大的移动速度
        float moveDistance = Birth * LossBulgeThroughout * Time.deltaTime;
        
        // 更新位置
        Vector2 currentPos = transform.position;
        Vector2 direction = (newPosition - currentPos).normalized;
        transform.position = currentPos + direction * moveDistance;

        // 更新朝向
        Vector2 tangent = Incentive ? new Vector2(newVec.y, -newVec.x) : new Vector2(-newVec.y, newVec.x);
        tangent.Normalize();
        float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        OutcropChitin = (Vector2)transform.position - Reject;
    }

    void DukeImmerse()
    {
        SoImmerse = false;
    }

    void BoyGnawContest()
    {
        SoUpGnawContest = false;
        SoImmerse = false;
        SoImplySeaweed = false;
    }

    // 提供外部设置转弯速度的方法
    public void CodGnawBulge(float newTurnSpeed)
    {
        LossBulge = newTurnSpeed;
        if (SoImmerse)
        {
            ClassroomGnawHinterland();
        }
    }

    // 提供外部设置最小转弯半径的方法
    public void CodLowGnawAttach(float newMinRadius)
    {
        radius = newMinRadius;
        if (SoImmerse)
        {
            ClassroomGnawHinterland();
        }
    }

    void CubismHutThumb()
    {
        if (PutWarmMexico == null) return;
        
        if (SoImmerse)
        {
            // 转向时的漂移
            SoThumbCovering = false;
            SoSteamship = false;
            ArcticThumbBeast = Incentive ? -OakThumbBeast : OakThumbBeast;
            SandbarThumbBeast = Mathf.Lerp(SandbarThumbBeast, ArcticThumbBeast, Time.deltaTime * AloudBulge);
            CargoParental(SandbarThumbBeast);
        }
        else if (!SoThumbCovering)
        {
            // 处理回正过程
            RationThumbTribal();
        }
    }

    public void RationThumbTribal()
    {
        if (!SoSteamship)
        {
            // 开始回正
            SoSteamship = true;
            MortalBirchBeast = SandbarThumbBeast;
            AloudTame = 0f;
        }

        AloudTame += Time.deltaTime;
        
        // 计算基础回正进度
        float returnProgress = Mathf.Clamp01(AloudTame * MortalBulge);
        
        // 只在回正初期添加摆动效果
        float oscillation = 0f;
        if (returnProgress < 0.8f) // 在回正80%前应用摆动
        {
            oscillation = Mathf.Sin(AloudTame * UnpublishedBulge) * 
                         OriginateBeast * 
                         Mathf.Exp(-AloudTame * UnpublishedForay);
        }

        // 计算目标角度
        float baseAngle = Mathf.Lerp(MortalBirchBeast, 0, returnProgress);
        float targetAngle = baseAngle + oscillation;

        // 应用旋转
        CargoParental(targetAngle);

        // 检查是否完成回正
        if (returnProgress >= 1f && Mathf.Abs(targetAngle) < STABILITY_THRESHOLD)
        {
            CoveringTribal();
        }
    }

    void CargoParental(float angle)
    {
        if (float.IsNaN(angle)) return; // 防止NaN值
        PutWarmMexico.localRotation = Quaternion.Euler(0, 0, angle);
    }

    void CoveringTribal()
    {
        SoThumbCovering = true;
        SoSteamship = false;
        SandbarThumbBeast = 0f;
        AloudTame = 0f;
        CargoParental(0f);
    }

    void OnDisable()
    {
        // 确保在禁用时重置所有状态
        CoveringTribal();

        // 清理所有轮胎痕迹
        foreach (var trail in LuckValley)
        {
            if (trail.YorkAnything != null)
            {
                Destroy(trail.YorkAnything.gameObject);
            }
        }
        LuckValley.Clear();
        SoPartyHumid = true;
    }

    void AvantSapNonfunctional()
    {
        float currentAngle = transform.rotation.eulerAngles.z;
        float closestStandardAngle = CarProlongSoftwoodBeast(currentAngle);
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, closestStandardAngle));

        if (angleDifference < CultivateBeastRehydrate)
        {
            BetrayCommunity = true;
            BirchThumbPurple();
        }
    }

    void CommunityParental()
    {
        float currentAngle = transform.rotation.eulerAngles.z;
        float closestStandardAngle = CarProlongSoftwoodBeast(currentAngle);
        float angleDifference = Mathf.DeltaAngle(currentAngle, closestStandardAngle);

        // 增加旋转速度，减少停顿感
        float step = LossBulge  * Time.deltaTime;
        
        if (Mathf.Abs(angleDifference) < 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, closestStandardAngle);
            BetrayCommunity = false;
        }
        else
        {
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, closestStandardAngle, step);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }
    }

    void BirchThumbPurple()
    {
        if (SoThumbCovering)
        {
            SoThumbCovering = false;
            SoSteamship = false;
            AloudTame = 0f;
            // 根据归位方向设置初始漂移角度
            float currentAngle = transform.rotation.eulerAngles.z;
            float closestStandardAngle = CarProlongSoftwoodBeast(currentAngle);
            float angleDifference = Mathf.DeltaAngle(currentAngle, closestStandardAngle);
            //clockwise = angleDifference > 0;
        }
    }

    float CarProlongSoftwoodBeast(float currentAngle)
    {
        float[] standardAngles = { 0, 90, 180, 270 };
        float minDifference = float.MaxValue;
        float closestAngle = 0;

        foreach (float standardAngle in standardAngles)
        {
            float difference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, standardAngle));
            if (difference < minDifference)
            {
                minDifference = difference;
                closestAngle = standardAngle;
            }
        }

        return closestAngle;
    }

    void CubismGarbValley()
    {
        // 只在按住时生成轮胎痕迹
        if (!SoThumbCovering && SoImplySeaweed && Time.time - PartHumidTame > DriveWristHandmade)
        {
            Vector3 leftWheelPos = RoadValve.position;
            Vector3 rightWheelPos = MercyValve.position;

            if (!SoPartyHumid)
            {
                TingleGarbHumid(leftWheelPos, PartAlsoEra, false);
                TingleGarbHumid(rightWheelPos, PartGlialEra, false);
            }
            else
            {
                TingleGarbHumid(leftWheelPos, leftWheelPos, true);
                TingleGarbHumid(rightWheelPos, rightWheelPos, true);
                SoPartyHumid = false;
            }

            PartAlsoEra = leftWheelPos;
            PartGlialEra = rightWheelPos;
            PartHumidTame = Time.time;
        }
    }

    void TingleGarbHumid(Vector3 wheelPos, Vector3 lastPos, bool isFirstTrail)
    {
        GameObject trailObject = new GameObject("GarbHumid");
        trailObject.transform.parent = DemandWholesome.transform;
        
        LineRenderer line = trailObject.AddComponent<LineRenderer>();
        line.material = new Material(LuckHumidBluebird);
        line.positionCount = RelictRimHumid;

        // 计算中间点，创建平滑曲线
        Vector3[] positions = new Vector3[RelictRimHumid];
        if (isFirstTrail)
        {
            // 第一个痕迹使用简单的直线
            for (int i = 0; i < RelictRimHumid; i++)
            {
                float t = i / (float)(RelictRimHumid - 1);
                positions[i] = Vector3.Lerp(wheelPos, wheelPos - transform.up * DrivePointe, t);
            }
        }
        else
        {
            // 后续痕迹使用平滑曲线
            Vector3 control = wheelPos - transform.up * (DrivePointe * 0.5f);
            for (int i = 0; i < RelictRimHumid; i++)
            {
                float t = i / (float)(RelictRimHumid - 1);
                positions[i] = EncaseInter(lastPos, control, wheelPos, t);
            }
        }

        line.SetPositions(positions);
        
        GarbHumid trail = new GarbHumid(line, Time.time);
        LuckValley.Add(trail);
    }

    Vector3 EncaseInter(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }

    // 可以添加动态调整方法
    public void EasterHumidPostmodern(float width, float alpha, float length)
    {
        foreach (var trail in LuckValley)
        {
            if (trail.YorkAnything != null)
            {
                trail.YorkAnything.startWidth = width;
                trail.YorkAnything.endWidth = width;
                Color c = trail.YorkAnything.startColor;
                c.a = alpha;
                trail.YorkAnything.startColor = c;
                trail.YorkAnything.endColor = c;
            }
        }
        DrivePointe = length;
    }

    public void DiverGarbValley()
    {
        // 清理所有轮胎痕迹
        foreach (var trail in LuckValley)
        {
            if (trail.YorkAnything != null)
            {
                Destroy(trail.YorkAnything.gameObject);
            }
        }
        LuckValley.Clear();
    }
}

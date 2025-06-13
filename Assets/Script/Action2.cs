using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action2 : MonoBehaviour
{
  /*
  [Header("基础参数")]
    public Transform objectB;          // 物体B的Transform
    public float speed = 5f;          // 移动速度
    public float turnSpeed = 360f;    // 旋转速度（改为每秒360度，即1圈）
    public float turnSpeedMultiplier = 2f; // 转弯速度倍数
    public float normalizeAngleThreshold = 45f; // 归位角度阈值

    [Header("漂移参数")]
    public Transform carGameObject;    // 赛车子物体
    public float maxDriftAngle = 30f; // 最大漂移角度
    public float driftSpeed = 8f;     // 漂移速度
    public float returnSpeed = 5f;    // 回正速度
    public float overshootAngle = 15f;// 过冲角度
    public float oscillationSpeed = 5f;// 摆动速度
    public float oscillationDecay = 3f;// 摆动衰减速度

    [Header("轮胎痕迹参数")]
    public Material tireTrailMaterial;
    public float trailSpawnInterval = 0.01f;  // 更高的生成频率
    public Transform leftWheel;
    public Transform rightWheel;
    public float trailLength = 0.2f;         // 更短的痕迹长度
    public int pointsPerTrail = 3;           // 每个痕迹的点数，使痕迹更平滑

    private bool isTurning = false;   // 是否正在转弯
    private bool isInTurnSession = false; // 是否在一次完整的转弯会话中
    private bool isMousePressed = false; // 鼠标按下状态
    private bool clockwise;           // 顺时针旋转
    private Vector2 center;           // 转弯圆心
    private float radius;             // 转弯半径
    private float angularSpeed;       // 角速度
    private Vector2 initialVector;    // 初始点到圆心的向量
    private bool shouldNormalize = false; // 是否需要归位
    private bool hasLeftTurn = false;  // 新增：标记是否已离开转弯状态

    // 漂移相关变量
    private float currentDriftAngle = 0f;
    private float targetDriftAngle = 0f;
    private float driftTime = 0f;
    private bool isReturning = false;
    private float returnStartAngle = 0f;
    private bool isDriftComplete = true;
    private const float STABILITY_THRESHOLD = 0.01f; // 稳定性阈值

    private List<GarbHumid> tireTrails = new List<GarbHumid>();
    private float lastTrailTime;
    private GameObject trailsContainer;
    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;
    private bool isFirstTrail = true;

    

    void Start()
    {
        if (carGameObject == null)
        {
            carGameObject = transform.Find("CarGameObject");
            if (carGameObject == null)
            {
                Debug.LogError("找不到CarGameObject子物体！");
                return;
            }
        }

        // 创建轮胎痕迹容器
        trailsContainer = new GameObject("TireTrails");
        trailsContainer.transform.parent = transform.parent; // 将容器放在玩家同级

        // 如果没有指定轮子位置，创建默认位置（调整轮子间距）
        if (leftWheel == null)
        {
            leftWheel = CreateWheelPoint("LeftWheel", new Vector3(-0.3f, 0, 0)); // 减小轮子间距
        }
        if (rightWheel == null)
        {
            rightWheel = CreateWheelPoint("RightWheel", new Vector3(0.3f, 0, 0)); // 减小轮子间距
        }

        // 如果没有指定材质，创建默认材质
        if (tireTrailMaterial == null)
        {
            tireTrailMaterial = new Material(Shader.Find("Sprites/Default"))
            {
                color = new Color(0.1f, 0.1f, 0.1f, 1f)
            };
            tireTrailMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            tireTrailMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        }
    }

    Transform CreateWheelPoint(string name, Vector3 localPosition)
    {
        GameObject wheel = new GameObject(name);
        wheel.transform.parent = carGameObject;
        wheel.transform.localPosition = localPosition;
        return wheel.transform;
    }

    void Update()
    {
        HandleInput();
        UpdateMovement();
        UpdateCarDrift();
        UpdateTireTrails();
    }

    void HandleInput()
    {
        if (Input.GetMouseButton(0) && RWarmDNA.Instance.IsOpenTurn)
        {
            if (!isMousePressed)
            {
                isMousePressed = true;
                if (!isInTurnSession)
                {
                    StartNewTurnSession();
                }
                else
                {
                    ContinueTurning();
                }
            }
            isTurning = true;
        }
        else
        {
            if (isMousePressed)
            {
                isMousePressed = false;
                StopTurning();
            }
        }

        // 只在完全结束转弯时检查角度矫正
        if (!RWarmDNA.Instance.IsOpenTurn && isInTurnSession)
        {
            EndTurnSession();
            CheckForNormalization();
        }
    }

    void UpdateMovement()
    {
        if (isTurning)
        {
            ExecuteTurning();
        }
        else if (shouldNormalize)
        {
            NormalizeRotation();
        }
        else
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }

    void StartNewTurnSession()
    {
        isInTurnSession = true;
        DetermineTurnDirection();
        CalculateTurnParameters();
        StartTurning();
    }

    void ContinueTurning()
    {
        StartTurning();
    }

    void DetermineTurnDirection()
    {
        clockwise = transform.position.x < objectB.transform.position.x;
    }

    void CalculateTurnParameters()
    {
        radius = Vector2.Distance(transform.position, objectB.position);
        center = objectB.transform.position;
        initialVector = (Vector2)transform.position - center;
        
        // 增加角速度
        angularSpeed = (speed / radius) * Mathf.Rad2Deg * turnSpeedMultiplier;
        
        // 确保最小角速度不会太小
        angularSpeed = Mathf.Max(angularSpeed, turnSpeed);
    }

    void StartTurning()
    {
        isTurning = true;
        initialVector = (Vector2)transform.position - center;
    }

    void ExecuteTurning()
    {
        if (!isTurning) return;

        // 增加每帧的旋转角度
        float deltaAngle = angularSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0, 0, deltaAngle * (clockwise ? -1 : 1));
        Vector2 newVec = rotation * initialVector;

        // 计算新位置
        Vector2 newPosition = center + newVec;
        
        // 使用更大的移动速度
        float moveDistance = speed * turnSpeedMultiplier * Time.deltaTime;
        
        // 更新位置
        Vector2 currentPos = transform.position;
        Vector2 direction = (newPosition - currentPos).normalized;
        transform.position = currentPos + direction * moveDistance;

        // 更新朝向
        Vector2 tangent = clockwise ? new Vector2(newVec.y, -newVec.x) : new Vector2(-newVec.y, newVec.x);
        tangent.Normalize();
        float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        initialVector = (Vector2)transform.position - center;
    }

    void StopTurning()
    {
        isTurning = false;
    }

    void EndTurnSession()
    {
        isInTurnSession = false;
        isTurning = false;
        isMousePressed = false;
    }

    // 提供外部设置转弯速度的方法
    public void SetTurnSpeed(float newTurnSpeed)
    {
        turnSpeed = newTurnSpeed;
        if (isTurning)
        {
            CalculateTurnParameters();
        }
    }

    // 提供外部设置最小转弯半径的方法
    public void SetMinTurnRadius(float newMinRadius)
    {
        radius = newMinRadius;
        if (isTurning)
        {
            CalculateTurnParameters();
        }
    }

    void UpdateCarDrift()
    {
        if (carGameObject == null) return;

        if (isTurning)
        {
            // 转向时的漂移
            isDriftComplete = false;
            isReturning = false;
            targetDriftAngle = clockwise ? -maxDriftAngle : maxDriftAngle;
            currentDriftAngle = Mathf.Lerp(currentDriftAngle, targetDriftAngle, Time.deltaTime * driftSpeed);
            ApplyRotation(currentDriftAngle);
        }
        else if (!isDriftComplete)
        {
            // 处理回正过程
            HandleDriftReturn();
        }
    }

    void HandleDriftReturn()
    {
        if (!isReturning)
        {
            // 开始回正
            isReturning = true;
            returnStartAngle = currentDriftAngle;
            driftTime = 0f;
        }

        driftTime += Time.deltaTime;
        
        // 计算基础回正进度
        float returnProgress = Mathf.Clamp01(driftTime * returnSpeed);
        
        // 只在回正初期添加摆动效果
        float oscillation = 0f;
        if (returnProgress < 0.8f) // 在回正80%前应用摆动
        {
            oscillation = Mathf.Sin(driftTime * oscillationSpeed) * 
                         overshootAngle * 
                         Mathf.Exp(-driftTime * oscillationDecay);
        }

        // 计算目标角度
        float baseAngle = Mathf.Lerp(returnStartAngle, 0, returnProgress);
        float targetAngle = baseAngle + oscillation;

        // 应用旋转
        ApplyRotation(targetAngle);

        // 检查是否完成回正
        if (returnProgress >= 1f && Mathf.Abs(targetAngle) < STABILITY_THRESHOLD)
        {
            CompleteReturn();
        }
    }

    void ApplyRotation(float angle)
    {
        if (float.IsNaN(angle)) return; // 防止NaN值
        carGameObject.localRotation = Quaternion.Euler(0, 0, angle);
    }

    void CompleteReturn()
    {
        isDriftComplete = true;
        isReturning = false;
        currentDriftAngle = 0f;
        driftTime = 0f;
        ApplyRotation(0f);
    }

    void OnDisable()
    {
        // 确保在禁用时重置所有状态
        CompleteReturn();

        // 清理所有轮胎痕迹
        foreach (var trail in tireTrails)
        {
            if (trail.lineRenderer != null)
            {
                Destroy(trail.lineRenderer.gameObject);
            }
        }
        tireTrails.Clear();
        isFirstTrail = true;
    }

    void CheckForNormalization()
    {
        float currentAngle = transform.rotation.eulerAngles.z;
        float closestStandardAngle = GetClosestStandardAngle(currentAngle);
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, closestStandardAngle));

        if (angleDifference < 45f)
        {
            shouldNormalize = true;
            StartDriftEffect();
        }
    }

    void NormalizeRotation()
    {
        float currentAngle = transform.rotation.eulerAngles.z;
        float closestStandardAngle = GetClosestStandardAngle(currentAngle);
        float angleDifference = Mathf.DeltaAngle(currentAngle, closestStandardAngle);

        float step = turnSpeed * Time.deltaTime;
        float newAngle;

        if (Mathf.Abs(angleDifference) < step)
        {
            newAngle = closestStandardAngle;
            shouldNormalize = false;
        }
        else
        {
            newAngle = Mathf.MoveTowardsAngle(currentAngle, closestStandardAngle, step);
        }

        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    void StartDriftEffect()
    {
        if (isDriftComplete)
        {
            isDriftComplete = false;
            isReturning = false;
            driftTime = 0f;
            // 根据归位方向设置初始漂移角度
            float currentAngle = transform.rotation.eulerAngles.z;
            float closestStandardAngle = GetClosestStandardAngle(currentAngle);
            float angleDifference = Mathf.DeltaAngle(currentAngle, closestStandardAngle);
            clockwise = angleDifference > 0;
        }
    }

    float GetClosestStandardAngle(float currentAngle)
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

    void UpdateTireTrails()
    {
        // 更新现有轮胎痕迹的淡出效果
        for (int i = tireTrails.Count - 1; i >= 0; i--)
        {
            if (!tireTrails[i].UpdateFade(Time.time))
            {
                Destroy(tireTrails[i].lineRenderer.gameObject);
                tireTrails.RemoveAt(i);
            }
        }

        // 在漂移时生成新的轮胎痕迹
        if (!isDriftComplete && Time.time - lastTrailTime > trailSpawnInterval)
        {
            Vector3 leftWheelPos = leftWheel.position;
            Vector3 rightWheelPos = rightWheel.position;

            if (!isFirstTrail)
            {
                CreateTireTrail(leftWheelPos, lastLeftPos, false);
                CreateTireTrail(rightWheelPos, lastRightPos, false);
            }
            else
            {
                CreateTireTrail(leftWheelPos, leftWheelPos, true);
                CreateTireTrail(rightWheelPos, rightWheelPos, true);
                isFirstTrail = false;
            }

            lastLeftPos = leftWheelPos;
            lastRightPos = rightWheelPos;
            lastTrailTime = Time.time;
        }
    }

    void CreateTireTrail(Vector3 wheelPos, Vector3 lastPos, bool isFirstTrail)
    {
        GameObject trailObject = new GameObject("GarbHumid");
        trailObject.transform.parent = trailsContainer.transform;
        
        LineRenderer line = trailObject.AddComponent<LineRenderer>();
        line.material = new Material(tireTrailMaterial);
        line.positionCount = pointsPerTrail;

        // 计算中间点，创建平滑曲线
        Vector3[] positions = new Vector3[pointsPerTrail];
        if (isFirstTrail)
        {
            // 第一个痕迹使用简单的直线
            for (int i = 0; i < pointsPerTrail; i++)
            {
                float t = i / (float)(pointsPerTrail - 1);
                positions[i] = Vector3.Lerp(wheelPos, wheelPos - transform.up * trailLength, t);
            }
        }
        else
        {
            // 后续痕迹使用平滑曲线
            Vector3 control = wheelPos - transform.up * (trailLength * 0.5f);
            for (int i = 0; i < pointsPerTrail; i++)
            {
                float t = i / (float)(pointsPerTrail - 1);
                positions[i] = BezierCurve(lastPos, control, wheelPos, t);
            }
        }

        line.SetPositions(positions);
        
        GarbHumid trail = new GarbHumid(line, Time.time);
        tireTrails.Add(trail);
    }

    Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }

    // 可以添加动态调整方法
    public void AdjustTrailProperties(float width, float alpha, float length)
    {
        foreach (var trail in tireTrails)
        {
            if (trail.lineRenderer != null)
            {
                trail.lineRenderer.startWidth = width;
                trail.lineRenderer.endWidth = width;
                Color c = trail.lineRenderer.startColor;
                c.a = alpha;
                trail.lineRenderer.startColor = c;
                trail.lineRenderer.endColor = c;
            }
        }
        trailLength = length;
    }*/

    
}

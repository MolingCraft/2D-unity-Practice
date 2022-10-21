using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : MonoBehaviour
{
    Rigidbody2D EnemyRigid;
    Animator EnemyAnim;

    //public float AlphaEnemy;

    [Header("基础设置")]
    public float EnemySpeed;//怪物移动速度

    [Header("追踪设置")]
    public bool PlayerTrackingIf;//是否追踪玩家
    public float Radius;//追踪半径
    public float interval;//怪物追踪时与玩家的间距
    
    private Transform PlayerTransform;

    //初始位置，用于enemy的回归
    public bool BackStartIf;//是否返回初始位置
    private float XStartDistance;
    private float YStartDistance;

    [Header("游荡设置")]
    public bool RandomWanderingIf;//是否随机游荡
    public float XRandomNumber;
    public float YRandomNumber;
    [Header("游荡类型设置")]
    /*
    public bool RandomWander;//完全随机
    [Header("目标终点坐标（x，y）")]
    public bool Line;//以直线游荡
    [Header("圆心坐标（x，y），半径R")]
    public bool Round;//圆形
    [Header("首参数1横0竖，横长W，纵长H")]
    public bool Rectangle;//长方形
    [Header("游荡参数，坐标请分开填入")]
    public float Number1;
    public float Number2;
    public float Number3;
    public float Number4;
    */
    [Header("脚印设置")]
    public bool FeetShowIf;//是否显示脚印
    public ParticleSystem stepSystem;
    public Material footStepMaterial;
    Vector3 lastEmit;
    public float delta = 1;
    public float gap = 0.5f;
    public float systemYOffset = -0.225f;
    Vector2 lookDirection = new Vector2(1, 0);

    int dir = 1;
    void Start()
    {
        EnemyRigid = GetComponent<Rigidbody2D>();//刚体
        EnemyAnim = GetComponent<Animator>();//动画
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        XStartDistance = transform.position.x;
        YStartDistance = transform.position.y;

        //AlphaEnemy = 0;
        //transform.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaEnemy);


    }
    private void Reset()
    {
        EnemySpeed = 2.0f;
    }
    private void FixedUpdate()
    {
        
        if(PlayerTrackingIf)TrackingPlayer();
        else if(RandomWanderingIf)RandomWandering();
        float XRotateDistance = transform.position.x - lastEmit.x;
        float YRotateDistance = transform.position.y - lastEmit.y;
        if (!Mathf.Approximately(XRotateDistance, 0.0f) || !Mathf.Approximately(YRotateDistance, 0.0f))
        {
            lookDirection.Set(XRotateDistance, YRotateDistance);
            lookDirection.Normalize();
        }
        if (FeetShowIf)SetFeetStep();
    }
    // Update is called once per frame
    void Update()
    {
       
    }


    void RandomWandering()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        EnemyRigid.velocity = new Vector2(XRandomNumber*EnemySpeed, YRandomNumber * EnemySpeed);
        
        //transform.position = new Vector2(transform.position.x+ Random.Range(0, 1) * EnemySpeed, transform.position.y+ Random.Range(0, 1) * EnemySpeed);
    }
    void TrackingPlayer()
    {
        if (PlayerTransform != null)
        {
            float distance = (transform.position - PlayerTransform.position).sqrMagnitude;
            float XDistance = PlayerTransform.position.x - transform.position.x;
            float YDistance = PlayerTransform.position.y - transform.position.y;
            if (distance < Radius && distance > interval)
            {
                EnemyRigid.velocity = new Vector2(XDistance * EnemySpeed, YDistance * EnemySpeed);
                //Prefs.storynumber = 1000;
                
            }
            else if(BackStartIf&&distance>Radius)
            {
                transform.position =new Vector2(XStartDistance,YStartDistance);
                
            }
            
        }
    }
    void SetFeetStep()
    {
        if (Vector2.Distance(lastEmit, transform.position) > delta)
        {
            Gizmos.color = Color.green;
            var pos = transform.position + new Vector3(lookDirection.y * gap * dir, -lookDirection.x * gap * dir, -5) + new Vector3(0, systemYOffset, 0);
            //利用两向量垂直公式：x1x2+y1y2=0，将lookDirection转90度。
            dir *= -1;
            ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
            ep.position = pos;
            if (lookDirection.x > 0)
            {
                ep.rotation = Vector2.Angle(new Vector2(0, 1), lookDirection);
            }
            else if (lookDirection.x <= 0)
            {
                ep.rotation = -Vector2.Angle(new Vector2(0, 1), lookDirection);
            }
            stepSystem.Emit(ep, 1);
            lastEmit = transform.position;

        }
    }
}

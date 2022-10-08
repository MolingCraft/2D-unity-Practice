using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D PlayerRigid;
    ////public Animator PlayerAnim;
    [Header("基础设置")]
    //public Transform Player;
    //public string PlayerName;
    public GameObject PlayerSprite;//玩家角色物体
    public float PlayerAlpha;//玩家不透明度，同时也是玩家的血量
    public float PlayerRunSpeed;//移动速度
    private float PlayerCurrentSpeed;//当前速度
    //public bool AlphaRecoverIf;//不透明度是否恢复
    public float AlphaRecoverNumber;//不透明度恢复数值
    public float AlphaMaxNumber;//不透明度可恢复至的最大数值
    private float moveHor;//水平移动键Horizontal是否被按下
    private float moveVer;//垂直移动键Vertical是否被按下

    [Header("冲刺设置")]
    public bool AllowRushIf;//是否允许冲刺
    private bool InRushIf;//是否在冲刺中
    public float PlayerRushSpeed;//冲刺速度
    public float RushAlphaExpend;//冲刺消耗的不透明度
    public float RushMinAlpha;//允许冲刺的最小不透明度值
    [Header("脚印设置")]
    public bool FeetShowIf;//是否显示脚印
    public ParticleSystem StepSystem;//脚印粒子的对象
    public Material FeetStepMaterial;//脚印粒子的材质
    Vector3 LastEmit;//上一个位置坐标
    public float delta = 1;
    public float gap = 0.5f;
    public float systemYOffset = -0.225f;
    Vector2 lookDirection = new Vector2(1, 0);

    [Header("技能生效对象")]
    public GameObject MapBlock;//地图
    public GameObject Enemy;

    [Header("声音可视化设置")]
    public bool VoiceShowIf;//是否显示脚印
    public ParticleSystem VoiceSystem;//脚印粒子的对象
    public Material VoiceMaterial;//脚印粒子的材质

    [Header("“视”技能参数设置")]
    //技能《视》
    public bool AllowEyeSkillIf;//是否允许使用技能
    private bool InEyeSkillIf;//是否在技能生效时间内
    public float EyeAlphaExpend;//《视》消耗的人物不透明度
    public float EyeMinAlpha;//允许使用“视”的最小不透明度值

    public float AlphaAddSkill;//技能增加的目标不透明度值
    public float InfluenceAlpha;//目标的不透明度值
    /*
    public int UseEyeSkillNumber;//技能已使用次数
    public float MaxNumberEyeSkill;//最大技能次数
    private float NumberEyeSkill;//剩余技能次数
    public float CDeyeSkill;//CD时间
    private bool InCDeyeIf;//是否在cd中
    public float TimeEyeSkill;//持续时间
    */


    [Header("“听”技能参数设置")]
    //技能《听》
    
    public bool AllowListenSkillIf;
    public int UseListenSkillNumber;
    public float MaxNumberListenSkill;
    private float NumberListenSkill;
    public float CDlistenSkill;
    private bool InCDlistenIf;
    public float TimelistenSkill;
    private float ListenAlphaConsumption;//技能增加的alpha值


    int dir = 1;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRigid.GetComponent<Rigidbody2D>();//人物刚体
        ////PlayerAnim.GetComponent<Animator>();//人物动画
        ///
        LastEmit = transform.position;
        StepSystem.GetComponent<Renderer>().material = FeetStepMaterial;

        AllowRushIf = true;
        InRushIf = false;
        VoiceSystem.Stop();//初始阶段直接关停声音可视化

        InEyeSkillIf = false;

        /*
        UseEyeSkillNumber = 0;
        NumberEyeSkill = MaxNumberEyeSkill;
        NumberListenSkill = MaxNumberListenSkill;
        UseListenSkillNumber = 0;
        AlphaAddSkill = 1 / MaxNumberEyeSkill;
        */
        MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0f);

    }

    private void Reset()
    {
        PlayerAlpha = 1;
        PlayerRunSpeed = 5.0f;
        PlayerRushSpeed = 10.0f;
    }


    private void FixedUpdate()
    {
        PlayerSprite.GetComponent<Renderer>().material.color=new Color(1.0f, 1.0f, 1.0f, PlayerAlpha);

        if (PlayerAlpha < RushMinAlpha) AllowRushIf = false;else AllowRushIf = true;

        Move();

        
        if ((PlayerAlpha<AlphaMaxNumber)&&!Input.GetKey(KeyCode.LeftShift)&&!InEyeSkillIf) PlayerAlpha += AlphaRecoverNumber;

        if (FeetShowIf)SetFeetStep();

        //if (AllowEyeSkillIf && Input.GetButtonDown("EyeSkill"))StartCoroutine(EyeSkill());

        if (AllowEyeSkillIf)
        {
            if (Input.GetButtonDown("EyeSkill")) InEyeSkillIf = !InEyeSkillIf;
            EyeSkill();
        }

        if (AllowListenSkillIf && Input.GetButtonDown("ListenSkill"))StartCoroutine(ListenSkill());

    }
    // Update is called once per frame
    void Update()
    {
    }
    
    void Move()
    {
        moveVer = Input.GetAxis("Vertical");
        moveHor = Input.GetAxis("Horizontal");
        if (!Mathf.Approximately(moveHor, 0.0f) || !Mathf.Approximately(moveVer, 0.0f))//使脚印可以旋转朝向移动方向
        {
            lookDirection.Set(moveHor,moveVer);
            lookDirection.Normalize();
        }
        ////PlayerAnim.SetFloat("Look X", lookDirection.x);
        ////PlayerAnim.SetFloat("Look Y", lookDirection.y);
        ////PlayerAnim.SetFloat("Speed", move.magnitude);

        
        if (AllowRushIf &&Input.GetKey(KeyCode.LeftShift))//冲刺
        {
            InRushIf = true;
            PlayerAlpha -= RushAlphaExpend;
            PlayerRigid.velocity = new Vector2(moveHor * PlayerRushSpeed, moveVer * PlayerRushSpeed);
            // PlayerRushSpeed * Time.deltaTime; * Time.deltaTime;
            // 这个似乎可以防止因不同电脑的性能导致的帧数差别，从而令奔跑速度不一致的问题
            PlayerCurrentSpeed = PlayerRushSpeed;
            ////PlayerAnim.SetBool("IsRush", true);
        }
        else
        {
            InRushIf = false;
            PlayerRigid.velocity = new Vector2(moveHor * PlayerRunSpeed, moveVer * PlayerRunSpeed);
            PlayerCurrentSpeed = PlayerRunSpeed;
            ////PlayerAnim.SetBool("IsRush", false);
        }
        //PlayerRigid.velocity=position;
        
        //Input.GetAxisRaw("Vertical")会导致移动时不是平移，缺少缓动

    }
    
    void SetFeetStep()//脚步
    {
        if (Vector2.Distance(LastEmit, transform.position) > delta)
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
            StepSystem.Emit(ep, 1);
            LastEmit = transform.position;
           
        }
    }

    void EyeSkill()
    {
        if (PlayerAlpha <= EyeMinAlpha) InEyeSkillIf = false;

        if (InEyeSkillIf)
        {
            PlayerAlpha -= EyeAlphaExpend;
            
            InfluenceAlpha += AlphaAddSkill;
            MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, InfluenceAlpha);
        }
        else
        {
            if(InfluenceAlpha>0)InfluenceAlpha -= AlphaAddSkill;
            MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, InfluenceAlpha);
        }
            
    }
    /*
    private IEnumerator EyeSkill()//使用技能，且制止玩家在技能期间重复使用技能
    {
        PlayerAlpha -= EyeAlphaExpend;
        NumberEyeSkill--;
        UseEyeSkillNumber++;
        AllowEyeSkill = false;
        InCDeyeIf = true;

        MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaAddSkill * UseEyeSkillNumber);
        yield return new WaitForSeconds(TimeEyeSkill);//技能持续时长


        yield return new WaitForSeconds(CDeyeSkill);//cd冷却
        InCDeyeIf = false;//CD结束
        NumberEyeSkill++;
        AllowEyeSkill = true;
    }
    */

    private IEnumerator ListenSkill()
    {
        PlayerAlpha -= ListenAlphaConsumption;
        NumberListenSkill--;
        UseListenSkillNumber++;
        AllowListenSkillIf = false;
        InCDlistenIf = true;


        yield return new WaitForSeconds(TimelistenSkill);//技能持续时长

        //chararigid.gravityScale = RushGravity;//重力回归初值

        yield return new WaitForSeconds(CDlistenSkill);//cd冷却
        InCDlistenIf = false;//CD结束
        NumberListenSkill++;
        AllowListenSkillIf = true;
    }
}

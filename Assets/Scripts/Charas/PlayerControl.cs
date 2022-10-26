using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D PlayerRigid;
    //public Animator PlayerAnim;
    [Header("基础设置")]
    public Transform PlayerTransform;
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
    int dir = 1;
    [Header("技能生效对象")]
    public GameObject MapBlock;//地图
    public GameObject Enemy;

    [Header("声音可视化设置")]
    public bool VoiceShowIf;//是否允许音圈显示
    private bool VoiceShowOne;//切换音圈显示时防止命令重复运行
    public ParticleSystem VoiceSystem;//音圈的对象
    public Material VoiceMaterial;//音圈的材质
    public ParticleSystem EnemyVoiceSystem;//音圈的对象
    public Material EnemyVoiceMaterial;//音圈的材质

    [Header("“视”技能参数设置")]
    //技能《视》
    public bool AllowEyeSkillIf;//是否允许使用技能
    public float AlphaStartEyeNumber;//起始生效alpha值
    public int UseEyeSkillNumber;//技能已使用次数
    public float MaxNumberEyeSkill;//最大技能次数
    public float TimeEyeSkill;//持续时间
    private float AlphaAddSkill;//技能增加的目标不透明度值

    [Header("“听”技能参数设置")]
    //技能《听》

    public bool AllowListenSkillIf;//是否允许使用技能
    public int UseListenSkillNumber;//技能已使用次数
    public float MaxNumberListenSkill;//最大技能次数
    public float TimeListenSkill;//持续时间


    private void Awake()
    {
        PlayerRigid.GetComponent<Rigidbody2D>();//人物刚体
        //PlayerAnim.GetComponent<Animator>();//人物动画

        StepSystem.GetComponent<Renderer>().material = FeetStepMaterial;

        MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0f);
        Enemy.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        LastEmit = transform.position;
        
        AllowRushIf = true;
        InRushIf = false;
        
        UseEyeSkillNumber = 0;
        AlphaAddSkill = 1 / MaxNumberEyeSkill;//确保在达到最大技能次数时，技能对象的alpha值达到100%

        UseListenSkillNumber = 0;
        
    }

    private void Reset()
    {
        PlayerAlpha = 1;
        PlayerRunSpeed = 5.0f;
        PlayerRushSpeed = 10.0f;
    }


    private void FixedUpdate()
    {
        //玩家不透明度改变
        PlayerSprite.GetComponent<Renderer>().material.color=new Color(1.0f, 1.0f, 1.0f, PlayerAlpha);
        //玩家alpha低于一定值时禁止冲刺

        Move();

        //alpha恢复
        if ((PlayerAlpha<AlphaMaxNumber)&&!Input.GetKey(KeyCode.LeftShift)) PlayerAlpha += AlphaRecoverNumber;
        //脚步显示
        if (FeetShowIf)SetFeetStep();
        if (VoiceShowOne&&VoiceShowIf)
        {
            VoiceSystem.Play();
            EnemyVoiceSystem.Play();
            VoiceShowOne = false;
        }
        else if (!VoiceShowOne && !VoiceShowIf)
        {
            VoiceSystem.Stop();
            EnemyVoiceSystem.Stop();
            VoiceShowOne = true;
        }

        
    }
    // Update is called once per frame
    void Update()
    {
        
        //当不透明度低于允许值时禁止冲刺
        if (PlayerAlpha < RushMinAlpha) AllowRushIf = false; else AllowRushIf = true;
        //《视》技能
        if (AllowEyeSkillIf && Input.GetButtonDown("EyeSkill")) StartCoroutine(EyeSkill());
        //《听》技能
        if (AllowListenSkillIf && Input.GetButtonDown("ListenSkill")) StartCoroutine(ListenSkill());

    }

    void Move()
    {
        moveVer = Input.GetAxisRaw("Vertical");
        moveHor = Input.GetAxisRaw("Horizontal");
        Vector2 MoveInput=(PlayerTransform.right * moveHor + PlayerTransform.up * moveVer).normalized;
        //Input.GetAxisRaw("Vertical")会导致移动时不是平移，缺少缓动
        //Input.GetAxis("Horizontal")会使移动时拥有缓动
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
            
            PlayerRigid.velocity = MoveInput * PlayerRushSpeed;
            // PlayerRushSpeed * Time.deltaTime; * Time.deltaTime;
            // 这个似乎可以防止因不同电脑的性能导致的帧数差别，从而令奔跑速度不一致的问题
            //在vector2()末尾添加的.normalized可以防止同时按下水平和垂直方向键后的速度叠加，令方向键移动时呈现圆周形式

            PlayerCurrentSpeed = PlayerRushSpeed;
            ////PlayerAnim.SetBool("IsRush", true);
        }
        else
        {
            InRushIf = false;
            PlayerRigid.velocity = MoveInput * PlayerRunSpeed;
            PlayerCurrentSpeed = PlayerRunSpeed;
            ////PlayerAnim.SetBool("IsRush", false);
        }

        
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
    /*
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
    */
    private IEnumerator EyeSkill()//使用技能，且制止玩家在技能期间重复使用技能
    {
        UseEyeSkillNumber++;
        AllowEyeSkillIf = false;
        Normal.PlayerTrackingIf = true;
        MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaStartEyeNumber + AlphaAddSkill * UseEyeSkillNumber);
        Enemy.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaStartEyeNumber + AlphaAddSkill * UseEyeSkillNumber);
        yield return new WaitForSeconds(TimeEyeSkill);//技能持续时长
        MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaStartEyeNumber + AlphaAddSkill * (UseEyeSkillNumber-1));
        Enemy.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaStartEyeNumber + AlphaAddSkill * (UseEyeSkillNumber - 1));
        Normal.PlayerTrackingIf = false;
        AllowEyeSkillIf = true;
    }
    

    private IEnumerator ListenSkill()
    {
        UseListenSkillNumber++;
        AllowListenSkillIf = false;

        VoiceShowIf = !VoiceShowIf;
        FeetShowIf = !FeetShowIf;
        Normal.EnemyFeetShowIf = !Normal.EnemyFeetShowIf;
        yield return new WaitForSeconds(TimeListenSkill);//技能持续时长
        AllowListenSkillIf = true;
      
    }
}

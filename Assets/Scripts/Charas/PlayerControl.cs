using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D PlayerRigid;
    ////public Animator PlayerAnim;
    [Header("��������")]
    //public Transform Player;
    //public string PlayerName;
    public GameObject PlayerSprite;//��ҽ�ɫ����
    public float PlayerAlpha;//��Ҳ�͸���ȣ�ͬʱҲ����ҵ�Ѫ��
    public float PlayerRunSpeed;//�ƶ��ٶ�
    private float PlayerCurrentSpeed;//��ǰ�ٶ�
    //public bool AlphaRecoverIf;//��͸�����Ƿ�ָ�
    public float AlphaRecoverNumber;//��͸���Ȼָ���ֵ
    public float AlphaMaxNumber;//��͸���ȿɻָ����������ֵ
    private float moveHor;//ˮƽ�ƶ���Horizontal�Ƿ񱻰���
    private float moveVer;//��ֱ�ƶ���Vertical�Ƿ񱻰���

    [Header("�������")]
    public bool AllowRushIf;//�Ƿ�������
    private bool InRushIf;//�Ƿ��ڳ����
    public float PlayerRushSpeed;//����ٶ�
    public float RushAlphaExpend;//������ĵĲ�͸����
    public float RushMinAlpha;//�����̵���С��͸����ֵ
    [Header("��ӡ����")]
    public bool FeetShowIf;//�Ƿ���ʾ��ӡ
    public ParticleSystem StepSystem;//��ӡ���ӵĶ���
    public Material FeetStepMaterial;//��ӡ���ӵĲ���
    Vector3 LastEmit;//��һ��λ������
    public float delta = 1;
    public float gap = 0.5f;
    public float systemYOffset = -0.225f;
    Vector2 lookDirection = new Vector2(1, 0);

    [Header("������Ч����")]
    public GameObject MapBlock;//��ͼ
    public GameObject Enemy;

    [Header("�������ӻ�����")]
    public bool VoiceShowIf;//�Ƿ���ʾ��ӡ
    public ParticleSystem VoiceSystem;//��ӡ���ӵĶ���
    public Material VoiceMaterial;//��ӡ���ӵĲ���

    [Header("���ӡ����ܲ�������")]
    //���ܡ��ӡ�
    public bool AllowEyeSkillIf;//�Ƿ�����ʹ�ü���
    private bool InEyeSkillIf;//�Ƿ��ڼ�����Чʱ����
    public float EyeAlphaExpend;//���ӡ����ĵ����ﲻ͸����
    public float EyeMinAlpha;//����ʹ�á��ӡ�����С��͸����ֵ

    public float AlphaAddSkill;//�������ӵ�Ŀ�겻͸����ֵ
    public float InfluenceAlpha;//Ŀ��Ĳ�͸����ֵ
    /*
    public int UseEyeSkillNumber;//������ʹ�ô���
    public float MaxNumberEyeSkill;//����ܴ���
    private float NumberEyeSkill;//ʣ�༼�ܴ���
    public float CDeyeSkill;//CDʱ��
    private bool InCDeyeIf;//�Ƿ���cd��
    public float TimeEyeSkill;//����ʱ��
    */


    [Header("���������ܲ�������")]
    //���ܡ�����
    
    public bool AllowListenSkillIf;
    public int UseListenSkillNumber;
    public float MaxNumberListenSkill;
    private float NumberListenSkill;
    public float CDlistenSkill;
    private bool InCDlistenIf;
    public float TimelistenSkill;
    private float ListenAlphaConsumption;//�������ӵ�alphaֵ


    int dir = 1;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRigid.GetComponent<Rigidbody2D>();//�������
        ////PlayerAnim.GetComponent<Animator>();//���ﶯ��
        ///
        LastEmit = transform.position;
        StepSystem.GetComponent<Renderer>().material = FeetStepMaterial;

        AllowRushIf = true;
        InRushIf = false;
        VoiceSystem.Stop();//��ʼ�׶�ֱ�ӹ�ͣ�������ӻ�

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
        if (!Mathf.Approximately(moveHor, 0.0f) || !Mathf.Approximately(moveVer, 0.0f))//ʹ��ӡ������ת�����ƶ�����
        {
            lookDirection.Set(moveHor,moveVer);
            lookDirection.Normalize();
        }
        ////PlayerAnim.SetFloat("Look X", lookDirection.x);
        ////PlayerAnim.SetFloat("Look Y", lookDirection.y);
        ////PlayerAnim.SetFloat("Speed", move.magnitude);

        
        if (AllowRushIf &&Input.GetKey(KeyCode.LeftShift))//���
        {
            InRushIf = true;
            PlayerAlpha -= RushAlphaExpend;
            PlayerRigid.velocity = new Vector2(moveHor * PlayerRushSpeed, moveVer * PlayerRushSpeed);
            // PlayerRushSpeed * Time.deltaTime; * Time.deltaTime;
            // ����ƺ����Է�ֹ��ͬ���Ե����ܵ��µ�֡����𣬴Ӷ�����ٶȲ�һ�µ�����
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
        
        //Input.GetAxisRaw("Vertical")�ᵼ���ƶ�ʱ����ƽ�ƣ�ȱ�ٻ���

    }
    
    void SetFeetStep()//�Ų�
    {
        if (Vector2.Distance(LastEmit, transform.position) > delta)
        {
            Gizmos.color = Color.green;
            var pos = transform.position + new Vector3(lookDirection.y * gap * dir, -lookDirection.x * gap * dir, -5) + new Vector3(0, systemYOffset, 0);
            //������������ֱ��ʽ��x1x2+y1y2=0����lookDirectionת90�ȡ�
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
    private IEnumerator EyeSkill()//ʹ�ü��ܣ�����ֹ����ڼ����ڼ��ظ�ʹ�ü���
    {
        PlayerAlpha -= EyeAlphaExpend;
        NumberEyeSkill--;
        UseEyeSkillNumber++;
        AllowEyeSkill = false;
        InCDeyeIf = true;

        MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaAddSkill * UseEyeSkillNumber);
        yield return new WaitForSeconds(TimeEyeSkill);//���ܳ���ʱ��


        yield return new WaitForSeconds(CDeyeSkill);//cd��ȴ
        InCDeyeIf = false;//CD����
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


        yield return new WaitForSeconds(TimelistenSkill);//���ܳ���ʱ��

        //chararigid.gravityScale = RushGravity;//�����ع��ֵ

        yield return new WaitForSeconds(CDlistenSkill);//cd��ȴ
        InCDlistenIf = false;//CD����
        NumberListenSkill++;
        AllowListenSkillIf = true;
    }
}

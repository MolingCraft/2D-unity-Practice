using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D PlayerRigid;
    //public Animator PlayerAnim;
    [Header("��������")]
    public Transform PlayerTransform;
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
    int dir = 1;
    [Header("������Ч����")]
    public GameObject MapBlock;//��ͼ
    public GameObject Enemy;

    [Header("�������ӻ�����")]
    public bool VoiceShowIf;//�Ƿ�������Ȧ��ʾ
    private bool VoiceShowOne;//�л���Ȧ��ʾʱ��ֹ�����ظ�����
    public ParticleSystem VoiceSystem;//��Ȧ�Ķ���
    public Material VoiceMaterial;//��Ȧ�Ĳ���
    public ParticleSystem EnemyVoiceSystem;//��Ȧ�Ķ���
    public Material EnemyVoiceMaterial;//��Ȧ�Ĳ���

    [Header("���ӡ����ܲ�������")]
    //���ܡ��ӡ�
    public bool AllowEyeSkillIf;//�Ƿ�����ʹ�ü���
    public float AlphaStartEyeNumber;//��ʼ��Чalphaֵ
    public int UseEyeSkillNumber;//������ʹ�ô���
    public float MaxNumberEyeSkill;//����ܴ���
    public float TimeEyeSkill;//����ʱ��
    private float AlphaAddSkill;//�������ӵ�Ŀ�겻͸����ֵ

    [Header("���������ܲ�������")]
    //���ܡ�����

    public bool AllowListenSkillIf;//�Ƿ�����ʹ�ü���
    public int UseListenSkillNumber;//������ʹ�ô���
    public float MaxNumberListenSkill;//����ܴ���
    public float TimeListenSkill;//����ʱ��


    private void Awake()
    {
        PlayerRigid.GetComponent<Rigidbody2D>();//�������
        //PlayerAnim.GetComponent<Animator>();//���ﶯ��

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
        AlphaAddSkill = 1 / MaxNumberEyeSkill;//ȷ���ڴﵽ����ܴ���ʱ�����ܶ����alphaֵ�ﵽ100%

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
        //��Ҳ�͸���ȸı�
        PlayerSprite.GetComponent<Renderer>().material.color=new Color(1.0f, 1.0f, 1.0f, PlayerAlpha);
        //���alpha����һ��ֵʱ��ֹ���

        Move();

        //alpha�ָ�
        if ((PlayerAlpha<AlphaMaxNumber)&&!Input.GetKey(KeyCode.LeftShift)) PlayerAlpha += AlphaRecoverNumber;
        //�Ų���ʾ
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
        
        //����͸���ȵ�������ֵʱ��ֹ���
        if (PlayerAlpha < RushMinAlpha) AllowRushIf = false; else AllowRushIf = true;
        //���ӡ�����
        if (AllowEyeSkillIf && Input.GetButtonDown("EyeSkill")) StartCoroutine(EyeSkill());
        //����������
        if (AllowListenSkillIf && Input.GetButtonDown("ListenSkill")) StartCoroutine(ListenSkill());

    }

    void Move()
    {
        moveVer = Input.GetAxisRaw("Vertical");
        moveHor = Input.GetAxisRaw("Horizontal");
        Vector2 MoveInput=(PlayerTransform.right * moveHor + PlayerTransform.up * moveVer).normalized;
        //Input.GetAxisRaw("Vertical")�ᵼ���ƶ�ʱ����ƽ�ƣ�ȱ�ٻ���
        //Input.GetAxis("Horizontal")��ʹ�ƶ�ʱӵ�л���
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
            
            PlayerRigid.velocity = MoveInput * PlayerRushSpeed;
            // PlayerRushSpeed * Time.deltaTime; * Time.deltaTime;
            // ����ƺ����Է�ֹ��ͬ���Ե����ܵ��µ�֡����𣬴Ӷ�����ٶȲ�һ�µ�����
            //��vector2()ĩβ��ӵ�.normalized���Է�ֹͬʱ����ˮƽ�ʹ�ֱ���������ٶȵ��ӣ������ƶ�ʱ����Բ����ʽ

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
    private IEnumerator EyeSkill()//ʹ�ü��ܣ�����ֹ����ڼ����ڼ��ظ�ʹ�ü���
    {
        UseEyeSkillNumber++;
        AllowEyeSkillIf = false;
        Normal.PlayerTrackingIf = true;
        MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaStartEyeNumber + AlphaAddSkill * UseEyeSkillNumber);
        Enemy.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaStartEyeNumber + AlphaAddSkill * UseEyeSkillNumber);
        yield return new WaitForSeconds(TimeEyeSkill);//���ܳ���ʱ��
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
        yield return new WaitForSeconds(TimeListenSkill);//���ܳ���ʱ��
        AllowListenSkillIf = true;
      
    }
}

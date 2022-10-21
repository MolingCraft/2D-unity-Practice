using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : MonoBehaviour
{
    Rigidbody2D EnemyRigid;
    Animator EnemyAnim;

    //public float AlphaEnemy;

    [Header("��������")]
    public float EnemySpeed;//�����ƶ��ٶ�

    [Header("׷������")]
    public bool PlayerTrackingIf;//�Ƿ�׷�����
    public float Radius;//׷�ٰ뾶
    public float interval;//����׷��ʱ����ҵļ��
    
    private Transform PlayerTransform;

    //��ʼλ�ã�����enemy�Ļع�
    public bool BackStartIf;//�Ƿ񷵻س�ʼλ��
    private float XStartDistance;
    private float YStartDistance;

    [Header("�ε�����")]
    public bool RandomWanderingIf;//�Ƿ�����ε�
    public float XRandomNumber;
    public float YRandomNumber;
    [Header("�ε���������")]
    /*
    public bool RandomWander;//��ȫ���
    [Header("Ŀ���յ����꣨x��y��")]
    public bool Line;//��ֱ���ε�
    [Header("Բ�����꣨x��y�����뾶R")]
    public bool Round;//Բ��
    [Header("�ײ���1��0�����᳤W���ݳ�H")]
    public bool Rectangle;//������
    [Header("�ε�������������ֿ�����")]
    public float Number1;
    public float Number2;
    public float Number3;
    public float Number4;
    */
    [Header("��ӡ����")]
    public bool FeetShowIf;//�Ƿ���ʾ��ӡ
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
        EnemyRigid = GetComponent<Rigidbody2D>();//����
        EnemyAnim = GetComponent<Animator>();//����
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
            stepSystem.Emit(ep, 1);
            lastEmit = transform.position;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [Header("������Ч����")]
    public GameObject MapBlock;
    public GameObject Enemy;

    
    [Header("���ӡ����ܲ�������")]
    //���ܡ��ӡ�
    public bool AllowEyeSkill;//�Ƿ�����ʹ�ü���
    public int UseEyeSkillNumber;//����ʹ�ô���
    public float MaxNumberEyeSkill;//����ܴ���
    private float NumberEyeSkill;//ʣ�༼�ܴ���
    public float CDeyeSkill;//CDʱ��
    private bool InCDeyeIf;//�Ƿ���cd��
    public float TimeEyeSkill;//����ʱ��
    private float AlphaAddSkill;//�������ӵ�alphaֵ
    [Header("���������ܲ�������")]
    //���ܡ�����
    public bool AllowListenSkill;
    public int UseListenSkillNumber;
    public float MaxNumberListenSkill;
    private float NumberListenSkill;
    public float CDlistenSkill;
    private bool InCDlistenIf;//�Ƿ���cd��
    public float TimelistenSkill;

    // Start is called before the first frame update
    void Start()
    {
        NumberEyeSkill = MaxNumberEyeSkill;
        NumberListenSkill = MaxNumberListenSkill;
        UseEyeSkillNumber = 0;
        UseListenSkillNumber = 0;
        AlphaAddSkill = 1 / MaxNumberEyeSkill;
        MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0f);
        
    }

    // Update is called once per frame
    void Update()
    {

        if (AllowEyeSkill && Input.GetButtonDown("EyeSkill"))
        {
            StartCoroutine(EyeSkill());
        }
        if (AllowListenSkill && Input.GetButtonDown("ListenSkill"))
        {
            StartCoroutine(ListenSkill());
        }
    }
    
    private IEnumerator EyeSkill()//ʹ�ü��ܣ�����ֹ����ڼ����ڼ��ظ�ʹ�ü���
    {
        NumberEyeSkill--;
        UseEyeSkillNumber++;
        AllowEyeSkill= false;
        InCDeyeIf = true;
            //float RushGravity = chararigid.gravityScale;//��ȡ��������ֵ
            //chararigid.gravityScale = 0f;//���ʱ��������0
            //chararigid.velocity = new Vector2(transform.localScale.x * RushSpeed * movedir, 0f);//��̼��ܵ�ʵ��
       
        MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaAddSkill*UseEyeSkillNumber);
        
        yield return new WaitForSeconds(TimeEyeSkill);//���ܳ���ʱ��

            //chararigid.gravityScale = RushGravity;//�����ع��ֵ

        yield return new WaitForSeconds(CDeyeSkill);//cd��ȴ
        InCDeyeIf = false;//��̽���
        NumberEyeSkill++;
        AllowEyeSkill = true;
    }


    private IEnumerator ListenSkill()
    {
        NumberListenSkill--;
        UseListenSkillNumber++;
        AllowListenSkill = false;
        InCDlistenIf = true;
        

        yield return new WaitForSeconds(TimelistenSkill);//���ܳ���ʱ��

        //chararigid.gravityScale = RushGravity;//�����ع��ֵ

        yield return new WaitForSeconds(CDlistenSkill);//cd��ȴ
        InCDlistenIf = false;//��̽���
        NumberListenSkill++;
        AllowListenSkill = true;
    }
}

    
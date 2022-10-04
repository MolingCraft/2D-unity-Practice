using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [Header("技能生效对象")]
    public GameObject MapBlock;
    public GameObject Enemy;

    
    [Header("“视”技能参数设置")]
    //技能《视》
    public bool AllowEyeSkill;//是否允许使用技能
    public int UseEyeSkillNumber;//技能使用次数
    public float MaxNumberEyeSkill;//最大技能次数
    private float NumberEyeSkill;//剩余技能次数
    public float CDeyeSkill;//CD时间
    private bool InCDeyeIf;//是否在cd中
    public float TimeEyeSkill;//持续时间
    private float AlphaAddSkill;//技能增加的alpha值
    [Header("“听”技能参数设置")]
    //技能《听》
    public bool AllowListenSkill;
    public int UseListenSkillNumber;
    public float MaxNumberListenSkill;
    private float NumberListenSkill;
    public float CDlistenSkill;
    private bool InCDlistenIf;//是否在cd中
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
    
    private IEnumerator EyeSkill()//使用技能，且制止玩家在技能期间重复使用技能
    {
        NumberEyeSkill--;
        UseEyeSkillNumber++;
        AllowEyeSkill= false;
        InCDeyeIf = true;
            //float RushGravity = chararigid.gravityScale;//获取刚体重力值
            //chararigid.gravityScale = 0f;//冲刺时重力等于0
            //chararigid.velocity = new Vector2(transform.localScale.x * RushSpeed * movedir, 0f);//冲刺技能的实现
       
        MapBlock.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaAddSkill*UseEyeSkillNumber);
        
        yield return new WaitForSeconds(TimeEyeSkill);//技能持续时长

            //chararigid.gravityScale = RushGravity;//重力回归初值

        yield return new WaitForSeconds(CDeyeSkill);//cd冷却
        InCDeyeIf = false;//冲刺结束
        NumberEyeSkill++;
        AllowEyeSkill = true;
    }


    private IEnumerator ListenSkill()
    {
        NumberListenSkill--;
        UseListenSkillNumber++;
        AllowListenSkill = false;
        InCDlistenIf = true;
        

        yield return new WaitForSeconds(TimelistenSkill);//技能持续时长

        //chararigid.gravityScale = RushGravity;//重力回归初值

        yield return new WaitForSeconds(CDlistenSkill);//cd冷却
        InCDlistenIf = false;//冲刺结束
        NumberListenSkill++;
        AllowListenSkill = true;
    }
}

    
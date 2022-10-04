using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : MonoBehaviour
{
    Rigidbody2D EnemyRigid;
    Animator EnemyAnim;

    public float AlphaEnemy;

    public bool PlayerTrackingIf;//是否追踪玩家
    public float Radius;//追踪半径
    public float interval;//怪物追踪时与玩家的间距
    public float EnemySpeed;//追踪时的速度
    private Transform PlayerTransform;

    public float XMaxTracking;//x轴最大追踪距离
    public float YMaxTracking;//y轴最大追踪距离
    //初始位置，用于enemy的回归
    private float XStartDistance;
    private float YStartDistance;

    void Start()
    {
        EnemyRigid = GetComponent<Rigidbody2D>();//人物刚体
        EnemyAnim = GetComponent<Animator>();//人物动画
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        XStartDistance = transform.position.x;
        YStartDistance = transform.position.y;

        AlphaEnemy = 0;
        //transform.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, AlphaEnemy);


    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (AlphaEnemy==0)
        {
            PlayerTrackingIf = false;
        }
        else
        {
            PlayerTrackingIf = true;
        }
        */
        TrackingPlayer();
        //GoBackStartPosition();
    }

    void TrackingPlayer()
    {
        if (PlayerTransform != null && PlayerTrackingIf)
        {
            float distance = (transform.position - PlayerTransform.position).sqrMagnitude;
            float XDistance = PlayerTransform.position.x - transform.position.x;
            float YDistance = PlayerTransform.position.y - transform.position.y;
            if (distance < Radius && distance > interval)
            {
                EnemyRigid.velocity = new Vector2(XDistance * EnemySpeed, YDistance * EnemySpeed);
                Prefs.storynumber = 1000;
                /*
                EnemyRigid.velocity=Vector2.MoveTowards(transform.position,
                                                        PlayerTransform.position,
                                                        EnemySpeed * Time.deltaTime);
                /*
                transform.position = Vector2.MoveTowards(transform.position,
                                                        PlayerTransform.position,
                                                        EnemySpeed * Time.deltaTime);
                */
            }
            else if(distance>Radius)
            {
                transform.position =new Vector2(XStartDistance,YStartDistance);
                //EnemyRigid.position= new Vector2(XDistance, YDistance);
            }
        }
    }
    /*void GoBackStartPosition()//返回起始位置
    {
        float XXDistance= XStartDistance - transform.position.x;
        float YYDistance =YStartDistance - transform.position.y;
        if (XXDistance > XMaxTracking || YYDistance>YMaxTracking)
        {
            while (XStartDistance!= transform.position.x || YStartDistance!=transform.position.y)
            {
                EnemyRigid.velocity = new Vector2(XXDistance * EnemySpeed, YYDistance * EnemySpeed);
                XXDistance = XStartDistance - transform.position.x;
                YYDistance = YStartDistance - transform.position.y;
            }
            

        }
    
    }
    */
}

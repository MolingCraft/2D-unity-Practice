using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D PlayerRigid;
    Animator PlayerAnim;
    [Header("基础设置")]
    //public Transform Player;
    //public string PlayerName;
    public float PlayerRunSpeed;

    private float moveHor;//水平移动键Horizontal是否被按下
    private float moveVer;//垂直移动键Vertical是否被按下

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigid = GetComponent<Rigidbody2D>();//人物刚体
        PlayerAnim = GetComponent<Animator>();//人物动画
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    void Move()
    {
        moveVer = Input.GetAxis("Vertical");
        moveHor = Input.GetAxis("Horizontal");
        //Input.GetAxisRaw("Vertical")会导致移动时不是平移，缺少缓动
        PlayerRigid.velocity = new Vector2(moveHor * PlayerRunSpeed, moveVer * PlayerRunSpeed);
        //PlayerRigid.velocity = new Vector2(moveHor * PlayerRunSpeed, PlayerRigid.velocity.y);

    }
    
    

}

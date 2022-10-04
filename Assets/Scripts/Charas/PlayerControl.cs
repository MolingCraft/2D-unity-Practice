using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D PlayerRigid;
    Animator PlayerAnim;
    [Header("��������")]
    //public Transform Player;
    //public string PlayerName;
    public float PlayerRunSpeed;

    private float moveHor;//ˮƽ�ƶ���Horizontal�Ƿ񱻰���
    private float moveVer;//��ֱ�ƶ���Vertical�Ƿ񱻰���

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigid = GetComponent<Rigidbody2D>();//�������
        PlayerAnim = GetComponent<Animator>();//���ﶯ��
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
        //Input.GetAxisRaw("Vertical")�ᵼ���ƶ�ʱ����ƽ�ƣ�ȱ�ٻ���
        PlayerRigid.velocity = new Vector2(moveHor * PlayerRunSpeed, moveVer * PlayerRunSpeed);
        //PlayerRigid.velocity = new Vector2(moveHor * PlayerRunSpeed, PlayerRigid.velocity.y);

    }
    
    

}

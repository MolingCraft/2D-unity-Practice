using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//在2.5D视角中直接挂在本脚本，相机会跟随被挂载物体移动
public class CameraFollowPlayer : MonoBehaviour
{
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = Camera.main.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = transform.position + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��2.5D�ӽ���ֱ�ӹ��ڱ��ű����������汻���������ƶ�
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    Transform[] Children;//遍历存储所有子物体

    // Start is called before the first frame update
    void Start()
    {
        Children = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            Children[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Children.Length; i++)
        {
            Children[i].rotation = Camera.main.transform.rotation;
        }
    }
}

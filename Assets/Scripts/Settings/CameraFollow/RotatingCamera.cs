using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCamera : MonoBehaviour
{
    public float RotateTime = 0.2f;

    private Transform PlayerTransform;
    private bool isRotating;//防止多次旋转引起冲突


    private void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerTransform.position;
        Rotate();
    }

    void Rotate()
    {
        float LookRotate=Input.GetAxis("LookRotate");
        if (Input.GetButtonDown("LookRotate") && !isRotating)
        {
           
            StartCoroutine(RotateAround(LookRotate * 45, RotateTime));
        }
    }

    IEnumerator RotateAround(float angel,float time)
    {
        float number = 50 * time;
        float nextAngel = angel / number;
        isRotating = true;

        for(int i = 0; i < number; i++)
        {
            transform.Rotate(new Vector3(0, 0, nextAngel));
            yield return new WaitForFixedUpdate();
        }

        isRotating = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs : MonoBehaviour
{
    //public string playername;
    public static int storynumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveawa()
    {
        SaveByPlayerPrefs();
    }
    public void Loadawa()
    {
        LoadFromPlayerPrefs();
        Debug.Log(storynumber);
    }

    void SaveByPlayerPrefs()
    {
        PlayerPrefs.SetInt("storynumber",storynumber);
        PlayerPrefs.Save();
    }

    void LoadFromPlayerPrefs()
    {
        storynumber = PlayerPrefs.GetInt("storynumber",1);
        //�����ע�� GetInt("PlayerName","�������ڲ���ֵʱ������ʾ���Ĭ��ֵ");
    }


    public void PlayGame()
    {
        //SceneManager.LoadScene(1);//���볡��1
    }
    public void QuitGame()
    {
        Application.Quit();//�˳���Ϸ
    }

    /*
    public void Menushow()
    {
        showMenuIf = !showMenuIf;
        gameObject.SetActive(showMenuIf);//�л��˵���ʾ״̬
        if (showMenuIf) { Time.timeScale = 0f; } else { Time.timeScale = 1f; }//��Ϸ��ͣ
    }
    */

}

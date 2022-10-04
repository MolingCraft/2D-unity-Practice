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
        //这段是注释 GetInt("PlayerName","当不存在参数值时，会显示这个默认值");
    }


    public void PlayGame()
    {
        //SceneManager.LoadScene(1);//载入场景1
    }
    public void QuitGame()
    {
        Application.Quit();//退出游戏
    }

    /*
    public void Menushow()
    {
        showMenuIf = !showMenuIf;
        gameObject.SetActive(showMenuIf);//切换菜单显示状态
        if (showMenuIf) { Time.timeScale = 0f; } else { Time.timeScale = 1f; }//游戏暂停
    }
    */

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject theMenu;
    public bool showMenuIf;

    


    // Start is called before the first frame update
    void Start()
    {
        showMenuIf = false;
        theMenu.SetActive(showMenuIf);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Esc"))
        {
            showMenuIf = !showMenuIf;
            theMenu.SetActive(showMenuIf);//«–ªª≤Àµ•œ‘ æ◊¥Ã¨
            if (showMenuIf) { Time.timeScale = 0f; } else { Time.timeScale = 1f; }//”Œœ∑‘›Õ£
        }
    }

    
}

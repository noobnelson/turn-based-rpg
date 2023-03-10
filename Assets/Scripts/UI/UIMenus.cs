using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenus : MonoBehaviour
{
    [SerializeField]
    private Canvas pauseMenu;

    public void PauseMenu()
    {
        if (pauseMenu.gameObject.activeInHierarchy)
        {
            ButtonResume();
        }
        else
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ButtonResume()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}

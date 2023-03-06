using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private UIMenus menus;

    void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        menus = FindObjectOfType<UIMenus>();
    }

    void Update()
    {
        if (playerInput.PauseKey)
        {
            menus.PauseMenu();
        }
    }
}

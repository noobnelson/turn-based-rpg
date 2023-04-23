using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private UIMenus uiMenus;
    private UIGameplay uiGameplay;

    void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        uiMenus = FindObjectOfType<UIMenus>();
        uiGameplay = FindObjectOfType<UIGameplay>();
    }

    void Update()
    {
        if (playerInput.PauseKey)
        {
            uiMenus.PauseMenu();
        }
    }

    public void UpdateActions(List<Action> actions)
    {
        uiGameplay.UpdateActions(actions);
    }
    
    public void UpdateInfoPanelPlayer(Entity entity)
    {
        uiGameplay.UpdateInfoPanelPlayer(entity);
    }

    public void UpdateInfoPanelOther(Entity entity)
    {
        uiGameplay.UpdateInfoPanelOther(entity);
    }

    public void ToggleInfoPanelOther(bool b)
    {
        uiGameplay.ToggleInfoPanelOther(b);
    }
}

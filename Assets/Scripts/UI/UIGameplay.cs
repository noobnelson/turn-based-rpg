using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private GameObject infoPlayerPanel;
    [SerializeField]
    private GameObject infoOtherPanel;
    [SerializeField]
    private Text textNamePlayer;
    [SerializeField]
    private Text textNameOther;
    [SerializeField]
    private Text textHealthPlayer;
    [SerializeField]
    private Text textHealthOther;
    [SerializeField]
    private Text textMovementPlayer;
    [SerializeField]
    private Text textMovementOther;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();    
    }

    public void ButtonEndTurn()
    {
        if (gameManager.gameState == GameManager.CurrentGameState.PlayerInput)
        {
            gameManager.gameState = GameManager.CurrentGameState.TurnEnd;
        }
    }

    public void UpdateInfoPanelPlayer(Entity entity)
    {
        UpdateInfoPanel(entity, textNamePlayer, textHealthPlayer, textMovementPlayer);
    }

    public void UpdateInfoPanelOther(Entity entity)
    {
        UpdateInfoPanel(entity, textNameOther, textHealthOther, textMovementOther);
    }

    private void UpdateInfoPanel(Entity entity, Text name, Text health, Text movement)
    {
        int endOfNameIndex = entity.gameObject.name.IndexOf("(");
        name.text = entity.gameObject.name.Substring(0, endOfNameIndex);
        health.text = "Health: " + entity.currentHealthPoints + "/" + entity.healthPoints;
        movement.text = "Movement: " + entity.currentMovementPoints + "/" + entity.movementPoints;
    }

    public void ToggleInfoPanelOther(bool b)
    {
        infoOtherPanel.SetActive(b);
    }
}

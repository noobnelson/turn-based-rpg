using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private List<Button> actionButtons = new List<Button>();
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
    [SerializeField]
    private Text textActionPlayer;
    [SerializeField]
    private Text textActionOther;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void UpdateActions(List<Action> actions)
    {
        for (int i = 0; i < actionButtons.Count; i++)
        {
            int x = i;
            actionButtons[i].GetComponentInChildren<Text>().text = actions[i].name;
            actionButtons[i].onClick.AddListener(() => UpdateCurrentAction(actions[x]));
        }
    }

    public void UpdateCurrentAction(Action action)
    {
        // Clicking the same action that is currently selected = go back to moving player
        if (gameManager.currentAction == action && gameManager.gameState == GameManager.CurrentGameState.ActionInput)
        {
            gameManager.currentAction = null;
            gameManager.gameState = GameManager.CurrentGameState.TurnStart;
        }
        // Click new action to perform
        else if (gameManager.currentAction != action &&
            (gameManager.gameState == GameManager.CurrentGameState.MoveInput || gameManager.gameState == GameManager.CurrentGameState.ActionInput))
        {
            gameManager.currentAction = action;
            gameManager.gameState = GameManager.CurrentGameState.ActionStart;
        }
    }

    public void ButtonEndTurn()
    {
        if (gameManager.gameState == GameManager.CurrentGameState.MoveInput || gameManager.gameState == GameManager.CurrentGameState.ActionInput)
        {
            gameManager.gameState = GameManager.CurrentGameState.TurnEnd;
        }
    }

    private void UpdateInfoPanel(Entity entity, Text name, Text health, Text movement, Text action)
    {
        int endOfNameIndex = entity.gameObject.name.IndexOf("(");
        name.text = entity.gameObject.name.Substring(0, endOfNameIndex);
        health.text = "Health: " + entity.currentHealthPoints + "/" + entity.healthPoints;
        movement.text = "Movement: " + entity.currentMovementPoints + "/" + entity.movementPoints;
        action.text = "Action Points: " + entity.currentActionPoints + "/" + entity.actionPoints;
    }

    public void UpdateInfoPanelPlayer(Entity entity)
    {
        UpdateInfoPanel(entity, textNamePlayer, textHealthPlayer, textMovementPlayer, textActionPlayer);
    }

    public void UpdateInfoPanelOther(Entity entity)
    {
        UpdateInfoPanel(entity, textNameOther, textHealthOther, textMovementOther, textActionOther);
    }

    public void ToggleInfoPanelOther(bool b)
    {
        infoOtherPanel.SetActive(b);
    }
}

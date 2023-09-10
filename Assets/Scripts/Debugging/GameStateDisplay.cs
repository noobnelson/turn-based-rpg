using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateDisplay : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private GameObject gameStateDisplay;
    [SerializeField]
    private Text displayText;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        gameStateDisplay.SetActive(true);
    }

    void Update()
    {
        // Entity currentEntity = gameManager.CurrentEntitySelected;
        // int endOfNameIndex = currentEntity.gameObject.name.IndexOf("(");

        displayText.text = 
            "Entity Turn: " + gameManager.CurrentEntitySelected.gameObject.name
            + "\nGame State: " + gameManager.gameState
            + "\nAction: " + gameManager.currentAction;
            
    }

}

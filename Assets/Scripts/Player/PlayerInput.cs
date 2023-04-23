using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MousePos { get; private set; }
    public bool MouseClick { get; private set; }
    public bool PauseKey { get; private set; }

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    void Start()
    {
        keys.Add("Pause", KeyCode.Escape);
        keys.Add("PrimaryMouse", KeyCode.Mouse0);
    }

    void Update()
    {
        MousePos = Input.mousePosition;
        MouseClick = Input.GetKeyDown(keys["PrimaryMouse"]);
        PauseKey = Input.GetKeyDown(keys["Pause"]);
        
    }
}

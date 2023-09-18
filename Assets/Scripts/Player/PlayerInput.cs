using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MousePos { get; private set; }
    public bool MouseClickLeft { get; private set; }
    public bool MouseClickRight { get; private set; }
    public bool PauseKey { get; private set; }

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    void Start()
    {
        keys.Add("Pause", KeyCode.Escape);
        keys.Add("PrimaryMouse", KeyCode.Mouse0);
        keys.Add("SecondaryMouse", KeyCode.Mouse1);
    }

    void Update()
    {
        MousePos = Input.mousePosition;
        MouseClickLeft = Input.GetKeyDown(keys["PrimaryMouse"]);
        MouseClickRight = Input.GetKeyDown(keys["SecondaryMouse"]);
        PauseKey = Input.GetKeyDown(keys["Pause"]);
    }
}

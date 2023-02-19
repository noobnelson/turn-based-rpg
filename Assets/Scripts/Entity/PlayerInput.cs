using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MousePos { get; private set; }

    void Update()
    {
        MousePos = Input.mousePosition;
    }
}

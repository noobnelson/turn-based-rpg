using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellWithMaterialPropertyBlock : MonoBehaviour
{
    public Renderer _renderer;
    public MaterialPropertyBlock _propBlock;

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }
}

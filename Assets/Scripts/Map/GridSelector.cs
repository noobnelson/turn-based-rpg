using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSelector : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;
    private int layerMask;
    private Block currentHighlightedBlock;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
        layerMask = 1 << 6;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(playerInput.MousePos);

        // RaycastHit hit;
        // if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        // {
        //     Block block = hit.collider.GetComponentInParent<Block>();
        //     if (!currentHighlightedBlock)
        //     {
        //         currentHighlightedBlock = block;
        //         block.HighlightCellAvailable(true);
        //     }
        //     else if (block != currentHighlightedBlock)
        //     {
        //         currentHighlightedBlock.HighlightCellAvailable(false);
        //         block.HighlightCellAvailable(true);
        //         currentHighlightedBlock = block;
        //     }
        // }
        // else
        // {
        //     if (currentHighlightedBlock)
        //     {
        //         currentHighlightedBlock.HighlightCellAvailable(false);
        //     }
        //     currentHighlightedBlock = null;
        // }
    }
}

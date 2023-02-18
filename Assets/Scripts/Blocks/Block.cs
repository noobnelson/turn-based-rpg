using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //private int cost = 1;
    private Transform cellAvailable;
    private Transform cellUnvailable;

    void Start()
    {
        cellAvailable = transform.GetChild(0);
        cellUnvailable = transform.GetChild(1);
    }

    public void HighlightCellAvailable(bool b)
    {
        cellAvailable.gameObject.SetActive(b);
    }

    public void HighlightCellUnavailable(bool b)
    {
        cellUnvailable.gameObject.SetActive(b);
    }
}

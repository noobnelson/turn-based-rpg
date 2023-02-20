using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class FileManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset mapFile;
    private string mapFilePath;
    public string[] FileText { get; private set; }

    void Awake()
    {
        mapFilePath = AssetDatabase.GetAssetPath(mapFile);
        FileText = File.ReadAllLines(mapFilePath);
    }
}

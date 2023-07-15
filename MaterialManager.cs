using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public Material[] allMaterials; 

    private Dictionary<string, Material> materialDict;

    private void Awake()
    {
        materialDict = new Dictionary<string, Material>();
        foreach (var material in allMaterials)
        {
            materialDict.Add(material.name, material);
        }
    }

    public Material GetMaterialByName(string name)
    {
        if (materialDict.TryGetValue(name, out var material))
        {
            return material;
        }
        else
        {
            return null;
        }
    }
}
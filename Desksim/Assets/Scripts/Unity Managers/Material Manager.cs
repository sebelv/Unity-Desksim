using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance;
    public List<Material> materials = new List<Material>();

    void Awake()
    {
        Instance = this;
    }

    public Material GetMaterial(string name)
    {
        for(int i = 0; i < materials.Count; i++)
        {
            if(materials[i].name == name)
            {
                return materials[i];
            }
        }
        return materials[0];
    }
}

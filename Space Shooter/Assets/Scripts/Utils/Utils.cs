using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    static public Material[] GetAllMaterials(GameObject go)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();

        List<Material> mats = new();
        foreach (Renderer rend in rends)
        {
            mats.Add(rend.material);
        }

        return mats.ToArray();
    }
}

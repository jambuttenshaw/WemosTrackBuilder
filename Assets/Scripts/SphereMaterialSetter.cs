using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMaterialSetter : MonoBehaviour
{
    public Material defaultMaterial;
    public Material selectedMaterial;

    public void SetMaterial(bool s)
    {
        if (s)
            GetComponent<MeshRenderer>().material = selectedMaterial;
        else
            GetComponent<MeshRenderer>().material = defaultMaterial;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMaterialSetter : MonoBehaviour
{
    public Material ring1Material;
    public Material ring2Material;
    public Material selectedMaterial;

    public enum SphereMaterial
    {
        Ring1,
        Ring2,
        Selected
    }

    public void SetMaterial(SphereMaterial material)
    {
        switch(material)
        {
        case SphereMaterial.Ring1:          GetComponent<MeshRenderer>().material = ring1Material; break;
        case SphereMaterial.Ring2:          GetComponent<MeshRenderer>().material = ring2Material; break;
        case SphereMaterial.Selected:       GetComponent<MeshRenderer>().material = selectedMaterial; break;
        }
    }
}

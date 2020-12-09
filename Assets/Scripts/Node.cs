using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{

    int nodeIndex;
    List<int> edgeConnections;
    List<int> areaConnections;

    bool selected;
    public bool ring1 = false;
    public bool ring2 = false;

    public SphereMaterialSetter materialSetter;

    private void Update()
    {
        if (selected)
        {
            materialSetter.SetMaterial(SphereMaterialSetter.SphereMaterial.Selected);
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            }
        } 
        else if (ring1)
        {
            materialSetter.SetMaterial(SphereMaterialSetter.SphereMaterial.Ring1);
        }
        else if (ring2)
        {
            materialSetter.SetMaterial(SphereMaterialSetter.SphereMaterial.Ring2);
        }
        else
        {
            materialSetter.SetMaterial(SphereMaterialSetter.SphereMaterial.Default);
        }
    }

    public void Setup(int index, SphereMaterialSetter matSetter)
    {
        edgeConnections = new List<int>();
        areaConnections = new List<int>();

        nodeIndex = index;

        materialSetter = matSetter;
    }

    public void ConnectNodeToEdge(int nodeToConnect)
    {
        edgeConnections.Add(nodeToConnect);
        if (edgeConnections.Count > 1)
            Debug.LogError("More than one connection to node; track is invalid.");
    }   

    public void DeleteConnection(int nodeIndex)
    {
        edgeConnections.Remove(nodeIndex);
    }

    public int GetIndex()
    {
        return nodeIndex;
    }

    public List<int> GetEdgeConnections()
    {
        return edgeConnections;
    }

    public List<int> GetAreaConnections()
    {
        return areaConnections;
    }

    public void SetSelected(bool s)
    {
        selected = s;
    }
}

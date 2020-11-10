using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Node : MonoBehaviour
{

    int nodeIndex;
    List<int> edgeConnections;
    List<int> areaConnections;

    bool selected;

    public GameObject visualSphere;

    private void Update()
    {
        if (selected)
        {
            if (Input.GetMouseButton(0))
            {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            }
        }
    }

    public void Setup(int index)
    {
        edgeConnections = new List<int>();
        areaConnections = new List<int>();

        nodeIndex = index;
    }

    public void ConnectNodeToEdge(int nodeToConnect)
    {
        edgeConnections.Add(nodeToConnect);
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

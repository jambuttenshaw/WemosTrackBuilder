using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData
{

    public int nodeIndex;
    public int[] edgeConnections;
    public int[] areaConnections;
    public float[] position;

    public NodeData(Node node)
    {
        nodeIndex = node.GetIndex();
        edgeConnections = node.GetEdgeConnections().ToArray();
        areaConnections = node.GetAreaConnections().ToArray();
        
        position = new float[3];
        position[0] = node.transform.position.x;
        position[1] = node.transform.position.y;
        position[2] = node.transform.position.z;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTriangulator
{

    private class NodeData
    {

        public int index;
        public Vector3 worldPos;
        public float triCoord;

        public NodeData(int index, Vector3 worldPos, float triangulationCoordinate)
        {
            this.index = index;
            this.worldPos = worldPos;
            this.triCoord = triangulationCoordinate;
        }

        public void SetTriangulationCoordinate(float tC)
        {
            triCoord = tC;
        }
    }


    public static int[] Triangulate(List<Node> nodes, Node ring1Node, Node ring2Node)
    {

        // get all of the nodes that are in one ring
        List<NodeData> ring1 = CreateRing(nodes, ring1Node);
        List<NodeData> ring2 = CreateRing(nodes, ring2Node);

        // put each ring onto a scale from 0 to 1
        SetTriangulationCoordinates(ref ring1);
        SetTriangulationCoordinates(ref ring2);

        List<NodeData> bigRing = ring1.Count > ring2.Count ? ring1 : ring2;
        List<NodeData> smallRing = ring1.Count <= ring2.Count ? ring1 : ring2;
        
        List<int> indices = new List<int>();

        int bigRingIndex = 1;
        for (int n = 0; n < smallRing.Count - 1; n++)
        {
            while (bigRing[bigRingIndex].triCoord < smallRing[n + 1].triCoord)
            {
                // create a triangle
                indices.Add(smallRing[n].index);
                indices.Add(bigRing[bigRingIndex].index);
                indices.Add(bigRing[bigRingIndex - 1].index);

                bigRingIndex++;
            }
            
            // create the triangle on the left half of the strip
            indices.Add(smallRing[n].index);
            indices.Add(smallRing[n + 1].index);
            indices.Add(bigRing[bigRingIndex - 1].index);
            
        }
        indices.Add(smallRing[smallRing.Count - 1].index);
        indices.Add(bigRing[bigRing.Count - 1].index);
        indices.Add(bigRing[bigRing.Count - 2].index);

        return indices.ToArray();
    }

    private static List<NodeData> CreateRing(List<Node> nodes, Node start)
    {
        List<NodeData> ring = new List<NodeData>();
        ring.Add(new NodeData(start.GetIndex(), start.transform.position, 0.0f));

        // follow the ring round until you get back to the start node
        // adding each node on the way to the list
        int nextNodeIndex = -1;
        Node currentNode = start;

        while (nextNodeIndex != start.GetIndex())
        {
            if (start.GetEdgeConnections().Count > 1)
            {
                Debug.LogError("More than one connection to node, track is invalid");
                return null;
            }

            nextNodeIndex = currentNode.GetEdgeConnections()[0];
            currentNode = nodes[nextNodeIndex];

            ring.Add(new NodeData(currentNode.GetIndex(), currentNode.transform.position, 0.0f));
        }

        return ring;
    }

    private static void SetTriangulationCoordinates(ref List<NodeData> nodes)
    {
        float perimeter = 0;
        Vector3 lastVertex = nodes[0].worldPos;
        
        for (int n = 1; n < nodes.Count; n++)
        {
            perimeter += (nodes[n].worldPos - lastVertex).magnitude;
            lastVertex = nodes[n].worldPos;

            nodes[n].SetTriangulationCoordinate(perimeter);
        }

        foreach (NodeData node in nodes)
        {
            node.SetTriangulationCoordinate(node.triCoord / perimeter);
        }
    }
}

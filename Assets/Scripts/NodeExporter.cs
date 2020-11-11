using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class NodeExporter
{

    public static string TriangleIndicesToString(ref int[] indices)
    {
        string indicesString = "";

        for (int i = 0; i < indices.Length; i++)
        {
            indicesString += "m_TrackAreaIndices[" + i + "] = " + indices[i] + ";\n";
        }

        return indicesString;
    }

    public static string NodeNetworkToString(float exportScale, ref Node[] nodes)
    {
        // info each node has:
        //  - position
        //  - index
        //  - nodes its connected to by edges
        //  - nodes its connected to by triangles (WIP)

        // order the positions
        Vector3[] positions = new Vector3[nodes.Length];
        foreach(Node n in nodes)
        {
            // insert the position of this node at its index in the array
            positions[n.GetIndex()] = exportScale * n.transform.position;
        }


        // sort out the connections, make sure there's no duplicates
        List<int[]> connections = new List<int[]>();
        foreach(Node n in nodes)
        {
            // for each connection in the node
            foreach (int connection in n.GetEdgeConnections())
            {
                int[] newConnection = { connection, n.GetIndex() };
                Array.Sort(newConnection);

                if (!connections.Contains(newConnection))
                    connections.Add(newConnection);
            }
        }

        // now we have an ordered array of positions
        // and an array of all the connections that doesnt contain any duplicates

        // this needs to be formatted and concatenated into a string
        // first of all, create an array of strings that define the vertices

        // the output of this exporter is designed specifically to generate C++ code for my wemos racing game project (https://github.com/jambuttenshaw/CMP101WemosProject)
        
        string positionsString = "";
        for (int i = 0; i < positions.Length; i++)
        {
            positionsString += "m_TrackVertices[" + i.ToString() + "] = Point({" + Vector3ToString(positions[i]) + "});\n";
        }

        string edgeIndicesString = "";
        int index = 0;
        for (int i = 0; i < connections.Count; i++)
        {
            edgeIndicesString += "m_TrackEdgeIndices[" + index +"] = " + connections[i][0] + ";\n";
            edgeIndicesString += "m_TrackEdgeIndices[" + (index + 1) +"] = " + connections[i][1] + ";\n";
            index += 2;
        }

        return positionsString + "\n\n" + edgeIndicesString;
    }

    private static string Vector3ToString(Vector3 p)
    {
        string x = Mathf.Round(p.x).ToString();
        string y = Mathf.Round(p.y).ToString();
        string z = Mathf.Round(p.z).ToString();

        return x + ", " + y + ", " + z;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveTrack (List<Node> nodes)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/track.wemostrack";
        FileStream stream = new FileStream(path, FileMode.Create);

        NodeData[] nodeData = new NodeData[nodes.Count];
        int i = 0;
        foreach (Node n in nodes)
        {
            NodeData newNodeData = new NodeData(n);
            nodeData[i] = newNodeData;
            i++;
        }
        Debug.Log("Saving " + i + " nodes");
        formatter.Serialize(stream, nodeData);

        stream.Close();
    }

    public static NodeData[] LoadTrack()
    {
        string path = Application.persistentDataPath + "/track.wemostrack";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            NodeData[] data = formatter.Deserialize(stream) as NodeData[];
            stream.Close();

            Debug.Log("Loaded " + data.Length + " nodes");

            return data;
        } else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}

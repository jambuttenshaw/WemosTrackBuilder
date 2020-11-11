﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBuilder : MonoBehaviour
{
    [Header("Export settings")]
    public float exportScale = 5;

    [Header("Attributes")]
    public GameObject connectionPrefab;
    public GameObject spherePrefab;

    List<Node> nodes;
    Node selectedNode = null;
    Node ring1Node = null;
    Node ring2Node = null;

    int[] triIndices;

    private void Awake()
    {
        nodes = new List<Node>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // run function for mouse left click
            if (ClickingNode(out Node clickedNode))
            {
                SelectNode(clickedNode);
            }
            else
            {
                Node newNode = CreateNode();
                ConnectNodeToSelected(newNode);

                SelectNode(newNode);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            // run function for mouse right click
            if (ClickingNode(out Node clickedNode))
            {
                ConnectNodeToSelected(clickedNode);
                SelectNode(clickedNode);
            }
            else
            {
                Node newNode = CreateNode();
                SelectNode(newNode);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Node[] nodesArray = nodes.ToArray();
            Debug.Log(NodeExporter.NodeNetworkToString(exportScale, ref nodesArray));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (selectedNode != null)
                ring1Node = selectedNode;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (selectedNode != null)
                ring2Node = selectedNode;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            triIndices = NodeTriangulator.Triangulate(nodes, ring1Node, ring2Node);
            Debug.Log(NodeExporter.TriangleIndicesToString(ref triIndices));
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            // delete the selected node
            if (selectedNode != null)
            {
                nodes.Remove(selectedNode);
                Destroy(selectedNode.gameObject);

                selectedNode = null;
            }
        }
    }

    private void SelectNode(Node node)
    {
        if (selectedNode != null)
        {
            selectedNode.SetSelected(false);
        }

        selectedNode = node;
        node.SetSelected(true);
    }

    private bool ClickingNode(out Node clickedNode)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            clickedNode = hit.collider.transform.parent.GetComponent<Node>();
            return true;
        }
        else
        {
            clickedNode = null;
            return false;
        }
    }

    private Node CreateNode()
    {
        // creates a new node at the mouse position
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        
        GameObject newNodeGO = new GameObject("Node " + (nodes.Count + 1));
        Node newNode = newNodeGO.AddComponent<Node>();

        newNodeGO.transform.position = worldPos;
        newNodeGO.transform.parent = transform;

        GameObject sphere = Instantiate(spherePrefab);
        sphere.transform.parent = newNodeGO.transform;
        sphere.transform.localPosition = Vector3.zero;

        newNode.Setup(nodes.Count, sphere.GetComponent<SphereMaterialSetter>());

        nodes.Add(newNode);
        return newNode;
    }

    void ConnectNodeToSelected(Node node)
    {
        if (selectedNode != null)
        {
            node.ConnectNodeToEdge(selectedNode.GetIndex());

            // create a connection game object
            GameObject connectionGO = Instantiate(connectionPrefab);
            LineRendererController lRC = connectionGO.GetComponent<LineRendererController>();

            lRC.Setup(node.transform, selectedNode.transform);

            connectionGO.transform.parent = node.transform;
        }
    }

    void DeleteConnectionsToNode(int nodeIndex)
    {
        foreach (Node node in nodes)
        { 
            foreach (int connection in node.GetEdgeConnections())
            {
                if (connection == nodeIndex)
                {
                    node.DeleteConnection(nodeIndex);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (triIndices.Length > 0)
        {
            int numTriangles = triIndices.Length / 3;
            int triIndex = 0;
            for (int i = 0; i < numTriangles; i++)
            {
                Vector3 pos1 = nodes[triIndices[triIndex]].transform.position;
                Vector3 pos2 = nodes[triIndices[triIndex + 1]].transform.position;
                Vector3 pos3 = nodes[triIndices[triIndex + 2]].transform.position;

                Debug.DrawLine(pos1, pos2);
                Debug.DrawLine(pos1, pos3);
                Debug.DrawLine(pos2, pos3);

                triIndex += 3;
            }
        }
    }

}

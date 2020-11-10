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
            selectedNode.transform.GetChild(0).GetComponent<SphereMaterialSetter>().SetMaterial(false);
        }

        selectedNode = node;
        node.SetSelected(true);
        node.transform.GetChild(0).GetComponent<SphereMaterialSetter>().SetMaterial(true);
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

        newNode.Setup(nodes.Count);

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

}

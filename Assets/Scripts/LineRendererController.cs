using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{

    Transform node1;
    Transform node2;

    LineRenderer lr;

    public void Setup(Transform n1, Transform n2)
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;

        node1 = n1;
        node2 = n2;
    }

    private void Update()
    {
        if (node1 == null || node2 == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3[] positions = { node1.position, node2.position };
        lr.SetPositions(positions);
    }

}

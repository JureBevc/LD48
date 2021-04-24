using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    [SerializeField] private RectTransform rect_transform;
    [SerializeField] public Camera ui_camera;
    [SerializeField] public Node[] children;
    [SerializeField] public Connection connection_prefab;

    private Dictionary<Node, Connection> connections = new Dictionary<Node, Connection>();

    void Start()
    {
        foreach (Node child in children)
        {
            if (child != null)
            {
                Connection connection = Instantiate(connection_prefab, this.transform);
                connections.Add(child, connection);
            }
        }
        this.SetConnectionPositions();
    }

    void Update()
    {
        this.SetConnectionPositions();
    }

    private void SetConnectionPositions()
    {
        foreach (KeyValuePair<Node, Connection> pair in connections)
        {
            Connection connection = pair.Value;
            Node child = pair.Key;

            connection.transform.position = (transform.position + child.transform.position) / 2;
            Vector3 targetDir = child.transform.position - transform.position;
            float angle = Vector3.SignedAngle(targetDir, new Vector3(1, 0, 0), new Vector3(0, 0, 1));
            connection.transform.rotation = Quaternion.Euler(0, 0, -angle);

            float size = targetDir.magnitude;

            ((RectTransform)connection.transform).sizeDelta = new Vector3(size, 10, 1);
        }

    }

    public void onClick()
    {
        Debug.Log("Test");
    }
}

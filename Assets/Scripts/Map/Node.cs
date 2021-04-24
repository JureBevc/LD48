using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    [SerializeField] public Connection connection_prefab;

    private Dictionary<Node, Connection> connections = new Dictionary<Node, Connection>();

    public void init(float position, List<Node> neigbors)
    {
        foreach (Node child in neigbors)
        {
            if (child != null)
            {
                Connection connection = Instantiate(connection_prefab, this.transform);
                connections.Add(child, connection);
            }
        }

        this.transform.position = new Vector3(position * 1, 0, 0);

        this.SetConnectionPositions();
    }

    public void update(float delta_time)
    {
        this.SetConnectionPositions();
    }
    void OnMouseDown()
    {
        Debug.Log("Sprite Clicked");
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

            connection.transform.localScale = new Vector3(size, 1, 1);
        }

    }

    public void onClick()
    {
        Debug.Log("Test");
    }
}

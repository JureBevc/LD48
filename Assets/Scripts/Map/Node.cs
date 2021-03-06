using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    [SerializeField] public Connection connection_prefab;
    public NodeType nodeType;

    private Dictionary<Node, Connection> connections = new Dictionary<Node, Connection>();
    public Level level { get; set; }

    public void init(Level level, float position, List<Node> neigbors)
    {
        this.level = level;
        this.transform.position = new Vector3(position * 1, 0, 0);

        List<Node> closest = GetClosest(neigbors);
        int count = 0;
        foreach (Node child in closest)
        {
            if (child != null && Vector3.Distance(child.transform.localPosition, transform.localPosition) < 2f)
            {
                count += 1;
                Connection connection = Instantiate(connection_prefab, this.transform);
                connections.Add(child, connection);
            }
        }


        this.SetConnectionPositions();
    }

    public List<Node> GetClosest(List<Node> nodes)
    {
        nodes.Sort((n1,n2) => {
            return Vector3.Distance(n1.transform.localPosition, transform.localPosition).CompareTo(
                Vector3.Distance(n2.transform.localPosition, transform.localPosition)
            );
        });

        return nodes;
    }

    public void update(float delta_time)
    {
        this.SetConnectionPositions();
    }
    void OnMouseDown()
    {
        transform.localScale = Vector3.one;
        Map.instance.NodeClick(this);
        foreach (KeyValuePair<Node, Connection> pair in connections)
        {
            pair.Value.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255);
        }
    }

    private void OnMouseEnter()
    {
        transform.localScale = Vector3.one * 1.5f;

        foreach (KeyValuePair<Node, Connection> pair in connections)
        {
            if (pair.Key == Map.instance.currentNode)
                pair.Value.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 156, 0);
            else
                pair.Value.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 0, 0);
        }
    }

    private void OnMouseExit()
    {
        transform.localScale = Vector3.one;
        foreach (KeyValuePair<Node, Connection> pair in connections)
        {
            pair.Value.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255);
        }
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

    public bool HasNeighbor(Node node)
    {
        return connections.ContainsKey(node);
    }
}

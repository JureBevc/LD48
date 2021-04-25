using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    private List<Node> nodes = new List<Node>();
    public int levelNumber { get; set; }
    public void init(int level_number, LevelData level_data, List<Node> previous_level_nodes)
    {
        levelNumber = level_number;
        for (int i = 0; i < level_data.nodes.Length; i++)
        {
            NodeData node_data = level_data.nodes[i];

            Node node = Instantiate<Node>(node_data.get_map_node_prefab(), this.transform, true);
            nodes.Add(node);

            int node_count = level_data.nodes.Length;
            node.init(this, i - node_count / 2.0f + .5f, previous_level_nodes);
        }
        this.transform.position = new Vector3(0, 5f - level_number, 0);
    }

    public void update(float delta_time)
    {
        foreach (Node node in nodes)
        {
            node.update(delta_time);
        }
    }

    public List<Node> get_nodes()
    {
        return nodes;
    }
}

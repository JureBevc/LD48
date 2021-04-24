﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    [SerializeField] private Node node_prefab;

    private List<Node> nodes = new List<Node>();

    public void init(int level_number, int node_count, List<Node> previous_level_nodes)
    {
        for (int i = 0; i < node_count; i++)
        {
            Node node = Instantiate<Node>(node_prefab, this.transform, true);
            nodes.Add(node);

            node.init(i - node_count / 2.0f + .5f, previous_level_nodes);
        }
        this.transform.position = new Vector3(0, level_number * 1, 0);
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
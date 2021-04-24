using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class NodeData : ScriptableObject
{
    [SerializeField] private Node node_prefab;

    public Node get_map_node_prefab()
    {
        return node_prefab;
    }
}

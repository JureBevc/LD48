using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private int[] nodes_per_level;

    [SerializeField] private Level level_prefab;

    [SerializeField] private float scroll_speed = 20;

    private List<Level> levels = new List<Level>();


    public void init()
    {
        List<Node> previous_nodes = new List<Node>();
        for (int i = 0; i < nodes_per_level.Length; i++)
        {
            int node_count = nodes_per_level[i];

            Level level = Instantiate(level_prefab, this.transform);
            levels.Add(level);
            level.init(i, node_count, previous_nodes);

            previous_nodes = level.get_nodes();
        }
    }

    public void update(float delta_time)
    {
        foreach (Level level in levels)
        {
            level.update(delta_time);
        }

        Vector3 position = this.transform.position;
        position.y -= Input.mouseScrollDelta.y * delta_time * scroll_speed;
        this.transform.position = position;
    }

    void Start()
    {
        this.init();
    }
    void Update()
    {
        this.update(Time.deltaTime);
    }


}

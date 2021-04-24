using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map instance;
    [SerializeField] private Level level_prefab;

    [SerializeField] private float scroll_speed = 20;

    [SerializeField] private MapData map_data;

    private List<Level> levels = new List<Level>();

    public Node currentNode { get; set; }
    public GameObject currentNodeIndicator;

    public Map()
    {
        instance = this;
    }
    public void init()
    {
        List<Node> previous_nodes = new List<Node>();
        for (int i = 0; i < map_data.levels.Length; i++)
        {
            LevelData level_data = map_data.levels[i];

            Level level = Instantiate(level_prefab, this.transform);
            levels.Add(level);
            level.init(i, level_data, previous_nodes);

            previous_nodes = level.get_nodes();
            if (i == 0)
            {
                currentNode = level.get_nodes()[0];
            }
        }

        // Move to the initial node
        MoveToNode(currentNode);
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

    public void NodeClick(Node node)
    {
        Level level = node.level;
        if (node.HasNeighbor(currentNode))
        {
            MoveToNode(node);
        }
        AudioPlayer.instance.PlayClick();

        switch (currentNode.nodeType)
        {
            case NodeType.CAMP:

                break;
            case NodeType.NORMAL_BATTLE:
                gameObject.SetActive(false);
                Combat.instance.gameObject.SetActive(true);
                Combat.instance.StartCombat();
                break;
            case NodeType.HARD_BATTLE:
                gameObject.SetActive(false);
                Combat.instance.gameObject.SetActive(true);
                Combat.instance.StartCombat();
                break;
            case NodeType.BOSS_BATTLE:
                gameObject.SetActive(false);
                Combat.instance.gameObject.SetActive(true);
                Combat.instance.StartCombat();
                break;
        }
    }

    public void MoveToNode(Node node)
    {
        Debug.Log("Moving to next node");
        currentNode = node;
        currentNodeIndicator.transform.position = node.transform.position;
    }

}

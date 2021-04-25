using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{
    public static Camp instance;

    public GameObject campUI;

    private List<GameObject> units = new List<GameObject>();
    public Camp()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowCamp();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowCamp()
    {
        foreach (Unit unit in Combat.instance.GetPlayerUnits())
        {
            GameObject obj = new GameObject("Unit");
            obj.AddComponent<SpriteRenderer>().sprite = unit.GetComponentInChildren<SpriteRenderer>().sprite;
            obj.transform.position = Random.insideUnitCircle.normalized * Random.Range(2.8f, 3.2f);
            obj.transform.parent = transform;
            units.Add(obj);
        }
        gameObject.SetActive(true);
        campUI.SetActive(true);
    }

    public void HideCamp()
    {
        foreach (GameObject unit in units)
        {
            Destroy(unit);
        }
        units = new List<GameObject>();
        gameObject.SetActive(false);
        campUI.SetActive(false);
    }
}

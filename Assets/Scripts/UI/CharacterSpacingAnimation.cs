using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSpacingAnimation : MonoBehaviour
{
    public float speed, min, max;

    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.characterSpacing = Mathf.Lerp(min, max, (Mathf.Sin(speed * Time.time) + 1) / 2);
    }
}

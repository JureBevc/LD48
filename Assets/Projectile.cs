using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime;

    public float minHeight, maxHeight;
    private float height;
    public AnimationCurve curve;
    private SpriteRenderer spriteRenderer;
    private bool isUsed { get; set; }
    private Vector3 origin, target;
    private float currentLifeTime;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ShowSprite(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isUsed)
        {
            currentLifeTime += Time.deltaTime;

            float t = currentLifeTime / lifeTime;
            float px = Mathf.Lerp(origin.x, target.x, t);
            float py = origin.y + curve.Evaluate(t) * height;
            transform.position = new Vector3(px, py, origin.z);
            if (t >= 1)
            {
                isUsed = false;
                ShowSprite(false);
            }
        }
    }

    public void Throw(Vector3 origin, Vector3 target)
    {
        this.origin = origin;
        this.target = target;
        currentLifeTime = 0;
        isUsed = true;
        transform.position = origin;
        height = Random.Range(minHeight, maxHeight);
        ShowSprite(true);
    }

    private void ShowSprite(bool value)
    {
        spriteRenderer.enabled = value;
    }
}

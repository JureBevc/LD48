using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime;

    public float yOffset;
    public float minHeight, maxHeight;
    private float height;
    public AnimationCurve curve;
    private SpriteRenderer spriteRenderer;
    private bool isUsed { get; set; }
    private Vector3 origin, target;
    private float currentLifeTime;
    private float hitChance;
    private Unit fromUnit { get; set; }

    private Vector3 previousPosition;
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(GameObject projectilePrefab)
    {
        spriteRenderer = Instantiate(projectilePrefab, transform).GetComponentInChildren<SpriteRenderer>();
        ShowSprite(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsed)
        {
            currentLifeTime += Time.deltaTime;

            float t = currentLifeTime / lifeTime;
            float px = Mathf.Lerp(origin.x, target.x, t);
            float py = origin.y + curve.Evaluate(t) * height + yOffset;
            transform.position = new Vector3(px, py, origin.z);
            UpdateSpriteAngle();
            if (t >= 1)
            {
                CheckForHit();
                ResetProjectile();
            }

            previousPosition = transform.position;
        }
    }

    public void Throw(Unit fromUnit, Vector3 origin, Vector3 target, float hitChance)
    {
        this.fromUnit = fromUnit;
        this.origin = origin;
        this.target = target;
        this.hitChance = hitChance;

        currentLifeTime = 0;
        isUsed = true;
        transform.position = origin + Vector3.up * yOffset;
        height = Random.Range(minHeight, maxHeight);
        previousPosition = transform.position;
        ShowSprite(true);
    }

    private void CheckForHit()
    {
        if (Random.Range(0f, 1f) < hitChance)
        {
            if (fromUnit.isEnemy)
            {
                Combat.instance.DamagePlayer();
            }
            else
            {
                Combat.instance.DamageEnemy();
            }
        }
    }

    private void ShowSprite(bool value)
    {
        spriteRenderer.color = new Color(255, 255, 255);
        spriteRenderer.enabled = value;
    }

    private void UpdateSpriteAngle()
    {
        float angle = Vector3.SignedAngle(Vector3.up, previousPosition - transform.position, Vector3.forward);
        transform.rotation = Quaternion.Euler(0, 0, 180 + angle);
    }

    private void OnMouseDown()
    {
        if (fromUnit.isEnemy && Combat.instance.divineJudgement)
        {
            AudioPlayer.instance.PlayClick2();
            isUsed = false;
            ShowSprite(false);
        }
        if (!fromUnit.isEnemy && Combat.instance.divineBlessing)
        {
            AudioPlayer.instance.PlayClick2();
            spriteRenderer.color = new Color(255, 120, 0);
        }
    }

    public void ResetProjectile()
    {
        isUsed = false;
        ShowSprite(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private readonly float baseSpeed = 0.05f;
    private float speed
    {
        get
        {
            return baseSpeed + Random.Range(0, 10) * 0.05f;
        }
    }
    private float horzExtent;
    private float vertExtent;
    private float sizeValue;

    private SpriteRenderer sr;
    private Color baseColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
        sizeValue = Mathf.Clamp(speed * 2, 0.2f, 1);
        sr.sortingOrder = Mathf.RoundToInt(sizeValue * 100);
        transform.localScale = sizeValue * Vector2.one;
        baseColor.a = sizeValue * .4f + 0.6f;
        GameController.NewBlock += VerticalPass;
    }

    private void Start()
    {
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.left;

        if (HorizontalPass())
        {
            Vector2 newPos = new Vector2(horzExtent * 1.2f + transform.localScale.x + 1, EnvironmentController.instance.RandomY);
            gameObject.SetActive(false);
            ObjectPooler.instance.SpawnFromPool("cloud", newPos, Quaternion.identity);
        }

    }

    private bool HorizontalPass()
    {
        return transform.position.x < -horzExtent * 1.6f;
    }

    private void VerticalPass()
    {
        if (transform.position.y + transform.localScale.y < -vertExtent + Camera.main.transform.position.y)
        {
            gameObject.SetActive(false);
            ObjectPooler.instance.SpawnFromPool("cloud", EnvironmentController.instance.RandomCloudPosition(), Quaternion.identity);

        }
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        GameController.NewBlock -= VerticalPass;
    }
}

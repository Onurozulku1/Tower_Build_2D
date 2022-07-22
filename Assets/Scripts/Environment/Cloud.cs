using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private readonly float baseSpeed = 0.2f;
    private float speed;
    private float horzExtent;
    private float vertExtent;

    private SpriteRenderer sr;
    private Color baseColor;
    private void Start()
    {
        speed = baseSpeed + Random.Range(0, 10) * 0.1f;
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
        return transform.position.x + transform.localScale.x < -horzExtent * 1.3f;
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
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
        speed = baseSpeed + Random.Range(0, 10) * 0.1f;
        float sizeValue;
        sizeValue = Mathf.Clamp(speed / 1.2f, 0.2f, 1);
        transform.localScale = sizeValue * Vector2.one;
        baseColor.a = sizeValue * .4f + 0.6f;
        //sr.color = baseColor;
        GameController.NewBlock += VerticalPass;
    }

    private void OnDisable()
    {
        GameController.NewBlock -= VerticalPass;
    }
}

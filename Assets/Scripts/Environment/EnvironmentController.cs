using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    ObjectPooler op;
    private float horzExtent;
    private float vertExtent;

    public static EnvironmentController instance;
    private void Awake()
    {
        instance = this;
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }

    private void Start()
    {
        op = ObjectPooler.instance;
        for (int i = 0; i < op.poolDictionary["cloud"].Count; i++)
        {
            op.SpawnFromPool("cloud", RandomCloudPosition(), Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < op.poolDictionary["cloud"].Count; i++)
            {
                op.SpawnFromPool("cloud", RandomCloudPosition(), Quaternion.identity);
            }
        }
    }

    public float RandomY { get
        {
            return Random.Range(-vertExtent * 0.8f, vertExtent * 5) + Camera.main.transform.position.y + vertExtent * .7f;
        } }

    public Vector2 RandomCloudPosition()
    {
        float x = Random.Range(-0.5f * horzExtent, horzExtent * 1.2f);

        return new Vector2(x, RandomY);
    }

}

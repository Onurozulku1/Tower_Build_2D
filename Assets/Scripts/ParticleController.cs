using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem perfectParticle;

    private void Awake()
    {
        perfectParticle = GetComponentInChildren<ParticleSystem>();
        perfectParticle.transform.localScale = new Vector2(BlockManager.instance.LastX * 0.5f, transform.localScale.y);

    }

    void SetPerfectParticle()
    {
        Vector3 particlePos = transform.position;
        particlePos.y = BlockManager.instance.posY;
        particlePos.x = BlockManager.instance.lastPosX;
        perfectParticle.transform.localScale = new Vector2(BlockManager.instance.LastX * 0.5f, transform.localScale.y);
        transform.position = particlePos;
        perfectParticle.Play();
    }

    private void OnEnable()
    {
        GameController.PerfectBLock += SetPerfectParticle;
    }

    private void OnDisable()
    {
        GameController.PerfectBLock -= SetPerfectParticle;
    }
}

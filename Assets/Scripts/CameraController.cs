using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 posVector;

    void Update()
    {

        if (BlockManager.instance.posY < 0)
            return;

        posVector = new Vector3(0, BlockManager.instance.posY + 1, -10);
        transform.position = Vector3.Lerp(transform.position, posVector, Time.deltaTime * 3);


        
    }

}

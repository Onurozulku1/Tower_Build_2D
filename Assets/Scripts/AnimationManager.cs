using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator uiAnimator;

    private void Awake()
    {
        uiAnimator = GetComponent<Animator>();
    }

    private void InGameTokenAnim()
    {
        if (BlockManager.instance.perfectBlock)
        {
            uiAnimator.SetTrigger("token");
        }
    }

    private void OnEnable()
    {
        GameController.NewBlock += InGameTokenAnim;
    }

    private void OnDisable()
    {
        GameController.NewBlock -= InGameTokenAnim;
    }
}

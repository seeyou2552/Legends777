using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandle : MonoBehaviour
{
    private readonly int IsMoving = Animator.StringToHash("IsRun");
    protected Animator animator;
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMoving, obj.magnitude > .5f);
    }
}

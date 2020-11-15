using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFace : MonoBehaviour
{
    // Idle 
    // walk
    // run

    [SerializeField]
    private Material _faceMaterial;

    [SerializeField]
    private string _offsetShaderField;

    [SerializeField]
    private float _idleOffset;
    [SerializeField]
    private float _walkOffset;
    [SerializeField]
    private float _runOffset;

    public void StateChanged(AIState aIState)
    {
        switch (aIState)
        {
            case AIState.Idle:
                IdleFace();
                break;
            case AIState.Alerted:
                RunFace();
                break;
            case AIState.Walk:
                WalkFace();
                break;
            case AIState.IsStunned:
                RunFace();
                break;
            default:
                break;
        }
    }

    public void RunFace()
    {
        _faceMaterial.SetTextureOffset(_offsetShaderField, new Vector2(_runOffset,0));
    }

    public void IdleFace()
    {
        _faceMaterial.SetTextureOffset(_offsetShaderField, new Vector2(_idleOffset, 0));
    }

    public void WalkFace()
    {
        _faceMaterial.SetTextureOffset(_offsetShaderField, new Vector2(_walkOffset, 0));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using ArmasCreator.Gameplay;

public class IsPlayerEnterStage : ActionNode
{
    private OnTriggerEvent stageCollider;

    protected override void OnStart() {

        stageCollider = GameObject.FindGameObjectWithTag("StageCollider").GetComponent<OnTriggerEvent>();

        stageCollider.onTriggerEnter.AddListener(OnPlayerEnter);
    }

    protected override void OnStop() 
    {

    }

    protected override State OnUpdate() 
    {
        if (blackboard.IsPlayerEnterStage)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }

    private void OnPlayerEnter(Collider col)
    {
        if (blackboard.IsPlayerEnterStage) { return; }

        blackboard.IsPlayerEnterStage = true;

        Debug.Log("Enter Stage");

        stageCollider.SetBoxCollider(true);
    }
}

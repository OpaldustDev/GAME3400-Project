using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : BaseState
{
    private float delay;

    public override void EnterState()
    {
        delay = 0.2f;
    }

    public override void UpdateState()
    {
        delay -= Time.deltaTime;
        if (delay > 0)
        {
            return;
        }

        if (characterController.CheckIfGrounded())
        {
            characterController.TransitionToState(characterController.RunningState);
        }
    }

    public override void ExitState()
    {
        if (characterController.CheckIfGrounded())
        {
            characterController.ResetJumps();

        }
    }

    public override void FixedUpdateState()
    {
        characterController.UpdateMovement();
    }
}

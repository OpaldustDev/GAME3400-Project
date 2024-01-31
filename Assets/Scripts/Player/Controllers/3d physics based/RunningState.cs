using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : BaseState
{

    public override void UpdateState()
    {
        if (characterController.GetInputVector() == Vector2.zero && characterController.GetVelocity() == Vector3.zero)
        {
            characterController.TransitionToState(characterController.IdleState);
        }
    }

    public override void FixedUpdateState()
    {
        characterController.UpdateMovement();
    }
}

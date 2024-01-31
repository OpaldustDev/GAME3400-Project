using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{

    public override void UpdateState()
    {
        if (characterController.GetInputVector() != Vector2.zero)
        {
            characterController.TransitionToState(characterController.RunningState);
        }
    }

    public override void FixedUpdateState()
    {

    }
}

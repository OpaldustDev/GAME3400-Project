using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected PlayerStateController characterController;

    public BaseState() { }

    public BaseState(PlayerStateController controller)
    {
        characterController = controller;
    }

    public virtual void SetController(PlayerStateController controller)
    {
        this.characterController = controller;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void ExitState() { }
}

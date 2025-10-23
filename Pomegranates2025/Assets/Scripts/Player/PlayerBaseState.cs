using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBaseState
{

    // Abstract methods do not provide an implementation and force the derived classes to override the method. 
    // override mandatory
    public abstract void EnterState(PlayerStateManager player);

    public abstract void UpdateState(PlayerStateManager player);
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class LittleBoyPlayerBaseState
{

    // Abstract methods do not provide an implementation and force the derived classes to override the method. 
    // override mandatory
    public abstract void EnterState(LittleBoyPlayerStateManager player);

    public abstract void UpdateState(LittleBoyPlayerStateManager player);
}

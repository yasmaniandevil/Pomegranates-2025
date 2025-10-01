using System;
using UnityEngine;

public abstract class NPCBaseState
{
    public abstract void EnterState(NPCStateManager npc);

    public abstract void UpdateState(NPCStateManager npc);
}

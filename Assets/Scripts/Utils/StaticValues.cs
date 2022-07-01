using System;
using UnityEngine;
using System.Collections.Generic;

public static class StaticValues
{
    public readonly static int EnemyLayer = LayerMask.NameToLayer("Enemy");
    public readonly static int RagdollLayer = LayerMask.NameToLayer("Ragdoll");
    public readonly static int DestinationLayer = LayerMask.NameToLayer("Destination");
    public readonly static int CharacterLayer = LayerMask.NameToLayer("Character");
}

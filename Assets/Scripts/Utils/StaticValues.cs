using System;
using UnityEngine;
using System.Collections.Generic;

public static class StaticValues
{
    private static byte _bulletFillPercent = 2;
    private static byte _bombFillPercent = 4;
    private static byte _rocketFillPercent = 4;
    private static byte _gooFillPercent = 5;

    public readonly static int EnemyLayer = LayerMask.NameToLayer("Enemy");
    public readonly static int RagdollLayer = LayerMask.NameToLayer("Ragdoll");
    public readonly static int DestinationLayer = LayerMask.NameToLayer("Destination");
    public readonly static int CharacterLayer = LayerMask.NameToLayer("Character");
}

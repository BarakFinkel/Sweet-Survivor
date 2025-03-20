using System;
using UnityEngine;

public class DonutCoin : Drop
{
    public static Action<DonutCoin> onCollect;

    protected override void HandleCollection()
    {
        onCollect?.Invoke(this);
    }
}

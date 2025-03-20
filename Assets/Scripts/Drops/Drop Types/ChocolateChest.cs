using System;
using UnityEngine;

public class ChocolateChest : Drop
{
    public static Action<ChocolateChest> onCollect;

    // Since we don't want the chest to move on collection, we simply collect it immediately upon interaction.
    public override void Collect(Player player)
    {
        if (collected)
            return;
        
        collected = true;

        HandleCollection();
    }

    protected override void HandleCollection()
    {
        onCollect?.Invoke(this);
    }
}

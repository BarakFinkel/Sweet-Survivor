using System;

public class SodaXP : Drop
{
    public static Action<SodaXP> onCollect;

    protected override void HandleCollection()
    {
        onCollect?.Invoke(this);
    }
}

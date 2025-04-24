using System;

public class PremiumDonut : Drop
{
    public static Action<PremiumDonut> onCollect;

    protected override void HandleCollection()
    {
        onCollect?.Invoke(this);
        CurrencyManager.instance.AddCurrency(CurrencyType.Premium, 5);
        PlayCollectSound();
    }
}

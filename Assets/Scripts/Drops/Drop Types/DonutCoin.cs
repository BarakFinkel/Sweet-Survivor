using System;

public class DonutCoin : Drop
{
    public static Action<DonutCoin> onCollect;

    protected override void HandleCollection()
    {
        onCollect?.Invoke(this);
        CurrencyManager.instance.AddCurrency(CurrencyType.Normal, 10);
        PlayCollectSound();
    }
}

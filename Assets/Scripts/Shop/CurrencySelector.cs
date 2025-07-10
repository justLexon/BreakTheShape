public static class CurrencySelector
{
    public enum Currency { Coins, Premium }
    public static Currency CurrentCurrency = Currency.Coins;

    public static bool UsePremium => CurrentCurrency == Currency.Premium;
}

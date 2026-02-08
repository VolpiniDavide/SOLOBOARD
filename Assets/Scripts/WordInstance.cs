[System.Serializable]
public class WordInstance
{
    public WordData data;

    public int bonusLevel;
    public int bonusEconomicValue;

    public int FinalLevel => data.level + bonusLevel;
    public int FinalEconomicValue => data.economicValue + bonusEconomicValue;

    public WordInstance(WordData data)
    {
        this.data = data;
        bonusLevel = 0;
        bonusEconomicValue = 0;
    }
}

public static class SentenceResolver
{
    public static UnitData Resolve(Word subject, Word predicate, Word obj)
    {
        UnitData data = new UnitData();
        data.unitName = subject.value;

        int bonus = GetObjectBonus(obj);

        switch (predicate.value)
        {
            case "eats":
                data.hp += bonus;
                break;

            case "runs":
                data.speed += bonus;
                break;

            case "lifts":
                data.strength += bonus;
                break;
        }

        return data;
    }

    static int GetObjectBonus(Word obj)
    {
        // placeholder
        return obj.level switch
        {
            0 => 5,
            1 => 10,
            2 => 20,
            3 => 40,
            _ => 5
        };
    }
}

public struct ValidatedSentence
{
    public WordData subject;
    public WordData predicate;
    public WordData obj;
    public bool statBoost;

    public ValidatedSentence(
        WordData subject,
        WordData predicate,
        WordData obj)
    {
        this.subject = subject;
        this.predicate = predicate;
        this.obj = obj;
        this.statBoost = false;
    }

    public ValidatedSentence(
        WordData subject,
        WordData predicate,
        WordData obj,
        bool statBoost)
    {
        this.subject = subject;
        this.predicate = predicate;
        this.obj = obj;
        this.statBoost = statBoost;
    }
}

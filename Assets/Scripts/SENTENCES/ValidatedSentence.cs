public struct ValidatedSentence
{
    public WordData subject;
    public WordData predicate;
    public WordData obj;

    public ValidatedSentence(
        WordData subject,
        WordData predicate,
        WordData obj)
    {
        this.subject = subject;
        this.predicate = predicate;
        this.obj = obj;
    }
}

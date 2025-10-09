namespace StreamNote.GraphQL.Types
{
    [QueryType]
    public static class Query
    {
        public static Book GetBook()
            => new Book("C# in depth.", new Author("Jon Skeet"));

        public static string SayHello(string name="World")
            => $"Hello, {name}!";
    }
}

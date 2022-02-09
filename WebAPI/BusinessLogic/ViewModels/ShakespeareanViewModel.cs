namespace BusinessLogic.ViewModels
{
    internal class ShakespeareanViewModel
    {
        internal Success Success { get; set; }

        internal Contents Contents { get; set; }
    }

    internal class Success
    {
        internal int Total { get; set; }
    }

    internal class Contents
    {
        internal string Translated { get; set; }
        internal string Text { get; set; }
        internal string Translation { get; set; }
    }
}
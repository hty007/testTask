namespace starter
{
    public interface ICmd
    {
        string[] GetName();
        string GetHelp();
        void Execute(params string[] pars);
    }
}

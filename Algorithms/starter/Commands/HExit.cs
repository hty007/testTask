namespace starter
{
    public class HExit : ICmd
    {
        public void Execute(params string[] pars)
        {
            Program.work = false;
        }

        public string GetHelp()
        {
            return "Осуществляется выход из прорграммы (параметры не учитываются)";
        }

        public string[] GetName()
        {
            return new[]
            {
                "exit",
                "quit",
                "выход"
            };
        }
    }
}

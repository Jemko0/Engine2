namespace Engine2
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        static Engine? engine;
        [STAThread]

        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            ApplicationConfiguration.Initialize();
            engine = new Engine();
            Application.Run(engine);
        }

        public static Engine getEngine()
        {
            return engine;
        }
    }
}
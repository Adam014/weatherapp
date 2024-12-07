using dotenv.net;

namespace weatherapp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Load environment variables from .env
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true, envFilePaths: new[] { ".env" }));

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}

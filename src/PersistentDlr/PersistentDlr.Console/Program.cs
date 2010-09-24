namespace PersistentDlr.Console
{
    class Program
    {
        static void Main(string[] args) {
            SetTitle("Starting...");
            Agent agent = new Agent();
            agent.Start();

            SetTitle("Started");

            System.Console.ReadLine();
            agent.Stop();

            SetTitle("Stopped");
        }

        private static void SetTitle(string title) {
            System.Console.Title = "PersistentDlr.Console " + title;
            System.Console.WriteLine(title);
        }
    }
}

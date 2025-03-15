namespace BVV_Task_8;

static class Program
{
    public static Settings Settings { get; set; } = null!;
    
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        var prompt = new SettingsPrompt();
        prompt.ShowDialog();
        Settings = prompt.Result;
        Application.Run(new Form1());
    }
}
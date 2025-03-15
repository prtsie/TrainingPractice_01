namespace BVV_Task_5
{
    internal static class Program
    {
        public static Pen borderPen = Pens.Black;
        public static Brush textBrush= Brushes.Black;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
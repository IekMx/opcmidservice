using System;
using System.Windows.Forms;

namespace OpcSocket
{
    class Program
    {
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                var applicationContext = new CustomApplicationContext();
                Application.Run(applicationContext);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Program Terminated Unexpectedly",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}

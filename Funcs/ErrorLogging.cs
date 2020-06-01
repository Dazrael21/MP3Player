using System;
using System.Collections.Generic;
using System.Text;

namespace Big_Finish_Player.Funcs
{
    public static class ErrorLogging
    {
        public static void Log(Exception ex)
        {
            string message = ex.Message;
            string caption = "ERROR";

            System.Windows.MessageBox.Show(message, caption, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }
}

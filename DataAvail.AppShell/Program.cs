﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataAvail.XObject;

namespace DataAvail.AppShell
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainFrame());
            }
            /*
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка!");
            }
             */

        }
    }
}

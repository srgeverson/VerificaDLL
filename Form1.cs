using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace VerificaDLL
{
    public partial class Form1 : Form
    {
        private string ARQUIVO_LOG = string.Format("{0}.txt", Path.Combine(Directory.GetCurrentDirectory(), AppDomain.CurrentDomain.FriendlyName.Split('.')[0]));
        private static string CLSID = "CLSID";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool achou = false;
            RegistryKey clsid = Registry.ClassesRoot.OpenSubKey(CLSID);
            string[] ClsIDs = clsid.GetSubKeyNames();
            string subkey = "";
            for (int i = 0; i < ClsIDs.Length; i++)
            {
                subkey = ClsIDs[i];
                if (subkey.Substring(0, 1) != "{") continue;
                RegistryKey cls = Registry.ClassesRoot.OpenSubKey(Path.Combine(CLSID, subkey, "InprocServer32"));
                if (cls == null) continue;
                //string x = cls.GetValue("", "").ToString();
                var valor = cls.GetValue("", "").ToString();
                if (!string.IsNullOrEmpty(valor))
                    LogAplicacao(valor);

                //if (x.IndexOf(nomeDLL) >= 0)
                //{
                //    achou = true;
                //    break;
                //}
            }
        }
        public void LogAplicacao(string texto)
        {
            using (var streamWriter = File.AppendText(ARQUIVO_LOG))
            {
                try
                {
                    streamWriter.WriteLine(string.Format("Log:: {0} -> {1}", texto, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Concat("Erro: ", e.Message));
                }
                finally
                {
                    Console.WriteLine("LogAplicacao executado....");
                }
            }
        }
    }
}

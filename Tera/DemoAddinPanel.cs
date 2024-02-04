using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tera
{
    public partial class DemoAddinPanel : UserControl
    {
        public DemoAddinPanel()
        {
            InitializeComponent();
        }

        private static Random random = new Random();

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new();
            textBox1.Clear();

            foreach (KeyValuePair<string, uint> kvp in Colors.colorDict)
            {
                builder.AppendLine($"{kvp.Key} - {kvp.Value:x}");
            }

            textBox1.Text = builder.ToString();
        }
    }
}

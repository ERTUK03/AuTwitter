using System;
using System.Windows.Forms;

namespace Auto_Twitter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
            this.Hide();
            newForm.ShowDialog();
            newForm.checkComms.Abort();
            this.Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Data.SQLite;
using Dapper;
using TweetSharp;

namespace Auto_Twitter
{
    public partial class Form2 : Form
    {
        static List<Command_model> commands = new List<Command_model>();
        private static TwitterService service;
        public Thread checkComms = new Thread(Form2.checkCommands);
        public Form2(string customer_key, string customer_key_secret, string access_key, string access_key_secret)
        {
            service = new TwitterService(customer_key, customer_key_secret, access_key, access_key_secret);
            checkComms.Start();
            InitializeComponent();
            LoadCommandList();
        }
        private void LoadCommandList()
        {
            commands = SQLite_Data_Access.LoadCommands();
            MakeCommandList();
        }

        private void MakeCommandList()
        {
            Scheduled_Commands.DataSource = commands;
            Scheduled_Commands.DisplayMember = "command_name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Command_model command = new Command_model();
            command.name = textBox1.Text;
            command.content = textBox2.Text;
            command.image = textBox3.Text;
            command.data = dateTimePicker1.Text;

            if (textBox1.Text == "" || textBox2.Text == "" || dateTimePicker1.Text == "")
            {
                MessageBox.Show("Value cannot be NULL");
            }
            else
            {
                try
                {
                    SQLite_Data_Access.SaveCommand(command);
                    LoadCommandList();
                }
                catch(Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            dateTimePicker1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"c:\\";
            openFileDialog.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = openFileDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SQLite_Data_Access.DeleteCommand(textBox4.Text);
                LoadCommandList();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
            textBox4.Text = "";
        }

        private static void checkCommands()
        {
            for(; ; )
            {
                foreach (var command in commands)
                {
                    if (command.data == DateTime.Now.ToString("HH:mm"))
                    {
                        Tweet(command);
                    }
                }
            }
        }

        private static void Tweet(Command_model command)
        {
            if (command.image == "")
            {
                service.SendTweet(new SendTweetOptions { Status = command.content }, (tweet,response) =>
                {
                    if(response.StatusCode != HttpStatusCode.OK)
                    {
                        MessageBox.Show(response.Error.Message);
                    }
                    SQLite_Data_Access.DeleteCommand(command.name);
                });
                
            }
            else
            {
                using (var stream = new FileStream(command.image, FileMode.Open))
                {
                    service.SendTweetWithMedia(new SendTweetWithMediaOptions { Status = command.content, Images = new Dictionary<string, Stream> { { command.image, stream } } });
                    SQLite_Data_Access.DeleteCommand(command.name);
                }
            }
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace AutoComplete
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AutoComplete.ImportWords();
            AutoComplete.ImportQueries();
            listBox1.Hide();
        }

        #region styling

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        Point mouseDownPoint = Point.Empty;

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.BackColor = Color.Red;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.BackColor = Color.Empty;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.BackColor = Color.AliceBlue;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.BackColor = Color.Empty;
        }

        private void listBox1_Leave(object sender, EventArgs e)
        {
            listBox1.Hide();
        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Up)
            {
                if (listBox1.SelectedIndex > 0)
                    listBox1.SelectedIndex--;
            }
            if (e.KeyChar == (char)Keys.Down)
            {
                if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                    listBox1.SelectedIndex++;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox1.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
                listBox1.Hide();
            }
        }
        #endregion

        #region code
        private static List<Query> Suggestions = new List<Query>();
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string userInput = textBox1.Text.ToLower();
            string prev = "";
            string current = "";
            int i = 0;
            foreach (char x in userInput)
            {
                if (x == ' ')
                {
                    if (current == "")
                        continue;
                    prev = current;
                    current = "";
                    i++;
                    continue;
                }
                current += x;
            }
            if (current.Length == 0)
            {
                current = prev;
                i--;
            }
            Suggestions = AutoComplete.getSuggestions(current);
            if (Suggestions != null)
            {
                listBox1.Items.Clear();
                if (radioButton1.Checked)
                    Suggestions = Sorting.mergeSort(Suggestions);
                else
                    Sorting.bubbleSort(Suggestions);
                foreach (Query q in Suggestions)
                {
                    if (q.query.Length >= userInput.Length)
                        if (q.index == i && q.query.Substring(0, userInput.Length) == userInput)
                            listBox1.Items.Add(q.query);
                }
                if (listBox1.Items.Count == 0)
                {
                    foreach (Query q in Suggestions)
                    {
                        listBox1.Items.Add(q.query);
                    }
                }
                if (listBox1.Items.Count > 0)
                    listBox1.Show();
            }
            else
            {
                listBox1.Items.Clear();
                listBox1.Hide();
            }
        }
    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ExcuseManager
{
    public partial class Form1 : Form
    {
        Excuse currentExcuse = new Excuse();
        bool formChangen = false;
        string folderPath = string.Empty;
        Random random = new Random();
        public Form1()
        {
            InitializeComponent();
            currentExcuse.LastUsed = dateTimePicker1.Value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            currentExcuse.Description = textBox1.Text;
            UpdateForm(true);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            currentExcuse.Result = textBox2.Text;
            UpdateForm(true);
        }
        private void UpdateForm(bool changed)
        {
            if (changed)
            {
                this.Text = "Excuse Manager*";
            }
            else
            {
                this.textBox1.Text = currentExcuse.Description;
                this.textBox2.Text = currentExcuse.Result;
                this.dateTimePicker1.Value = currentExcuse.LastUsed;
                if (!string.IsNullOrEmpty(currentExcuse.ExcusePath))
                    labelDataTime.Text = File.GetLastWriteTime(currentExcuse.ExcusePath).ToString();
                this.Text = "Excuse Manager";
            }
            this.formChangen = changed;
        }

        private void buttonFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = folderPath;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
                buttonSave.Enabled = true;
                buttonOpen.Enabled = true;
                buttonRandom.Enabled = true;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Pleas specify an excuse and a result", "Unable to save",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            saveFileDialog1.Title = "Save as";
            saveFileDialog1.FileName = textBox1.Text + ".txt";
            saveFileDialog1.Filter = "Excuse file(*.excuse)|*.excuse|All file(*.*)|*.*";
            saveFileDialog1.InitialDirectory = folderPath;
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                currentExcuse.Save(saveFileDialog1.FileName);
                UpdateForm(false);
                MessageBox.Show("Excuse written");
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (CheckChengen())
            {
                openFileDialog1.InitialDirectory = folderPath;
                openFileDialog1.Title = "Open";
                openFileDialog1.Filter = "Excuse file(*.excuse)|*.excuse|All file(*.*)|*.*";
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    currentExcuse = new Excuse(openFileDialog1.FileName);
                    UpdateForm(false);
                }
            }
        }
        private bool CheckChengen()
        {
            if (formChangen)
            {
                DialogResult result = MessageBox.Show("The curret excuse has not been saved. Continue?",
                    "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return false;
            }
            return true;
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            if (CheckChengen())
            {
                currentExcuse = new Excuse(random, folderPath);
                UpdateForm(false);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            currentExcuse.LastUsed = dateTimePicker1.Value;
            UpdateForm(true);
        }
    }
}

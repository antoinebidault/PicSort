using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicSort
{
    public partial class Selecteur : Form
    {
        private string folder = @"C:\";
        private BackgroundWorker worker;
        private List<FileInfo> listFile;
        private string[] extensions = new string[] { ".jpg", ".jpeg", ".gif", ".png", ".tiff" };
        private FolderBrowserDialog dialogFolder;

        public Selecteur()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Init(folder);
        }

        private void textBox1_Load()
        {
            this.textBox1.Text = folder;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            folder = this.textBox1.Text.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            dialogFolder = new FolderBrowserDialog();
            DialogResult result = dialogFolder.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    this.textBox1.Text = dialogFolder.SelectedPath;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("An error occurred while attempting to load the file. The error is:"
                                    + System.Environment.NewLine + exp.ToString() + System.Environment.NewLine);
                }
                Invalidate();
            }

            // Cancel button was pressed.
            else if (result == DialogResult.Cancel)
            {
                return;
            }

        }

        private void Init(string path)
        {
            this.button1.Enabled = false;
            this.button2.Enabled = false;
            this.textBox1.ReadOnly = true;
            this.listFile = new List<FileInfo>();

            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;

            this.worker = new BackgroundWorker();
            this.worker.WorkerReportsProgress = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            worker.ReportProgress(0, "Calcul...");
            FlattenFiles(path);
            this.label4.Text = listFile.Count + " Pictures to sort... Veuillez patienter.";
            worker.ReportProgress(0,"Waiting...");
            worker.RunWorkerAsync();


        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string inputDir = this.textBox2.Text;
            var backgroundWorker = sender as BackgroundWorker;
            var years = listFile.GroupBy(m => m.LastWriteTime.Year).OrderByDescending(m=>m.Key);
            double progressPerFile = (100 / (double)listFile.Count());
            double progress = 0;

            foreach (var year in years)
            {
                var newPathYear = Path.Combine(inputDir, year.Key.ToString());
                if (!Directory.Exists(newPathYear))
                    Directory.CreateDirectory(newPathYear);

                var months = year.GroupBy(m => m.LastWriteTime.ToString("MM-yyyy"));
                foreach (var month in months.OrderBy(m=>m.Key))
                {
                    var newPathMonth = Path.Combine(newPathYear, month.Key.ToString());
                    if (!Directory.Exists(newPathMonth)) { 
                        Directory.CreateDirectory(newPathMonth);
                        int counter = 1;
                    }
                    else
                    {

                    }

                    foreach (FileInfo file in month.OrderBy(m=>m.CreationTime))
                    {
                        progress = progress + progressPerFile;
                        backgroundWorker.ReportProgress((int)Math.Round(progress), file.FullName);

                        string newFilePath = GetFileDir(newPathMonth, file);
                        if (!File.Exists(newFilePath)) { 
                            file.CopyTo(newFilePath,true);
                        }
                    }
                }
            }
        }

        private string GetFileDir(string newPathMonth, FileInfo file)
        {
            string newFileName = file.LastWriteTime.ToString("ddMMyyyy-HHmmss") + "-" + file.Length + file.Extension;
            var newFilePath = Path.Combine(newPathMonth, newFileName);
            return newFilePath;
        }





        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            this.label3.Text = e.UserState.ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar1.Value = 100;
            this.button1.Enabled = true;
            this.button2.Enabled = true;
            this.textBox1.ReadOnly = false;
            // TODO: do something with final calculation.
            worker.Dispose();
            MessageBox.Show("Tri des photos terminé !");
        }

        private void FlattenFiles(string path)
        {

            Console.WriteLine(path);
            var files = Directory.GetFiles(path);
            var folders = Directory.GetDirectories(path);

            foreach (string file in files)
            {
                if (extensions.Contains(Path.GetExtension(file).ToLower()))
                {
                    listFile.Add(new FileInfo(file));
                }
            }

            foreach (string folder in folders)
            {
                FlattenFiles(folder);
            }
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dialogFolder = new FolderBrowserDialog();
            DialogResult result = dialogFolder.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    this.textBox2.Text = dialogFolder.SelectedPath;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("An error occurred while attempting to load the file. The error is:"
                                    + System.Environment.NewLine + exp.ToString() + System.Environment.NewLine);
                }
                Invalidate();
            }

            // Cancel button was pressed.
            else if (result == DialogResult.Cancel)
            {
                return;
            }

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void Calculate(int i)
        {
            double pow = Math.Pow(i, i);
        }


    }
}

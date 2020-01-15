using ParseLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AntonPaarTask
{
    public partial class MainForm : Form
    {
        private Int64 maxLines = Int64.MaxValue;
        private Int64 currentLine = 0;

        public MainForm()
        {
            InitializeComponent();
            this.initializeBackgroundWorker();
        }

        private void initializeBackgroundWorker()
        {
            this.backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataLayer.dataLayer.ClearWordOccurrence();
            this.resetChart();
            this.resetDataGridView();
            this.progressBar1.Maximum = 100;
            this.openFileDialog1.Filter = "Textdateien (*.txt)|*.txt";
            DialogResult result = openFileDialog1.ShowDialog();
            this.currentLine = 0;
            if (result == DialogResult.OK)
            {
                this.progressBar1.Maximum = 100;
                this.progressBar1.Step = 1;
                this.progressBar1.Value = 0;
                this.progressBar1.Visible = true;
                this.btn_cancel.Visible = true;
                this.backgroundWorker1.WorkerReportsProgress = true;
                this.backgroundWorker1.WorkerSupportsCancellation = true;
                this.backgroundWorker1.RunWorkerAsync();
            }
        }

        private void saveWordOccurenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.Filter = "CSV|*.csv";
            this.saveFileDialog1.Title = "Exportieren in CSV-Datei";
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(this.saveFileDialog1.FileName);
                foreach (KeyValuePair<string, int> item in DataLayer.dataLayer.GetOrderedWordOccurrencePairs())
                {
                    writer.WriteLine("{0} \t {1}", item.Key, item.Value);
                }
                writer.Close();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            this.currentLine = 0;
            this.maxLines = File.ReadLines(openFileDialog1.FileName).Count();
            using (StreamReader sr = File.OpenText(openFileDialog1.FileName))
            {
                string line = String.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (backgroundWorker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }
                    foreach (string word in StringParseLibrary.GetWordsInString(line))
                    {
                        DataLayer.dataLayer.AddWord(word);
                    }
                    this.currentLine++;
                    backgroundWorker.ReportProgress((int)(this.currentLine * 100 / this.maxLines));
                }
            }
        }

        private void resetDataGridView()
        {
            this.dataGridView1.DataSource = null;
        }

        private void resetChart()
        {
            this.chart1.Series.Clear();
        }

        private void initLineChart()
        {
            this.chart1.Series.Clear();
            this.chart1.Legends.Clear();
            Series series = this.chart1.Series.Add("Occurrences");
            this.chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            series.ChartType = SeriesChartType.Line;
            foreach (KeyValuePair<string, int> item in DataLayer.dataLayer.GetOrderedWordOccurrencePairs())
            {
                series.Points.AddY(item.Value);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                Console.WriteLine("Canceled");
                DataLayer.dataLayer.ClearWordOccurrence();
            }
            else if (e.Error != null)
            {
                Console.WriteLine("Error: " + e.Error.Message);
                DataLayer.dataLayer.ClearWordOccurrence();
            }
            else
            {
                Console.WriteLine("Done");
                var orderedWordOccurrenceList = DataLayer.dataLayer.GetOrderedWordCountList();
                dataGridView1.DataSource = orderedWordOccurrenceList;
                dataGridView1.Columns[0].HeaderText = "Word";
                dataGridView1.Columns[1].HeaderText = "Occurrences";
                saveWordOccurenceToolStripMenuItem.Enabled = true;
                this.initLineChart();
            }
            btn_cancel.Visible = false;
            progressBar1.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
            }
        }
    }
}

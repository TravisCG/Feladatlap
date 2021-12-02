using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Feladatlap
{
    public partial class Form1 : Form
    {
        private bool hasHeader;
        private bool hasExam;
        private List<Exam> exams;
        private int examstartnum;
        public Form1()
        {
            InitializeComponent();
            hasHeader = false;
            hasExam = false;
            exams = new List<Exam>();
            try
            {
                examstartnum = Int16.Parse(System.IO.File.ReadAllText("examnum.txt"));
            } catch(System.IO.FileNotFoundException)
            {
                examstartnum = 0;
            }
        }

        private void FillExams()
        {
            Regex numreg = new Regex("^[0-9]+\\.,", RegexOptions.Compiled);
            int counter = 0;
            string prevline = "";

            foreach (string line in examSource.Lines)
            {
                Match m = numreg.Match(line);
                if (m.Success)
                {
                    exams.Add(new Exam(line));
                    counter = 1;
                    continue;
                }
                if (line.Length == 0)
                {
                    continue;
                }

                if (counter % 2 == 0)
                {
                    exams.Last().add(prevline, line);
                }
                else
                {
                    prevline = line;
                }
                counter++;
            }
        }

        private void ShuffleExams()
        {
            Random rnd = new Random();
            // Shuffling thems
            for (int i = 0; i < 100; i++)
            {
                int from = rnd.Next(0, exams.Count);
                int to = rnd.Next(0, exams.Count);
                Exam tmp = exams[to];
                exams[to] = exams[from];
                exams[from] = tmp;
            }
        }

        private void megnyitasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openRTF.FileName = "*.rtf";
            DialogResult res = openRTF.ShowDialog();
            if(res == DialogResult.OK)
            {
                examSource.LoadFile(openRTF.FileName);
                hasExam = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openHeaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openRTF.FileName = "*.txt";
            DialogResult res = openRTF.ShowDialog();
            if(res == DialogResult.OK)
            {
                // read the header file
                hasHeader = true;
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            if (!hasHeader)
            {
                MessageBox.Show("Header file not opened");
                return;
            }
            if (!hasExam)
            {
                MessageBox.Show("Exam file not opened");
                return;
            }

            FillExams();
            ShuffleExams();

            if(questionNumSet.Value > exams.Count)
            {
                MessageBox.Show("You want more questions than the number of available topics");
                return;
            }

            Random rnd = new Random();
            for (int examnum = examstartnum; examnum < examstartnum + examNumSet.Value; examnum++)
            {
                examDestination.Clear();
                answerDestination.Clear();
                examDestination.AppendText("No: " + examnum + "\n");
                answerDestination.AppendText("No: " + examnum + "\n");

                for (int i = 0; i < questionNumSet.Value; i++)
                {
                    int select = rnd.Next(0, exams[i].Count());
                    examDestination.AppendText(exams[i].getQuestion(select) + "\n");
                    answerDestination.AppendText(exams[i].getAnswer(select) + "\n");
                }

                if (checkBox2.Checked)
                {
                    examDestination.SaveFile(outPrefix.Text + ".question." + examnum + ".rtf");
                    answerDestination.SaveFile(outPrefix.Text + ".answer." + examnum + ".rtf");
                }

                if (checkBox1.Checked)
                {
                    // Shomehow need to print RTF
                }
            }
            examstartnum += (int)examNumSet.Value;
            System.IO.File.WriteAllText("examnum.txt", "" + examstartnum);
        }

        
    }
}

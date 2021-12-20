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
using System.Xml;

namespace Feladatlap
{
    public partial class Form1 : Form
    {
        private int examstartnum;
        private XmlDocument exams;
        public Form1()
        {
            InitializeComponent();
            exams = new XmlDocument();
        }

        private void megnyitasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openRTF.FileName = "*.xml";
            DialogResult res = openRTF.ShowDialog();
            if(res == DialogResult.OK)
            {
                exams.Load(openRTF.FileName);
                buttonGenerate.Enabled = true;
                XmlNodeList langnode = exams.GetElementsByTagName("language");
                foreach( XmlNode n in langnode)
                {
                    languageSelect.Items.Add(n.InnerText);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void createOneWorksheet(XmlNode examnode, int num)
        {
            HTMLcreator qHTML = new HTMLcreator();
            HTMLcreator aHTML = new HTMLcreator();
            Random rnd = new Random();
            XmlNodeList header = examnode.SelectNodes("header/element");
            foreach (XmlNode headeelement in header)
            {
                qHTML.addHeader(headeelement.InnerText);
            }
            XmlNodeList topics = examnode.SelectNodes("questions/topic");
            for (int i = 0; i < questionNumSet.Value; i++)
            {
                int topici = rnd.Next(0, topics.Count);
                XmlNodeList questions = topics[topici].SelectNodes("block");
                int questi = rnd.Next(0, questions.Count);
                string question = questions[questi].SelectSingleNode("question").InnerText;
                string answer = questions[questi].SelectSingleNode("answer").InnerText;
                qHTML.addTopic(question);
                aHTML.addTopic(answer);
            }

            questionHTML.DocumentText = qHTML.toString(OutputType.QUESTION);
            answerHTML.DocumentText = aHTML.toString(OutputType.ANSWER);
            if (checkBox2.Checked)
            {
                // Save worksheets
                System.IO.File.WriteAllText(outPrefix.Text + ".Q." + num + ".html",qHTML.toString(OutputType.QUESTION));
                System.IO.File.WriteAllText(outPrefix.Text + ".A." + num + ".html", aHTML.toString(OutputType.ANSWER));
            }

            if (checkBox1.Checked)
            {
                // Print
                questionHTML.Print();
                answerHTML.Print();
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            XmlNodeList l = exams.GetElementsByTagName("language");
            foreach(XmlNode n in l)
            {
                if(n.InnerText == (string)languageSelect.SelectedItem)
                {
                    XmlNode examnode = n.ParentNode;
                    for (int i = 0; i < examNumSet.Value; i++)
                    {
                        createOneWorksheet(examnode, i);
                    }
                    examstartnum += (int)examNumSet.Value;
                    System.IO.File.WriteAllText("examnum.txt", "" + examstartnum);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult r = saveFileDialog1.ShowDialog();
            if(r == DialogResult.OK)
            {
                outPrefix.Text = saveFileDialog1.FileName;
            }
        }
    }
}

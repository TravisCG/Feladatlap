using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        string old_header;
        string old_footer;
        const string keyName = @"Software\Microsoft\Internet Explorer\PageSetup";
        string examfilename;
        public Form1()
        {
            InitializeComponent();
            exams = new XmlDocument();
            RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true);
            old_footer = (string)key.GetValue("footer");
            old_header = (string)key.GetValue("header");
            key.SetValue("footer", "");
            key.SetValue("header", "");
        }

        private void megnyitasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openRTF.FileName = "*.xml";
            DialogResult res = openRTF.ShowDialog();
            if(res == DialogResult.OK)
            {
                try
                {
                    exams.Load(openRTF.FileName);
                    examfilename = openRTF.FileName;
                } catch(XmlException){
                    MessageBox.Show("Malformed XML file");
                    return;
                } catch(IOException)
                {
                    MessageBox.Show("Cannot open file");
                    return;
                }
                buttonGenerate.Enabled = true;
                XmlNodeList langnode = exams.GetElementsByTagName("language");
                foreach( XmlNode n in langnode)
                {
                    languageSelect.Items.Add(n.InnerText);
                }
                XmlNodeList seq = exams.GetElementsByTagName("sequence");
                examstartnum = Int32.Parse(seq[0].InnerText);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void createOneWorksheet(XmlNode examnode, int num)
        {
            HTMLcreator qHTML = new HTMLcreator(num);
            HTMLcreator aHTML = new HTMLcreator(num);
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
            // This magic is needed to print the document
            questionHTML.Navigate("about:blank");
            answerHTML.Navigate("about:blank");
            questionHTML.Document.OpenNew(true);
            answerHTML.Document.OpenNew(true);
            questionHTML.Document.Write(qHTML.toString(OutputType.QUESTION)); // This is need to print the document
            answerHTML.Document.Write(aHTML.toString(OutputType.ANSWER));
            questionHTML.DocumentText = qHTML.toString(OutputType.QUESTION); // This is need to see the document in the control
            answerHTML.DocumentText = aHTML.toString(OutputType.ANSWER);

            if (checkBox2.Checked)
            {
                // Save worksheets
                System.IO.File.WriteAllText(outPrefix.Text + ".Q." + num + ".html",qHTML.toString(OutputType.QUESTION));
                System.IO.File.WriteAllText(outPrefix.Text + ".A." + num + ".html", aHTML.toString(OutputType.ANSWER));
            }

            if (checkBox1.Checked)
            {
                questionHTML.Print();
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        { 
            XmlNodeList l = exams.GetElementsByTagName("language");
            foreach (XmlNode n in l)
            {
                if (n.InnerText == (string)languageSelect.SelectedItem)
                {
                    XmlNode examnode = n.ParentNode;
                    for (int i = 0; i < examNumSet.Value; i++)
                    {
                        createOneWorksheet(examnode, examstartnum + i);
                    }
                    examstartnum += (int)examNumSet.Value;
                    exams.GetElementsByTagName("sequence")[0].InnerText = "" + examstartnum;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true);
            key.SetValue("footer", old_footer);
            key.SetValue("header", old_header);
            exams.Save(examfilename); //FIXME if the file cannot be writable the information will lost
        }
    }
}

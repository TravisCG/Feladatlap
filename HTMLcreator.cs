using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feladatlap
{
    enum OutputType { QUESTION, ANSWER};
    class HTMLcreator
    {
        private string html;
        private List<string> headerelements;
        private List<string> topics;
        private int serialnum;
        public HTMLcreator(int num)
        {
            headerelements = new List<string>();
            topics = new List<string>();
            serialnum = num;
        }

        private void addHTMLHeader()
        {
            html = "<!DOCTYPE html>\n<html><head><style>\n" +
                "@media print,screen {\n" +
                "@page {\nsize: A4;\nmargin:0;width:100%;\n}\n" +
                ".question { \n" +
                "margin-bottom: 1cm;\n" +
                "text-decoration: underline;\n" +
                "font-size: 12px;\n" +
                "}\n" +
                ".answer {\n" +
                "margin-bottom: 0.5cm;\n" +
                "}\n" +
                ".examheader {\n" +
                "width: 100%;\n" +
                "overflow: hidden;\n" +
                "margin-bottom: 0.5cm;\n" +
                "font-size: 10px;\n" +
                "}\n" +
                ".headerelement {\n" +
                "width: 150px;\n" +
                "float: left;\n" +
                "}\n}\n" +
                "</style>\n<meta charset=\"UTF-8\">\n</head>\n<body>\n";
        }

        private void addHTMLFooter()
        {
            html = html + "</body></html>";
        }

        public void addHeader(string element)
        {
            headerelements.Add(element);
        }

        public void addTopic(string element)
        {
            topics.Add(element);
        }

        public string toString(OutputType t)
        {
            addHTMLHeader();
            setSerial();
            if(t == OutputType.QUESTION)
            {
                generateExamHeader();
                generateTopics("question");
            } else
            {
                generateTopics("answer");
            }
            addHTMLFooter();

            return html;
        }

        private void generateTopics(string cls)
        {
            int topicnum = 1;
            foreach(string t in topics)
            {
                html = html + "<div class=\"" + cls + "\">" + topicnum + ". " + t + "</div>\n";
                topicnum++;
            }
        }

        private void setSerial()
        {
            html = html + "<div>" + serialnum + "</div>\n";
        }

        private void generateExamHeader()
        {
            html = html + "<div class=\"examheader\">\n";
            foreach(string e in headerelements)
            {
                html = html + "<div class=\"headerelement\">" + e + "</div>\n";
            }
            html = html + "</div>\n";
        }
    }
}

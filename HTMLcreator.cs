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
        public HTMLcreator()
        {
            headerelements = new List<string>();
            topics = new List<string>();
        }

        private void addHTMLHeader()
        {
            html = "<!DOCTYPE html>\n<html><head><style>\n" +
                "@page {\nsize: A4;\nmargin:0;\n}\n" +
                ".question { \n" +
                "margin-bottom: 2cm;\n" +
                "text-decoration: underline;\n" +
                "font-size: 10px;\n" +
                "}\n" +
                ".answer {\n" +
                "margin-bottom: 1cm;\n" +
                "}\n" +
                ".examheader {\n" +
                "width: 100%;\n" +
                "overflow: hidden;\n" +
                "margin-bottom: 1cm;\n" +
                "}\n" +
                ".headerelement {\n" +
                "width: 150px;\n" +
                "float: left;\n" +
                "}\n" +
                "</style></head><body>\n";
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

        private void generateExamHeader()
        {
            html = html + "<div class=\"examheader\">";
            foreach(string e in headerelements)
            {
                html = html + "<div class=\"headerelement\">" + e + "</div>";
            }
            html = html + "</div>";
        }
    }
}

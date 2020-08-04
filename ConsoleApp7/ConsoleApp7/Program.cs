using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestChart;
using System.Xml;

namespace ConsoleApp7
{
    class Program 
    {
        static void Main(string[] args)
        {
            XmlDocument Sales_Comparison = (XmlDocument)TestChart.MyChart.GetDataFromXML(Environment.CurrentDirectory + "\\Sales comparsion.xml");
            TestChart.MyChart chart = new TestChart.MyChart();

            bool log_create_chart = chart.Create_Chart(Sales_Comparison, "Sales 2019", "Sales 2020");
            bool log_save_chart = chart.Save_Chart(Environment.CurrentDirectory);
            
            Console.WriteLine("Creating chart: " + log_create_chart);
            Console.WriteLine("Saving chart: " + log_save_chart);

            Console.ReadKey();
        }
    }
}

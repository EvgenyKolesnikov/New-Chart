using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MccCharts;
using System.Xml;

namespace ConsoleApp7
{
    class Program 
    {
        static void Main(string[] args)
        {
            XmlDocument Sales_Comparison = (XmlDocument)MccCharts.GetData.GetDataFromXML(Environment.CurrentDirectory + "\\Sales comparsion.xml");
            MccCharts.Charts chart = new MccCharts.Charts();

            bool log_create_chart = chart.Create_Charts(Sales_Comparison);
            bool log_save_chart = chart.Save_Chart(Environment.CurrentDirectory,"Jpeg");
            
            Console.WriteLine("Creating chart: " + log_create_chart);
            Console.WriteLine("Saving chart: " + log_save_chart);
            Console.WriteLine("Save Path: " + Environment.CurrentDirectory);

            Console.ReadKey();
        }
    }
}

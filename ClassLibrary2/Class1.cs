using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Xml;

namespace TestChart
{
    public class MyChart
    {
        Chart chart = new Chart();
        public delegate void ErrorEventHandler(object sender, string e);
        public event ErrorEventHandler Error;
        protected virtual void OnError(string e) { if (Error != null) {Error(this, e); }; }

        public bool Create_Chart(XmlDocument Data)
        {
            Dictionary<object, string> chart_types = new Dictionary<object, string>
            {
                {System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar, "Bar" },
                {System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column, "Column" },
                {System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area, "Area" },

                {System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie, "Pie" },
                {System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut, "Doughnut" },

                {System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line, "Line" },
                {System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine, "Stepline"},

                { System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedBar , "StackedBar " },
                { System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn  , "StackedColumn" }
            };

            Dictionary<object, string> chart_colors = new Dictionary<object, string>
            {
                {System.Drawing.Color.FromArgb(255,70,58),"Red" },
                {System.Drawing.Color.FromArgb(209,213,225),"Light gray"},
                {System.Drawing.Color.FromArgb(89,123,161),"Gray"},
                {System.Drawing.Color.FromArgb(0,52,126),"Blue" },
                {System.Drawing.Color.FromArgb(2,33,98),"Dark blue"},
                {System.Drawing.Color.FromArgb(254,254,10),"Yellow"},
                {System.Drawing.Color.FromArgb(23,168,89),"Green"}
            };

            Dictionary<object, string> Border_Dash_Style = new Dictionary<object, string>
            {
                {ChartDashStyle.Dash,"Dash" },
                {ChartDashStyle.DashDot, "Dash dot" },
                {ChartDashStyle.DashDotDot, "Dash dot dot" },
                {ChartDashStyle.Dot, "Dot" },
                {ChartDashStyle.NotSet, "NotSet" },
                {ChartDashStyle.Solid , "Solid" }
            };

            Dictionary<string, string> mark_style = new Dictionary<string, string>
            {
                {"#PERCENT","Percent" },
                {"#VAL", "Value" },
                {"","null" }
            };

            try
            {
                XmlNodeList Series = Data.SelectNodes("//Chart//Series");

                chart.Titles.Add(Data.SelectSingleNode("//Chart/Title").InnerText);                               //Имя диаграммы 
                chart.Width = Convert.ToInt32(Data.SelectSingleNode("//Chart/Size/Width").InnerText);            //Высота
                chart.Height = Convert.ToInt32(Data.SelectSingleNode("//Chart/Size/Height").InnerText);         //Ширина
                
                chart.ChartAreas.Add(Series[0].Attributes[0].InnerText);                                             //создание ChartArea
                chart.ChartAreas[Series[0].Attributes[0].InnerText].AxisX.Interval = 1;                             //интервал сетки
                chart.ChartAreas[Series[0].Attributes[0].InnerText].AxisX.MajorGrid.LineColor = Color.DarkGray;    //Цвет сетки (Ось Х)
                chart.ChartAreas[Series[0].Attributes[0].InnerText].AxisY.MajorGrid.LineColor = Color.DarkGray;   //Цвет сетки (Ось Y)

                foreach (XmlNode xNode in Series)
                {
                    string Series_id = xNode.Attributes[0].InnerText;

                    chart.Series.Add(Series_id);
                    chart.Series[Series_id].IsVisibleInLegend = Convert.ToBoolean(Data.SelectSingleNode("//Chart//Series[@id = '" + Series_id + "']//IsVisibleInLegend").InnerText);                                                  // Легенда
                    chart.Series[Series_id].Color = (System.Drawing.Color)chart_colors.FirstOrDefault(x => x.Value == Data.SelectSingleNode("//Chart//Series[@id = '" + Series_id + "']/Color").InnerText).Key;                      // Цвет
                    chart.Series[Series_id].BorderWidth = Convert.ToInt32(Data.SelectSingleNode("//Chart//Series[@id = '" + Series_id + "']/BorderWidth").InnerText);                                                               // Толщина линий (Line only)
                    chart.Series[Series_id].BorderDashStyle = (ChartDashStyle)Border_Dash_Style.FirstOrDefault(x => x.Value == Data.SelectSingleNode("//Chart//Series[@id = '" + Series_id + "']/BorderDashStyle").InnerText).Key; // Стиль линии (line only)
                    chart.Series[Series_id].ChartType = (SeriesChartType)chart_types.FirstOrDefault(x => x.Value == Data.SelectSingleNode("//Chart//Series[@id = '" + Series_id + "']//ChartType").InnerText).Key;                // Тип диаграммы
                    chart.Series[Series_id].Label = mark_style.FirstOrDefault(x => x.Value == Data.SelectSingleNode("//Chart//Series[@id = '" + Series_id + "']/Label").InnerText).Key;                                          // Подписи данных
                    chart.Series[Series_id].Font = new System.Drawing.Font("Verdana", 9, System.Drawing.FontStyle.Bold);                                                                                                        //стиль шрифта

                    if (new[] { "Pie", "Doughnut" }.Contains(Data.SelectSingleNode("//Chart//Series[@id = '" + Series_id + "']//ChartType").InnerText)) //исключение для pie-диаграммы
                    {
                        chart.Series[Series_id].LegendText = "#VALX";
                    }

                    XmlNodeList Names = Data.SelectNodes("//Chart/Series[@id = '" + Series_id + "']/Point/Name");        //cоздание DataPoints
                    XmlNodeList Values = Data.SelectNodes("//Chart/Series[@id = '" + Series_id + "']/Point/Value");
                    for (int i = 0; i < Names.Count; i++)
                    {
                        chart.Series[Series_id].Points.AddXY(Names[i].InnerText, Values[i].InnerText);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
               OnError(e.Message);
               return false;
            }
        }

        
        public bool Save_Chart(string Path, string Format)
        {
            Dictionary<object, string> image_format = new Dictionary<object, string>
            {
                {ChartImageFormat.Bmp,"Bmp" },
                {ChartImageFormat.Emf, "Emf"},
                {ChartImageFormat.EmfDual,"EmfDual"},
                {ChartImageFormat.EmfPlus, "EmfPlus"},
                {ChartImageFormat.Gif, "Gif"},
                {ChartImageFormat.Jpeg, "Jpeg"},
                {ChartImageFormat.Png,"Png" },
                {ChartImageFormat.Tiff,"Tiff" }
            };
            ChartImageFormat ExportFormat = (ChartImageFormat)image_format.FirstOrDefault(x => x.Value == Format).Key;

            try
            {
                string PathName = Path + "\\chart." + Format;
                chart.SaveImage(PathName, ExportFormat);
                
                return true;
            }
            catch (Exception e)
            {
                OnError(e.Message);
                return false;
            }
        }
    }
    public class GetData
    {
        public static object GetDataFromXML(string Path)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Path);

            return xDoc;
        }


    }







}

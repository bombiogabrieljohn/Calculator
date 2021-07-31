using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DemoCalculator.Model
{
    class DataAccess
    {

        String filepath = @"CalculatorHistory.xml";

        public void CreateXMLDatabase()
        {

            XmlTextWriter xtw;
            xtw = new XmlTextWriter(filepath, Encoding.UTF8);
            xtw.WriteStartDocument();
            xtw.WriteStartElement("CalculatorHistory");
            xtw.WriteEndElement();
            xtw.Close();
        }

        public void InsertData(CalcuHistoryModel obj)
        {
            var xDoc = XDocument.Load(filepath);
            var count = xDoc.Descendants("Calculation").Count();
            var newUser = new XElement("Calculation",
                              new XElement("Hist_ID", count + 1),
                              new XElement("Hist_Action", obj.Hist_Action),
                              new XElement("Hist_Value", obj.Hist_Value));
            xDoc.Root.Add(newUser);
            xDoc.Save(filepath);
        }

        public List<CalcuHistoryModel> GetData()
        {
            CalcuHistoryModel obj = new CalcuHistoryModel();
            List<CalcuHistoryModel> listObj = new List<CalcuHistoryModel>();

            XDocument xDoc = XDocument.Load(filepath);

            listObj = (from item in xDoc.Element("CalculatorHistory").Elements("Calculation")
                       select new CalcuHistoryModel
                       {
                           Hist_ID = Convert.ToInt32(item.Element("Hist_ID").Value),
                           Hist_Action = item.Element("Hist_Action").Value,
                           Hist_Value = item.Element("Hist_Value").Value
                       }).ToList();

            return listObj;
        }

    }
}

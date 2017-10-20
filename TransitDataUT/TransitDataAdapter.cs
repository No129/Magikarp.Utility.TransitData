using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace TransitDataUT
{
    [Binding]
    public sealed class TransitDataAdapter
    {
        [Given(@"僱員資料")]
        public void Given僱員資料(Table table)
        {
            EmployeeInfo objEmployeeInfo = table.CreateInstance<EmployeeInfo>();
            List<XElement> xeToDoList = new List<XElement>();
            List<ToDoInfo> objToDoList = new List<ToDoInfo>();
            ToDoInfo objToDoInfo1 = new ToDoInfo() { Title = "ToDo1", Description = "Do Something." };
            ToDoInfo objToDoInfo2 = new ToDoInfo() { Title = "2", Description = "Do Something 2" };

            objEmployeeInfo.TEL = new List<string>() { "abc", "def" };
            objToDoList.Add(objToDoInfo1);
            objToDoList.Add(objToDoInfo2);
            
            foreach(ToDoInfo objToDo in objToDoList)
            {
                XElement objElement = XElement.Parse(new Magikarp.Utility.TransitData.TransitDataAdapter().Export<ToDoInfo>(objToDo));
                xeToDoList.Add(objElement);
            }
            objEmployeeInfo.ToDoList = xeToDoList;

            ScenarioContext.Current.Set<EmployeeInfo>(objEmployeeInfo , "Employee");
        }

        [Given(@"字串中介資料")]
        public void Given字串中介資料(string pi_sSource)
        {
            ScenarioContext.Current.Set<string>(pi_sSource, "Source");
        }

        [When(@"要求轉換為字串中介資料")]
        public void When要求轉換為字串中介資料()
        {
            string sResult = string.Empty;
            EmployeeInfo objEmployee = ScenarioContext.Current.Get<EmployeeInfo>("Employee");

            sResult = new Magikarp.Utility.TransitData.TransitDataAdapter().Export<EmployeeInfo>(objEmployee);
            ScenarioContext.Current.Set<string>(sResult, "Result");
        }

        [When(@"要求轉換為 Employee 資料物件")]
        public void When要求轉換為Employee資料物件()
        {
            string sSource = ScenarioContext.Current.Get<string>("Source");
            EmployeeInfo objResult = null;

            objResult = new Magikarp.Utility.TransitData.TransitDataAdapter().Loading<EmployeeInfo>(sSource);
            ScenarioContext.Current.Set<EmployeeInfo>(objResult, "Result");
        }

        [Then(@"得到字串中介資料")]
        public void Then得到字串中介資料(string pi_sExpected)
        {
            string sActual = ScenarioContext.Current.Get<string>("Result");
            System.Xml.Linq.XElement objExpected = System.Xml.Linq.XElement.Parse(pi_sExpected);

            Assert.AreEqual(objExpected.ToString(), sActual);
        }

        [Then(@"得到 Employee 資料物件")]
        public void Then得到Employee資料物件(Table table)
        {
            EmployeeInfo objActual = ScenarioContext.Current.Get<EmployeeInfo>("Result");
                       
            table.CompareToInstance<EmployeeInfo>(objActual);
        }

        [Then(@"得到 Employee 資料物件包含電話清單")]
        public void Then得到Employee資料物件包含電話清單(Table table)
        {
            EmployeeInfo objActual = ScenarioContext.Current.Get<EmployeeInfo>("Result");
            List<string> objExpected = new List<string>();

            foreach(TableRow objRow in table.Rows)
            {
                objExpected.Add(objRow["TEL"]);
            }

            CollectionAssert.AreEqual(objExpected, objActual.TEL );
        }


    }
}

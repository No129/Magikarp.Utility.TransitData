using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TransitDataUT
{
    [Binding]
    public sealed class TransitDataAdapter
    {
        [Given(@"僱員資料")]
        public void Given僱員資料(Table table)
        {
            ScenarioContext.Current.Set<EmployeeInfo>(table.CreateInstance<EmployeeInfo>(), "Employee");
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

    }
}

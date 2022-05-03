using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;



namespace VadenStock.View.Components.Containers
{
    public partial class Table : Border
    {
        public List<Header> Headers { get; private set; }
        public List<Row> Rows { get; private set; }



        public Table()
        {
            InitializeComponent();

            Headers = new();
            Rows = new();
        }



        public void Draw()
        {
            foreach (Header header in Headers)
            {
                //_GridContainer.Children.Add()
            }
        }



        public class Header
        {
            public string Content { get; private set; }


            private Header(string content)
            {
                Content = content;
            }


            public static Header Data(string data)
            {
                return new Header(data);
            }
        }



        public class Row
        {
            public string Content { get; private set; }


            private Row(string content)
            {
                Content = content;
            }


            public static Row Data(string data)
            {
                return new Row(data);
            }
        }
    }
}

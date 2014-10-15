using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SharePointDataPrinting
{
    public partial class XtraReport1 : DevExpress.XtraReports.UI.XtraReport
    {

        public dynamic DataRow { get; set; }
        public XtraReport1()
        {
            InitializeComponent();
           
        }

        private void XtraReport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel3.Text = this.DataRow[4];
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint;
using DevExpress.XtraTreeList;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Newtonsoft.Json;
using System.Net.Http;
using DevExpress.XtraGrid.Views.Grid;

namespace SharePointDataPrinting
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            using (ClientContext spContext = new ClientContext("http://myipektest.ipek.edu.tr"))
            {
                Web spWeb = spContext.Web;
                ListCollection spLists = spContext.Web.Lists;
                spContext.Load(spLists);
                spContext.ExecuteQuery();

                List<Items> spListNames = new List<Items>();

                foreach (List listItem in spLists)
                {
                    spListNames.Add(new Items { Title = listItem.Title });

                }

                treeList1.DataSource = spListNames;


            }



        }

        private void treeList1_Click(object sender, EventArgs e)
        {
            var Title = treeList1.FocusedNode[treeList1.Columns[0]].ToString();

            BindGridFromSharePoint(Title);

        }

        private void BindGridFromSharePoint(string Title)
        {
            using (ClientContext spContext = new ClientContext("http://myipektest.ipek.edu.tr"))
            {
                Web spWeb = spContext.Web;


                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View/>";

                List spLists = spContext.Web.GetList("http://myipektest.ipek.edu.tr/lists/" + Title.Replace(" ", "%20"));
                var spSelectedListItems = spLists.GetItems(camlQuery);
                spContext.Load(spLists);
                spContext.Load(spSelectedListItems);

                spContext.ExecuteQuery();

                DataTable dt = new DataTable();
                foreach (var item in spSelectedListItems)
                {
                    try
                    {
                        foreach (var item1 in item.FieldValues)
                        {
                            dt.Columns.Add(item1.Key, typeof(string));

                        }
                    }
                    catch (Exception)
                    {

                        // throw;
                    }
                    DataRow dr1 = dt.NewRow();
                    foreach (var item1 in item.FieldValues)
                    {
                        dr1[item1.Key] = item1.Value;
                    }
                    dt.Rows.Add(dr1.ItemArray);
                }
                gridControl1.DataSource = dt;

            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DoRowDoubleClick(view, pt);

        }
        private static void DoRowDoubleClick(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
             if (info.InRow || info.InRowCell)
                {
                    string colCaption = info.Column == null ? "N/A" : info.Column.GetCaption();
                    DataRow dataRow = (DataRow)view.GetDataRow(info.RowHandle);// view.GetDataRow(view.GetSelectedRows()[0]);

                    int id = Convert.ToInt16(view.GetRowCellValue(info.RowHandle, "id"));
                    XtraReport1 r = new XtraReport1();
                                      r.DataRow = dataRow;
                    DevExpress.XtraReports.UI.ReportPrintTool report = new DevExpress.XtraReports.UI.ReportPrintTool(r);



                    report.ShowPreview();
                }
        }
    }

    public class Items
    {
        public string Title { get; set; }
    }


}

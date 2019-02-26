using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace XUnitCompareAPIs.Common
{
    class ExcelExtension
    {
        public void CreateExcel()
        {
            var pck = new ExcelPackage();
            var ws = pck.Workbook.Worksheets.Add("Sample1");

            ws.Cells["A1"].Value = "Sample 1";
            ws.Cells["A1"].Style.Font.Bold = true;
            var shape = ws.Drawings.AddShape("Shape1", eShapeStyle.Rect);
            shape.SetPosition(50, 200);
            shape.SetSize(200, 100);
            shape.Text = "Sample 1 saves to the Response.OutputStream";

            pck.SaveAs(new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Web.Sample1.xlsx")));
        }
        private void UpdateExcel(string sheetName, int row, int col, string data)
        {
            
        }

        private void Readexcel(string sheetName, int row, int col, string data)
        {

        }

    }
}

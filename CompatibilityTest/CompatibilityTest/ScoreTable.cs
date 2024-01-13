using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace CompatibilityTest
{
    public class ScoreTable
    {
        private readonly Excel.Application m_xlApp;
        private readonly Excel.Workbook m_xlWorkbook;
        private readonly Excel._Worksheet m_xlWorksheet;
        private readonly Excel.Range m_xlRange;
        private readonly int m_height;
        private readonly int m_width;
        public Score GetScore(char _char)
        {
            int _index = -1;
            for (int y = 0; y < m_height; y++)
            {
                if (_char == m_xlRange.Cells[y][0].Value2.ToString()[0])
                {
                    _index = y;
                    break;
                }
            }

            return new Score(_index == -1 ? -1 : int.Parse(m_xlRange.Cells[_index][1].Value2.ToString()));
        }
        public void CloseTable()
        {
            m_xlApp.Workbooks.Close();
            m_xlApp.Quit();

            Marshal.ReleaseComObject(m_xlWorksheet);
            Marshal.ReleaseComObject(m_xlWorkbook);
            Marshal.ReleaseComObject(m_xlApp);
        }
        public ScoreTable()
        {
            m_xlApp = new Excel.Application();
            m_xlWorkbook = m_xlApp.Workbooks.Open(@"C:\Users\KW\OneDrive\문서\datatable.xlsx");
            m_xlWorksheet = m_xlWorkbook.Sheets[1];
            m_xlRange = m_xlWorksheet.UsedRange;

            m_height = m_xlWorksheet.Cells.Find("*", System.Reflection.Missing.Value,
                               System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                               Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious,
                               false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

            m_width = m_xlWorksheet.Cells.Find("*", System.Reflection.Missing.Value,
                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                           Excel.XlSearchOrder.xlByColumns, Excel.XlSearchDirection.xlPrevious,
                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;

            CloseTable();
        }
    }
}

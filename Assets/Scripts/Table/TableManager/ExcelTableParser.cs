using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class ExcelTableParser
{
    public static string ExcelName = "Book.xlsx";
    public static string[] SheetNames = { "sheet1", "sheet2", "sheet3", "sheet4" };

    //public static List<Menu> SelectMenuTable(int tableId)
    //{
    //    DataRowCollection collect = ExcelTableParser.ReadExcel(SheetNames[tableId - 1]);
    //    List<Menu> menuArray = new List<Menu>();

    //    for (int i = 1; i < collect.Count; i++)
    //    {
    //        if (collect[i][1].ToString() == "") continue;

    //        Menu menu = new Menu();
    //        menu.m_Id = collect[i][0].ToString();
    //        menu.m_level = collect[i][1].ToString();
    //        menu.m_parentId = collect[i][2].ToString();
    //        menu.m_name = collect[i][3].ToString();
    //        menuArray.Add(menu);
    //    }
    //    return menuArray;
    //}

    ///// <summary>  
    ///// 读取 Excel 需要添加 Excel; System.Data;  
    ///// </summary>  
    ///// <param name="sheet"></param>  
    ///// <returns></returns>  
    //static DataRowCollection ReadExcel(string sheet)
    //{
    //    FileStream stream = File.Open(FilePath(ExcelName), FileMode.Open, FileAccess.Read, FileShare.Read);
    //    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

    //    DataSet result = excelReader.AsDataSet();
    //    //int columns = result.Tables[0].Columns.Count;  
    //    //int rows = result.Tables[0].Rows.Count;  
    //    return result.Tables[sheet].Rows;
    //}

    //static T[] Parse<T>(string name)
    //{

    //}

    private static void ReadExcel(string TableName)
    {
        WWW www = new WWW(Application.dataPath + "/Table/" + TableName + ".xlsx");
        string textAsset = www.text;
        Debug.Log("textAsset = "+ textAsset);
    }
}

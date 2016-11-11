using UnityEngine;
using System.Collections;

public class Data : ITableItem
{
    public int Id;
    public int Key()
    {
        return Id;
    }
}

public class DataManager : TableManager<Data, DataManager>
{
    public override string TableName()
    {
        return "Data";
    }
}

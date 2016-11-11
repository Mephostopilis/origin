using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Reflection;

public interface ITableItem
{
    int Key();
}

public interface ITableManager
{
    string TableName();
    object TableData { get; }
}

public abstract class TableManager<T, U> : SingletonTable<U>, ITableManager where T : ITableItem
{
    // abstract functions need tobe implements.
    public abstract string TableName();
    public object TableData { get { return mItemArray; } }

    // the data arrays.
    T[] mItemArray;
    Dictionary<int, int> mKeyItemMap = new Dictionary<int, int>();

    // constructor.
    internal TableManager()
    {
        SetData();
    }

    // get a item base the key.
    public T GetItem(int key)
    {
        int itemIndex;
        if (mKeyItemMap.TryGetValue(key, out itemIndex))
            return mItemArray[itemIndex];
        return default(T);
    }
	
    // get the item array.
	public T[] GetAllItem()
	{
		return mItemArray;
	}

    public void SetData()
    {
        // load from excel txt file.
        mItemArray = TableParser.Parse<T>(TableName());
        if (mItemArray == null) return;
        // build the key-value map.
        for (int i = 0; i < mItemArray.Length; i++)
            mKeyItemMap[mItemArray[i].Key()] = i;
    }


    public void OnWriteField(T[] configs, string TableName)
    {
		Debug.LogError ("configs = "+configs.Length);
        StringBuilder builders = new StringBuilder();
        FileInfo fi = new FileInfo(Application.dataPath + "/Table/" + "//" + TableName + ".txt");
        if (!fi.Exists)
        {
            FieldInfo[] fields = configs[0].GetType().GetFields();

			Debug.LogError (configs.Length);
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    builders.Append(fields[i].Name);
                    builders.Append("\t");
                }
                builders.Append("\r\n");
            }
        }
        else
        {
            WWW www = new WWW(Application.dataPath + "/Table/" + TableName + ".txt");
            string textAsset = www.text;
            string[] lines = textAsset.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i < 2)
                {
                    builders.Append(lines[i]);
                    if (i < lines.Length - 1)
                        builders.Append("\r\n");
                }
            }
        }

		Debug.LogError (builders.Length);
        List<T> lst = new List<T>(configs);
//        T[] allItems = GetAllItem();
//        if (allItems != null && allItems.Length > 0)
//        {
//            for (int i = 0; i < allItems.Length; i++)
//            {
//                string line = SetFieldInfo(allItems[i], ref lst);
//                builders.Append(line);
//                if (i < allItems.Length - 1)
//                    builders.Append("\r\n");
//            }
//        }
        for (int i = 0; i < lst.Count; i++)
        {
            string line = SetStringBuilder(lst[i]);
            builders.Append(line);
            if (i < lst.Count - 1)
                builders.Append("\r\n");
		}
        CreateOrOPenFile(Application.dataPath + "/Table/", TableName + ".txt", builders.ToString());
    }

    private string SetFieldInfo(T config, ref List<T> configs)
    {
        T updateConfigs = default(T);
        T targetConfig = config;
        FieldInfo[] fields = config.GetType().GetFields();
        for (int i = 0; i < configs.Count; i++)
        {
            FieldInfo[] updateFields = configs[i].GetType().GetFields();
            if (updateFields[0].GetValue(configs[i]).ToString() == fields[0].GetValue(targetConfig).ToString())
            {
                updateConfigs = configs[i];
            }
        }
        if (updateConfigs != null)
        {
            targetConfig = updateConfigs;
            configs.Remove(updateConfigs);
        }
        return SetStringBuilder(targetConfig);
    }

    private string SetStringBuilder(T targetConfig)
    {
        FieldInfo[] fields = targetConfig.GetType().GetFields();
        StringBuilder builders = new StringBuilder();
        for (int i = 0; i < fields.Length; i++)
        {
            builders.Append(fields[i].GetValue(targetConfig));
            builders.Append("\t");
        }
        return builders.ToString();
    }

    private void CreateOrOPenFile(string path, string name, string info)
    {
        StreamWriter sw;
        FileInfo fi = new FileInfo(path + "//" + name);
        sw = fi.CreateText();
        sw.WriteLine(info);
        sw.Close();
        //其实Close方法内部已经实现了Dispose方法
        sw.Dispose();
    }
}
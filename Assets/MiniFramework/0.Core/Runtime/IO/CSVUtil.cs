using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniFramework
{
    class CSVUtil
    {
        public static CSVData FromCSV(string txt)
        {
            CSVData csv = new CSVData();
            string[] rows = txt.Split('\n');
            string[] title = rows[0].Split(',');
            for (int i = 0; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split(',');
                for (int j = 0; j < columns.Length; j++)
                {
                    csv[i][title[j]] = columns[j];
                }
            }
            return csv;
        }
    }

    class CSVData : IEnumerable
    {
        public CSVData()
        {

        }
        public CSVData(string str)
        {
            this.str = str;
        }
        private string str;
        private IDictionary<string, CSVData> data = new Dictionary<string, CSVData>();
        private string lineData;

        public CSVData this[string key]
        {
            get
            {
                if (!data.ContainsKey(key))
                {
                    data[key] = new CSVData();
                }
                return data[key];
            }
            set
            {
                data[key] = value;
                str = (string)value;
                lineData += str;
            }
        }
        public CSVData this[int key]
        {
            get
            {
                if (!data.ContainsKey(key.ToString()))
                {
                    data[key.ToString()] = new CSVData();
                }
                return data[key.ToString()];
            }
            set
            {
                data[key.ToString()] = value;
                str = (string)value;
                lineData += str;
            }
        }
        public static implicit operator CSVData(string data)
        {
            return new CSVData(data);
        }
        public static explicit operator string(CSVData data)
        {
            return data.str;
        }
        public override string ToString()
        {
            return lineData;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
}
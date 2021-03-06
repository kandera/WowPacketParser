using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WowPacketParser.Misc;

namespace WowPacketParser.SQL
{
    public class SQLFile : IDisposable
    {
        private StreamWriter _file;

        private readonly List<string> _sqls = new List<string>();

        public SQLFile(string file)
        {
            if (string.IsNullOrWhiteSpace(Settings.SQLFileName)) // only delete file if no global
                File.Delete(file);                               // file name was specified
            _file = new StreamWriter(file, true);
        }

        ~SQLFile()
        {
            Dispose(false);
        }

        public void WriteData(string sql)
        {
            if (_file == null)
                return;

            if (string.IsNullOrWhiteSpace(sql))
                return;

            _sqls.Add(sql);
        }

        public bool WriteToFile(string header)
        {
            if (_file == null)
                return false;

            if (_sqls.All(String.IsNullOrWhiteSpace))
                return false;

            _file.WriteLine(header);

            foreach (var sql in _sqls)
                _file.WriteLine(sql);

            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                _sqls.Clear();

            if (_file != null)
            {
                _file.Flush();
                _file.Close();
                _file = null;
            }
        }
    }
}

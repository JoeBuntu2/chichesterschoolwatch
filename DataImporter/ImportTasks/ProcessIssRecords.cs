using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using DataImporter.FileRecords;

namespace DataImporter.ImportTasks
{
    public class ProcessIssRecords : ITask
    {
        public void Run(IServiceProvider service)
        {
            var records =  ReadRecords();
            PrintStats(records);
            PrintRecords(records);
        }

        private void PrintStats(IEnumerable<IssRecord> records)    
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();
            foreach (var issRecord in records)
            {
                //if(!issRecord.UriQuery.StartsWith("url=/tutorials"))
                //    continue;
                if(issRecord.UriQuery.StartsWith("fbclid"))
                    continue;
                

                var path = issRecord.UriQuery;
                var queryIndex = issRecord.UriQuery.IndexOf("?", StringComparison.CurrentCulture);
                if (queryIndex > -1)
                    path = issRecord.UriQuery.Substring(0, queryIndex);

                if (!stats.ContainsKey(path))
                    stats.Add(path, 1);
                else
                    stats[path] = stats[path] + 1;

            }

            foreach (var stat in stats.OrderByDescending(x => x.Value))
            {
                Trace.WriteLine($"{stat.Value}: {stat.Key}");
            }
        }

        private void PrintRecords(IEnumerable<IssRecord> issRecords)
        {
            using (var fs = File.Open($"c:/temp/iis/combined.{DateTime.Now.Ticks}.csv", FileMode.Create, FileAccess.Write))
            {
                using (var textWriter = new StreamWriter(fs))
                {
                    using (var csvWriter = new CsvHelper.CsvWriter(textWriter))
                    {
                        csvWriter.WriteRecords(issRecords.OrderByDescending(x => x.DateTime));
                    }
                }
            }
        }

        private  List<IssRecord> ReadRecords()
        {
            var results = new List<IssRecord>();

            foreach (var file in Directory.GetFiles("c:/temp/iis"))
            {
                if(file.EndsWith(".csv"))
                    continue;
                
                using (var fs = File.OpenRead(file))
                {
                    using (var textReader = new StreamReader(fs))
                    {
                        using (var csv = new CsvReader(textReader, new Configuration{ Delimiter = " "}))
                        {
                            while (csv.Read())
                            {
                                var record = new IssRecord();

                                var firstField = csv.GetField(0);
                                if(firstField.StartsWith("#"))
                                    continue;
                                

                                var rawDateTime = csv.GetField((int) Fields.date);
                                rawDateTime = " " + csv.GetField((int) Fields.time);
                                record.DateTime = DateTime.Parse(rawDateTime, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal);

                                record.IP = csv.GetField((int) Fields.s_ip);

                                record.ClientIp = csv.GetField((int) Fields.c_ip);
                                record.UriStem = csv.GetField((int) Fields.cs_uri_stem);
                                record.UriQuery = csv.GetField((int) Fields.cs_uri_query);

                                record.TimeTaken = csv.GetField((int) Fields.time_taken);

                                var skip = false;
                                foreach (var path in PathFilters)
                                {
                                    if (record.UriStem.EndsWith(path))
                                        skip = true;
                                }
                                foreach (var ip in IpFilters)
                                {
                                    if (record.ClientIp.Equals(ip))
                                        skip = true;
                                }
                                if(skip)
                                    continue;
                                
                                results.Add(record);
                            }
                        }
                    }
                }
            }

            return results;
        }

        public string[] PathFilters = new[]
        {
            ".png",
            ".js",
            ".ico",
            "keepalive",
            ".css",
            ".gif"

        };

        public string[] IpFilters = new[]
        {
            "72.92.49.27",
            "100.11.32.130"
        };

        public class IssRecord
        {
            public DateTime DateTime { get; set; }
            public string IP { get; set; }
            public  string Method { get; set; }
            public string UriStem { get; set; }
            public string UriQuery { get; set; }
            public  string Port { get; set; }
            public string ClientIp { get; set; }
            public string UserAgent { get; set; }
            public string Referrer { get; set; }
            public string Status { get; set; }
            public string SubStatus { get; set; }
            public string Win32Status { get; set; }
            public string TimeTaken { get; set; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private enum Fields
        {
            date = 0,
            time,
            s_ip,
            cs_method,
            cs_uri_stem,
            cs_uri_query,
            s_port,
            cs_username,
            c_ip,
            cs_user_agent,
            cs_referrer,
            sc_status,
            sc_substatus,
            sc_win32_status,
            time_taken
        }
    }
}

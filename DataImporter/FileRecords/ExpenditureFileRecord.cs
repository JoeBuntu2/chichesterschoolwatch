using System;
using System.Collections.Generic;
using System.Text;

namespace DataImporter.FileRecords
{
    //Level 1	Level 2	Code	Description	FY19-20	FY18-19	FY17-18	FY16-17	FY15-16
    class ExpenditureFileRecord
    {
        [CsvHelper.Configuration.Attributes.Name("Level 1")]
        public int Level1 { get; set; }
        [CsvHelper.Configuration.Attributes.Name("Level 2")]
        public int? Level2 { get; set; }

        public int? Code { get; set; }
        public string Description { get; set; }

        [CsvHelper.Configuration.Attributes.Name("FY19-20")]
        public string FY1920 { get; set; }
        [CsvHelper.Configuration.Attributes.Name("FY18-19")]
        public string FY1819 { get; set; }
        [CsvHelper.Configuration.Attributes.Name("FY17-18")]
        public string FY1718 { get; set; }
        [CsvHelper.Configuration.Attributes.Name("FY16-17")]
        public string FY1617 { get; set; }
        [CsvHelper.Configuration.Attributes.Name("FY15-16")]
        public string FY1516 { get; set; }

        [CsvHelper.Configuration.Attributes.Ignore]
        public bool IsTopLevel => !Level2.HasValue && !Code.HasValue;

        [CsvHelper.Configuration.Attributes.Ignore]
        public bool IsMidLevel => Level2.HasValue && !Code.HasValue;

        [CsvHelper.Configuration.Attributes.Ignore]
        public bool IsCode => Level2.HasValue && Code.HasValue;

    }
}

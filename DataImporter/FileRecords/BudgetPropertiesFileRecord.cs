namespace DataImporter.FileRecords
{
    public  class BudgetPropertiesFileRecord
    {
        public string District { get; set; }

        public string Attribute { get; set; }

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
    }
}

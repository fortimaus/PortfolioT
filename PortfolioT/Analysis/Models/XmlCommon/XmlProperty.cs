namespace PortfolioT.Analysis.Models.XmlCommon
{
    public class XmlProperty
    {
        public string name { get; set; }
        public string value { get; set; }
        public XmlProperty(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}

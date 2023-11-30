using System.Xml.Serialization;

namespace ConsoleApp;

[XmlRoot("GetTicketDataResult", Namespace = "http://gemi.lansforsakringar.se/SecurityApi-1.0/SecurityApi")]
[Serializable]
public class GemiGetTicketDataResult
{

    public string UserId { get; set; } = string.Empty;

    public int SecurityLevel { get; set; }

    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }

    public string Guid { get; set; } = string.Empty;

    public string IP { get; set; } = string.Empty;

}
// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp;

public class Program
{
    const int ColumnWidth = 50;
    private const int Indentation = 10;
    protected static void PrintResultTableHeader()
    {
        Console.Write($"{"Type of creation",-ColumnWidth}"); 
        Console.Write($"{"Elapsed time ms (ticks)",-ColumnWidth}"); 
        Console.Write($"{"Number of items created",-ColumnWidth}");
    }

    protected static void PrintResultTable2(List<TestResult> list)
    {
        var results = list.OrderBy(x => x.ElapsedTicks);
        int i = 1;
        foreach (var result in results)
        {
            i++;
            var original = Console.ForegroundColor;
            if (i % 2 == 0) Console.ForegroundColor = ConsoleColor.Green;
            

            Console.Write($"{result.TypeOfCreation,-ColumnWidth}");
            Console.Write($"{result.ElapsedMilliseconds + " (" + result.ElapsedTicks + ")",-ColumnWidth}");
            Console.Write($"{result.ItemsCreated,-ColumnWidth}");
            Console.WriteLine();
            Console.ForegroundColor = original;
        }
    }

    static void Main(string[] args)
    {
        // Just a little bit of warmup....
        TestFastestWayToCreateInstanceFromXmlString(10);
        PrintResultTableHeader();
        int numberOfCreations = 5000;
        var watch = Stopwatch.StartNew();
        
        Console.WriteLine();
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(1));
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(10));
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));
        numberOfCreations += 5000;
        PrintResultTable2(TestFastestWayToCreateInstanceFromXmlString(numberOfCreations));

        watch.Stop();
        Console.WriteLine();
        Console.WriteLine("###########################################################");
        Console.WriteLine($"### Overall Run Time in milliseconds: {watch.ElapsedMilliseconds}");
        Console.WriteLine($"### Overall Run Time: {watch.Elapsed}");
        Console.WriteLine("###########################################################");
        Console.WriteLine();
        Console.WriteLine("Hit Enter to Exit");
        Console.ReadLine();
    }

    static List<TestResult> TestFastestWayToCreateInstanceFromXmlString(int numberOfCreations)
    {
        var resultList = new List<TestResult>();
        const string tagName = "GetTicketDataResult";
        ICallerTypeParser callerTypeParser = new CallerTypeParser();
        var xmlNameSpace = "http://gemi.lansforsakringar.se/SecurityApi-1.0/SecurityApi";
        XName xName = XName.Get(tagName, xmlNameSpace);
        XName xNameUserId = XName.Get("UserId", xmlNameSpace);
        XName xNameSecurityLevel = XName.Get("SecurityLevel", xmlNameSpace);
        XName xNameCreateTime = XName.Get("CreateTime", xmlNameSpace);
        XName xNameUpdateTime = XName.Get("UpdateTime", xmlNameSpace);

        XName xNameGuid = XName.Get("Guid", xmlNameSpace);
        XName xNameIP = XName.Get("IP", xmlNameSpace);

        // TODO: SKAPA LISTA MED AV XML SOM SKA INSTANS AV OBJEKT PÅ SIKT
        // ERSÄTT ALLA VÄRDEN MED RANDOM STRÄNGAR (PWD GENERATOR)
        var list = new List<string>();

        for (int i = 0; i < numberOfCreations; i++)
        {
            string anyIp = PasswordWithOutPunktuations.Generate(20);
            string anyUserId = PasswordWithOutPunktuations.Generate(15);
            var getTicketDataResponse =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body>" +
                "<GetTicketDataResponse xmlns=\"http://gemi.lansforsakringar.se/SecurityApi-1.0/SecurityApi\">" +
                    "<GetTicketDataResult>" +
                        "<UserId>" + anyUserId + "</UserId>" +
                        "<SecurityLevel>10</SecurityLevel>" +
                        "<CreateTime>2023-11-13T15:43:18+01:00</CreateTime>" +
                        "<UpdateTime>2023-11-13T15:43:18+01:00</UpdateTime>" +
                        "<Guid>0000018b-c7d4-62c9-0000-018bc7d462c9</Guid>" +
                        "<IP>" + anyIp + "</IP>" +
                    "</GetTicketDataResult>" +
                "</GetTicketDataResponse></soap:Body></soap:Envelope>";
            list.Add(getTicketDataResponse);
        }


        // TODO SKAPA INSANS MED Linq2Xml
        int itemsCreated = 0;
        var watch = Stopwatch.StartNew();
        foreach (var  item in list)
        {
            var xDocument = XDocument.Parse(item);
            IEnumerable<XElement> elements;
            elements = xDocument.Descendants(xName);
            // XElement? element = elements.FirstOrDefault();
            var ticketData = (from c in elements
                select new GemiGetTicketDataResult()
                {
                    UserId = c.Element(xNameUserId).Value,
                    SecurityLevel = int.Parse( c.Element(xNameSecurityLevel).Value),
                    CreateTime = DateTime.Parse(c.Element(xNameCreateTime).Value),
                    UpdateTime = DateTime.Parse(c.Element(xNameUpdateTime).Value),
                    Guid = c.Element(xNameGuid).Value,
                    IP = c.Element(xNameIP).Value,
                }).SingleOrDefault();
            if (ticketData != null) itemsCreated++;

        }
        watch.Stop();
        resultList.Add(new TestResult
        {
            ParseToType = nameof(GemiGetTicketDataResult),
            TypeOfCreation = "Linq2Xml",
            ElapsedMilliseconds = watch.ElapsedMilliseconds,
            ElapsedTicks = watch.ElapsedTicks,
            ItemsCreated = itemsCreated
        });

        // TODO SKAPA INSANS MED XmlSerialiser
        itemsCreated = 0;
        watch = Stopwatch.StartNew();
        foreach (var item in list)
        {
            var doc = new XmlDocument();
            doc.LoadXml(item);
            XmlNode? node = doc.GetElementsByTagName(tagName).Item(0);
            var ticketData = GetGemiTicketData(node, callerTypeParser);
            if (ticketData != null) itemsCreated++;
        }
        watch.Stop();
        resultList.Add(new TestResult
        {
            ParseToType = nameof(GemiGetTicketDataResult),
            TypeOfCreation = "XmlSerializer",
            ElapsedMilliseconds = watch.ElapsedMilliseconds,
            ElapsedTicks = watch.ElapsedTicks,
            ItemsCreated = itemsCreated
        });

        return resultList;
    }

    private static GemiGetTicketDataResult? GetGemiTicketData(XmlNode? node, ICallerTypeParser callerTypeParser)
    {
        if (node == null) return null;
        try
        {
            GemiGetTicketDataResult? data;
            using (TextReader sr = new StringReader(node.OuterXml))
            {
                var serializer = new XmlSerializer(typeof(GemiGetTicketDataResult));
                data = serializer.Deserialize(sr) as GemiGetTicketDataResult;
            }

            return data;
           
        }
        catch (Exception e)
        {
            var msg = "Error occurred during parsing the \"GetTicketDataResult\" node.";
            Console.WriteLine(msg);
            // logger.LogError(e, msg);
            throw;
        }
    }
}
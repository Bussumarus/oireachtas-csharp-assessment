using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OireachtasAPI
{

    public class Bills
    {
        public BillHead head { get; set; }
        public BillDetails[] results { get; set; }
    }

    public class BillCount : Count
    {
        public int billCount { get; set; }
    }

    public class BillHead : Head
    {
        public BillCount counts { get; set; }
    }

    public class BillDetails
    {
        public Bill bill { get; set; }
        public Billsort billSort { get; set; }
        public string contextDate { get; set; }
    }

    public class Bill
    {
        public object act { get; set; }
        public object[] amendmentLists { get; set; }
        public string billNo { get; set; }
        public string billType { get; set; }
        public string billTypeURI { get; set; }
        public string billYear { get; set; }
        public Debate[] debates { get; set; }
        public EventDetails[] events { get; set; }
        public DateTime lastUpdated { get; set; }
        public string longTitleEn { get; set; }
        public string longTitleGa { get; set; }
        public string method { get; set; }
        public string methodURI { get; set; }
        public MostRecentStage mostRecentStage { get; set; }
        public OriginHouse originHouse { get; set; }
        public string originHouseURI { get; set; }
        public RelatedDocDetails[] relatedDocs { get; set; }
        public string shortTitleEn { get; set; }
        public string shortTitleGa { get; set; }
        public string source { get; set; }
        public string sourceURI { get; set; }
        public SponsorDetails[] sponsors { get; set; }
        public Stage[] stages { get; set; }
        public string status { get; set; }
        public string statusURI { get; set; }
        public string uri { get; set; }
        public VersionDetails[] versions { get; set; }
    }

    public class MostRecentStage
    {
        public Event _event { get; set; }
    }

    public class Event
    {
        public Chamber chamber { get; set; }
        public Date[] dates { get; set; }
        public House house { get; set; }
        public int progressStage { get; set; }
        public string showAs { get; set; }
        public bool stageCompleted { get; set; }
        public object stageOutcome { get; set; }
        public string stageURI { get; set; }
        public string uri { get; set; }
    }

    public class Chamber
    {
        public string chamberCode { get; set; }
        public string showAs { get; set; }
        public string uri { get; set; }
    }

    public class Date
    {
        public string date { get; set; }
    }

    public class OriginHouse
    {
        public string showAs { get; set; }
        public string uri { get; set; }
    }

    public class Debate
    {
        public Chamber chamber { get; set; }
        public string date { get; set; }
        public string debateSectionId { get; set; }
        public string showAs { get; set; }
        public string uri { get; set; }
    }

    

    public class EventDetails
    {
        public Event _event { get; set; }
    }

    
    public class RelatedDocDetails
    {
        public RelatedDoc relatedDoc { get; set; }
    }

    public class RelatedDoc
    {
        public string date { get; set; }
        public string docType { get; set; }
        public Formats formats { get; set; }
        public string lang { get; set; }
        public string showAs { get; set; }
        public string uri { get; set; }
    }

    public class Formats
    {
        public Pdf pdf { get; set; }
        public object xml { get; set; }
    }

    public class Pdf
    {
        public string uri { get; set; }
    }

    public class SponsorDetails
    {
        public Sponsor sponsor { get; set; }
    }

    public class Sponsor
    {
        public As _as { get; set; }
        public By by { get; set; }
    }

    public class As
    {
        public string showAs { get; set; }
        public string uri { get; set; }
    }

    public class By
    {
        public string showAs { get; set; }
        public string uri { get; set; }
    }

    public class Stage
    {
        public Event _event { get; set; }
    }

    public class VersionDetails
    {
        public Version version { get; set; }
    }

    public class Version
    {
        public string date { get; set; }
        public string docType { get; set; }
        public Formats formats { get; set; }
        public string lang { get; set; }
        public string showAs { get; set; }
        public string uri { get; set; }
    }

    public class Pdf1
    {
        public string uri { get; set; }
    }

    public class Billsort
    {
        public object actNoSort { get; set; }
        public object actShortTitleEnSort { get; set; }
        public object actShortTitleGaSort { get; set; }
        public object actYearSort { get; set; }
        public int billNoSort { get; set; }
        public string billShortTitleEnSort { get; set; }
        public string billShortTitleGaSort { get; set; }
        public int billYearSort { get; set; }
    }
}
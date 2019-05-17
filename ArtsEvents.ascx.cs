using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ektron.Cms;
using Ektron.Cms.API;
using Ektron.Cms.Widget;
using Ektron.Cms.PageBuilder;
using Ektron.Cms.Common;
using Ektron.Cms.Common.Calendar;
using Ektron.Cms.Organization;
using Ektron.Cms.Framework.Organization;

public partial class widgets_CalendarEvents : System.Web.UI.UserControl
{
    private IWidgetHost _host;
    private string _title;
    private int _calendarID;
    private int _numEvents;
    private string _calendarURL;
    private string _taxonomyPath;

    [WidgetDataMember("Upcoming Performances")]
    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    [WidgetDataMember(1448)]
    public int CalendarID
    {
        get { return _calendarID; }
        set { _calendarID = value; }
    }

    [WidgetDataMember("\\The Arts\\Kaleidoscope")]
    public string TaxonomyPath
    {
        get { return _taxonomyPath; }
        set { _taxonomyPath = value; }
    }

    [WidgetDataMember(5)]
    public int NumEvents
    {
        get { return _numEvents; }
        set { _numEvents = value; }
    }

    public string CalendarURL
    {
        get { return _calendarURL; }
        set { _calendarURL = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _host = Ektron.Cms.Widget.WidgetHost.GetHost(this);
        _host.Title = "Arts Events";
        _host.Edit += new EditDelegate(EditButtonClicked);
        ViewSet.SetActiveView(View);

        SetCalendarURL();

        // Load events into calendar
        if (!IsPostBack)
        {
            LoadEvents();
        }
    }

    private void LoadEvents()
    {
        var start = DateTime.Today;
        var end = DateTime.Today.AddYears(1);
        int limit = NumEvents; // Number of events to be displayed on calendar

        var webEvents = GetEventsItems(CalendarID, start, end);

        // sort the list chronologically
        webEvents.Sort((we1, we2) => we1.EventStartUtc.CompareTo(we2.EventStartUtc));

        // Populate the widget with the HTML for the events
        litEvents.Text = PopulateLiteral(webEvents, limit);
    }

    private List<WebEventData> GetEventsItems(long calendarId, DateTime startUtc, DateTime endUtc)
    {
        var webEventApi = new Ektron.Cms.Content.Calendar.WebEventManager(Ektron.Cms.ObjectFactory.GetRequestInfoProvider().GetRequestInformation());
        
        // Define the taxonomy you want
        TaxonomyCriteria criteria = new TaxonomyCriteria();
        criteria.AddFilter(TaxonomyProperty.Path, CriteriaFilterOperator.EqualTo, TaxonomyPath);
        TaxonomyManager taxonomyManager = new TaxonomyManager();
        List<TaxonomyData> taxonomyItemList = taxonomyManager.GetList(criteria);

        // Get all webEvents in the given taxonomy
        var webEvents = webEventApi.GetEventOccurrenceList(calendarId, startUtc, endUtc, taxonomyItemList.ConvertAll(IdsOnly));

        return webEvents;
    }

    private static long IdsOnly(TaxonomyData input)
    {
        return input.Id;
    }
    
    private string PopulateLiteral(List<WebEventData> webEvents, int limit)
    {
        string html = ""; // Literal to be populated
        List<String> eventsList = new List<String>(); // Keeps a record of all the event titles listed so far

        // Check if there are no events
        if (webEvents.Count == 0) { return "No events are scheduled at this time. Check back soon!"; }

        // Remove duplicates
        foreach (WebEventData webEvent in webEvents)
        {
            if (!eventsList.Contains(webEvent.Teaser))
            {
                eventsList.Add(webEvent.Teaser);
            }
        }

        // Prevent "index out of bounds" error
        if (limit > eventsList.Count) { limit = eventsList.Count; }

        // Populate HTML
        for (int i = 0; i < limit; i++)
        {
            html += "<p id='arts-events-literal'>" +
                        eventsList[i] +
                    "</p>" +
                    "<hr>";
        }
        return html;
    }

    protected void SetCalendarURL()
    {
        if (CalendarID == 1409) { CalendarURL = "/academiccalendar"; }
        else if (CalendarID == 1448) { CalendarURL = "/artscalendar"; }
        else if (CalendarID == 2455) { CalendarURL = "/contedcalendar"; }
        else if (CalendarID == 2457) { CalendarURL = "/facultysenatecouncilmeetings"; }
        else if (CalendarID == 2456) { CalendarURL = "/facultysenatemeetings"; }
        else if (CalendarID == 2413) { CalendarURL = "/staffcalendar"; }
        else { CalendarURL = "/calendar"; } // if calendar is not found in the list, default to main calendar
    }
    
    protected void EditButtonClicked(string settings)
    {
        titleTextBox.Text = Title;
        calendarIDtextBox.Text = CalendarID.ToString();
        taxonomyTextBox.Text = TaxonomyPath;
        numEventsTextBox.Text = NumEvents.ToString();
        ViewSet.SetActiveView(Edit);
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        ViewSet.SetActiveView(View);
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        Title = titleTextBox.Text;
        CalendarID = Convert.ToInt32(calendarIDtextBox.Text);
        TaxonomyPath = taxonomyTextBox.Text;
        NumEvents = Convert.ToInt32(numEventsTextBox.Text);
        _host.SaveWidgetDataMembers();
        LoadEvents();
        SetCalendarURL();
        ViewSet.SetActiveView(View);
    }
}
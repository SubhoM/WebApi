public class CustomerSupport
{
    string email;
    string userID;
    string userName;
    string siteID;
    int? hCOID;
    string programName;
    string submitTime;
    string subject;
    string body;
    int? eProductID;
    int? programID;

    public string Email
    {
        get { return email; }
        set { email = value; }
    }
    public string UserID
    {
        get { return userID; }
        set { userID = value; }
    }

    public string UserName
    {
        get { return userName; }
        set { userName = value; }
    }
    public string SiteID
    {
        get { return siteID; }
        set { siteID = value; }
    }
    public int? HCOID
    {
        get { return hCOID; }
        set { hCOID = value; }
    }

    public string ProgramName
    {
        get { return programName; }
        set { programName = value; }
    }
    public string SubmitTime
    {
        get { return submitTime; }
        set { submitTime = value; }
    }
    public string Subject
    {
        get { return subject; }
        set { subject = value; }
    }
    public string Body
    {
        get { return body; }
        set { body = value; }
    }
    public int? EProductID
    {
        get { return eProductID; }
        set { eProductID = value; }
    }
    public int? ProgramID
    {
        get { return programID; }
        set { programID = value; }
    }
}
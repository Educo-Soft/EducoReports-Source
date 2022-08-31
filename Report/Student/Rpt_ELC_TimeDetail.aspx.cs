using System;
using System.Data;
using System.Web;
using System.Runtime;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;



public partial class ELC_TimeDetail : System.Web.UI.Page
{

    private string imageFilePath = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            GetData();    

        }
    }

    protected void GetData()
    {
        DataTable dtTestTime = null, dtTuTime = null;

        ELCrptClass cs = new ELCrptClass();
        object studentId = Request["id"];
        object courseId = Session["CourseId"];

        string userImage = string.Empty, userImageURL = string.Empty;
       // LoadUserImageFilePath();    //Get user image physical path

        int child = 0;

        
        DataTable dtSkills = cs.getELC_TestTimeDetail(studentId);

        DataTable dtTu = getELC_TutorialTimeDetail(studentId,courseId);

        string HtmlTable = "<Table align='left' rules='rows' width='100%'><tr style='color:blue;' bgcolor='lightblue'>";
        double Score = 0;

        HtmlTable += "<td>ACTIVITIES</td>" +
            "<td width='20px' ></td>" +
            "<td align='center'>TYPE</td><td width='20px'></td>" +
            "<td align='center'>TIME SPENT</td><td width='20px'></td>" +
            "<td align='center'>Last accessed</td><td width='20px'></td></tr>";
    string result = "";
        string type = Request["type"];
        DataRow[] drs = null, drsTu=null;
        if (type=="QZ")
            drs = dtSkills.Select("TestType like '%Quiz%'", "");
        
        if (type == "TS")
            drs = dtSkills.Select("TestType like '%Test%'", "");

        if (type == "PS")
            drs = dtSkills.Select("TestType like '%Practice%'", "");
        if (type == "TU")
        {
            drs = dtSkills.Select("TestType like '%TU%'", "");
            drsTu = dtTu.Select("Tutorial_Time<>0", "");
        }
        else
            drsTu = dtTu.Select("Tutorial_Time=0", "");

        if (type == "AL")
        {
            drs = dtSkills.Select("TestType<>'Null'", "");
            drsTu = dtTu.Select("Tutorial_Time<>0", "");
        }
            

        foreach (DataRow dr in drs)
        {
            HtmlTable += "<tr ><td align='left'>" + dr["course_name"].ToString().Trim() + ":" + dr["CRLevel_Name"] + "</td><td> </td>";
            HtmlTable += "<td align='center'>" + dr["TestType"] + "</td><td> </td>";
            HtmlTable += "<td align='center'>" + cs.getDurationIn_hms(Convert.ToDouble(dr["Time_Spent"])) + "</td><td></td>";
            HtmlTable += "<td align='center'>"+ dr["LastAccess"] +"</td>";
            HtmlTable += "</tr>";
             
        } //foreach
        foreach (DataRow dr in drsTu)
        {
            HtmlTable += "<tr><td align='left'>" + dr["course_name"].ToString().Trim() + ":" + dr["CRLevel_Name"] + "</td><td> </td>";
            HtmlTable += "<td align='center'>Tutorial</td><td> </td>";
            HtmlTable += "<td align='center'>" + cs.getDurationIn_hms(Convert.ToDouble(dr["Tutorial_Time"])) + "</td><td> </td>";
            HtmlTable += "<td align='center'>"+ dr["LastAccess"] + "</td><td></td>";
            HtmlTable += "</tr>";

        } //foreach

        HtmlTable += "</Table>";
        lblData.Text = HtmlTable;
    }

    public DataTable getELC_TutorialTimeDetail(object childId, object courseId)
    {
        ELCrptClass cs = new ELCrptClass();
        string strSql = "SELECT Sum([CRTimeSpent_TimeSpent]) as Tutorial_Time" +
                        ", Max([CRTimeSpent_TimeStamp]) as LastAccess" +
                        " ,CRTimeSpent_LOId,cr_levels.CRLevel_ParentId,cr_levels.CRLevel_name,course_name" +
                        " FROM [dbo].[CR_TimeSpent],Cr_levels,course" +
                        " where CRTimeSpent_UserId=" + childId + " and CRTimeSpent_UserType is not null" +
                        " and course.courseid=CRTimeSpent_CourseId" +
                        " and cr_levels.CRLevelid=CRTimeSpent_LOId" +
                        " and course.courseid=" + courseId +
                        " Group by CRTimeSpent_LOId,cr_levels.CRLevel_ParentId,cr_levels.CRLevel_name,course_name" +
                        " order by LastAccess desc";
        //Response.Write(strSql);
        return cs.ExecuteSelectQuery(strSql);
    }



}





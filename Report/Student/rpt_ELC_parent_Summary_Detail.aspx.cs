using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;



public partial class ELC_Time : System.Web.UI.Page
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
        object studentId = Request["studentId"];

        string userImage = string.Empty, userImageURL = string.Empty;
        LoadUserImageFilePath();    //Get user image physical path

        int child = 0;

        string sql = "SELECT Max((([TestUser_TotMrksObt]*100)/(TestUser_TotMrks))) as Percent_Score" +
                    ",course.course_name,Cr_levels.CRLevel_Name" +
                   
                    ",TestType=case when (TestSettings_TestModeType=1 and Test_CrSectionLevelId<>0) then 'Quiz'" +
                        " when (TestSettings_TestModeType=2) then 'PracticeSheet'" +
                        " when (Test_Type=1 and Test_CrSectionLevelId=0) then 'Test'" +
                        " when (Test_Type=1 and Test_CrSectionLevelId<>0) then 'Section_Quiz' end" +

                    " FROM [dbo].[Test_User],Testpaper,Test_Settings,CR_Levels,Course,sections" +
                    " Where testpaper.testid=TestUser_TestID" +
                    " AND Testpaper.Test_TestSettingID=Test_Settings.TestSettingsID" +
                    " AND cr_levels.CRLevelID=Testpaper.Test_CrSectionLevelID" +
                    " AND cr_levels.CRLevelID=Testpaper.Test_CrSectionLevelID" +
                    " AND Cr_levels.CRLevel_CourseId=course.courseid" +
                    " and TestUser_UserID=" + studentId +
                    " group by Testpaper.Test_CrSectionLevelID,Cr_levels.CRLevel_Name,course.course_name,Test_Type,TestSettings_TestModeType";
        //--having Max((([TestUser_TotMrksObt]*100)/(TestUser_TotMrks)))>90
        
        DataTable dtSkills = cs.ExecuteSelectQuery(sql);

        string HtmlTable = "<Table align='center' rules='all' style='width:100%'><tr bgcolor='lightblue'>";
        double Score = 0;

        HtmlTable += "<td><b>Skills practiced/attempted</b></td><td><b>Type</b></td><td><b>Score</b></td><td><b>Result</b></td></tr>";
        string result="";
        foreach (DataRow dr in dtSkills.Rows)
        {
            HtmlTable += "<tr><td align='left'>" + dr["course_name"].ToString().Trim() + ":" + dr["CRLevel_Name"] + "</td>";
            result = "Not satisfactory";
            HtmlTable += "<td align='center'>" + dr["TestType"] + "</td>";
            HtmlTable += "<td align='center'>" + Convert.ToDouble(dr["Percent_score"]).ToString("N2") + "</td>";
            if (Convert.ToDouble(dr["Percent_score"])>90)
                result = "Mastered";
            if (Convert.ToDouble(dr["Percent_score"]) < 90 && Convert.ToDouble(dr["Percent_score"]) > 80)
                result = "Good";
            HtmlTable += "<td align='center'>" + result + "</td>";

            HtmlTable += "</tr>";

        } //foreach
        HtmlTable += "</Table>";
        lblReport.Text = HtmlTable;
    }

    

    private void LoadUserImageFilePath()
    {
        DataSet dstFileInfo = null;
        string colSeperator = ((char)195).ToString();

        Educo.ELS.Utilities.FileUpload ObjFileUpload = new Educo.ELS.Utilities.FileUpload();
        dstFileInfo = ObjFileUpload.GetVirtualPath("1" + colSeperator + "FP16", "Strings-En");

        if (dstFileInfo.Tables[0].Rows[0][0].ToString() == "0")
        {
            imageFilePath = dstFileInfo.Tables[0].Rows[0][4].ToString();
        }
    }




}





using System;
using System.Data;
using System.Runtime;
using System.Data.Common;


public partial class ELCSkills : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            populate_SkillTable();
    }

    private void populate_SkillTable()
    {
        DataTable dtSkills = null;
        ELCrptClass util = new ELCrptClass(); 
        dtSkills = util.getSkills();
        object userId = Session["Userid"]; 
                
        DataTable dtUser = util.getUser(userId);

        lblStudent.Text = dtUser.Rows[0]["uName"].ToString();

        DataTable dtSkillTuTime = util.getELC_SkillTutorialTime(userId);

        DataTable dtSkillPracticeTime = util.getELC_SkillPractuiceTime(userId);

        DataTable dtSkillQnAttemptTotalAndTime=util.getELC_SkillQnAttemptCountAndTime(userId);

        DataTable dtSkillQnAttemptCorr = util.getELC_SkillQnCorrectCount(userId);
       


        string timeObj = "";
        string rptTable = " <table rules='all' align='center' style='width:99%; font-size: small'>" +
        "<tr><td colspan='9' align ='center' style='background-color: #33CCFF'><b>GRADE 1 SKILL ANALYSIS</b></td></tr>" +

         "<tr style='background-color: SteelBlue; color: #FFFFFF'>" +
             "<td align ='center' width='5%'>#</td>" +
             "<td align ='center' width='25%'>Skills</td>" +
             "<td align ='center' width='10%'>Learning Time</td>" +
             "<td align ='center' width='10%'>Question Time</td>" +
             "<td align ='center' width='10%'>Correct/Total</td>" +
             "<td columspan='4' align ='center'>Level of Mastery</td>"+
	                  
         "</tr>";
        int RowCnt = 0;
        object obj = null;
        string CorrObj = "", TimeObj = "0", TotQnObj = "", barcolor="";
        DataRow[] drs;
        double DbltuTime = 0, DblPracTime=0;
        foreach (DataRow dr in dtSkills.Rows)
        {
            obj = null;
            DbltuTime = 0; DblPracTime = 0;
            timeObj = ""; CorrObj = ""; TimeObj = "0";
            RowCnt++;
            rptTable += "<tr>" +
            "<td align ='center'>" + RowCnt + "</td>" +
            "<td >" + dr["CRSkill_Name"] + "</td>";
            TimeObj = "";
            obj = dtSkillTuTime.Compute("Sum(TutorialTime)", "SkillId=" + dr["CRSkill_ID"]);
            if (obj != null && obj != DBNull.Value)
                DbltuTime = Convert.ToDouble(obj);

            TimeObj = "";
            obj = dtSkillPracticeTime.Compute("Sum(TimeSpent)", "CRSkill_ID=" + dr["CRSkill_ID"]);
            if (obj != null && obj != DBNull.Value)
                DblPracTime = Convert.ToDouble(obj);

            if (DbltuTime + DblPracTime > 0)
                timeObj = util.getDurationIn_hrmin(DbltuTime + DblPracTime);

            rptTable += "<td align ='center'>" + timeObj + "</td>";

            TimeObj = "";
            TotQnObj = "";
            drs = dtSkillQnAttemptTotalAndTime.Select("CRSkill_ID=" + dr["CRSkill_ID"]);
            if (drs.Length > 0)
            {
                TotQnObj = drs[0]["TotQuesCount"].ToString();
                TimeObj = drs[0]["TimeSpent"].ToString();
                TimeObj = util.getDurationIn_hrmin(Convert.ToDouble(TimeObj));
            }
            else
                TimeObj = "";

            rptTable += "<td align ='center'>" + TimeObj + "</td>";
            obj = null;
            CorrObj = "";
            obj = dtSkillQnAttemptCorr.Compute("Sum(TotQuesCount)", "CRSkill_ID=" + dr["CRSkill_ID"]);
            if (obj != null && obj != DBNull.Value)
                CorrObj = obj.ToString();

            rptTable += "<td align ='center'>" + CorrObj + "/" + TotQnObj + "</td>";

            double per = 0;
            string bar = "";
            if (CorrObj != "")
            {
                per = Math.Round((Convert.ToDouble(CorrObj) * 100) / Convert.ToDouble(TotQnObj), 0);
            }

		

		    barcolor = "";
                    if (per < 70)
                        barcolor = "bgColor='#FFCC66'";
                    if (per >= 70 && per < 80)
                        barcolor = "bgColor='#FFFFCC'";
                    if (per >= 80 && per < 90)
                        barcolor = "bgColor='#C8FFFF'";
                    if (per >= 90)
                        barcolor = "bgColor='#DBDBFF'";

            if (per == 0)
                bar = "";
            else
                bar = "<table width='" + per + "%'><tr><td height='10' align='right' style='color: Black; font-size: small' "+barcolor+">" + per + "%</td></tr></table>";
            rptTable += "<td ColSpan='4'>" + bar + "</td>";

            rptTable += "</tr>";
        }
        rptTable += "<tr>" +
             "<td align ='center'></td>" +
             "<td align ='center'>TOTAL</td>" +
             "<td align ='center'></td>" +
             "<td align ='center'></td>" +
             "<td align ='center'></td>" +
             "<td ColSpan='4'></td>" +
             //"<td ></td>" +
             //"<td ></td>" +
             //"<td ></td>" +

         "</tr>";
        rptTable += "</table>";
        lblReport.Text = rptTable;
    }
}

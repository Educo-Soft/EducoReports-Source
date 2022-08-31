using System;
using System.Data;
using System.Data.Common;


public partial class ELCStandards : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            populate_SkillTable();
    }

    private void populate_SkillTable()
    {
        ELCrptClass util = new ELCrptClass();

        DataTable dtDomain = util.getDomain();

        DataTable dtStandard = util.getStandards();

        object userId = Session["Userid"];

        DataTable dtUser = util.getUser(userId);

        lblStudent.Text = dtUser.Rows[0]["uName"].ToString();

        DataTable dtStandardTutorialTime = util.getELC_StandardTutorialTime(userId);
        DataTable dtStandardPracticeTime = util.getELC_StandardPracticeTime(userId);

        DataTable dtStandardQAttemptTotalAndTime = util.getELC_StandardQAttemptCountAndTime(userId);

        DataTable dtStandardQnAttemptCorr = util.getELC_StandardQnCorrectCount(userId);

       // DataTable dt = getELC(userId);

        DataRow[] drStandards = null, drST = null;
        string rptTable = "<table rules='all' align='center' style='width:99%; font-size: small'>" +
            "<tr><td colspan='6' align ='center' style='background-color: #33CCFF'><b>GRADE 1 STANDARD ANALYSIS</b></td></tr>" +
       "<tr align='center' style='background-color: SteelBlue; color: #FFFFFF'>" +
            "<td align='left' width='20%'>Domain</td>" +
            "<td align='left' width='25%'>Standard</td>" +
            "<td width='8%'>Learning Time</td>" +
            "<td width='8%'>Question Time</td>" +
            "<td width='8%'>#Correct/#Total</td>" +
            "<td>Level of Mastery</td>" +
        "</tr>";
        object obj = null;
        string TimeObj = "", CorrObj = "", TotQnObj = "", barcolor = "";
        double DbltuTime = 0, DblPracTime = 0;
        string StdIds = "";
        double DblTotTuTime = 0;
        foreach (DataRow drM in dtDomain.Rows)
        {
            rptTable += "<tr bgColor= '#FFECD9'>" +
                "<td>" + drM["Domain_Name"] + "</td>";
            rptTable += "<td></td>";

            StdIds = "";
            
            drStandards = dtStandard.Select("CRDomainId=" + drM["CRDomainID"]);
            if (drStandards.Length > 0)
            {
                foreach (DataRow drS in drStandards)
                {
                    StdIds += "'" + drS["CRStandardID"] + "',";
                }
                
                TimeObj = "";
                obj = null;
                DblTotTuTime = 0; 
                obj = dtStandardTutorialTime.Compute("Sum(TutorialTime)", "StandardId in (" + StdIds + "'0')");
                if (obj != null && obj != DBNull.Value)
                    DblTotTuTime = Convert.ToDouble(obj);
                
                TimeObj = "";
                obj = null;
                obj = dtStandardPracticeTime.Compute("Sum(TimeSpent)", "CRStandardID in (" + StdIds + "'0')");
                if (obj != null && obj != DBNull.Value)
                    DblTotTuTime+= Convert.ToDouble(obj);

                
                //Response.Write(DblTotTuTime);

                if (DblTotTuTime > 0)

                TimeObj = util.getDurationIn_hrmin((DblTotTuTime));


                rptTable += "<td align='center'>" + TimeObj + "</td>";

                obj = "";
                TimeObj = "";
                obj = dtStandardQAttemptTotalAndTime.Compute("Sum(TimeSpent)", "CRStandardID in (" + StdIds + "'0')");
                if (obj != null && obj != DBNull.Value)
                    TimeObj = util.getDurationIn_hrmin(Convert.ToDouble(obj));

                rptTable += "<td align='center'>" + TimeObj + "</td>";

                obj = "";
                TimeObj = "";
                obj = dtStandardQAttemptTotalAndTime.Compute("Sum(TotQuesCount)", "CRStandardID in (" + StdIds + "'0')");
                if (obj != null && obj != DBNull.Value)
                    TimeObj = obj.ToString();

                CorrObj = "";
                obj = dtStandardQnAttemptCorr.Compute("Sum(TotQuesCount)", "CRStandardID in (" + StdIds + "'0')");
                if (obj != null && obj != DBNull.Value)
                    CorrObj = obj.ToString();

                if (CorrObj!="" && TimeObj!="")
                rptTable += "<td align ='center'>" + CorrObj + "/" + TimeObj + "</td>";
                else
                    rptTable += "<td align ='center'></td>";


            } //if  Summary
      

        rptTable += "<td></td>" +
                     "</tr>";  // Domain line end

            drST = null;
            if (drStandards.Length > 0)
            {
                foreach (DataRow drS in drStandards)
                {
                    rptTable += "<tr>" +
                    "<td></td>" +
                    "<td>" + drS["Standard_Name"] + "</td>";
                    TimeObj = "";
                    obj = null;
                    DbltuTime = 0; DblPracTime = 0;

                    obj = dtStandardTutorialTime.Compute("Sum(TutorialTime)", "StandardId=" + drS["CRStandardID"]);
                    if (obj != null && obj != DBNull.Value)
                        DbltuTime = Convert.ToDouble(obj);

                    TimeObj = "";
                    obj = null;
                    obj = dtStandardPracticeTime.Compute("Sum(TimeSpent)", "CRStandardID=" + drS["CRStandardID"]);
                    if (obj != null && obj != DBNull.Value)
                        DblPracTime = Convert.ToDouble(obj);

                    if (DbltuTime + DblPracTime > 0)
                        TimeObj = util.getDurationIn_hrmin(DbltuTime + DblPracTime);

                    rptTable += "<td align ='center'>" + TimeObj + "</td>";

                    TimeObj = "";
                    drST = dtStandardQAttemptTotalAndTime.Select("CRStandardID=" + drS["CRStandardID"]);
                    if (drST.Length > 0)
                    {
                        TotQnObj = drST[0]["TotQuesCount"].ToString();
                        TimeObj = drST[0]["TimeSpent"].ToString();
                        TimeObj = util.getDurationIn_hrmin(Convert.ToDouble(TimeObj));
                    }
                    else
                    {
                        TimeObj = "";
                        TotQnObj = "";
                    }
                    rptTable += "<td align ='center'>" + TimeObj + "</td>";
                    obj = null;
                    CorrObj = "";
                    obj = dtStandardQnAttemptCorr.Compute("Sum(TotQuesCount)", "CRStandardID=" + drS["CRStandardID"]);
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
                        bar = "<table width='" + per + "%'><tr><td height='10' align='right' style='color: Black; font-size: small' " + barcolor + "'>" + per + "%</td></tr></table>";
                    rptTable += "<td ColSpan='4'>" + bar + "</td>";

                    rptTable += "</tr>";
                }//Standard loop
            } // Dr Length
        }// Domain loop

        rptTable += "</table>";

        lblReport.Text = rptTable;

    }

    

    public DataTable getELC(object studentId)
    {
        string sql = "SELECT CR_TutorialStandMap.StandardId" +
              ",Sum(CR_TimeSpent.CRTimeSpent_TimeSpent) as TutorialTime  " +
              " from CR_TimeSpent " +
              " LEFT JOIN CR_Levels ON CRLevel_LOId=CRTimeSpent_LOId " +
              " LEFT JOIN CR_TutorialStandMap ON CR_TutorialStandMap.TutorialLevelId=CRLevelID " +
              " WHERE CRTimeSpent_UserId =  " + studentId +
              " and  CR_TutorialStandMap.StandardId in (SELECT CRstandardId from cr_Domain,CR_Standard where CRDomain_CurriculumID=8" +
              " and CRStandard_DomainID=CRDomainID) " +
              " group by CR_TutorialStandMap.StandardId--,CRTimeSpent_UserId";
        Response.Write(sql);
        //DataTable dtStandardTutorialTime = this.ExecuteSelectQuery(sql);
        return null;// dtStandardTutorialTime;
    }
}

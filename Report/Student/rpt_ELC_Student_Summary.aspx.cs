using System;
using System.Data;
using System.Web;
//using System.Runtime;
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
            if (Session["UserId"] == "")
                lblChart1.Text = "Session Timed out";
            else
                GetData();

        }
    }


    protected void GetData()
    {
        DataTable dtTestTime = null, dtTuTime = null;
	ELCrptClass cs = new ELCrptClass();
        //rptUtils cs = new rptUtils();
        object studentId=  Session["USERID"];
        Page.RegisterStartupScript("myScript2", "<script language=\"JavaScript\">SetDivWidth('" + 1 + "');</script>");

        string userImage = string.Empty, userImageURL = string.Empty;
        //LoadUserImageFilePath();    //Get user image physical path

        int child = 1;
        
            
            dtTestTime = cs.getELC_TestTime(studentId);
            dtTuTime = cs.getELC_TutorialTime(studentId);

            
            //GridView1.DataSource = dtTestTime;
            //GridView1.DataBind();
            string buttonLink = "<img src='../images/detail.png' onclick=\"javascript:popDetail(" + studentId + ",'grade');\" style='width:90px;height:35px;'></img>";

           
            DataTable dtSkills = cs.getELC_TopicSkills(studentId);
            string skilAttempt = dtSkills.Rows.Count.ToString();
            string skillMastered = dtSkills.Select("Percent_Score>90").Length.ToString();

            DataTable dt = new DataTable("Activity_Time");
            DataColumn[] dc = new DataColumn[2];
            dc[0] = new DataColumn("Activity", System.Type.GetType("System.String"));
            dt.Columns.Add(dc[0]);
            dc[1] = new DataColumn("Time", System.Type.GetType("System.Double"));
            dt.Columns.Add(dc[1]);


            string HtmlTable = "<Table rules='all' style='border: 1px solid #C0C0C0' width='90%'><tr style='color: #FFFFFF' bgcolor='#669999'><td>Activity</td><td>Time spent</td><td>Average Score</td></tr>";
            string Score = "";


            Double TimeSpent = 0, TotalTime = 0;


            Object obj = dtTuTime.Compute("Sum(Tutorial_Time)", "");
            if (obj != DBNull.Value)
                TimeSpent = Convert.ToDouble(obj);

            DataRow drRecord = dt.NewRow();
            drRecord["Activity"] = "Tutorial";
            drRecord["Time"] = TimeSpent;
            dt.Rows.Add(drRecord);

            TotalTime += TimeSpent;

            HtmlTable += "<tr><td>Tutorial</td>" +
            "<td align='center'>" + cs.getDurationIn_hms(TimeSpent) + "</td>" +
            "<td></td></tr>";

            ///------------------
            TimeSpent = 0;
            obj = dtTestTime.Compute("Sum(Time_Spent)", "TestType like '%Quiz%'");
            if (obj != DBNull.Value)
                TimeSpent = Convert.ToDouble(obj);

            drRecord = dt.NewRow();
            drRecord["Activity"] = "Quiz";
            drRecord["Time"] = TimeSpent;
            dt.Rows.Add(drRecord);

            TotalTime += TimeSpent;
            Score = "";
            obj = dtTestTime.Compute("AVG(Percent_Score)", "TestType like '%Quiz%'");
            if (obj != DBNull.Value)
                Score = Convert.ToDouble(obj).ToString("N2");

            HtmlTable += "<tr><td>Quiz</td>" +
            "<td align='center'>" + cs.getDurationIn_hms(TimeSpent) + "</td>" +
            "<td align='center'>" + Score + "</td></tr>";
            ///----------------------------------------------

            TimeSpent = 0;
            obj = dtTestTime.Compute("Sum(Time_Spent)", "TestType like '%PracticeSheet%'");
            if (obj != DBNull.Value)
                TimeSpent = Convert.ToDouble(obj);

            drRecord = dt.NewRow();
            drRecord["Activity"] = "Practice Sheet";
            drRecord["Time"] = TimeSpent;
            dt.Rows.Add(drRecord);

            TotalTime += TimeSpent;
            Score = "";
            obj = dtTestTime.Compute("AVG(Percent_Score)", "TestType like '%PracticeSheet%'");
            if (obj != DBNull.Value)
                Score = Convert.ToDouble(obj).ToString("N2");

            HtmlTable += "<tr><td>Practice Sheet</td>" +
            "<td align='center'>" + cs.getDurationIn_hms(TimeSpent) + "</td>" +
            "<td align='center'>" + Score + "</td></tr>";
            ///-----------------------------------------------------------
            ///
            TimeSpent = 0;
            obj = dtTestTime.Compute("Sum(Time_Spent)", "TestType like '%Test%'");
            if (obj != DBNull.Value)
                TimeSpent = Convert.ToDouble(obj);
            
                
            drRecord = dt.NewRow();
            drRecord["Activity"] = "Test";
            drRecord["Time"] = TimeSpent;
            dt.Rows.Add(drRecord);

            TotalTime += TimeSpent;
            Score = "";
            obj = dtTestTime.Compute("AVG(Percent_Score)", "TestType like '%Test%'");
            if (obj != DBNull.Value)
                Score = Convert.ToDouble(obj).ToString("n2");
            

            HtmlTable += "<tr><td>Test</td>" +
            "<td align='center'>" + cs.getDurationIn_hms(TimeSpent) + "</td>" +
            "<td align='center'>" + Score + "</td></tr></table>";
            ///-----------------------------------------------------------

            userImageURL = "../../images/NoChildImage.png";
            //userImage = dr["Users_Image"].ToString(); //enable code after component changed
            

            if (userImage != "" && userImage != "&nbsp;")
            {
                userImageURL = imageFilePath + studentId + "/" + userImage; //Image path is made up of the User's ID and the file name (Vishal - 23/01/2006)
            }

            if (child == 1)
            {
                lblTable1.Text = HtmlTable;
                lblStudent1.Text = "<img src='" + userImageURL + "' width='150' height='150' /> <br />" + "";
                lblMastery1.Text = "Number of skills attempted : " + skilAttempt + "</br>Number of skills mastered : " + skillMastered;
                lblDetail1.Text = buttonLink;
            }
            

            // GridView3.DataSource = dt;
            // GridView3.DataBind();

            string myCanvas = "";
            if (child == 1)
                myCanvas = "myCanvas1";
           

            string Chartcript = HTMLChart(dt, TotalTime, myCanvas,studentId);
            
            if (child == 1)
                lblChart1.Text = Chartcript;
 
    }


    private string HTMLChart(DataTable dt, double totaltime, string myCanvas,object student)
    {
        string sNames = "", sName = "", bkcolor = "", Timespent = "", borColor = "", borWidth = "", color = "";
        String chart = "<script type='text/javascript'>";
        //int canVasHeight=600;

        // labels along the x-axis
        double avgTime = 0, percent = 0, maxTime = 0, stepSize = 0;
        foreach (DataRow dr in dt.Rows)
        {
            percent = Math.Round(Convert.ToDouble(dr["Time"]) * 100 / totaltime);
            if (Double.IsNaN(percent))
                sName = dr["Activity"].ToString();
            else
                sName = dr["Activity"].ToString() + " " + percent + "%";
            sNames += "'" + sName + "',";
            avgTime = Math.Round(Convert.ToDouble(dr["Time"]));
            Timespent += Math.Round(avgTime) + ",";
            //Timespent += Convert.ToDouble(dr["Time"])+",";

            if (dr["Activity"].ToString() == "Tutorial")
                color = "'#FFA500'";
            if (dr["Activity"].ToString() == "Quiz")
                color = "'#666699'";
            if (dr["Activity"].ToString() == "Test")
                color = "'#3399CC'";
            if (dr["Activity"].ToString() == "Practice Sheet")
                color = "'#8BBA00'";
            if (dr["Activity"].ToString() == "Test")
                color = "'#1932B2'";


            bkcolor += color + ",";
            borColor += color + ",";
            borWidth += "1,";


            if (avgTime > maxTime)
                maxTime = avgTime;

            stepSize = Math.Round(maxTime / 6);


        }
        sNames = sNames.Substring(0, sNames.Length - 1);
        bkcolor = bkcolor.Substring(0, bkcolor.Length - 1);
        Timespent = Timespent.Substring(0, Timespent.Length - 1);
        borColor = borColor.Substring(0, borColor.Length - 1);
        borWidth = borWidth.Substring(0, borWidth.Length - 1);



        sNames = "[" + sNames + "];";
        bkcolor = "[" + bkcolor + "];";
        Timespent = "[" + Timespent + "];";
        borColor = "[" + borColor + "];";
        borWidth = "[" + borWidth + "];";

        chart += " var years = " + sNames;
        chart += " var backColor = " + bkcolor;
        chart += " var africa = " + Timespent;
        chart += " var borColor = " + borColor;
        chart += " var borWidth = " + borWidth;


        chart += " var ctx = document.getElementById('" + myCanvas + "');";

        chart += " var myChart = new Chart(ctx, {";
        chart += " type: 'bar',";
        chart += " data: {";
        chart += " highlightEnabled: false,";
        //chart += " events: [],";
        chart += " labels: years,";
        chart += " datasets: [{";
        chart += " data: africa,";
        //chart += " label: 'COUNT',";
        //chart += " fill: true,";
        chart += " backgroundColor: backColor,";
        chart += " borderColor: borColor";
        chart += ", borderWidth: borWidth";
        chart += "},";
        chart += "]";
        chart += "}"; // data

        chart += ", options: {";
        chart += " events: ['click'],";
        chart += " tooltips: {enabled: false},";
        chart += " hover: {cursor: 'pointer'},";
        //chart += " filter: {type: 'none'},";

	//chart += " tooltips: {mode: 'none',},";
        chart += " hover: {"+
      		 " onHover: (event, chartElement) => {"+
    		"  event.target.style.cursor = chartElement[0] ? 'pointer' : 'default';}},";

        chart += " responsive: true,";
        chart += " maintainAspectRatio: false,";
        chart += " onClick:function(e){";
        chart += " var activePoints = myChart.getElementsAtEvent(e);";
        chart += " var selectedIndex = activePoints[0]._index;";
        chart += " javascript:popDetail(" + student + ",'Time');"; //alert(this.data.labels[selectedIndex]+','+this.data.datasets[0].data[selectedIndex]);";
        chart += "}"; // function
        chart += ", legend: {" +
                    " display: false";
        chart += "}"; // legend

        chart += ", scales: {";

        chart += " yAxes: [{";
        chart += " ticks: {";
        chart += "               min: 0,";
        chart += "               stepSize: " + stepSize + ",";
        chart += "               max: " + (maxTime + stepSize + 2) + ",";
        chart += "		 display: false,";
        chart += "		 mirror: true";
        chart += "  },";
        chart += " display: true,";
        chart += " scaleLabel: {";
        chart += " display: true,";
        chart += " labelString: 'TIME SPENT'" + "}" + "}],";
        chart += " xAxes: [{";
        chart += " display: true,";
        chart += " scaleLabel: {";
        chart += " display: true,";
        chart += " labelString: 'ACTIVITIES'" + "}" + "}]";
        chart += "}"; // Scales

        chart += ", animation: {";
        chart += " duration: 0,";
        //chart += " easing: easeOutQuart,";
        chart += " onComplete: function () {";
        chart += " var chartInstance = this.chart,";
        chart += " ctx = chartInstance.ctx;";
        //chart += " ctx.height = 100;";
        //bar
        chart += " ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);";

        chart += " ctx.textAlign = 'center';";
        chart += " ctx.textBaseline = 'bottom';";


        chart += " this.data.datasets.forEach(function (dataset, i) {";

        //Bar
        chart += " var meta = chartInstance.controller.getDatasetMeta(i);";
        chart += " meta.data.forEach(function (bar, index) {";
        chart += " var data = dataset.data[index];";
        chart += " var min=0,hrs=0,sec=0;";
        chart += " hrs = Math.floor(data / 3600);"+
		 " data %= 3600;"+
		 " min = Math.floor(data / 60);"+
		 " sec = data % 60;"+
		 " data = hrs+':'+min+':'+sec;";

		    /*" if (data>3600)" +
                    " {" +
                       " if ((data/3600)>60)" +
                        "{   min=Math.floor((data/3600)/60);" +
                             " data=Math.floor(data/3600)+':'+min+':'+min%60;" +
                       " } else " +
                             "{ data=Math.floor(data/3600)+':'+data%3600+':0';}" +
                    " }" +
                    " else { data='0:'+Math.floor(data/60)+':'+data%60; }";*/

        chart += " ctx.fillText(data, bar._model.x, bar._model.y+1 );";
        chart += "});";
        chart += "});";
        chart += "}}";

        //--------

        chart += "}"; // options*/
        chart += "});"; // new chart
        chart += "</script>";

        return chart;
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




<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rpt_ELC_student_Summary.aspx.cs"
    Inherits="ELC_Time" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript">

    function SetDivWidth(cnt) {

        document.getElementById('chartDiv1').style.width = '400px';
     }
</script>
<head id="Head1" runat="server">
    <title>Summary Report</title>
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="-1">
    <link href="../../Style Sheets/CC/cc.css" type="text/css" rel="stylesheet" />
    <link type='text/css' rel='stylesheet' href="../js/chart.css" />
    <script type="text/javascript" language="Javascript" src="../js/chart.js"></script>
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="form1" runat="server">

    <table id="Table1" cellspacing="0" cellpadding="0" width="96%" align="center" border="0">
        <tr>
            <td>
                
                                        <table width='100%' align='center' style="font-size: 8pt; font-family: Ariel;">
                                            <tr>
                                                <td align='left'>
                                                    &nbsp;</td>
                                                <td align='left' colspan="3">
                                                </td>
                                            </tr>
                                            <tr id='chartrow1'>
                                                <td align='center'>
                                                    <asp:label id="lblStudent1" runat="server" style="font-size: 14pt;"></asp:label>
                                                </td>
                                                <td align='center'>
                                                    &nbsp;&nbsp<div id='chartDiv1' style='width: 400px; height: 200px;'>
                                                        <canvas id="myCanvas1"></canvas>
                                                        <asp:label id="lblChart1" runat="server"></asp:label>
                                                        <br />
                                                        <br />
                                                        <asp:label id="timeDetail1" runat="server" style="font-size: 10pt">Click the 
                                                        chart to view activity list</asp:label>
                                                    </div>
                                                </td>
                                                <td align='center' valign="top">
                                                </td>
                                                <td align='center' valign="top">
                                                    &nbsp;&nbsp<asp:label id="lblTable1" runat="server"></asp:label>
                                                </br><asp:Label ID="lblMastery1" runat="server" 
                                                        style="font-weight: 700; font-size: 10pt"></asp:Label>
                                                    </br><asp:label id="lblDetail1" runat="server"></asp:label>
                                                </td>
                                            </tr>
                                            
                                            </table>
                
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    </form>
</body>
</html>
<script type="text/javascript">

   function popDetail(studentid,activity) {
	if (activity=='Time')
		url = "Rpt_ELC_TimeDetail.aspx?id=" + studentid+"&type=AL";
	else
       		url = "rpt_ELC_parent_Summary_Detail.aspx?studentId=" + studentid;
        
        var w = screen.width / 1.5;
        var h = screen.height / 2;
        var x = (screen.width - w) / 2;
        y = 0;
        winprops = 'height=' + h + ',width=' + w + ',top=' + y + ',left=' + x + ',resizable=0,' + 'status=1,scrollbars=1';
        winClassGradeRpt = window.open(url, 'name', winprops);
        if (!winClassGradeRpt.opener) winClassGradeRpt.opener = self;
        winClassGradeRpt.focus();
    }
	

</script>

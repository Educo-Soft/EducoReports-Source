<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ELCStandard.aspx.cs" Inherits="ELCStandards" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Skills Report</title>
    </head>
<body bgcolor='#FFFFFF'>
    

<table rules="all" width='99%' align='center'>
<tr><td width='70%'>STUDENT:
<asp:Label ID="lblStudent" runat="server" style="font-weight: 700"></asp:Label></td>
<td align='right'>
    <table rules="all" width='100%' align='right' style='font-size: small;'>
        <tr>
            <td>
                <strong>Percentatge</strong></td>
            <td>
                <strong>Level of Mastery</strong></td>
            <td>
                &nbsp;</td>
        </tr>
        <tr bgColor='#DBDBFF'>
            <td>
                90-100</td>
            <td>
                Adavanced</td>
            <td>
                Exceeding academic standards</td>
        </tr>
        <tr bgColor='#C8FFFF'>
            <td>
                80-89</td>
            <td>
                Proficient</td>
            <td>
                Meeting academic standards</td>
        </tr>
        <tr bgColor='#FFFFCC'>
            <td>
                70-79</td>
            <td>
                Progressing</td>
            <td>
                Demonstrates partial understanding</td>
        </tr>
        <tr bgColor='#FFCC66'>
            <td>
                0-69</td>
            <td>
                Developing</td>
            <td>
                Not meeting sstandards at this time</td>
        </tr>
    </table>
    </td>
    </tr>
    </table>
    <form id="form1" runat="server">
    
    <p>
    <asp:Label ID="lblReport" runat="server"></asp:Label>
    </p>
    </form>

    </body>
</html>

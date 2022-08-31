<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Rpt_ELC_TimeDetail.aspx.cs"  Inherits="ELC_TimeDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta name="viewport" content="width=device-width, initial-scale=1">
<style>


/* Style the tab */
.tab {
  overflow: hidden;
  border: 1px solid #ccc;
  background-color: #f1f1f1;
}

/* Style the buttons inside the tab */
.tab button {
  background-color: inherit;
  float: left;
  border: none;
  outline: none;
  cursor: pointer;
  padding: 14px 16px;
  transition: 0.3s;
  font-size: 14px;
}

/* Change background color of buttons on hover */
.tab button:hover {
  background-color: #ddd;
}

/* Create an active/current tablink class */
.tab button.active {
  background-color: #ccc;
}

/* Style the tab content */
.tabcontent {
  /*display: none;*/
  overflow: hidden;
  padding: 0px 0px;
  border: 1px solid #ccc;
  border-top: none;
}
</style>

  
      
   </head>
   
   <body>
    
	<form id='td' runat="server">
         <img alt="" src="" />
                <asp:Label ID="lblName" runat="server" ></asp:Label>
     </form>
	<div class="tab">
  		<button class="tablinks" id='AL' onclick="javascript:openTab('All');"><b>All</b></button>
  		<button class="tablinks" id='TU' onclick="javascript:openTab('Tutorial');"><b>Tutorial</b></button>
  		<button class="tablinks" id='QZ' onclick="javascript:openTab('Quiz');"><b>Quiz</b></button>
  		<button class="tablinks" id='PS' onclick="javascript:openTab('Practice-Sheet');"><b>Practice Sheet</b></button>
  		<button class="tablinks" id='TS' onclick="javascript:openTab('Test');"><b>Test</b></button>
	</div>
    <div id="All" class="tabcontent">
     <asp:Label ID="lblData" runat="server"></asp:Label>
</div>
</body>
</html>

<script type="text/javascript">

var param = getParameterByName('type');
document.getElementById(param).style.backgroundColor = "lightblue";

    function openTab(activity) {
    
    var id = getParameterByName('id');
        if (activity == 'All')
            window.location = "Rpt_ELC_TimeDetail.aspx?id="+id+"&type=AL";
        if (activity == 'Quiz')
            window.location = "Rpt_ELC_TimeDetail.aspx?id="+id+"&type=QZ";
        if (activity == 'Test')
            window.location = "Rpt_ELC_TimeDetail.aspx?id="+id+"&type=TS";
        if (activity == 'Tutorial')
            window.location = "Rpt_ELC_TimeDetail.aspx?id="+id+"&type=TU";
        if (activity == 'Practice-Sheet')
            window.location = "Rpt_ELC_TimeDetail.aspx?id="+id+"&type=PS";
    }

    function getParameterByName(name, url = window.location.href) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
	
   </script>



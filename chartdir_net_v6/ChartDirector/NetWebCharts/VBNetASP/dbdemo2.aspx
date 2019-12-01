<%@ Page Language="VB" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>

<script runat="server">

'
' Create the first chart based on the given data
'
Private Sub createChart1(viewer As WebChartViewer, selectedYear As String, software As Double(), _
    hardware As Double(), services As Double())

    ' Create a XYChart object of size 600 x 300 pixels, with a light grey (eeeeee) background, black
    ' border, 1 pixel 3D border effect and rounded corners.
    Dim c As XYChart = New XYChart(600, 300, &Heeeeee, &H000000, 1)
    c.setRoundedFrame()

    ' Set the plotarea at (60, 60) and of size 520 x 200 pixels. Set background color to white
    ' (ffffff) and border and grid colors to grey (cccccc)
    c.setPlotArea(60, 60, 520, 200, &Hffffff, -1, &Hcccccc, &Hccccccc)

    ' Add a title to the chart using 15pt Times Bold Italic font, with a light blue (ccccff)
    ' background and with glass lighting effects.
    c.addTitle("Global Revenue for Year " & selectedYear, "Times New Roman Bold Italic", 15 _
        ).setBackground(&Hccccff, &H000000, Chart.glassEffect())

    ' Add a legend box at (70, 32) (top of the plotarea) with 9pt Arial Bold font
    c.addLegend(70, 32, False, "Arial Bold", 9).setBackground(Chart.Transparent)

    ' Add a line chart layer using the supplied data
    Dim layer As LineLayer = c.addLineLayer2()
    layer.addDataSet(software, &Hff0000, "Software").setDataSymbol(Chart.CircleShape, 9)
    layer.addDataSet(hardware, &H00ff00, "Hardware").setDataSymbol(Chart.DiamondShape, 11)
    layer.addDataSet(services, &Hffaa00, "Services").setDataSymbol(Chart.Cross2Shape(), 11)

    ' Set the line width to 3 pixels
    layer.setLineWidth(3)

    ' Set the x axis labels. In this example, the labels must be Jan - Dec.
    Dim labels() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", _
        "Oct", "Nov", "Dec"}
    c.xAxis().setLabels(labels)

    ' Set the y axis title
    c.yAxis().setTitle("USD (Millions)")

    ' Set axes width to 2 pixels
    c.xAxis().setWidth(2)
    c.yAxis().setWidth(2)

    ' Output the chart
    viewer.Image = c.makeWebImage(Chart.PNG)

    ' Include tool tip for the chart
    viewer.ImageMap = c.getHTMLImageMap("", "", _
        "title='{dataSetName} Revenue for {xLabel} = USD {value}M'")

End Sub

'
' Create the second chart based on the given data
'
Private Sub createChart2(viewer As WebChartViewer, selectedYear As String, software As Double(), _
    hardware As Double(), services As Double())

    ' Create a XYChart object of size 600 x 300 pixels, with a light grey (eeeeee) background, black
    ' border, 1 pixel 3D border effect and rounded corners.
    Dim c As XYChart = New XYChart(600, 300, &Heeeeee, &H000000, 1)
    c.setRoundedFrame()

    ' Set the plotarea at (60, 60) and of size 520 x 200 pixels. Set background color to white
    ' (ffffff) and border and grid colors to grey (cccccc)
    c.setPlotArea(60, 60, 520, 200, &Hffffff, -1, &Hcccccc, &Hccccccc)

    ' Add a title to the chart using 15pt Times Bold Italic font, with a dark green (006600)
    ' background and with glass lighting effects.
    c.addTitle("Global Revenue for Year " & selectedYear, "Times New Roman Bold Italic", 15, _
        &Hffffff).setBackground(&H006600, &H000000, Chart.glassEffect(Chart.ReducedGlare))

    ' Add a legend box at (70, 32) (top of the plotarea) with 9pt Arial Bold font
    c.addLegend(70, 32, False, "Arial Bold", 9).setBackground(Chart.Transparent)

    ' Add a stacked area chart layer using the supplied data
    Dim layer As AreaLayer = c.addAreaLayer2(Chart.Stack)
    layer.addDataSet(software, &H40ff0000, "Software")
    layer.addDataSet(hardware, &H4000ff00, "Hardware")
    layer.addDataSet(services, &H40ffaa00, "Services")

    ' Set the area border color to the same as the fill color
    layer.setBorderColor(Chart.SameAsMainColor)

    ' Set the x axis labels. In this example, the labels must be Jan - Dec.
    Dim labels() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", _
        "Oct", "Nov", "Dec"}
    c.xAxis().setLabels(labels)

    ' Set the y axis title
    c.yAxis().setTitle("USD (Millions)")

    ' Set axes width to 2 pixels
    c.xAxis().setWidth(2)
    c.yAxis().setWidth(2)

    ' Output the chart
    viewer.Image = c.makeWebImage(Chart.PNG)

    ' Include tool tip for the chart
    viewer.ImageMap = c.getHTMLImageMap("", "", _
        "title='{dataSetName} Revenue for {xLabel} = USD {value}M'")

End Sub

'
' Page Load event handler
'
Private Sub Page_Load(sender As System.Object, e As System.EventArgs)

    ' The currently selected year
    Dim selectedYear As String = yearSelect.SelectedItem.Value

    ' SQL statement to get the monthly revenues for the selected year.
    Dim SQL As String = _
        "Select Software, Hardware, Services From revenue Where Year(TimeStamp) = " & selectedYear _
         & " Order By TimeStamp"

    '
    ' Connect to database and read the query result into arrays
    '

    ' In this example, we use OleDbConnection to connect to MS Access (Jet Engine). If you are using
    ' MS SQL, you can use SqlConnection instead of OleConnection.
    Dim dbconn As System.Data.IDbConnection = New System.Data.OleDb.OleDbConnection( _
        "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("sample.mdb;"))
    dbconn.Open()

    ' Set up the SQL statement
    Dim sqlCmd As System.Data.IDbCommand = dbconn.CreateCommand()
    sqlCmd.CommandText = SQL

    ' Read the data into the DBTable object
    Dim table As DBTable = New DBTable(sqlCmd.ExecuteReader())
    dbconn.Close()

    ' Get the data as arrays
    Dim software() As Double = table.getCol(0)
    Dim hardware() As Double = table.getCol(1)
    Dim services() As Double = table.getCol(2)

    '
    ' Now we obtained the data into arrays, we can draw the chart using ChartDirector
    '

    createChart1(WebChartViewer1, yearSelect.SelectedItem.Value, software, hardware, services)
    createChart2(WebChartViewer2, yearSelect.SelectedItem.Value, software, hardware, services)

End Sub

</script>

<html>
<head>
    <title>Database Integration Demo (2)</title>
</head>
<body style="margin:5px 0px 0px 5px">
<div style="font-size:18pt; font-family:verdana; font-weight:bold">
    Database Integration Demo (2)
</div>
<hr style="border:solid 1px #000080" />
<div style="font-size:10pt; font-family:verdana; width:600px">
<a href='viewsource.aspx?file=<%=Request("SCRIPT_NAME")%>'>
    View Source Code
</a>
<br />
<br />
The example demonstrates creating two charts in the same page using data from a database.
<br />
<br />
<form id="Form1" method="post" runat="server">
<div>
    I want to obtain the revenue data for the year
    <asp:DropDownList id="yearSelect" runat="server">
        <asp:ListItem>1990</asp:ListItem>
        <asp:ListItem>1991</asp:ListItem>
        <asp:ListItem>1992</asp:ListItem>
        <asp:ListItem>1993</asp:ListItem>
        <asp:ListItem>1994</asp:ListItem>
        <asp:ListItem>1995</asp:ListItem>
        <asp:ListItem>1996</asp:ListItem>
        <asp:ListItem>1997</asp:ListItem>
        <asp:ListItem>1998</asp:ListItem>
        <asp:ListItem>1999</asp:ListItem>
        <asp:ListItem>2000</asp:ListItem>
        <asp:ListItem Selected="True">2001</asp:ListItem>
    </asp:DropDownList>
    <asp:Button id="OKPB" runat="server" Text="OK"></asp:Button>
    <br /><br />
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
    <br />
    <chart:WebChartViewer id="WebChartViewer2" runat="server" />
</div>
</form>
</div>
</body>
</html>

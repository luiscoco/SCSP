<%@ Page Language="VB" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>

<script runat="server">

'
' Page Load event handler
'
Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '
    ' Displays the monthly revenue for the selected year. The selected year should be passed in as a
    ' query parameter called "xLabel"
    '
    Dim selectedYear As String = Request("xLabel")

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
    ' Now we have read data into arrays, we can draw the chart using ChartDirector
    '

    ' Create a XYChart object of size 600 x 360 pixels
    Dim c As XYChart = New XYChart(600, 360)

    ' Set the plotarea at (60, 50) and of size 480 x 270 pixels. Use a vertical gradient color from
    ' light blue (eeeeff) to deep blue (0000cc) as background. Set border and grid lines to white
    ' (ffffff).
    c.setPlotArea(60, 50, 480, 270, c.linearGradientColor(60, 50, 60, 270, &Heeeeff, &H0000cc), _
        -1, &Hffffff, &Hffffff)

    ' Add a title to the chart using 15pt Times Bold Italic font
    c.addTitle("Global Revenue for Year " & selectedYear, "Times New Roman Bold Italic", 18)

    ' Add a legend box at (60, 25) (top of the plotarea) with 9pt Arial Bold font
    c.addLegend(60, 25, False, "Arial Bold", 9).setBackground(Chart.Transparent)

    ' Add a line chart layer using the supplied data
    Dim layer As LineLayer = c.addLineLayer2()
    layer.addDataSet(software, &Hffaa00, "Software").setDataSymbol(Chart.CircleShape, 9)
    layer.addDataSet(hardware, &H00ff00, "Hardware").setDataSymbol(Chart.DiamondShape, 11)
    layer.addDataSet(services, &Hff0000, "Services").setDataSymbol(Chart.Cross2Shape(), 11)

    ' Set the line width to 3 pixels
    layer.setLineWidth(3)

    ' Set the x axis labels. In this example, the labels must be Jan - Dec.
    Dim labels() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", _
        "Oct", "Nov", "Dec"}
    c.xAxis().setLabels(labels)

    ' Set y-axis tick density to 30 pixels. ChartDirector auto-scaling will use this as the
    ' guideline when putting ticks on the y-axis.
    c.yAxis().setTickDensity(30)

    ' Synchronize the left and right y-axes
    c.syncYAxis()

    ' Set the y axes titles with 10pt Arial Bold font
    c.yAxis().setTitle("USD (Millions)", "Arial Bold", 10)
    c.yAxis2().setTitle("USD (Millions)", "Arial Bold", 10)

    ' Set all axes to transparent
    c.xAxis().setColors(Chart.Transparent)
    c.yAxis().setColors(Chart.Transparent)
    c.yAxis2().setColors(Chart.Transparent)

    ' Set the label styles of all axes to 8pt Arial Bold font
    c.xAxis().setLabelStyle("Arial Bold", 8)
    c.yAxis().setLabelStyle("Arial Bold", 8)
    c.yAxis2().setLabelStyle("Arial Bold", 8)

    ' Create the image and save it in a temporary location
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG)

    ' Create an image map for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("xystub.aspx", "", _
        "title='{dataSetName} @ {xLabel} = USD {value|0}M'")

End Sub

</script>

<html>
<head>
    <title>Database Clickable Charts</title>
</head>
<body style="margin:5px 0px 0px 5px">
<div style="font-size:18pt; font-family:verdana; font-weight:bold">
    Database Clickable Charts
</div>
<hr style="border:solid 1px #000080" />
<div style="font-size:10pt; font-family:verdana; width:600px; margin-bottom:20px">
    You have click the bar for the year <%=Request("xLabel")%>.
    Below is the "drill-down" chart showing the monthly details.
<br /><br />
<a href='viewsource.aspx?file=<%=Request("SCRIPT_NAME")%>'>
    View source code
</a>
</div>

<chart:WebChartViewer id="WebChartViewer1" runat="server" />

</body>
</html>

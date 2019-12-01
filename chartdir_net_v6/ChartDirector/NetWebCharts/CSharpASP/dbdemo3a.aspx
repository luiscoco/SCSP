<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>

<script runat="server">

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    //
    // Displays the monthly revenue for the selected year. The selected year should be passed in as
    // a query parameter called "xLabel"
    //
    string selectedYear = Request["xLabel"];

    // SQL statement to get the monthly revenues for the selected year.
    string SQL = "Select Software, Hardware, Services From revenue Where Year(TimeStamp) = " +
        selectedYear + " Order By TimeStamp";

    //
    // Connect to database and read the query result into arrays
    //

    // In this example, we use OleDbConnection to connect to MS Access (Jet Engine). If you are
    // using MS SQL, you can use SqlConnection instead of OleConnection.
    System.Data.IDbConnection dbconn = new System.Data.OleDb.OleDbConnection(
        "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("sample.mdb;"));
    dbconn.Open();

    // Set up the SQL statement
    System.Data.IDbCommand sqlCmd = dbconn.CreateCommand();
    sqlCmd.CommandText = SQL;

    // Read the data into the DBTable object
    DBTable table = new DBTable(sqlCmd.ExecuteReader());
    dbconn.Close();

    // Get the data as arrays
    double[] software = table.getCol(0);
    double[] hardware = table.getCol(1);
    double[] services = table.getCol(2);

    //
    // Now we have read data into arrays, we can draw the chart using ChartDirector
    //

    // Create a XYChart object of size 600 x 360 pixels
    XYChart c = new XYChart(600, 360);

    // Set the plotarea at (60, 50) and of size 480 x 270 pixels. Use a vertical gradient color from
    // light blue (eeeeff) to deep blue (0000cc) as background. Set border and grid lines to white
    // (ffffff).
    c.setPlotArea(60, 50, 480, 270, c.linearGradientColor(60, 50, 60, 270, 0xeeeeff, 0x0000cc), -1,
        0xffffff, 0xffffff);

    // Add a title to the chart using 15pt Times Bold Italic font
    c.addTitle("Global Revenue for Year " + selectedYear, "Times New Roman Bold Italic", 18);

    // Add a legend box at (60, 25) (top of the plotarea) with 9pt Arial Bold font
    c.addLegend(60, 25, false, "Arial Bold", 9).setBackground(Chart.Transparent);

    // Add a line chart layer using the supplied data
    LineLayer layer = c.addLineLayer2();
    layer.addDataSet(software, 0xffaa00, "Software").setDataSymbol(Chart.CircleShape, 9);
    layer.addDataSet(hardware, 0x00ff00, "Hardware").setDataSymbol(Chart.DiamondShape, 11);
    layer.addDataSet(services, 0xff0000, "Services").setDataSymbol(Chart.Cross2Shape(), 11);

    // Set the line width to 3 pixels
    layer.setLineWidth(3);

    // Set the x axis labels. In this example, the labels must be Jan - Dec.
    string[] labels = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov",
        "Dec"};
    c.xAxis().setLabels(labels);

    // Set y-axis tick density to 30 pixels. ChartDirector auto-scaling will use this as the
    // guideline when putting ticks on the y-axis.
    c.yAxis().setTickDensity(30);

    // Synchronize the left and right y-axes
    c.syncYAxis();

    // Set the y axes titles with 10pt Arial Bold font
    c.yAxis().setTitle("USD (Millions)", "Arial Bold", 10);
    c.yAxis2().setTitle("USD (Millions)", "Arial Bold", 10);

    // Set all axes to transparent
    c.xAxis().setColors(Chart.Transparent);
    c.yAxis().setColors(Chart.Transparent);
    c.yAxis2().setColors(Chart.Transparent);

    // Set the label styles of all axes to 8pt Arial Bold font
    c.xAxis().setLabelStyle("Arial Bold", 8);
    c.yAxis().setLabelStyle("Arial Bold", 8);
    c.yAxis2().setLabelStyle("Arial Bold", 8);

    // Create the image and save it in a temporary location
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

    // Create an image map for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("xystub.aspx", "",
        "title='{dataSetName} @ {xLabel} = USD {value|0}M'");
}

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
    You have click the bar for the year <%=Request["xLabel"]%>.
    Below is the "drill-down" chart showing the monthly details.
<br /><br />
<a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>
    View source code
</a>
</div>

<chart:WebChartViewer id="WebChartViewer1" runat="server" />

</body>
</html>

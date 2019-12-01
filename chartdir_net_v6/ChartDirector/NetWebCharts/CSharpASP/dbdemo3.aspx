<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>

<script runat="server">

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    // SQL statement to get the revenue for the 12 years from 1990 to 2001
    string SQL =
        "Select Sum(Software + Hardware + Services), Year(TimeStamp) From revenue Where " +
        "Year(TimeStamp) >= 1990 And Year(TimeStamp) <= 2001 Group By Year(TimeStamp) Order By " +
        "Year(TimeStamp)";

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
    double[] revenue = table.getCol(0);
    double[] timestamp = table.getCol(1);

    //
    // Now we have read data into arrays, we can draw the chart using ChartDirector
    //

    // Create a XYChart object of size 600 x 360 pixels
    XYChart c = new XYChart(600, 360);

    // Set the plotarea at (60, 40) and of size 480 x 280 pixels. Use a vertical gradient color from
    // light blue (eeeeff) to deep blue (0000cc) as background. Set border and grid lines to white
    // (ffffff).
    c.setPlotArea(60, 40, 480, 280, c.linearGradientColor(60, 40, 60, 280, 0xeeeeff, 0x0000cc), -1,
        0xffffff, 0xffffff);

    // Add a title to the chart using 18pt Times Bold Italic font
    c.addTitle("Annual Revenue for Star Tech", "Times New Roman Bold Italic", 18);

    // Add a multi-color bar chart layer using the supplied data
    BarLayer layer = c.addBarLayer3(revenue);

    // Use glass lighting effect with light direction from the left
    layer.setBorderColor(Chart.Transparent, Chart.glassEffect(Chart.NormalGlare, Chart.Left));

    // Set the x axis labels
    c.xAxis().setLabels(timestamp);

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

    // Create the image
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

    // Create an image map for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("dbdemo3a.aspx", "",
        "title='{xLabel}: US$ {value|0}M'");
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
    The example demonstrates creating a clickable chart using data from a database.
    Click on a bar below to "drill down" onto a particular year.
<br /><br />
<a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>
    View source code
</a>
</div>

<chart:WebChartViewer id="WebChartViewer1" runat="server" />

</body>
</html>

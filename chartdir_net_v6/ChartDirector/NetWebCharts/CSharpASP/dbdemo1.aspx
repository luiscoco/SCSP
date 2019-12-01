<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>

<script runat="server">

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    // The currently selected year
    string selectedYear = yearSelect.SelectedItem.Value;

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

    // Create a XYChart object of size 600 x 300 pixels, with a light grey (eeeeee) background,
    // black border, 1 pixel 3D border effect and rounded corners.
    XYChart c = new XYChart(600, 300, 0xeeeeee, 0x000000, 1);
    c.setRoundedFrame();

    // Set the plotarea at (60, 60) and of size 520 x 200 pixels. Set background color to white
    // (ffffff) and border and grid colors to grey (cccccc)
    c.setPlotArea(60, 60, 520, 200, 0xffffff, -1, 0xcccccc, 0xccccccc);

    // Add a title to the chart using 15pt Times Bold Italic font, with a light blue (ccccff)
    // background and with glass lighting effects.
    c.addTitle("Global Revenue for Year " + selectedYear, "Times New Roman Bold Italic", 15
        ).setBackground(0xccccff, 0x000000, Chart.glassEffect());

    // Add a legend box at (70, 32) (top of the plotarea) with 9pt Arial Bold font
    c.addLegend(70, 32, false, "Arial Bold", 9).setBackground(Chart.Transparent);

    // Add a stacked bar chart layer using the supplied data
    BarLayer layer = c.addBarLayer2(Chart.Stack);
    layer.addDataSet(software, 0xff0000, "Software");
    layer.addDataSet(hardware, 0x00ff00, "Hardware");
    layer.addDataSet(services, 0xffaa00, "Services");

    // Use soft lighting effect with light direction from the left
    layer.setBorderColor(Chart.Transparent, Chart.softLighting(Chart.Left));

    // Set the x axis labels. In this example, the labels must be Jan - Dec.
    string[] labels = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov",
        "Dec"};
    c.xAxis().setLabels(labels);

    // Draw the ticks between label positions (instead of at label positions)
    c.xAxis().setTickOffset(0.5);

    // Set the y axis title
    c.yAxis().setTitle("USD (Millions)");

    // Set axes width to 2 pixels
    c.xAxis().setWidth(2);
    c.yAxis().setWidth(2);

    // Output the chart in PNG format
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

    // Include tool tip for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
        "title='{dataSetName} Revenue for {xLabel} = USD {value}M'");
}

</script>

<html>
<head>
    <title>Database Integration Demo (1)</title>
</head>
<body style="margin:5px 0px 0px 5px">
<div style="font-size:18pt; font-family:verdana; font-weight:bold">
    Database Integration Demo (1)
</div>
<hr style="border:solid 1px #000080" />
<div style="font-size:10pt; font-family:verdana; margin-bottom:20px">
<a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>
    View Source Code
</a>
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
</div>
</form>
</div>
</body>
</html>

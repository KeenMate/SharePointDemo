using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Drawing.Text;

/// <summary>
/// Summary description for XtraReport1
/// </summary>
public class StockReport : DevExpress.XtraReports.UI.XtraReport
{
	private DevExpress.XtraReports.UI.DetailBand Detail;
	private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
	private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
	private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
	private DetailReportBand DetailReport;
	private DetailBand Detail1;
	private XRTable xrTable2;
	private XRTableRow xrTableRow2;
	private XRTableCell xrTableCell5;
	private XRTableCell xrTableCell9;
	private XRTableCell xrTableCell10;
	private GroupHeaderBand GroupHeader1;
	private XRTable xrTable3;
	private XRTableRow xrTableRow3;
	private XRTableCell xrTableCell11;
	private XRTableCell xrTableCell15;
	private XRTableCell xrTableCell16;
	private XRControlStyle xrControlStyle1;
	private GroupHeaderBand GroupHeader2;
	private XRLabel xrLabel1;
	private XRLabel xrLabel5;
	private XRLabel xrLabel4;
	private XRLabel xrLabel3;
	private XRLabel xrLabel2;
	private XRLine xrLine1;
	private GroupFooterBand GroupFooter1;
	private XRLabel xrLabel6;
	private XRLine xrLine2;
	private XRPictureBox xrPictureBox1;
	private XRLabel xrLabel7;
	private XRLine xrLine3;
	private XRLine xrLine4;
	private XRLabel xrLabel8;
	private XRPageInfo xrPageInfo1;
	private XRLabel xrLabel9;
	private ReportFooterBand ReportFooter;
	private XRLine xrLine5;

	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;
	public PrivateFontCollection Fonts;
	public StockReport()
	{
		Fonts = new PrivateFontCollection();
		var baseDir = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory).Replace("file:\\", string.Empty);
		Fonts.AddFontFile(baseDir + "\\fonts\\roboto\\Roboto-Regular.ttf");//converted otf file that we want to use, airstrike.ttf is a genuine ttf file but gives the same results.
		Fonts.AddFontFile(baseDir + "\\fonts\\roboto\\Roboto-Medium.ttf");
		Fonts.AddFontFile(baseDir + "\\fonts\\roboto\\Roboto-Bold.ttf");
		//var f = new Font(Fonts.Families[0], 24, FontStyle.Regular, GraphicsUnit.Pixel);
		InitializeComponent();

		foreach (Band band in this.Bands)
		{
			foreach (XRControl control in band.Controls)
			{
				if (control is XRLabel || control is XRTableCell)
				{
					control.Font = new Font(Fonts.Families[0], control.Font.Size, control.Font.Style);
				}
				else
				{
					if (control.Controls.Count > 0)
						foreach (XRControl child in control.Controls)
						{
							child.Font = new Font(Fonts.Families[0], control.Font.Size, child.Font.Style);
						}
				}

			}
		}
	}

	/// <summary> 
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockReport));
			DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
			this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
			this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
			this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
			this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
			this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
			this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
			this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
			this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
			this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
			this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
			this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
			this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
			this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
			this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
			this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
			this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
			this.xrLine5 = new DevExpress.XtraReports.UI.XRLine();
			this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
			this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
			((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.HeightF = 0F;
			this.Detail.KeepTogether = true;
			this.Detail.KeepTogetherWithDetailReports = true;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrLabel5
			// 
			this.xrLabel5.Font = new System.Drawing.Font("Trebuchet MS", 10.2F);
			this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(376.2085F, 49.57748F);
			this.xrLabel5.Name = "xrLabel5";
			this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel5.SizeF = new System.Drawing.SizeF(344.7907F, 23.00001F);
			this.xrLabel5.StylePriority.UseFont = false;
			this.xrLabel5.StylePriority.UseTextAlignment = false;
			this.xrLabel5.Text = "Report generated by [FullName]";
			this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel4
			// 
			this.xrLabel4.Font = new System.Drawing.Font("Trebuchet MS", 10.2F);
			this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(376.2085F, 26.57749F);
			this.xrLabel4.Name = "xrLabel4";
			this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel4.SizeF = new System.Drawing.SizeF(344.7907F, 23F);
			this.xrLabel4.StylePriority.UseFont = false;
			this.xrLabel4.StylePriority.UseTextAlignment = false;
			this.xrLabel4.Text = "Report generated on [On!d. MMMM yyyy H:mm]";
			this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel3
			// 
			this.xrLabel3.Font = new System.Drawing.Font("Trebuchet MS", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(376.2085F, 10F);
			this.xrLabel3.Name = "xrLabel3";
			this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel3.SizeF = new System.Drawing.SizeF(344.7916F, 23F);
			this.xrLabel3.StylePriority.UseFont = false;
			this.xrLabel3.StylePriority.UseTextAlignment = false;
			this.xrLabel3.Text = "Total unique items in stock: [TotalStockItems]";
			this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel2
			// 
			this.xrLabel2.Font = new System.Drawing.Font("Trebuchet MS", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(376.2081F, 32.99998F);
			this.xrLabel2.Name = "xrLabel2";
			this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel2.SizeF = new System.Drawing.SizeF(344.7916F, 23F);
			this.xrLabel2.StylePriority.UseFont = false;
			this.xrLabel2.StylePriority.UseTextAlignment = false;
			this.xrLabel2.Text = "Total stock cost: [TotalCost!#,#] €";
			this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// TopMargin
			// 
			this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrLabel4,
            this.xrPictureBox1,
            this.xrLine3});
			this.TopMargin.Font = new System.Drawing.Font("Trebuchet MS", 9.75F);
			this.TopMargin.HeightF = 95.83334F;
			this.TopMargin.Name = "TopMargin";
			this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.TopMargin.StylePriority.UseFont = false;
			this.TopMargin.StylePriority.UsePadding = false;
			this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrPictureBox1
			// 
			this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
			this.xrPictureBox1.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
			this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(9.999561F, 26.57749F);
			this.xrPictureBox1.Name = "xrPictureBox1";
			this.xrPictureBox1.SizeF = new System.Drawing.SizeF(46.88154F, 41.67249F);
			this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.AutoSize;
			// 
			// xrLine3
			// 
			this.xrLine3.LineWidth = 3;
			this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 72.57748F);
			this.xrLine3.Name = "xrLine3";
			this.xrLine3.SizeF = new System.Drawing.SizeF(721.0001F, 12.42252F);
			// 
			// BottomMargin
			// 
			this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine4,
            this.xrLabel7,
            this.xrLabel8,
            this.xrPageInfo1});
			this.BottomMargin.HeightF = 100F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrLine4
			// 
			this.xrLine4.LineWidth = 3;
			this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrLine4.Name = "xrLine4";
			this.xrLine4.SizeF = new System.Drawing.SizeF(720.9996F, 13.62502F);
			// 
			// xrLabel7
			// 
			this.xrLabel7.Font = new System.Drawing.Font("Trebuchet MS", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 13.62503F);
			this.xrLabel7.Multiline = true;
			this.xrLabel7.Name = "xrLabel7";
			this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel7.SizeF = new System.Drawing.SizeF(175.2144F, 39.66668F);
			this.xrLabel7.StylePriority.UseFont = false;
			this.xrLabel7.Text = "KEEN|MATE\r\nMilíčova 12, Ostrava";
			// 
			// xrLabel8
			// 
			this.xrLabel8.Font = new System.Drawing.Font("Trebuchet MS", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(528.9166F, 13.62503F);
			this.xrLabel8.Name = "xrLabel8";
			this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel8.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
			this.xrLabel8.SizeF = new System.Drawing.SizeF(192.0825F, 23F);
			this.xrLabel8.StylePriority.UseFont = false;
			this.xrLabel8.Text = "contact@keenmate.com";
			// 
			// xrPageInfo1
			// 
			this.xrPageInfo1.Font = new System.Drawing.Font("Trebuchet MS", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(185.2144F, 13.62503F);
			this.xrPageInfo1.Name = "xrPageInfo1";
			this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrPageInfo1.SizeF = new System.Drawing.SizeF(343.7022F, 23F);
			this.xrPageInfo1.StylePriority.UseFont = false;
			this.xrPageInfo1.StylePriority.UseTextAlignment = false;
			this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			this.xrPageInfo1.TextFormatString = "Page {0} of {1}";
			// 
			// DetailReport
			// 
			this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1,
            this.GroupHeader1,
            this.GroupHeader2,
            this.GroupFooter1,
            this.ReportFooter});
			this.DetailReport.DataMember = "Items";
			this.DetailReport.DataSource = this.objectDataSource1;
			this.DetailReport.Level = 0;
			this.DetailReport.Name = "DetailReport";
			// 
			// Detail1
			// 
			this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
			this.Detail1.HeightF = 25F;
			this.Detail1.Name = "Detail1";
			// 
			// xrTable2
			// 
			this.xrTable2.EvenStyleName = "xrControlStyle1";
			this.xrTable2.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrTable2.Name = "xrTable2";
			this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
			this.xrTable2.SizeF = new System.Drawing.SizeF(720.9999F, 25F);
			this.xrTable2.StylePriority.UseFont = false;
			this.xrTable2.StylePriority.UseTextAlignment = false;
			this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrTableRow2
			// 
			this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.xrTableCell9,
            this.xrTableCell10});
			this.xrTableRow2.Name = "xrTableRow2";
			this.xrTableRow2.Weight = 11.5D;
			// 
			// xrTableCell5
			// 
			this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Items.Amount")});
			this.xrTableCell5.Font = new System.Drawing.Font("Trebuchet MS", 11F);
			this.xrTableCell5.Name = "xrTableCell5";
			this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 40, 0, 0, 100F);
			this.xrTableCell5.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
			this.xrTableCell5.StylePriority.UseFont = false;
			this.xrTableCell5.StylePriority.UsePadding = false;
			this.xrTableCell5.Text = "xrTableCell5";
			this.xrTableCell5.Weight = 1.8745559627458075D;
			// 
			// xrTableCell9
			// 
			this.xrTableCell9.CanGrow = false;
			this.xrTableCell9.CanShrink = true;
			this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Items.Title")});
			this.xrTableCell9.Font = new System.Drawing.Font("Trebuchet MS", 11F);
			this.xrTableCell9.Name = "xrTableCell9";
			this.xrTableCell9.StylePriority.UseFont = false;
			this.xrTableCell9.StylePriority.UseTextAlignment = false;
			this.xrTableCell9.Text = "xrTableCell9";
			this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrTableCell9.Weight = 7.3353841952404579D;
			// 
			// xrTableCell10
			// 
			this.xrTableCell10.Font = new System.Drawing.Font("Trebuchet MS", 11F);
			this.xrTableCell10.Name = "xrTableCell10";
			this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0, 100F);
			this.xrTableCell10.StylePriority.UseFont = false;
			this.xrTableCell10.StylePriority.UsePadding = false;
			this.xrTableCell10.StylePriority.UseTextAlignment = false;
			this.xrTableCell10.Text = "[TotalPrice!#,#] €";
			this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			this.xrTableCell10.Weight = 3.8960435978455088D;
			// 
			// GroupHeader1
			// 
			this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
			this.GroupHeader1.HeightF = 25F;
			this.GroupHeader1.Level = 1;
			this.GroupHeader1.Name = "GroupHeader1";
			// 
			// xrTable3
			// 
			this.xrTable3.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrTable3.Name = "xrTable3";
			this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
			this.xrTable3.SizeF = new System.Drawing.SizeF(720.9999F, 25F);
			this.xrTable3.StylePriority.UseFont = false;
			// 
			// xrTableRow3
			// 
			this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell15,
            this.xrTableCell16});
			this.xrTableRow3.Name = "xrTableRow3";
			this.xrTableRow3.Weight = 11.5D;
			// 
			// xrTableCell11
			// 
			this.xrTableCell11.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.xrTableCell11.Name = "xrTableCell11";
			this.xrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0, 100F);
			this.xrTableCell11.StylePriority.UseFont = false;
			this.xrTableCell11.StylePriority.UsePadding = false;
			this.xrTableCell11.Text = "Amount";
			this.xrTableCell11.Weight = 1.9038464993232898D;
			// 
			// xrTableCell15
			// 
			this.xrTableCell15.CanGrow = false;
			this.xrTableCell15.CanShrink = true;
			this.xrTableCell15.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.xrTableCell15.Name = "xrTableCell15";
			this.xrTableCell15.StylePriority.UseFont = false;
			this.xrTableCell15.Text = "Title";
			this.xrTableCell15.Weight = 7.4500011557194874D;
			// 
			// xrTableCell16
			// 
			this.xrTableCell16.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.xrTableCell16.Name = "xrTableCell16";
			this.xrTableCell16.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0, 100F);
			this.xrTableCell16.StylePriority.UseFont = false;
			this.xrTableCell16.StylePriority.UsePadding = false;
			this.xrTableCell16.StylePriority.UseTextAlignment = false;
			this.xrTableCell16.Text = "Total Price";
			this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			this.xrTableCell16.Weight = 3.9569196061232503D;
			// 
			// GroupHeader2
			// 
			this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9,
            this.xrLine1,
            this.xrLabel1});
			this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("MaterialType", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
			this.GroupHeader2.HeightF = 25.08332F;
			this.GroupHeader2.Name = "GroupHeader2";
			// 
			// xrLabel9
			// 
			this.xrLabel9.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(62.71445F, 0F);
			this.xrLabel9.Name = "xrLabel9";
			this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel9.SizeF = new System.Drawing.SizeF(130F, 23F);
			this.xrLabel9.StylePriority.UseFont = false;
			this.xrLabel9.StylePriority.UseTextAlignment = false;
			this.xrLabel9.Text = "[MaterialType]";
			this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrLine1
			// 
			this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 22.99995F);
			this.xrLine1.Name = "xrLine1";
			this.xrLine1.SizeF = new System.Drawing.SizeF(720.9998F, 2.083334F);
			// 
			// xrLabel1
			// 
			this.xrLabel1.Font = new System.Drawing.Font("Trebuchet MS", 10.2F);
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new System.Drawing.SizeF(62.71445F, 23F);
			this.xrLabel1.StylePriority.UseFont = false;
			this.xrLabel1.StylePriority.UseTextAlignment = false;
			this.xrLabel1.Text = "Material:";
			this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// GroupFooter1
			// 
			this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine2,
            this.xrLabel6});
			this.GroupFooter1.HeightF = 35.41667F;
			this.GroupFooter1.Name = "GroupFooter1";
			// 
			// xrLine2
			// 
			this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrLine2.Name = "xrLine2";
			this.xrLine2.SizeF = new System.Drawing.SizeF(720.9996F, 2.083333F);
			// 
			// xrLabel6
			// 
			this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Items.TotalPrice")});
			this.xrLabel6.Font = new System.Drawing.Font("Trebuchet MS", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(444.958F, 12.41665F);
			this.xrLabel6.Name = "xrLabel6";
			this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel6.SizeF = new System.Drawing.SizeF(276.0418F, 23F);
			this.xrLabel6.StylePriority.UseFont = false;
			this.xrLabel6.StylePriority.UseTextAlignment = false;
			xrSummary1.FormatString = "Total: {0:#,#} €";
			xrSummary1.IgnoreNullValues = true;
			xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
			this.xrLabel6.Summary = xrSummary1;
			this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// ReportFooter
			// 
			this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrLabel3,
            this.xrLine5});
			this.ReportFooter.HeightF = 95.83334F;
			this.ReportFooter.Name = "ReportFooter";
			// 
			// xrLine5
			// 
			this.xrLine5.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
			this.xrLine5.LineWidth = 3;
			this.xrLine5.LocationFloat = new DevExpress.Utils.PointFloat(376.2085F, 0F);
			this.xrLine5.Name = "xrLine5";
			this.xrLine5.SizeF = new System.Drawing.SizeF(344.7916F, 3F);
			// 
			// objectDataSource1
			// 
			this.objectDataSource1.DataSource = typeof(StockRequestApprovalWeb.Models.DevExpressReportModel);
			this.objectDataSource1.Name = "objectDataSource1";
			// 
			// xrControlStyle1
			// 
			this.xrControlStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.xrControlStyle1.Name = "xrControlStyle1";
			this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			// 
			// StockReport
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailReport});
			this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
			this.DataSource = this.objectDataSource1;
			this.Margins = new System.Drawing.Printing.Margins(71, 58, 96, 100);
			this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1});
			this.Version = "17.2";
			((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

	}

	#endregion
}

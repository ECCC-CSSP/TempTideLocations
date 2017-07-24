using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        HtmlDocument doc = null;
        int CurrentZone = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CurrentZone += 1;
            button5.Enabled = false;
            textBox2.Text = "";
            richTextBox1.AppendText("Doing ... " + CurrentZone + "\r\n");
            webBrowser1.Navigate(textBox1.Text + CurrentZone.ToString());
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            doc = webBrowser1.Document;
            button5.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(@"c:\WindowsFormsApplication1\WindowsFormsApplication1\GeoCode.html");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            using (CSSPWebToolsDBEntities db = new CSSPWebToolsDBEntities())
            {
                List<TideLocation> tideLocationList = (from c in db.TideLocations
                                                       where c.Lat == 0
                                                       select c).ToList();

                foreach (TideLocation tideLocation in tideLocationList)
                {
                    sb.AppendLine(@"addressArr.push(""" + tideLocation.Name + "," + tideLocation.Prov + @""");");
                }
                richTextBox1.Text = sb.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FileInfo fi = new FileInfo(@"C:\Users\leblancc\Desktop\TideLocationLatLng.txt");

            StreamReader sr = fi.OpenText();
            string content = sr.ReadToEnd();
            sr.Close();

            string[] strArr = new string[1];
            strArr[0] = "|||";
            List<string> elemList = content.Split(strArr, StringSplitOptions.None).ToList();
            using (CSSPWebToolsDBEntities db = new CSSPWebToolsDBEntities())
            {
                foreach (string sv in elemList)
                {
                    List<string> itemsList = sv.Split(",".ToCharArray(), StringSplitOptions.None).ToList();
                    if (itemsList.Count != 4)
                    {
                        richTextBox1.AppendText("itemsList not equal to 3\r\n");
                        continue;
                    }
                    string name = itemsList[0];
                    string prov = itemsList[1];
                    float lat = float.Parse(itemsList[2]);
                    float lng = float.Parse(itemsList[3]);

                    richTextBox1.AppendText("doing ... " + name + "\r\n");

                    TideLocation tideLocation = (from c in db.TideLocations
                                                 where c.Name == name
                                                 && c.Prov == prov
                                                 select c).FirstOrDefault();

                    if (tideLocation != null)
                    {
                        tideLocation.Lat = lat;
                        tideLocation.Lng = lng;
                    }
                    else
                    {
                        richTextBox1.AppendText("Could not find " + name + "\r\n");
                        continue;
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        richTextBox1.AppendText("could not save all new lat and lng");
                        return;
                    }

                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0)
            {
                return;
            }

            HtmlElement heMapZone = doc.GetElementById("map-zone");
            if (heMapZone == null)
                return;

            for (int i = 0, count = heMapZone.Children.Count; i < count; i++)
            {
                string name = heMapZone.Children[i].GetAttribute("alt");
                string href = heMapZone.Children[i].GetAttribute("href");
                int sid = int.Parse(href.Substring(href.IndexOf("=") + 1));

                TideLocation tideLocation = new TideLocation()
                {
                    Name = name,
                    sid = sid,

                };

                using (CSSPWebToolsDBEntities db = new CSSPWebToolsDBEntities())
                {
                    TideLocation tideLocationExist = (from c in db.TideLocations
                                                      where c.Name == name
                                                      && c.sid == sid
                                                      select c).FirstOrDefault();

                    if (tideLocationExist == null)
                    {
                        db.TideLocations.Add(tideLocation);

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            richTextBox1.AppendText("Error: [" + ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : "") + "\r\n");
                            return;
                        }
                    }
                    else
                    {
                        tideLocationExist.Zone = CurrentZone;
                        tideLocationExist.Prov = textBox2.Text;
                        db.SaveChanges();
                    }
                }
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<string> Prov2Letter = new List<string>()
            {
                "BC", "NB", "NL", "NS", "PE", "QC"
            };
            List<string> ProvFull = new List<string>()
            {
                "British Columbia", "New Brunswick", "Newfoundland and Labrador", "Nova Scotia", "Prince Edward Island", "Québec"
            };

            StringBuilder sb = new StringBuilder();
            StringBuilder sbRTB = new StringBuilder();

            sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.AppendLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2"" xmlns:gx=""http://www.google.com/kml/ext/2.2"" xmlns:kml=""http://www.opengis.net/kml/2.2"" xmlns:atom=""http://www.w3.org/2005/Atom"">");
            sb.AppendLine(@"  <Document>");
            sb.AppendLine(@"    <name>Subsector and Tide Location</name>");
            sb.AppendLine(@"	<Style id=""s_ylw-pushpin"">");
            sb.AppendLine(@"		<IconStyle>");
            sb.AppendLine(@"			<scale>1.1</scale>");
            sb.AppendLine(@"			<Icon>");
            sb.AppendLine(@"				<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>");
            sb.AppendLine(@"			</Icon>");
            sb.AppendLine(@"			<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>");
            sb.AppendLine(@"		</IconStyle>");
            sb.AppendLine(@"		<LineStyle>");
            sb.AppendLine(@"			<color>ffff00ff</color>");
            sb.AppendLine(@"		</LineStyle>");
            sb.AppendLine(@"		<PolyStyle>");
            sb.AppendLine(@"			<fill>0</fill>");
            sb.AppendLine(@"		</PolyStyle>");
            sb.AppendLine(@"	</Style>");
            sb.AppendLine(@"	<StyleMap id=""msn_grn-pushpin"">");
            sb.AppendLine(@"		<Pair>");
            sb.AppendLine(@"			<key>normal</key>");
            sb.AppendLine(@"			<styleUrl>#sn_grn-pushpin</styleUrl>");
            sb.AppendLine(@"		</Pair>");
            sb.AppendLine(@"		<Pair>");
            sb.AppendLine(@"			<key>highlight</key>");
            sb.AppendLine(@"			<styleUrl>#sh_grn-pushpin</styleUrl>");
            sb.AppendLine(@"		</Pair>");
            sb.AppendLine(@"	</StyleMap>");
            sb.AppendLine(@"	<Style id=""sh_grn-pushpin"">");
            sb.AppendLine(@"		<IconStyle>");
            sb.AppendLine(@"			<scale>1.3</scale>");
            sb.AppendLine(@"			<Icon>");
            sb.AppendLine(@"				<href>http://maps.google.com/mapfiles/kml/pushpin/grn-pushpin.png</href>");
            sb.AppendLine(@"			</Icon>");
            sb.AppendLine(@"			<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>");
            sb.AppendLine(@"		</IconStyle>");
            sb.AppendLine(@"		<ListStyle>");
            sb.AppendLine(@"		</ListStyle>");
            sb.AppendLine(@"		<LineStyle>");
            sb.AppendLine(@"			<color>ffff00ff</color>");
            sb.AppendLine(@"		</LineStyle>");
            sb.AppendLine(@"		<PolyStyle>");
            sb.AppendLine(@"			<fill>0</fill>");
            sb.AppendLine(@"		</PolyStyle>");
            sb.AppendLine(@"	</Style>");
            sb.AppendLine(@"	<StyleMap id=""m_ylw-pushpin"">");
            sb.AppendLine(@"		<Pair>");
            sb.AppendLine(@"			<key>normal</key>");
            sb.AppendLine(@"			<styleUrl>#s_ylw-pushpin</styleUrl>");
            sb.AppendLine(@"		</Pair>");
            sb.AppendLine(@"		<Pair>");
            sb.AppendLine(@"			<key>highlight</key>");
            sb.AppendLine(@"			<styleUrl>#s_ylw-pushpin_hl</styleUrl>");
            sb.AppendLine(@"		</Pair>");
            sb.AppendLine(@"	</StyleMap>");
            sb.AppendLine(@"	<Style id=""sn_grn-pushpin"">");
            sb.AppendLine(@"		<IconStyle>");
            sb.AppendLine(@"			<scale>1.1</scale>");
            sb.AppendLine(@"			<Icon>");
            sb.AppendLine(@"				<href>http://maps.google.com/mapfiles/kml/pushpin/grn-pushpin.png</href>");
            sb.AppendLine(@"			</Icon>");
            sb.AppendLine(@"			<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>");
            sb.AppendLine(@"		</IconStyle>");
            sb.AppendLine(@"		<ListStyle>");
            sb.AppendLine(@"		</ListStyle>");
            sb.AppendLine(@"		<LineStyle>");
            sb.AppendLine(@"			<color>ffff00ff</color>");
            sb.AppendLine(@"		</LineStyle>");
            sb.AppendLine(@"		<PolyStyle>");
            sb.AppendLine(@"			<fill>0</fill>");
            sb.AppendLine(@"		</PolyStyle>");
            sb.AppendLine(@"	</Style>");
            sb.AppendLine(@"	<Style id=""s_ylw-pushpin_hl"">");
            sb.AppendLine(@"		<IconStyle>");
            sb.AppendLine(@"			<scale>1.3</scale>");
            sb.AppendLine(@"			<Icon>");
            sb.AppendLine(@"				<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>");
            sb.AppendLine(@"			</Icon>");
            sb.AppendLine(@"			<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>");
            sb.AppendLine(@"		</IconStyle>");
            sb.AppendLine(@"		<LineStyle>");
            sb.AppendLine(@"			<color>ffff00ff</color>");
            sb.AppendLine(@"		</LineStyle>");
            sb.AppendLine(@"		<PolyStyle>");
            sb.AppendLine(@"			<fill>0</fill>");
            sb.AppendLine(@"		</PolyStyle>");
            sb.AppendLine(@"	</Style>");


            using (CSSPWebToolsDBEntities db = new CSSPWebToolsDBEntities())
            {
                for (int i = 0, count = Prov2Letter.Count; i < count; i++)
                {
                    string Full = ProvFull[i];
                    TVItem tvItemProv = (from c in db.TVItems
                                         from cl in db.TVItemLanguages
                                         where c.TVItemID == cl.TVItemID
                                         && cl.TVText == Full
                                         select c).FirstOrDefault();

                    if (tvItemProv == null)
                    {
                        richTextBox1.AppendText("could not find " + ProvFull[i]);
                        return;
                    }

                    var tvItemList = (from c in db.TVItems
                                      from cl in db.TVItemLanguages
                                      where c.TVItemID == cl.TVItemID
                                      && c.TVPath.StartsWith(tvItemProv.TVPath + "p")
                                      && c.TVType == 20
                                      && cl.Language == "en"
                                      orderby cl.TVText
                                      select new { c, cl }).ToList();


                    sb.AppendLine(@"    <Folder>");
                    sb.AppendLine(@"        <name>" + Prov2Letter[i] + "</name>");


                    foreach (var tvItem in tvItemList)
                    {
                        button6.Text = tvItem.cl.TVText;
                        button6.Refresh();
                        Application.DoEvents();
                        List<MapInfoPoint> mapInfoPointList = (from c in db.MapInfos
                                                               from p in db.MapInfoPoints
                                                               where c.MapInfoID == p.MapInfoID
                                                               && c.TVItemID == tvItem.c.TVItemID
                                                               && c.TVType == 20
                                                               && c.MapInfoDrawType == 3 // polygon
                                                               select p).ToList();

                        if (mapInfoPointList.Count == 0)
                        {
                            continue;
                        }

                        sb.AppendLine(@"    <Folder>");
                        sb.AppendLine(@"        <name>" + tvItem.cl.TVText + "</name>");


                        sb.AppendLine(@"            <Placemark>");
                        sb.AppendLine(@"                <name>" + tvItem.cl.TVText + "</name>");
                        sb.AppendLine(@"                <styleUrl>#m_ylw-pushpin</styleUrl>");
                        sb.AppendLine(@"                <Polygon>");
                        sb.AppendLine(@"                    <outerBoundaryIs>");
                        sb.AppendLine(@"                        <LinearRing>");
                        sb.AppendLine(@"                            <coordinates>");
                        foreach (MapInfoPoint mapInfoPoint in mapInfoPointList) // should only be 1
                        {
                            sb.Append(mapInfoPoint.Lng + "," + mapInfoPoint.Lat + ",0 ");
                        }
                        sb.AppendLine(@"                            </coordinates>");
                        sb.AppendLine(@"                        </LinearRing>");
                        sb.AppendLine(@"                    </outerBoundaryIs>");
                        sb.AppendLine(@"                </Polygon>");
                        sb.AppendLine(@"            </Placemark>");

                        mapInfoPointList = (from c in db.MapInfos
                                            from p in db.MapInfoPoints
                                            where c.MapInfoID == p.MapInfoID
                                            && c.TVItemID == tvItem.c.TVItemID
                                            && c.TVType == 20
                                            && c.MapInfoDrawType == 1 // point, 3 polygon
                                            select p).ToList();

                        if (mapInfoPointList.Count == 0)
                        {
                            continue;
                        }

                        sb.AppendLine(@"            <Placemark>");
                        sb.AppendLine(@"                <name>" + tvItem.cl.TVText + "</name>");
                        sb.AppendLine(@"                <styleUrl>#m_ylw-pushpin</styleUrl>");
                        sb.AppendLine(@"                <Point>");
                        foreach (MapInfoPoint mapInfoPoint in mapInfoPointList) // should only be 1
                        {
                            sb.AppendLine(@"                    <coordinates>" + mapInfoPoint.Lng + "," + mapInfoPoint.Lat + ",0 </coordinates>");
                        }
                        sb.AppendLine(@"                </Point>");
                        sb.AppendLine(@"            </Placemark>");

                        double factor = 0.01D;
                        List<TideLocation> tideLocationList = new List<TideLocation>();
                        while (tideLocationList.Count < 3)
                        {
                            double MinLat = mapInfoPointList[0].Lat - factor;
                            double MaxLat = mapInfoPointList[0].Lat + factor;
                            double MinLng = mapInfoPointList[0].Lng - factor;
                            double MaxLng = mapInfoPointList[0].Lng + factor;
                            string prov = Prov2Letter[i];
                            List<TideLocation> tideLocationTempList = (from c in db.TideLocations
                                                                       where c.Lat != 0
                                                                       && c.Lat > MinLat
                                                                       && c.Lat < MaxLat
                                                                       && c.Lng > MinLng
                                                                       && c.Lng < MaxLng
                                                                       select c).ToList();

                            foreach (TideLocation tideLocation in tideLocationTempList)
                            {
                                if (!tideLocationList.Contains(tideLocation))
                                {
                                    tideLocationList.Add(tideLocation);
                                }
                            }
                            factor += 0.01D;
                        }

                        sbRTB.Append("" + tvItem.c.TVItemID + "\t" + tvItem.cl.TVText + "\t");

                        foreach (TideLocation tideLocation in tideLocationList)
                        {
                            sbRTB.Append("" + tideLocation.sid + "," + tideLocation.Name + "\t");

                            sb.AppendLine(@"        <Placemark>");
                            sb.AppendLine(@"            <name>" + tideLocation.sid + "," + tideLocation.Name + "</name>");
                            sb.AppendLine(@"            <styleUrl>#msn_grn-pushpin</styleUrl>");
                            sb.AppendLine(@"            <Point>");
                            sb.AppendLine(@"                <coordinates>" + tideLocation.Lng + "," + tideLocation.Lat + ",0 </coordinates>");
                            sb.AppendLine(@"            </Point>");
                            sb.AppendLine(@"        </Placemark>");
                        }
                        sbRTB.AppendLine("");



                        sb.AppendLine(@"    </Folder>");

                    }


                    sb.AppendLine(@"    </Folder>");

                }
                db.SaveChanges();
            }

            sb.AppendLine(@"  </Document>");
            sb.AppendLine(@"</kml>");

            FileInfo fi = new FileInfo(@"c:\WindowsFormsApplication1\WindowsFormsApplication1\SubsectorAndTideLocation.kml");

            StreamWriter sw = fi.CreateText();

            sw.Write(sb.ToString());

            sw.Close();

            richTextBox1.Text = sbRTB.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            List<string> Prov2Letter = new List<string>()
            {
                "BC", "NB", "NL", "NS", "PE", "QC"
            };
            List<string> ProvFull = new List<string>()
            {
                "British Columbia", "New Brunswick", "Newfoundland and Labrador", "Nova Scotia", "Prince Edward Island", "Québec"
            };

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.AppendLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2"" xmlns:gx=""http://www.google.com/kml/ext/2.2"" xmlns:kml=""http://www.opengis.net/kml/2.2"" xmlns:atom=""http://www.w3.org/2005/Atom"">");
            sb.AppendLine(@"  <Document>");
            sb.AppendLine(@"    <name>Tide Location</name>");
            sb.AppendLine(@"	<Style id=""s_ylw-pushpin"">");
            sb.AppendLine(@"		<IconStyle>");
            sb.AppendLine(@"			<scale>1.1</scale>");
            sb.AppendLine(@"			<Icon>");
            sb.AppendLine(@"				<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>");
            sb.AppendLine(@"			</Icon>");
            sb.AppendLine(@"			<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>");
            sb.AppendLine(@"		</IconStyle>");
            sb.AppendLine(@"		<LineStyle>");
            sb.AppendLine(@"			<color>ffff00ff</color>");
            sb.AppendLine(@"		</LineStyle>");
            sb.AppendLine(@"		<PolyStyle>");
            sb.AppendLine(@"			<fill>0</fill>");
            sb.AppendLine(@"		</PolyStyle>");
            sb.AppendLine(@"	</Style>");
            sb.AppendLine(@"	<StyleMap id=""msn_grn-pushpin"">");
            sb.AppendLine(@"		<Pair>");
            sb.AppendLine(@"			<key>normal</key>");
            sb.AppendLine(@"			<styleUrl>#sn_grn-pushpin</styleUrl>");
            sb.AppendLine(@"		</Pair>");
            sb.AppendLine(@"		<Pair>");
            sb.AppendLine(@"			<key>highlight</key>");
            sb.AppendLine(@"			<styleUrl>#sh_grn-pushpin</styleUrl>");
            sb.AppendLine(@"		</Pair>");
            sb.AppendLine(@"	</StyleMap>");
            sb.AppendLine(@"	<Style id=""sh_grn-pushpin"">");
            sb.AppendLine(@"		<IconStyle>");
            sb.AppendLine(@"			<scale>1.3</scale>");
            sb.AppendLine(@"			<Icon>");
            sb.AppendLine(@"				<href>http://maps.google.com/mapfiles/kml/pushpin/grn-pushpin.png</href>");
            sb.AppendLine(@"			</Icon>");
            sb.AppendLine(@"			<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>");
            sb.AppendLine(@"		</IconStyle>");
            sb.AppendLine(@"		<ListStyle>");
            sb.AppendLine(@"		</ListStyle>");
            sb.AppendLine(@"		<LineStyle>");
            sb.AppendLine(@"			<color>ffff00ff</color>");
            sb.AppendLine(@"		</LineStyle>");
            sb.AppendLine(@"		<PolyStyle>");
            sb.AppendLine(@"			<fill>0</fill>");
            sb.AppendLine(@"		</PolyStyle>");
            sb.AppendLine(@"	</Style>");
            sb.AppendLine(@"	<StyleMap id=""m_ylw-pushpin"">");
            sb.AppendLine(@"		<Pair>");
            sb.AppendLine(@"			<key>normal</key>");
            sb.AppendLine(@"			<styleUrl>#s_ylw-pushpin</styleUrl>");
            sb.AppendLine(@"		</Pair>");
            sb.AppendLine(@"		<Pair>");
            sb.AppendLine(@"			<key>highlight</key>");
            sb.AppendLine(@"			<styleUrl>#s_ylw-pushpin_hl</styleUrl>");
            sb.AppendLine(@"		</Pair>");
            sb.AppendLine(@"	</StyleMap>");
            sb.AppendLine(@"	<Style id=""sn_grn-pushpin"">");
            sb.AppendLine(@"		<IconStyle>");
            sb.AppendLine(@"			<scale>1.1</scale>");
            sb.AppendLine(@"			<Icon>");
            sb.AppendLine(@"				<href>http://maps.google.com/mapfiles/kml/pushpin/grn-pushpin.png</href>");
            sb.AppendLine(@"			</Icon>");
            sb.AppendLine(@"			<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>");
            sb.AppendLine(@"		</IconStyle>");
            sb.AppendLine(@"		<ListStyle>");
            sb.AppendLine(@"		</ListStyle>");
            sb.AppendLine(@"		<LineStyle>");
            sb.AppendLine(@"			<color>ffff00ff</color>");
            sb.AppendLine(@"		</LineStyle>");
            sb.AppendLine(@"		<PolyStyle>");
            sb.AppendLine(@"			<fill>0</fill>");
            sb.AppendLine(@"		</PolyStyle>");
            sb.AppendLine(@"	</Style>");
            sb.AppendLine(@"	<Style id=""s_ylw-pushpin_hl"">");
            sb.AppendLine(@"		<IconStyle>");
            sb.AppendLine(@"			<scale>1.3</scale>");
            sb.AppendLine(@"			<Icon>");
            sb.AppendLine(@"				<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>");
            sb.AppendLine(@"			</Icon>");
            sb.AppendLine(@"			<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>");
            sb.AppendLine(@"		</IconStyle>");
            sb.AppendLine(@"		<LineStyle>");
            sb.AppendLine(@"			<color>ffff00ff</color>");
            sb.AppendLine(@"		</LineStyle>");
            sb.AppendLine(@"		<PolyStyle>");
            sb.AppendLine(@"			<fill>0</fill>");
            sb.AppendLine(@"		</PolyStyle>");
            sb.AppendLine(@"	</Style>");


            using (CSSPWebToolsDBEntities db = new CSSPWebToolsDBEntities())
            {
                for (int i = 0, count = Prov2Letter.Count; i < count; i++)
                {
                    string Full = ProvFull[i];
                    TVItem tvItemProv = (from c in db.TVItems
                                         from cl in db.TVItemLanguages
                                         where c.TVItemID == cl.TVItemID
                                         && cl.TVText == Full
                                         select c).FirstOrDefault();

                    if (tvItemProv == null)
                    {
                        richTextBox1.AppendText("could not find " + ProvFull[i]);
                        return;
                    }

                    sb.AppendLine(@"    <Folder>");
                    sb.AppendLine(@"        <name>" + Prov2Letter[i] + "</name>");

                    string prov = Prov2Letter[i];
                    List<TideLocation> tideLocationList = (from c in db.TideLocations
                                                           where c.Prov == prov
                                                           && c.Lat != 0
                                                           select c).ToList();

                    foreach (TideLocation tideLocation in tideLocationList)
                    {
                        button7.Text = tideLocation.sid.ToString();
                        button7.Refresh();
                        Application.DoEvents();

                        sb.AppendLine(@"        <Placemark>");
                        sb.AppendLine(@"            <name>" + tideLocation.sid + "," + tideLocation.Name + "</name>");
                        sb.AppendLine(@"            <styleUrl>#msn_grn-pushpin</styleUrl>");
                        sb.AppendLine(@"            <Point>");
                        sb.AppendLine(@"                <coordinates>" + tideLocation.Lng + "," + tideLocation.Lat + ",0 </coordinates>");
                        sb.AppendLine(@"            </Point>");
                        sb.AppendLine(@"        </Placemark>");
                    }

                    sb.AppendLine(@"    </Folder>");

                }
                db.SaveChanges();
            }

            sb.AppendLine(@"  </Document>");
            sb.AppendLine(@"</kml>");

            FileInfo fi = new FileInfo(@"c:\WindowsFormsApplication1\WindowsFormsApplication1\TideLocation.kml");

            StreamWriter sw = fi.CreateText();

            sw.Write(sb.ToString());

            sw.Close();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            FileInfo fi = new FileInfo(@"C:\CSSP Latest Code\TempTideLocations\TempTideLocation\Subsector Tide Options by priorities.xls");

            string connectionString = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + fi.FullName + ";Extended Properties=Excel 8.0;";
            Application.DoEvents();

            OleDbCommand comm = new OleDbCommand("Select * from [Sheet1$];");
            OleDbConnection conn = new OleDbConnection(connectionString);

            conn.Open();
            OleDbDataReader reader;
            comm.Connection = conn;
            reader = comm.ExecuteReader();

            List<string> FieldNameList = new List<string>();
            FieldNameList = new List<string>() { "SSID", "SSName", "First", "Second", "Third", "Fourth", "Fifth" };
            for (int i = 0; i < reader.FieldCount; i++)
            {
                //richTextBox1.AppendText(reader.GetName(i) + " " + reader.GetValue(i).GetType() + "\r\n");
                if (reader.GetName(i) != FieldNameList[i])
                {
                    richTextBox1.AppendText(reader.GetName(i) + " is not equal to " + FieldNameList[i] + "\r\n");
                    return;
                }
            }
            reader.Close();
            richTextBox1.AppendText("\r\n");

            reader = comm.ExecuteReader();

            while (reader.Read())
            {
                int ID = 0;
                string SSID = "";
                string SSName = "";
                string First = "";
                string Second = "";
                string Third = "";
                string Fourth = "";
                string Fifth = "";

                // doing SSID
                ID = 0;
                if (reader.GetValue(ID).GetType() == typeof(DBNull) || string.IsNullOrEmpty(reader.GetValue(ID).ToString().Trim()))
                {
                    // nothing
                }
                else
                {
                    SSID = reader.GetValue(ID).ToString();
                }

                // doing SSName
                ID = 1;
                if (reader.GetValue(ID).GetType() == typeof(DBNull) || string.IsNullOrEmpty(reader.GetValue(ID).ToString().Trim()))
                {
                    // nothing
                }
                else
                {
                    SSName = reader.GetValue(ID).ToString();
                }

                // doing First
                ID = 2;
                if (reader.GetValue(ID).GetType() == typeof(DBNull) || string.IsNullOrEmpty(reader.GetValue(ID).ToString().Trim()))
                {
                    // nothing
                }
                else
                {
                    First = reader.GetValue(ID).ToString();
                }

                // doing Second
                ID = 3;
                if (reader.GetValue(ID).GetType() == typeof(DBNull) || string.IsNullOrEmpty(reader.GetValue(ID).ToString().Trim()))
                {
                    // nothing
                }
                else
                {
                    Second = reader.GetValue(ID).ToString();
                }

                // doing Third
                ID = 4;
                if (reader.GetValue(ID).GetType() == typeof(DBNull) || string.IsNullOrEmpty(reader.GetValue(ID).ToString().Trim()))
                {
                    // nothing
                }
                else
                {
                    Third = reader.GetValue(ID).ToString();
                }

                // doing Fourth
                ID = 5;
                if (reader.GetValue(ID).GetType() == typeof(DBNull) || string.IsNullOrEmpty(reader.GetValue(ID).ToString().Trim()))
                {
                    // nothing
                }
                else
                {
                    Fourth = reader.GetValue(ID).ToString();
                }

                // doing Fifth
                ID = 6;
                if (reader.GetValue(ID).GetType() == typeof(DBNull) || string.IsNullOrEmpty(reader.GetValue(ID).ToString().Trim()))
                {
                    // nothing
                }
                else
                {
                    Fifth = reader.GetValue(ID).ToString();
                }

                //if (SSID == "561")
                //{
                    using (CSSPWebToolsDBEntities db = new CSSPWebToolsDBEntities())
                    {
                        string AllSID = "";
                        if (First.Length > 0)
                        {
                            AllSID += First.Substring(0, First.IndexOf(","));
                        }
                        if (Second.Length > 0)
                        {
                            AllSID += "," + Second.Substring(0, Second.IndexOf(","));
                        }
                        if (Third.Length > 0)
                        {
                            AllSID += "," + Third.Substring(0, Third.IndexOf(","));
                        }
                        if (Fourth.Length > 0)
                        {
                            AllSID += "," + Fourth.Substring(0, Fourth.IndexOf(","));
                        }
                        if (Fifth.Length > 0)
                        {
                            AllSID += "," + Fifth.Substring(0, Fifth.IndexOf(","));
                        }
                        int MWQMSubsectorTVItemID = int.Parse(SSID);
                        MWQMSubsector mwqmSubsector = (from c in db.MWQMSubsectors
                                                       where c.MWQMSubsectorTVItemID == MWQMSubsectorTVItemID
                                                       select c).FirstOrDefault();
                        if (mwqmSubsector != null)
                        {
                            mwqmSubsector.TideLocationSIDText = AllSID;
                        }
                        else
                        {
                            richTextBox1.AppendText("ERROR --- " + SSID + "\t" + SSName + "\t" + First + "\t" + Second + "\t" + Third + "\t" + Fourth + "\t" + Fifth + "\r\n");
                        }

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            richTextBox1.AppendText("ERROR --- " + ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : ""));
                        }
                    }
                //}

                label1.Text = SSName;
                label1.Refresh();
                Application.DoEvents();

            }
        }
    }
}

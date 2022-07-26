using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using WpfApp1.Model;
using Brushes = System.Drawing.Brushes;
using Pen = System.Drawing.Pen;
using Point = WpfApp1.Model.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
using Size = System.Drawing.Size;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Promenjive za deljenje na podeoke
        int divCountX;
        int divCountY;
        Element[,] divs;
        
        //Boje
        public SolidColorBrush subColor = new SolidColorBrush(System.Windows.Media.Colors.Yellow);
        public SolidColorBrush nodeColor = new SolidColorBrush(System.Windows.Media.Colors.Cyan);
        public SolidColorBrush switchColor = new SolidColorBrush(System.Windows.Media.Colors.Magenta);
        public SolidColorBrush intersectionColor = new SolidColorBrush(System.Windows.Media.Colors.Red);

        public double noviX, noviY;

        //Liste za cuvanje
        List<Element> elements = new List<Element>();
        List<Ellipse> ellipses = new List<Ellipse>();
        private Dictionary<long, UIElement> entities = new Dictionary<long, UIElement>();
        int[,] lineMatrix = new int[400, 200];

        public MainWindow()
        {
            InitializeComponent();

            //Deljenje na podeoke i smestanje u matricu
            divCountX = (int)Math.Floor(myCanvas.Width) + 1;
            divCountY = (int)Math.Floor(myCanvas.Height) + 1;

            divs = new Element[divCountX, divCountY];

            for (int i = 0; i <= myCanvas.Width; i++)
            {
                for (int j = 0; j <= myCanvas.Height; j++)
                {
                    Element el = new Element(i, j, 0);
                    divs[i, j] = el;
                }
            }
        }


        private void LoadButton_Click(object sender, RoutedEventArgs e)
		{
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Geographic.xml");

            XmlNodeList nodeList;

            double minX = 0;
            double minY = 0;
            double maxX = 0;
            double maxY = 0;

            bool first = true;

            #region Stanice
            List<SubstationEntity> subList = new List<SubstationEntity>();

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");
            foreach (XmlNode node in nodeList)
            {
                //Parsovanje XML podataka
                SubstationEntity sub = new SubstationEntity();
                sub.Id = long.Parse(node["Id"].InnerText);
                sub.Name = node["Name"].InnerText;
                sub.X = double.Parse(node["X"].InnerText, CultureInfo.InvariantCulture);
                sub.Y = double.Parse(node["Y"].InnerText, CultureInfo.InvariantCulture);

                ToLatLon(sub.X, sub.Y, 34, out noviY, out noviX);

                if (Double.IsNaN(noviX) || Double.IsNaN(noviY))
                    continue;

                sub.X = noviX;
                sub.Y = noviY;

                subList.Add(sub);

                //Odredjujemo koji su najmanji
                if (first)
                {
                    minX = noviX;
                    minY = noviY;
                    maxX = noviX;
                    maxY = noviY;

                    first = false;
                }

                if (noviX > maxX)
                    maxX = noviX;
                if (noviX < minX)
                    minX = noviX;
                if (noviY > maxY)
                    maxY = noviY;
                if (noviY < minY)
                    minY = noviY;
            }
            #endregion

            #region Nodovi
            List<NodeEntity> nodeobjList = new List<NodeEntity>();

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            foreach (XmlNode node in nodeList)
            {
                //Parsovanje XML podataka
                NodeEntity nodeobj = new NodeEntity();
                nodeobj.Id = long.Parse(node["Id"].InnerText);
                nodeobj.Name = node["Name"].InnerText;
                nodeobj.X = double.Parse(node["X"].InnerText, CultureInfo.InvariantCulture);
                nodeobj.Y = double.Parse(node["Y"].InnerText, CultureInfo.InvariantCulture);

                ToLatLon(nodeobj.X, nodeobj.Y, 34, out noviY, out noviX);

                if (Double.IsNaN(noviX) || Double.IsNaN(noviY))
                    continue;

                nodeobj.X = noviX;
                nodeobj.Y = noviY;

                nodeobjList.Add(nodeobj);

                //Odredjujemo koji su najmanji
                if (noviX > maxX)
                    maxX = noviX;
                if (noviX < minX)
                    minX = noviX;
                if (noviY > maxY)
                    maxY = noviY;
                if (noviY < minY)
                    minY = noviY;
            }
            #endregion

            #region Switch-evi
            List<SwitchEntity> switchList = new List<SwitchEntity>();

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            foreach (XmlNode node in nodeList)
            {
                //Parsovanje XML podataka
                SwitchEntity switchobj = new SwitchEntity();
                switchobj.Id = long.Parse(node["Id"].InnerText);
                switchobj.Name = node["Name"].InnerText;
                switchobj.X = double.Parse(node["X"].InnerText, CultureInfo.InvariantCulture);
                switchobj.Y = double.Parse(node["Y"].InnerText, CultureInfo.InvariantCulture);
                switchobj.Status = node["Status"].InnerText;

                ToLatLon(switchobj.X, switchobj.Y, 34, out noviY, out noviX);

                if (Double.IsNaN(noviX) || Double.IsNaN(noviY))
                    continue;

                switchobj.X = noviX;
                switchobj.Y = noviY;

                switchList.Add(switchobj);

                //Odredjujemo koji su najmanji
                if (noviX > maxX)
                    maxX = noviX;
                if (noviX < minX)
                    minX = noviX;
                if (noviY > maxY)
                    maxY = noviY;
                if (noviY < minY)
                    minY = noviY;
            }
            #endregion

            double canvasHeight = maxY - minY;
            double canvasWidth = maxX - minX;

            #region Crtanje Stanica
            //Crtanje entiteta
            foreach (SubstationEntity se in subList)
            {
                Ellipse ellipse = new Ellipse
                {
                    Height = 1,
                    Width = 1,
                    Fill = subColor,
                    ToolTip = se.ToString()
                };

                //Pomocna promenjiva za stavljanje entiteta na kanvas
                int offsetX = (int)Math.Round((myCanvas.Width * (se.X - minX)) / canvasWidth);
                int offsetY = (int)Math.Round((myCanvas.Height * (se.Y - minY)) / canvasHeight);

                Element el;
                if (IsFree(offsetX, offsetY) == false)
                {
                    //Kretanje kroz matricu
                    int offset = 1;
                    while (true)
                    {
                        el = CheckAround(offsetX, offsetY, offset);
                        if (el != null)
                        {
                            offsetX = el.X;
                            offsetY = el.Y;
                            break;
                        }
                        offset++;
                    }
                    elements.Add(el);
                }
                else
                {
                    elements.Add(new Element(offsetX, offsetY, false, se.Id));
                }

                Canvas.SetLeft(ellipse, offsetX);
                Canvas.SetBottom(ellipse, offsetY);

                myCanvas.Children.Add(ellipse);
                entities.Add(se.Id, ellipse);

                ellipses.Add(ellipse);
            }
            #endregion

            #region Crtanje Nodova
            //Crtanje entiteta
            foreach (NodeEntity ne in nodeobjList)
            {
                Ellipse ellipse = new Ellipse
                {
                    Height = 1,
                    Width = 1,
                    Fill = nodeColor,
                    ToolTip = ne.ToString()
                };

                //Pomocna promenjiva za stavljanje entiteta na kanvas
                int offsetX = (int)Math.Round((myCanvas.Width * (ne.X - minX)) / canvasWidth);
                int offsetY = (int)Math.Round((myCanvas.Height * (ne.Y - minY)) / canvasHeight);

                Element el;
                if (IsFree(offsetX, offsetY) == false)
                {
                    //Kretanje kroz matricu
                    int offset = 1;
                    while (true)
                    {
                        el = CheckAround(offsetX, offsetY, offset);
                        if (el != null)
                        {
                            offsetX = el.X;
                            offsetY = el.Y;
                            break;
                        }
                        offset++;
                    }
                    elements.Add(el);
                }
                else
                {
                    elements.Add(new Element(offsetX, offsetY, ne.Id));
                }

                Canvas.SetLeft(ellipse, offsetX);
                Canvas.SetBottom(ellipse, offsetY);

                myCanvas.Children.Add(ellipse);
                entities.Add(ne.Id, ellipse);

                ellipses.Add(ellipse);
            }
            #endregion

            #region Crtanje Switch-eva
            //Crtanje entiteta
            foreach (SwitchEntity se in switchList)
            {
                Ellipse ellipse = new Ellipse
                {
                    Height = 1,
                    Width = 1,
                    Fill = switchColor,
                    ToolTip = se.ToString()
                };

                //Pomocna promenjiva za stavljanje entiteta na kanvas
                int offsetX = (int)Math.Round((myCanvas.Width * (se.X - minX)) / canvasWidth);
                int offsetY = (int)Math.Round((myCanvas.Height * (se.Y - minY)) / canvasHeight);

                Element el;
                if (IsFree(offsetX, offsetY) == false)
                {
                    //Kretanje kroz matricu
                    int offset = 1;
                    while (true)
                    {
                        el = CheckAround(offsetX, offsetY, offset);
                        if (el != null)
                        {
                            offsetX = el.X;
                            offsetY = el.Y;
                            break;
                        }
                        offset++;
                    }
                    elements.Add(el);
                }
                else
                {
                    elements.Add(new Element(offsetX, offsetY, se.Id));
                }

                Canvas.SetLeft(ellipse, offsetX);
                Canvas.SetBottom(ellipse, offsetY);

                myCanvas.Children.Add(ellipse);
                entities.Add(se.Id, ellipse);

                ellipses.Add(ellipse);
            }
            #endregion

            #region Linije i crtanje linija
            List<LineEntity> lineList = new List<LineEntity>();
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity");
            foreach (XmlNode node in nodeList)
            {
                //Parsovanje XML podataka
                LineEntity l = new LineEntity();
                l.Id = long.Parse(node["Id"].InnerText);
                l.Name = node["Name"].InnerText;
                if (node["IsUnderground"].InnerText.Equals("true"))
                {
                    l.IsUnderground = true;
                }
                else
                {
                    l.IsUnderground = false;
                }
                l.R = float.Parse(node["R"].InnerText, CultureInfo.InvariantCulture);
                l.ConductorMaterial = node["ConductorMaterial"].InnerText;
                l.LineType = node["LineType"].InnerText;
                l.ThermalConstantHeat = long.Parse(node["ThermalConstantHeat"].InnerText);
                l.FirstEnd = long.Parse(node["FirstEnd"].InnerText);
                l.SecondEnd = long.Parse(node["SecondEnd"].InnerText);

                PointCollection points = new PointCollection();

                if (entities.ContainsKey(l.FirstEnd) && entities.ContainsKey(l.SecondEnd))
                {
                    System.Windows.Point p1 = new System.Windows.Point(Canvas.GetLeft(entities[l.FirstEnd]) + 0.5, 200 - Canvas.GetBottom(entities[l.FirstEnd]) - 0.5);
                    System.Windows.Point p3 = new System.Windows.Point(Canvas.GetLeft(entities[l.SecondEnd]) + 0.5, 200 - Canvas.GetBottom(entities[l.SecondEnd]) - 0.5);

                    points.Add(p1);

                    if (p1.X != p3.X && p1.Y != p3.Y)
                    {
                        System.Windows.Point p2 = GetTurnPoint(p1, p3);

                        points.Add(p2);
                    }

                    points.Add(p3);

                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Colors.Green;

                    Polyline polyline = new Polyline
                    {
                        Points = points,
                        Stroke = brush,
                        StrokeThickness = 0.3,
                        ToolTip = $"Line\nID: {l.Id}\nName: {l.Name}",
                    };

                    myCanvas.Children.Add(polyline);
                    CountPath(points);
                    polyline.MouseRightButtonDown += Polyline_RightClick;

                }

                /*foreach (XmlNode pointNode in node.ChildNodes[9].ChildNodes) // 9 posto je Vertices 9. node u jednom line objektu
                {
                    Point p = new Point();

                    p.X = double.Parse(pointNode["X"].InnerText);
                    p.Y = double.Parse(pointNode["Y"].InnerText);

                    ToLatLon(p.X, p.Y, 34, out noviX, out noviY);

                }*/

                lineList.Add(l);
            }
            #endregion

            DrawIntersections();
        }

        //Funkcija za nalazenje pravog ugla
        public System.Windows.Point GetTurnPoint(System.Windows.Point p1, System.Windows.Point p2)
        {
            if (p1.X < p2.X)
            {
                return new System.Windows.Point(p2.X, p1.Y);
            }
            else
            {
                return new System.Windows.Point(p1.X, p2.Y);
            }
        }
       
        //Funkcija za prebrojavanje matrice da bi videli gde su preseci
        private void CountPath(PointCollection points)
        {
            for(int i = 0; i < points.Count - 1; i++)
            {
                if(points[i].Y == points[i + 1].Y)
                {
                   if(points[i].X < points[i + 1].X)
                    {
                        for (int j = (int)points[i].X; j <= (int)points[i + 1].X; j++)
                        {
                            lineMatrix[j, (int)points[i].Y] += 1;
                        }
                    }
                    else
                    {
                        for (int j = (int)points[i].X; j >= (int)points[i + 1].X; j--)
                        {
                            lineMatrix[j, (int)points[i].Y] += 1;
                        }
                    }
                }
                else
                {
                    if (points[i].Y < points[i + 1].Y)
                    {
                        for (int j = (int)points[i].Y; j <= (int)points[i + 1].Y; j++)
                        {
                            lineMatrix[(int)points[i].X, j] += 1;
                        }
                    }
                    else
                    {
                        for (int j = (int)points[i].Y; j >= (int)points[i + 1].Y; j--)
                        {
                            lineMatrix[(int)points[i].X, j] += 1;
                        }
                    }
                }
            }
        }

        //Funkcija za crtanje preseka izmedju linija
        private void DrawIntersections()
        {
            for(int i = 0; i < 400; i++)
            {
                for(int j = 0; j < 200; j++)
                {
                    if (lineMatrix[i, j] > 1)
                    {
                        if (i > 0 && i < 399)
                        {
                            if(j > 0 && j < 199)
                            {
                                if(lineMatrix[i,j] > lineMatrix[i,j+1] && lineMatrix[i, j] > lineMatrix[i, j - 1] && lineMatrix[i, j] > lineMatrix[i + 1, j] && lineMatrix[i, j] > lineMatrix[i - 1, j])
                                {
                                    //if (lineMatrix[i, j + 1] > 0 && lineMatrix[i, j - 1] > 0 && lineMatrix[i + 1, j] > 0 && lineMatrix[i - 1, j] > 0) //Ako se odkomentarise nece obelezavati T preseke
                                    //{
                                    if ((lineMatrix[i, j] > lineMatrix[i, j + 1] && lineMatrix[i, j] > lineMatrix[i, j - 1]) || (lineMatrix[i, j] > lineMatrix[i + 1, j] && lineMatrix[i, j] > lineMatrix[i - 1, j]))
                                    {
                                        Element el = new Element();
                                        double X = el.X + 0.5;
                                        double Y = myCanvas.Height - el.Y - 0.5; //Gledamo u odnosu na Canvas Top
                                        Polygon p = new Polygon();
                                        p.Stroke = System.Windows.Media.Brushes.Red;
                                        p.Fill = System.Windows.Media.Brushes.Red;
                                        p.StrokeThickness = 0.2;

                                        p.Points.Add(new System.Windows.Point(X, Y - 0.5));
                                        p.Points.Add(new System.Windows.Point(X - 0.5, Y + 0.5));
                                        p.Points.Add(new System.Windows.Point(X + 0.5, Y + 0.5));

                                      /*Ellipse ellipse = new Ellipse
                                        {
                                            Height = 1,
                                            Width = 1,
                                            Fill = intersectionColor
                                        };*/

                                        Canvas.SetLeft(p, i);
                                        Canvas.SetBottom(p, 200 - 1 - j);

                                        myCanvas.Children.Add(p);
                                    }
                                    //}
                                }
                            }
                        }
                    }
                }
            }
        }

        //Pomocna funkcija za proveru slobodnih mesta u okolini entiteta
        public bool IsFree(int offsetX, int offsetY)
        {
            if (divs[offsetX, offsetY].Free)
            {
                divs[offsetX, offsetY].Free = false;
                return true;
            }

            return false;
        }

        //Pomocna funkcija za kretanje kroz okolinu entiteta
        public Element CheckAround(int x, int y, int offset)
        {
            for (int i = x - offset; i <= x + offset; i++)
            {
                if (i < 0)
                    continue;
                else if (i >= divCountX)
                    continue;

                for (int j = y - offset; j <= y + offset; j++)
                {
                    if (j < 0)
                        continue;
                    else if (j >= divCountY)
                        continue;

                    if (IsFree(i, j))
                        return divs[i,j];
                }
            }
            return null;
        }

        //Desni klik na vod menja boju entiteta
        List<Tuple<System.Windows.Media.Brush, Ellipse>> coloredEllipses = new List<Tuple<System.Windows.Media.Brush, Ellipse>>();
        private void Polyline_RightClick(object sender, MouseButtonEventArgs e)
        {
            foreach (Tuple<System.Windows.Media.Brush, Ellipse> t in coloredEllipses)
            {
                t.Item2.Fill = t.Item1;
            }

            coloredEllipses.Clear();


            Polyline polyline = (Polyline)sender;

            System.Windows.Point start = new System.Windows.Point();
            start = polyline.Points[0];
            System.Windows.Point end = new System.Windows.Point();
            end = polyline.Points[polyline.Points.Count - 1];

            foreach (Ellipse el in ellipses)
            {
                double bottom = Canvas.GetBottom(el);
                double left = Canvas.GetLeft(el);

                if (left == (start.X - 0.5) && bottom == (myCanvas.Height - start.Y - 0.5))
                {
                    coloredEllipses.Add(new Tuple<System.Windows.Media.Brush, Ellipse>(el.Fill, el));
                    el.Fill = System.Windows.Media.Brushes.Orange;
                }

                if (left == (end.X - 0.5) && bottom == (myCanvas.Height - end.Y - 0.5))
                {
                    coloredEllipses.Add(new Tuple<System.Windows.Media.Brush, Ellipse>(el.Fill, el));
                    el.Fill = System.Windows.Media.Brushes.Orange;
                }
            }
        }

        //From UTM to Latitude and longitude in decimal
        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }

        //Pomocne promenjive za komande
        public List<Canvas> canvasList = new List<Canvas>();
        Canvas undoSave;
        List<Canvas> clearSave;
        bool clear = false;

        //Elipsa dodatni
        #region Ellipse

        bool enableDrawEllipse = false;
        public Ellipse objEllipse;
        public TextBlock textEllipse;
        System.Windows.Point pointEllipse = new System.Windows.Point();
        public Ellipse editEllipse;

        private void DrawEllipse(object sender, RoutedEventArgs e)
        {
            enableDrawEllipse = true;
            enableDrawPolygon = false;
            enableAddText = false;
        }

        public void FinishedEllipse()
        {
            objEllipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;

            Canvas subCanvas = new Canvas();
            subCanvas.Height = objEllipse.Height;
            subCanvas.Width = objEllipse.Width;

            subCanvas.Children.Add(objEllipse);
            Canvas.SetLeft(textEllipse, objEllipse.Width/4);
            Canvas.SetTop(textEllipse, objEllipse.Height/4);
            subCanvas.Children.Add(textEllipse);

            Canvas.SetLeft(subCanvas, pointEllipse.X);
            Canvas.SetTop(subCanvas, pointEllipse.Y);
            myCanvas.Children.Add(subCanvas);

            canvasList.Add(subCanvas);
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            editEllipse = (Ellipse)sender;

            EditEllipseWindow eew = new EditEllipseWindow(this);
            eew.Show();
        }

        #endregion

        //Poligon dodatni
        #region Polygon

        bool enableDrawPolygon = false;
        List<System.Windows.Point> polygonPoints = new List<System.Windows.Point>();
        public Polygon objPolygon;
        public TextBlock textPolygon;
        public Polygon editPolygon;

        private void DrawPolygon(object sender, RoutedEventArgs e)
        {
            enableDrawPolygon = true;
            enableDrawEllipse = false;
            enableAddText = false;
        }

        public void FinishedPolygon()
        {
            objPolygon.MouseLeftButtonDown += Polygon_MouseLeftButtonDown;

            Canvas subCanvas = new Canvas();

            bool first = true;
            double minX = 0;
            double maxX = 0;
            double minY = 0;
            double maxY = 0;
            foreach (System.Windows.Point p in objPolygon.Points)
            {
                if (first)
                {
                    minX = p.X;
                    maxX = p.X;
                    minY = p.Y;
                    maxY = p.Y;
                    first = false;
                }
                else
                {
                    if (p.X < minX)
                        minX = p.X;
                    if (p.X > maxX)
                        maxX = p.X;
                    if (p.Y < minY)
                        minY = p.Y;
                    if (p.Y > maxY)
                        maxY = p.Y;
                }
            }

            double height = maxY - minY;
            double width = maxX - minX;

            subCanvas.Height = height;
            subCanvas.Width = width;

            subCanvas.Children.Add(objPolygon);
            Canvas.SetLeft(textPolygon, minX + width / 4);
            Canvas.SetTop(textPolygon, minY + height / 4);
            subCanvas.Children.Add(textPolygon);

            // kod poligona nema pomeranja
            myCanvas.Children.Add(subCanvas);

            canvasList.Add(subCanvas);

            polygonPoints.RemoveRange(0, polygonPoints.Count);
        }

        private void Polygon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            editPolygon = (Polygon)sender;

            EditPolygonWindow epw = new EditPolygonWindow(this);
            epw.Show();
        }

        #endregion
        
        //Text dodatni
        #region Text

        bool enableAddText = false;
        public TextBlock objText;
        System.Windows.Point pointText = new System.Windows.Point();
        public TextBlock editText;

        private void AddText(object sender, RoutedEventArgs e)
        {
            enableAddText = true;
            enableDrawEllipse = false;
            enableDrawPolygon = false;
        }

        public void FinishedText()
        {
            objText.MouseLeftButtonDown += Text_MouseLeftButtonDown;

            Canvas subCanvas = new Canvas();
            subCanvas.Height = objText.Height;
            subCanvas.Width = objText.Width;

            subCanvas.Children.Add(objText);

            Canvas.SetLeft(subCanvas, pointText.X);
            Canvas.SetTop(subCanvas, pointText.Y);
            myCanvas.Children.Add(subCanvas);

            canvasList.Add(subCanvas);
        }

        private void Text_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            editText = (TextBlock)sender;

            EditTextWindow etw = new EditTextWindow(this);
            etw.Show();
        }

        #endregion

        private void myCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (enableDrawEllipse)
            {
                clear = false;
                if (clearSave != null)
                    clearSave.RemoveRange(0, clearSave.Count);

                pointEllipse = e.GetPosition(myCanvas);

                textEllipse = new TextBlock();
                objEllipse = new Ellipse();
                EllipseWindow ellipseWindow = new EllipseWindow(this);
                ellipseWindow.Show();
            }

            if (enableDrawPolygon)
            {
                clear = false;
                if (clearSave != null)
                    clearSave.RemoveRange(0, clearSave.Count);

                polygonPoints.Add(e.GetPosition(myCanvas));
            }
            else
            {
                polygonPoints.RemoveRange(0, polygonPoints.Count);
            }
            
            if (enableAddText)
            {
                clear = false;
                if (clearSave != null)
                    clearSave.RemoveRange(0, clearSave.Count);

                pointText = e.GetPosition(myCanvas);

                objText = new TextBlock();
                TextWindow textWindow = new TextWindow(this);
                textWindow.Show();
            }
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (enableDrawPolygon && polygonPoints.Count > 0)
            {
                clear = false;
                if (clearSave != null)
                    clearSave.RemoveRange(0, clearSave.Count);

                textPolygon = new TextBlock();
                objPolygon = new Polygon();

                foreach (System.Windows.Point p in polygonPoints)
                {
                    objPolygon.Points.Add(p);
                }

                System.Windows.Point firstPoint = new System.Windows.Point();
                firstPoint.X = objPolygon.Points.First().X;
                firstPoint.Y = objPolygon.Points.First().Y;

                objPolygon.Points.Add(firstPoint);
                polygonPoints.Add(firstPoint);

                PolygonWindow polygonWindow = new PolygonWindow(this);
                polygonWindow.Show();
            }
        }

        //Undo, Redo, Clear
        #region Controls
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (clear)          //Ne moze undo posle clear
                return;
            if (undoSave != null)       //Vec smo uradili undo, a nismo redo
                return;
            if (canvasList.Count == 0)   //Ostao je samo graf
                return;

            undoSave = (Canvas)myCanvas.Children[myCanvas.Children.Count - 1];      //Cuvamo poslednje iscrtani element
            canvasList.Remove(undoSave);        //Brisemo taj element iz svih trenutno iscrtanih elemenata
            myCanvas.Children.RemoveAt(myCanvas.Children.Count - 1);        //Brisemo taj element sa canvasa
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (clear)      //Posebna logika za redo posle clear
            {
                foreach (UIElement uie in clearSave)        //Prolazimo kroz sve elemente koji su se sacuvali nakon Clear poziva
                {
                    canvasList.Add(uie as Canvas);      //Svaki dodajemo u listu trenutnih elemenata
                    myCanvas.Children.Add(uie as Canvas);      //I dodajemo na canvas
                }

                clearSave.RemoveRange(0, clearSave.Count);      //Brisemo sve elemente iz liste za cuvanje elemenata nakon Clear poziva
                clear = false;      //Zavrsili smo clear + redo

                return;
            }

            if (undoSave == null)       //Nemamo sta da vratimo
                return;
            
            canvasList.Add(undoSave);       //Vracamo element u listu trenutnih elemenata
            myCanvas.Children.Add(undoSave);        //Dodajemo ga na canvas da se prikaze
            undoSave = null;        //Zavrsili smo undo + redo
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            radioDrawEllipse.IsChecked = false;
            enableDrawEllipse = false;
            radioDrawPolygon.IsChecked = false;
            enableDrawPolygon = false;
            radioAddText.IsChecked = false;
            enableAddText = false;

            if (canvasList.Count == 0)      //Nemamo sta da obrisemo
                return;

            clear = true;

            clearSave = new List<Canvas>();
            foreach (UIElement uie in canvasList)   //Cuvamo UI elemente za redo
            {
                clearSave.Add(uie as Canvas);
            }

            myCanvas.Children.RemoveRange(myCanvas.Children.Count - canvasList.Count, canvasList.Count);    //Brisemo sve iznad grafa   

            canvasList.RemoveRange(0, canvasList.Count);        //Brisemo sve trenutne UI elemente
        }

        #endregion
    }
}

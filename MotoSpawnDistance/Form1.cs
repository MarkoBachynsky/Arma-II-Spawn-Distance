using Equin.ApplicationFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotoSpawnDistance
{
    public partial class Form1 : Form
    {
        public Location[] spawnLocations = new Location[23];
        public List<Location> List = new List<Location>(23);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create Spawns
            spawnLocations[0] = new Location("Balota ", new XY(50, 128));
            spawnLocations[1] = new Location("Berezino ", new XY(123.5, 61.25));
            spawnLocations[2] = new Location("Chernogorsk", new XY(68.7, 121.2));
            spawnLocations[3] = new Location("Devil's Castle", new XY(70.5, 45.5));
            spawnLocations[4] = new Location("Elektrozavodsk ", new XY(96.5, 132.2));
            spawnLocations[5] = new Location("Gorka", new XY(91, 70));
            spawnLocations[6] = new Location("Guglovo", new XY(76, 84));
            spawnLocations[7] = new Location("Grishino", new XY(64.6, 48));
            spawnLocations[8] = new Location("Gvozdno", new XY(87.6, 34.2));
            spawnLocations[9] = new Location("Kabanino", new XY(55,75));
            spawnLocations[10] = new Location("Kamenka", new XY(16.5, 129));
            spawnLocations[11] = new Location("Kamyshovo", new XY(113.5, 120.5));
            spawnLocations[12] = new Location("Komarovo", new XY(39, 123.5));
            spawnLocations[13] = new Location("Krasnostav", new XY(119.5, 29.8));
            spawnLocations[14] = new Location("Krutoy Cap", new XY(135.5, 105.5));
            spawnLocations[15] = new Location("Lopatino", new XY(19.8, 55.8));
            spawnLocations[16] = new Location("Petrovka", new XY(44.5, 29.7));
            spawnLocations[17] = new Location("Pogorevka", new XY(48.8, 82.6));
            spawnLocations[18] = new Location("Prigorodki", new XY(80, 120));
            spawnLocations[19] = new Location("Pusta", new XY(93.2, 112.9));
            spawnLocations[20] = new Location("Solnichniy", new XY(129, 86));
            spawnLocations[21] = new Location("Vybor", new XY(29, 65));
            spawnLocations[21] = new Location("Vyshnoye", new XY(61, 86));
            spawnLocations[22] = new Location("Zelenogorsk", new XY(23, 94));
            orderList();
            fillTable();
        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)) * 100;
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void orderList()
        {
            List.Clear();
            for (int i = 0; i < spawnLocations.Length; i++)
            {
                if (List.Count == 0) List.Add(spawnLocations[0]);
                else
                {
                    for (int j = 0; j < List.Count; j++)
                    {
                        Location previousLocation = List.ElementAt(j);
                        Location currentLocation = spawnLocations[i];
                        previousLocation.distance = GetDistance(Double.Parse(textBox1.Text) + .5, Double.Parse(textBox2.Text) + .5, previousLocation.coordinates.X, previousLocation.coordinates.Y);
                        currentLocation.distance = GetDistance(Double.Parse(textBox1.Text), Double.Parse(textBox2.Text), currentLocation.coordinates.X, currentLocation.coordinates.Y);
                        if (previousLocation.distance >= currentLocation.distance) // if new location is farther than previous location, add and break loop
                        {
                            List.Insert(j, currentLocation);
                            break;
                        }
                        if (j == List.Count - 1) List.Add(currentLocation);
                        Debug.WriteLine("PL = " + previousLocation.name + ", " + Math.Round(previousLocation.distance) + " - CL = " + currentLocation.name + ", " + Math.Round(currentLocation.distance));

                        }
                    }
            }
            foreach(Location l in List)
            {
                Debug.WriteLine(l.name + " - " + Math.Round(l.distance));
            }

        }

        private void fillTable()
        {
            dataGridView1.Rows.Clear();
                foreach (Location location in List)
                {
                    dataGridView1.Rows.Add(location.name, Math.Round(location.distance).ToString("#,#", CultureInfo.InvariantCulture) + " meters");
                }

            for (int i = 0; i < dataGridView1.Columns.Count - 1; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                int colw = dataGridView1.Columns[i].Width;
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView1.Columns[i].Width = colw;
            }
        }

        private void textbox_KeyChange(object sender, EventArgs e)
        {
            if (!textBox1.Text.All(char.IsDigit) || !textBox2.Text.All(char.IsDigit)) textBox1.Text = "0";
            if (textBox1.Text.Equals("") || textBox2.Text.Equals("")) return;
            orderList();
            fillTable();
        }

        private void myDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void fix_size(object sender, EventArgs e)
        {
            Width = 300;
            Height = 437;
        }
    }
}

public class XY
{
    public double X;
    public double Y;

    public XY()
    {
        X = 0;
        Y = 0;
    }

    public XY(int a, int b)
    {
        X = a;
        Y = b;
    }

    public XY(double a, double b)
    {
        X = a;
        Y = b;
    }

    override public string ToString()
    {
        return X + ", " + Y;
    }

}


public class Location
{
    public string name;
    public XY coordinates;
    public double distance;

    public Location()
    {
        name = "blank";
        coordinates = new XY(0, 0);
    }

    public Location(string n, XY xy)
    {
        name = n;
        coordinates = xy;
    }

}
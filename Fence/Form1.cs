using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Fence
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        List<Point> points = new List<Point>();
        List<Point> fence = new List<Point>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateField();
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CreateField();
        }

        private void CreateField()
        {
            points.Clear();
            fence.Clear();
            
            for (int i = 0; i < 100; i++)
            {
                points.Add(new Point(rand.Next(20, 1000), rand.Next(20, 650)));
            }

            fence = ConvexHull(points);
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            points.ForEach(p => e.Graphics.FillEllipse(new SolidBrush(Color.Red), p.X - 5, p.Y - 5, 10, 10));

            if (fence.Count < 3) return;
            var path = new GraphicsPath();
            path.AddPolygon(fence.ToArray());
            e.Graphics.DrawPath(new Pen(Color.Red), path);
        }

        public static List<Point> ConvexHull(List<Point> points)
        {
            if (points.Count < 3) throw new ArgumentException("At least 3 points reqired", "points");

            List<Point> hull = new List<Point>();

            // get leftmost point
            Point vPointOnHull = points.Where(p => p.X == points.Min(min => min.X)).First();

            Point vEndpoint;
            do
            {
                hull.Add(vPointOnHull);
                vEndpoint = points[0];

                for (int i = 1; i < points.Count; i++)
                {
                    if ((vPointOnHull == vEndpoint) || (Orientation(vPointOnHull, vEndpoint, points[i]) == -1))
                    {
                        vEndpoint = points[i];
                    }
                }

                vPointOnHull = vEndpoint;

            }
            while (vEndpoint != hull[0]);

            return hull;
        }

        private static int Orientation(Point p1, Point p2, Point p)
        {
            // Determinant
            int orient = (p2.X - p1.X) * (p.Y - p1.Y) - (p.X - p1.X) * (p2.Y - p1.Y);

            if (orient > 0)
                return -1; //          (* Orientation is to the left-hand side  *)
            if (orient < 0)
                return 1; // (* Orientation is to the right-hand side *)

            return 0; //  (* Orientation is neutral aka collinear  *)
        }
    }
}
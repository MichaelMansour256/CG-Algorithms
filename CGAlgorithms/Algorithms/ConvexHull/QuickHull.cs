using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            
            Point min_x = points[0];
            Point max_x = points[0];
            foreach (Point i in points) {
                if (min_x.X > i.X) {
                    min_x = i;
                }
                if (max_x.X < i.X) {
                    max_x = i;
                }
            }
            Line line = new Line(min_x,max_x);
            Line line_rev = new Line(max_x, min_x);
            List<Point> left_points = new List<Point>();
            List<Point> right_points = new List<Point>();
            foreach (Point p in points) {
                Enums.TurnType type =HelperMethods.CheckTurn(line, p);
                if (type==Enums.TurnType.Left) {
                    left_points.Add(p);
                }
                if (type == Enums.TurnType.Right)
                {
                    right_points.Add(p);
                }
            }
            List<Point> extreme_Left  = QuickHullRec(left_points,line);
            List<Point> extreme_Right = QuickHullRec(right_points,line_rev);
            extreme_Left.Add(min_x);
            extreme_Left.Add(max_x);
            extreme_Left.AddRange(extreme_Right);
            List<Point> result = new List<Point>();
            foreach (Point p in extreme_Left)
            {
                if (!result.Contains(p)) {
                    result.Add(p);
                }
            }
            outPoints = result;

        }
        public List<Point> QuickHullRec(List<Point> points, Line line) {
            if (points.Count == 0) {
                return new List<Point>();
            }
            Point Pmax = GetMaxPoint(points,line);
            Line line1 = new Line(line.Start,Pmax);
            List<Point> left_points1 = GetLeftPoints(points, line1);
            List<Point> R1 = QuickHullRec(left_points1, line1);


            Line line2 = new Line(Pmax,line.End);
            List<Point> left_points2 = GetLeftPoints(points, line2);
            List<Point> R2 = QuickHullRec(left_points2, line2);
            
            R1.Add(Pmax);
            R1.AddRange(R2);
            List<Point> result = new List<Point>();
            foreach (Point p in R1) {
                if (!result.Contains(p))
                {
                    
                     result.Add(p);
                    
                }
            }
            return result;
        }
        public Point GetMaxPoint(List<Point> points, Line line) {
            Point max_distance_point = points[0];
            double max_distance = -10000000.0;
            double x1 = line.Start.X;
            double y1 = line.Start.Y;
            double x2 = line.End.X;
            double y2 = line.End.Y;
            double x0, y0, H1;
            foreach (Point p in points) {
                

                x0 = p.X;
                y0 = p.Y;
                double xx = x2 - x1, yy = y2 - y1;
                H1 = Math.Abs(((x2 - x1) * (y1 - y0)) - ((x1 - x0) * (y2 - y1))) / Math.Sqrt((xx * xx) + (yy * yy));
                
                if (H1>max_distance)
                {
                    max_distance = H1;
                    max_distance_point = p;
                }
            }
            return max_distance_point;
        }

        public List<Point> GetLeftPoints(List<Point> points, Line line) {
            List<Point> left_points = new List<Point>();
            
            foreach (Point p in points)
            {
                
                Enums.TurnType type = HelperMethods.CheckTurn(line, p);
                if (type == Enums.TurnType.Left)
                {
                    left_points.Add(p);
                }
                

            }
            return left_points;
        }
        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}

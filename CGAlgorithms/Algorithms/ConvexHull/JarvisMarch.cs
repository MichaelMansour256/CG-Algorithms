using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// if Conlinear is isempty
// Add I& j
//else 
//loop on coliner
// for every index get slop pair  & slop I, J
// compare 
// if slop eqaul 
//calculate distanse1 & 2
//if dis2 > dis1
//pair[index] = I, J
//if new line no slope in list
//add I, J

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            int len_points = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count;)
                {
                    if (points[i].X == points[j].X && points[i].Y == points[j].Y)//x, y
                    {
                        points.Remove(points[j]);
                    }
                    else
                    {
                        j++;
                    }
                }
            }
            len_points = points.Count;
            if (len_points <= 3)
            {
                outPoints = new List<Point>(points);
            }
            else
            {
                double y_min = points[0].Y;
                Point smallest_point = new Point(points[0].X, points[0].Y);
                foreach (Point p in points)
                {
                    if (p.Y < y_min)
                    {
                        y_min = p.Y;
                        smallest_point = p;
                    }
                }
                double x = smallest_point.X;
                double y = smallest_point.Y;
                Point start_point = new Point(x, y);

                len_points = points.Count;
                int at_end = 0;
                outPoints.Add(smallest_point);
                do
                {
                    at_end = outPoints.Count;
                    smallest_point = outPoints[at_end - 1];
                    Point coliner = null;
                    foreach (Point i in points)
                    {
                        if (smallest_point == i)
                            continue;
                        int l = 0, r = 0;
                        foreach (Point j in points)
                        {
                            if (smallest_point != j && i != j)
                            {
                                Enums.TurnType type = HelperMethods.CheckTurn(new Line(smallest_point, i), j);
                                if (type == Enums.TurnType.Left)
                                {
                                    l++;
                                }
                                else if (type == Enums.TurnType.Right)
                                {
                                    r++;
                                    break;
                                }
                            }
                        }
                        if (l == len_points - 2)
                        {
                            outPoints.Add(i);
                            break;
                        }
                        else if (r == 0)
                        {
                            if (coliner == null)
                            {
                                coliner = i;
                            }
                            else
                            {
                                //smallest, coliner, i
                                //smallest, colliner
                                //smallest, i
                                double xx = 0f, yy = 0f;
                                xx = smallest_point.X - coliner.X;
                                yy = smallest_point.Y - coliner.Y;
                                double dis1 = Math.Sqrt(xx * xx + yy * yy);
                                xx = smallest_point.X - i.X;
                                yy = smallest_point.Y - i.Y;
                                double dis2 = Math.Sqrt(xx * xx + yy * yy);
                                if (dis2 > dis1)
                                {
                                    coliner = i;
                                }
                            }
                        }

                    }
                    if (coliner != null)
                    {
                        outPoints.Add(coliner);
                    }
                    at_end = outPoints.Count;
                    if (outPoints[at_end - 1].X == start_point.X && outPoints[at_end - 1].Y == start_point.Y)
                    {
                        outPoints.Remove(outPoints[at_end - 1]);
                        break;
                    }
                } while (true);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
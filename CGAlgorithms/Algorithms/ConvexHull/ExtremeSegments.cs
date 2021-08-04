using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
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

                List<KeyValuePair<Point, Point>> colinear_list = new List<KeyValuePair<Point, Point>>();
                int left, right, colinear;
                double slope1 = 0, slope2 = 0;
                double dis1, dis2;
                Point p1, p2;
                foreach (Point i in points)
                {
                    foreach (Point j in points)
                    {
                        if (i == j)
                            continue;
                        left = 0;
                        right = 0;
                        colinear = 0;
                        Line line = new Line(i, j);
                        foreach (Point k in points)
                        {

                            if (i != j && i != k && k != j)
                            {
                                Enums.TurnType type = HelperMethods.CheckTurn(line, k);
                                if (type == Enums.TurnType.Left)
                                {
                                    left++;
                                }
                                else if (type == Enums.TurnType.Right)
                                {
                                    right++;
                                }
                                //else
                                //{
                                //    colinear++;

                                //    //double slope = 0;
                                //    //int lastIN = colinear_list.Count;
                                //    //Point temp = colinear_list[lastIN -1 ];
                                //    //slope = (k.Y - temp.Y) / (k.X - temp.X);

                                //    //colinear_list.Add(k);
                                //}
                            }
                        }
                        bool new_coliner = false;
                        if (left == points.Count - 2 || right == points.Count - 2)
                        {
                            if (!outPoints.Contains(i))
                            {
                                outPoints.Add(i);
                            }
                            if (!outPoints.Contains(j))
                            {
                                outPoints.Add(j);
                            }
                        }
                        else if (left == 0 || right == 0) ///
                        {
                            KeyValuePair<Point, Point> x = new KeyValuePair<Point, Point>(i, j);
                            if (colinear_list.Count == 0)
                            {
                                colinear_list.Add(x);
                            }
                            else
                            {
                                for (int p = 0; p < colinear_list.Count; p++)
                                {
                                    p1 = colinear_list[p].Key;
                                    p2 = colinear_list[p].Value;
                                    slope1 = (p2.Y - p1.Y) / (p2.X - p1.X);
                                    slope2 = (i.Y - j.Y) / (i.X - j.X);

                                    if (slope2 == slope1)
                                    {
                                        new_coliner = true;
                                        double xx = 0f, yy = 0f;
                                        xx = p1.X - p2.X;
                                        yy = p1.Y - p2.Y;
                                        dis1 = Math.Sqrt(xx * xx + yy * yy);
                                        xx = i.X - j.X;
                                        yy = i.Y - j.Y;
                                        dis2 = Math.Sqrt(xx * xx + yy * yy);
                                        if (dis2 > dis1)
                                        {
                                            colinear_list[p] = x;
                                        }
                                    }
                                }
                                if (!new_coliner)
                                {
                                    colinear_list.Add(x);
                                }
                            }

                        }
                    }
                }
                foreach (KeyValuePair<Point, Point> c in colinear_list)
                {
                    if (!outPoints.Contains(c.Key))
                    {
                        outPoints.Add(c.Key);
                    }
                    if (!outPoints.Contains(c.Value))
                    {
                        outPoints.Add(c.Value);
                    }
                }
            }

        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
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
            List<Point> temp_points_extream = new List<Point>(points);
            len_points = points.Count;
            foreach (Point i in points)
            {
                bool outt = false;
                foreach (Point j in points)
                {
                    foreach (Point k in points)
                    {
                        foreach (Point l in points)
                        {
                            if (i != j && i != k && i != l
                                       && j != k && j != l
                                                 && k != l)
                            {
                                Enums.PointInPolygon type_point = HelperMethods.PointInTriangle(i, j, k, l);
                                if (type_point == Enums.PointInPolygon.Inside || type_point == Enums.PointInPolygon.OnEdge)
                                {
                                    temp_points_extream.Remove(i);
                                    outt = true;
                                    break;
                                    // output_points.Add(i);
                                }
                            }
                        }
                        if (outt)
                            break;
                    }
                    if (outt)
                        break;
                }
            }

            outPoints = new List<Point>(temp_points_extream);

        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}

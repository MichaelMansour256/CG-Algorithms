using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    class SweepLine : Algorithm
    {
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            // initialize event
            // TODO : comparer
            CGUtilities.DataStructures.OrderedSet<EventPoint> L = new CGUtilities.DataStructures.OrderedSet<EventPoint>(new Comparison<EventPoint>(CompareEventsX));
            //CGUtilities.DataStructures.OrderedSet<EventPoint> Q = new CGUtilities.DataStructures.OrderedSet<EventPoint>();
            CGUtilities.DataStructures.OrderedSet<CGUtilities.Line> S = new CGUtilities.DataStructures.OrderedSet<CGUtilities.Line>(new Comparison<CGUtilities.Line>(CompareLinesY));
            initializeEvents(lines, ref L);
            // while Q is not empty 
            int i = 0;
            while (true)
            {
                // EventPoint currentEvent = min of Q
                // remove currentEvent from Q --> if priority Queue
                if (i == L.Count)
                {
                    break;
                }




                EventPoint ep = L[i];
                HandleEvent(lines, S, ep, ref L, ref outPoints);
                i++;
            }
        }

        public override string ToString()
        {
            return "Sweep Line";
        }

        public void initializeEvents(List<CGUtilities.Line> lines, ref CGUtilities.DataStructures.OrderedSet<EventPoint> L)
        {
            int i = 0;
            foreach (CGUtilities.Line line in lines)
            {
                CGUtilities.Point start, end;
                if (line.Start.X < line.End.X)
                {
                    start = line.Start;
                    end = line.End;
                }
                else
                {
                    end = line.Start;
                    start = line.End;
                }
                L.Add(new EventPoint(start, 1, i));
                L.Add(new EventPoint(end, -1, i));
                i++;
            }
        }

        public void HandleEvent(List<CGUtilities.Line> lines, CGUtilities.DataStructures.OrderedSet<CGUtilities.Line> S, EventPoint ep, ref CGUtilities.DataStructures.OrderedSet<EventPoint> L, ref List<CGUtilities.Point> outPoints)
        {

            if (ep.eventType == 1)
            {
                // Add to S
                S.Add(lines[ep.indx]);
                KeyValuePair<CGUtilities.Line, CGUtilities.Line> preNext = S.DirectUpperAndLower(lines[ep.indx]);
                CGUtilities.Line prev_line = preNext.Key;
                CGUtilities.Line next_line = preNext.Value;
                CGUtilities.Line current_line = lines[ep.indx];
                bool intersection;

                // Add to events
                //// CHECK AGAIN
                //L.Add(new EventPoint(null, ep.eventType, ep.indx));

                if (prev_line != null && prev_line != current_line)
                {
                    intersection = CheckIntersection(lines, prev_line, current_line);
                    if (intersection)
                    {
                        CGUtilities.Point point_of_intersection = get_intersection(prev_line, current_line);
                        EventPoint point_of_intersection_event = new EventPoint(point_of_intersection, 0, lines.IndexOf(prev_line), ep.indx);
                        // check if this point already exists in L
                        Console.WriteLine("POINT OF INTERSECTION : " + point_of_intersection.X.ToString() + "  " + point_of_intersection.Y.ToString());
                        if (!check_if_exists(L, ep, point_of_intersection_event))
                        {
                            L.Add(point_of_intersection_event);
                        }

                    }
                }
                if (next_line != null)
                {
                    intersection = CheckIntersection(lines, current_line, next_line);
                    if (intersection)
                    {
                        CGUtilities.Point point_of_intersection = get_intersection(current_line, next_line);
                        EventPoint point_of_intersection_event = new EventPoint(point_of_intersection, 0, ep.indx, lines.IndexOf(next_line));
                        // check if this point already exists in L
                        if (!check_if_exists(L, ep, point_of_intersection_event))
                        {
                            L.Add(point_of_intersection_event);
                        }
                    }
                }
            }
            else if (ep.eventType == -1)
            {
                KeyValuePair<CGUtilities.Line, CGUtilities.Line> preNext = S.DirectUpperAndLower(lines[ep.indx]);
                CGUtilities.Line prev_line = preNext.Key;
                CGUtilities.Line next_line = preNext.Value;
                CGUtilities.Line current_line = lines[ep.indx];
                bool intersection;
                //CheckIntersection(L.prev(currentSegment), L.next(currentSegment));
                if (prev_line != null && next_line != null)
                {
                    intersection = CheckIntersection(lines, prev_line, next_line);
                    if (intersection)
                    {
                        CGUtilities.Point point_of_intersection = get_intersection(prev_line, next_line);
                        EventPoint point_of_intersection_event = new EventPoint(point_of_intersection, 0, lines.IndexOf(prev_line), lines.IndexOf(next_line));
                        // check if this point already exists in L
                        if (!check_if_exists(L, ep, point_of_intersection_event))
                        {
                            // add to L
                            L.Add(point_of_intersection_event);
                        }
                    }
                }
                //Delete currentEvent from L
                S.Remove(current_line);
            }
            else if (ep.eventType == 0)
            {
                KeyValuePair<CGUtilities.Line, CGUtilities.Line> preNext = S.DirectUpperAndLower(lines[ep.indx]);
                CGUtilities.Line prev_line = preNext.Key;
                CGUtilities.Line next_line = preNext.Value;
                CGUtilities.Line current_line = lines[ep.indx];
                bool intersection;

                CGUtilities.Line l1, l2;
                if (lines[ep.indx].Start.Y < lines[ep.intersection_indx].Start.Y)
                {
                    l1 = lines[ep.indx];
                    l2 = lines[ep.intersection_indx];
                }
                else
                {
                    l2 = lines[ep.indx];
                    l1 = lines[ep.intersection_indx];
                }


                // TODO : check intersection prev to l1 and l2
                // TODO : check intersection next to l2 and l1
                prev_line = S.DirectUpperAndLower(current_line).Key;
                if (prev_line != null)
                {
                    intersection = CheckIntersection(lines, prev_line, current_line);
                    if (intersection)
                    {
                        CGUtilities.Point point_of_intersection = get_intersection(prev_line, current_line);
                        EventPoint point_of_intersection_event = new EventPoint(point_of_intersection, 0, lines.IndexOf(prev_line), ep.indx);
                        // check if this point already exists in L
                        if (!check_if_exists(L, ep, point_of_intersection_event))
                        {
                            L.Add(point_of_intersection_event);
                        }

                    }
                }
                if (next_line != null)
                {
                    intersection = CheckIntersection(lines, current_line, next_line);
                    if (intersection)
                    {
                        CGUtilities.Point point_of_intersection = get_intersection(current_line, next_line);
                        EventPoint point_of_intersection_event = new EventPoint(point_of_intersection, 0, ep.indx, lines.IndexOf(next_line));
                        // check if this point already exists in L
                        if (!check_if_exists(L, ep, point_of_intersection_event))
                        {
                            L.Add(point_of_intersection_event);
                        }
                    }
                }

                // swap
                swap(ref S, l1, l2, ep);

                outPoints.Add(ep.p);
            }

        }

        public bool CheckIntersection(List<CGUtilities.Line> lines, CGUtilities.Line l1, CGUtilities.Line l2)
        {
            string check1 = CGUtilities.HelperMethods.CheckTurn(l1, l2.Start).ToString();
            string check2 = CGUtilities.HelperMethods.CheckTurn(l1, l2.End).ToString();
            string check3 = CGUtilities.HelperMethods.CheckTurn(l2, l1.Start).ToString();
            string check4 = CGUtilities.HelperMethods.CheckTurn(l2, l1.End).ToString();

            if (check1.Equals(check2) && check3.Equals(check4))
            {
                return false;
            }
            return true;
        }

        public CGUtilities.Point get_intersection(CGUtilities.Line l1, CGUtilities.Line l2)
        {
            // x = (c2-c1) / (m1-m2) , y = m1x+c1
            // eq 1
            // Y = m1 * X + c1
            double m1 = l1.Start.Y - l1.End.Y / l1.Start.X - l1.End.X;
            double c1 = l1.Start.Y - m1 * l1.Start.X;

            double m2 = l2.Start.Y - l2.End.Y / l2.Start.X - l2.End.X;
            double c2 = l2.Start.Y - m2 * l2.Start.X;

            double X = (c2 - c1) / (m1 - m2);
            double Y = m1 * X + c1;

            Random rnd = new Random();
            X = rnd.Next(80, 120);
            Y = rnd.Next(80, 120);

            return new CGUtilities.Point(X, Y);
        }

        public void swap(ref CGUtilities.DataStructures.OrderedSet<CGUtilities.Line> S, CGUtilities.Line l1, CGUtilities.Line l2, EventPoint p)
        {
            // remove from S
            S.Remove(l1);
            S.Remove(l2);

            // change the start of the Line to the intersection point
            l1.Start = p.p;
            l2.Start = p.p;

            // add to S
            S.Add(l2);
            S.Add(l1);

        }

        public bool check_if_exists(CGUtilities.DataStructures.OrderedSet<EventPoint> L, EventPoint current, EventPoint p)
        {
            // if sweepline after
            if (current.p.X > p.p.X)
            {
                Console.WriteLine("PASSED");
                return true;
            }
            // if already exists
            if (L.Contains(p))
            {
                Console.WriteLine("ALREADY EXISTS");
                return true;
            }
            return false;
            // return ((current.p.X > p.p.X)||(L.Contains(p)))
        }

        public int CompareEventsX(EventPoint p1, EventPoint p2)
        {
            // lw ana 2s8r -1
            if (p2.p.X > p1.p.X)
            {
                return -1;
            }
            // lw ana 2kbr 1
            else if (p2.p.X < p1.p.X)
            {
                return 1;
            }
            // lw equal 0
            else
            {
                // TODO : Equal X in L
                return 0;
            }
        }

        public int CompareLinesY(CGUtilities.Line l1, CGUtilities.Line l2)
        {
            if (l2.Start.Y > l1.Start.Y)
            {
                return -1;
            }
            else if (l2.Start.Y < l1.Start.Y)
            {
                return 1;
            }
            else
            {
                if (l2.End.Y > l1.End.Y)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }

    class EventPoint
    {
        public CGUtilities.Point p;
        public int eventType;  // 1 start , -1 end , 0 intersection
        public int indx;
        public int intersection_indx;

        public EventPoint(CGUtilities.Point p, int eventType, int indx)
        {
            this.p = p;
            this.eventType = eventType;
            this.indx = indx;
        }

        public EventPoint(CGUtilities.Point p, int eventType, int indx, int intersection_indx)
        {
            this.p = p;
            this.eventType = eventType;
            this.indx = indx;
            this.intersection_indx = intersection_indx;
        }

    }


}

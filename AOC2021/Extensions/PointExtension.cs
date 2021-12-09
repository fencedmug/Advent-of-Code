using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021.Extensions
{
    public static class PointExtension
    {
        public static Point Shift(this Point pt, int x = 0, int y = 0) => new Point(pt.X + x, pt.Y + y);

        public static IEnumerable<Point> GetAllNeighbours(this Point pt, int width, int height)
        {
            return pt.GetStraightNeighbours(width, height)
                .Concat(pt.GetDiagonalNeighbours(width, height));
        }

        public static IEnumerable<Point> GetDiagonalNeighbours(this Point pt, int width, int height)
        {
            var neighbours = new List<Point>();

            if (pt.X != 0 && pt.Y != 0)
                neighbours.Add(pt.Shift(-1, -1));

            if (pt.X != 0 && pt.Y < height - 1)
                neighbours.Add(pt.Shift(-1, 1));

            if (pt.X < width - 1 && pt.Y != 0)
                neighbours.Add(pt.Shift(1, -1));

            if (pt.X < width - 1 && pt.Y < height - 1)
                neighbours.Add(pt.Shift(1, 1));

            return neighbours;
        }

        public static IEnumerable<Point> GetDiagonalNeighbours(this Point pt)
        {
            return new List<Point>
            {
                pt.Shift(-1, -1),
                pt.Shift(-1, 1),
                pt.Shift(1, -1),
                pt.Shift(1, 1),
            };
        }

        public static IEnumerable<Point> GetStraightNeighbours(this Point pt, int width, int height)
        {
            var neighbours = new List<Point>();

            if (pt.Y != 0)
                neighbours.Add(pt.Shift(0, -1));

            if (pt.X != 0)
                neighbours.Add(pt.Shift(-1, 0));

            if (pt.Y < height - 1)
                neighbours.Add(pt.Shift(0, 1));

            if (pt.X < width - 1)
                neighbours.Add(pt.Shift(1, 0));

            return neighbours;
        }

        public static IEnumerable<Point> GetStraightNeighbours(this Point pt)
        {
            return new List<Point> 
            {
                pt.Shift(0, -1),
                pt.Shift(-1, 0),
                pt.Shift(0, 1),
                pt.Shift(1, 0),
            };
        }

        public static IEnumerable<Point> InsidePositiveSpace(this IEnumerable<Point> points, int width, int height)
        {
            foreach (var pt in points)
            {
                if ((pt.X > 0 && pt.X < width) && (pt.Y > 0 && pt.Y < height))
                    yield return pt;
            }
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021.Extensions
{
    public static class PointExtension
    {
        public static Point Shift(this Point pt, int dx = 0, int dy = 0) => new Point(pt.X + dx, pt.Y + dy);
        public static Point ShiftX(this Point pt, int dx = 0) => new Point(pt.X + dx, pt.Y);
        public static Point ShiftY(this Point pt, int dy = 0) => new Point(pt.X, pt.Y + dy);
        public static void OffsetX(this Point pt, int dx = 0) => pt.X = pt.X + dx;
        public static void OffsetY(this Point pt, int dy = 0) => pt.Y = pt.Y + dy;

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
            var dx = new int[] { 1, -1, 0, 0 };
            var dy = new int[] { 0, 0, 1, -1 };

            var pts = new List<Point>();

            for (var i = 0; i < dx.Length; i++)
            {
                var nbX = pt.X + dx[i];
                var nbY = pt.Y + dy[i];

                if (nbX >= 0 && nbX < width && nbY >= 0 && nbY < height)
                    pts.Add(new Point(nbX, nbY));
            }

            return pts;
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

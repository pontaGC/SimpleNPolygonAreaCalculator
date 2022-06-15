using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNPolygonAreaCalculator
{
    public readonly struct Position
    {
        #region Constructors

        public Position(int x, int y)
            : this(1.0 * x, 1.0 * y)
        {

        }

        public Position(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region Properties

        public static readonly Position Zero = new Position(MathConstants.Zero, MathConstants.Zero);

        public double X { get; }

        public double Y { get; }

        #endregion
    }
}

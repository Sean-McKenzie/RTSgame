using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Vector
    {
        #region Properties
        protected double x;
        /// <summary>
        /// Horizontal component of the Vector
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        protected double y;
        /// <summary>
        /// Vertical component of the Vector
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        /// <summary>
        /// Length of the Vector
        /// </summary>
        public double Magnitude
        {
            get
            {
                return (double)Math.Sqrt(x * x + y * y);
            }
        }

        /// <summary>
        /// Get a unit-length version of this Vector
        /// </summary>
        public Vector Unitized
        {
            get
            {
                return this / Magnitude;
            }
        }

        /// <summary>
        /// Gets the equivalent angle that this Vector points at
        /// </summary>
        public double Angle
        {
            get
            {
                if (this.Magnitude == 0)
                {
                    return 0;
                }
                if (y <= 0)
                    return (double)(Math.Asin(X / Magnitude)
                        * 180 / Math.PI);
                else
                    return (double)(180 - Math.Asin(X / Magnitude)
                        * 180 / Math.PI);
            }
        }
        #endregion

        #region Operators
        public static Vector operator + (Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator - (Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator /(Vector v, double f)
        {
            return new Vector(v.X / f, v.Y / f);
        }

        public static Vector operator * (double f, Vector v)
        {
            return new Vector(f * v.X, f * v.Y);
        }

        public static Vector operator * (Vector v, double f)
        {
            return new Vector(f * v.X, f * v.Y);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Builds a zero-length Vector pointing nowhere
        /// </summary>
        public Vector()
        {
            x = 0;
            y = 0;
        }

        /// <summary>
        /// Builds a Vector based on X and Y components
        /// </summary>
        /// <param name="x">Horizontal Component</param>
        /// <param name="y">Vertical Component</param>
        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Builds a new Vector that is a copy of another Vector passed in
        /// </summary>
        /// <param name="v">The Vector to copy</param>
        public Vector(Vector v)
        {
            x = v.X;
            y = v.Y;
        }

        /// <summary>
        /// Builds a new unit-length Vector given an angle for it to point to
        /// </summary>
        /// <param name="angle">Angle in degrees</param>
        public Vector(double angle)
        {
            x = (double)Math.Sin(angle*Math.PI/180);
            y = -1*(double)Math.Cos(angle*Math.PI/180);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the angle between 2 Vectors
        /// </summary>
        /// <param name="v2">The 2nd Vector to calculate angle from</param>
        /// <returns>Angle in degrees</returns>
        public double AngleBetween(Vector v2)
        {
            double numerator = this.X * v2.X + this.Y * v2.Y;
            double denominator = this.Magnitude * v2.Magnitude;
            return Math.Acos(numerator / denominator);
        }
        #endregion
    }
}

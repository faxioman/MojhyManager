using System;
using System.Collections.Generic;
using System.Text;

namespace Mojhy.Utils
{
    class Math
    {
        /// <summary>
        /// Calculate the direction angle from a source point to a destination point in RAD.
        /// </summary>
        /// <param name="px1">The PX1.</param>
        /// <param name="py1">The py1.</param>
        /// <param name="px2">The PX2.</param>
        /// <param name="py2">The py2.</param>
        /// <returns></returns>
        public static double Angle(double px1, double py1, double px2, double py2)
        {
            // Negate X and Y value
            double pxRes = px2 - px1;
            double pyRes = py2 - py1;
            double angle = 0.0;
            // Calculate the angle
            if (pxRes == 0.0)
            {
                if (pxRes == 0.0)
                    angle = 0.0;
                else if (pyRes > 0.0)
                    angle = System.Math.PI / 2.0;
                else
                    angle = System.Math.PI * 3.0 / 2.0;
            }
            else if (pyRes == 0.0)
            {
                if (pxRes > 0.0)
                    angle = 0.0;
                else
                    angle = System.Math.PI;
            }
            else
            {
                if (pxRes < 0.0)
                    angle = System.Math.Atan(pyRes / pxRes) + System.Math.PI;
                else if (pyRes < 0.0)
                    angle = System.Math.Atan(pyRes / pxRes) + (2 * System.Math.PI);
                else
                    angle = System.Math.Atan(pyRes / pxRes);
            }
            // Convert to degrees
            angle = angle * 180 / System.Math.PI;
            //Return to RADIANT ;-) non chiedetemi il perchè 
            angle = (((double)(360 - angle)) / 180) * System.Math.PI;
            return angle;
        }
    }
}

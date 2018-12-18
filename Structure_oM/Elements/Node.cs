﻿using BH.oM.Base;
using BH.oM.Common;
using BH.oM.Geometry;
using BH.oM.Structure.Properties.Constraint;

namespace BH.oM.Structure.Elements
{
    public class Node : BHoMObject, IElement0D
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public Point Position { get; set; } = new Point();

        public Constraint6DOF Constraint { get; set; } = null;


        /***************************************************/
        /**** Explicit Casting                          ****/
        /***************************************************/

        public static explicit operator Node(Point point)
        {
            return new Node { Position = point };
        }

        /***************************************************/
    }
}
    
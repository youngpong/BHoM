﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHoM.Geometry;
using System.Reflection;

namespace BHoM.Structural
{
    /// <summary>
    /// Node objects
    /// </summary>
    [Serializable]
    public class Node : BHoM.Global.BHoMObject, IStructuralObject
    {
        /////////////////
        ////Properties///
        /////////////////
        private double[] m_Point;

        public double[] Coordinates
        {
            get
            {
                return m_Point != null ? m_Point : m_Point = Parameters.LookUp<List<double>>(Global.Param.Coordinates).ToArray();
            }
            set
            {
                m_Point = value;
                Parameters.AddList<double>(Global.Param.Coordinates, value.ToList());
            }
        }

        /// <summary>Node position as a point object</summary>
        public Point Point 
        {
            get
            {           
                return new Point(Coordinates);
            }
            set
            {
                Coordinates = Point;
            }
        }

        /// <summary>Node constraint (support/restraint)</summary>
        public BHoM.Structural.Constraint Constraint
        {
            get
            {
                return Project.GetObject(Parameters.LookUp<Guid>(Global.Param.Constraint)) as Constraint;
            }
            set
            {
                Parameters.AddItem<Guid>(Global.Param.Constraint, value.BHoM_Guid);
            }
        }

        /// <summary>Returns true is node is constrained</summary>
        public bool IsConstrained { get; private set; }

        /// <summary>Constraint name - is inherited from constraint object if exists</summary>
        public string ConstraintName { get; private set; }

        /// <summary>Bars connected to the node</summary>
        public List<Bar> ConnectedBars { get; private set; }

        /// <summary>Faces connected to the node</summary>
        public List<Face> ConnectedFaces { get; private set; }

        /// <summary>Valence of node</summary>
        public int Valence { get; private set; }

        /// <summary>Absolute angles between connected bars (direct measurement of bar vectors)</summary>
        public List<double> BarAbsoluteAngles { get; private set; }

        /// <summary>Delta angles between connected bars measured in the node plane</summary>
        public List<double> BarDeltaAngles { get; private set; }

        /// <summary>Theta angles between connected bars measured in the node plane</summary>
        public List<double> BarThetaAngles { get; private set; }

        /// <summary>Node plane for angular and setting out methods</summary>
        public Plane Plane { get; private set; }


        ///////////////////
        ////Constructors///
        ///////////////////

        internal Node() { }

        /// <summary>
        /// Constructes a node from CartesianCoordinates and an index number
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="number"></param>
        internal Node(double x, double y, double z, int number)
        {
            Coordinates = new double[] { x, y, z };
            Number = number;
        }

        /// <summary>
        /// Returns true if node is valid (number less than 0 or position is invalid)
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (Number < 0) { return false; }
                if (!Point.IsValid) { return false; }
                return true;
            }
        }

        /// <summary>
        /// Returns true if node number is greater than 0
        /// </summary>
        /// <returns></returns>
        public bool HasValidNumber()
        {
            return Number > 0;
        }

        //////////////
        ////Methods///
        //////////////

        /// <summary>
        /// Sets the node number
        /// </summary>
        /// <param name="number"></param>
        public void SetNumber(int number)
        {
            this.Number = number;
        }

        /// <summary>
        /// Gets or sets the CartesianCoordinates of the node position
        /// </summary>
        public double[] CartesianCoordinates
        {
            get { return m_Point; }
        }

        /// <summary>
        /// Gets or sets the X value of the node position
        /// </summary>
        public double X
        {
            get 
            {
                return m_Point[0]; 
            }
            set
            {
                m_Point[0] = value;
                Coordinates = m_Point;
            }
        }

        /// <summary>
        /// Gets or sets the Y value of the node position
        /// </summary>
        public double Y
        {
            get
            {
                return m_Point[1];
            }
            set
            {
                m_Point[1] = value;
                Coordinates = m_Point;
            }
        }

        /// <summary>
        /// Gets or sets the Z value of the node position
        /// </summary>
        public double Z
        {
            get
            {
                return m_Point[2];
            }
            set
            {
                m_Point[2] = value;
                Coordinates = m_Point;
            }
        }

        /// <summary>
        /// Calculates the distance from the input node to this
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public double DistanceTo(Node node)
        {

            double dist = 0;
            double[] target = this.CartesianCoordinates;
            double[] search = node.CartesianCoordinates;

            for (int i = 0; i < 3; i++)
            {
                dist += (Math.Pow((target[i] - search[i]), 2));
            }
            dist = Math.Sqrt(dist);
            return dist;
        }

        /// <summary>
        /// Sets the name of the node
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Sets the constraint of a node
        /// </summary>
        /// <param name="constraint"></param>
        public void SetConstraint(BHoM.Structural.Constraint constraint)
        {
            this.Constraint = constraint;
            this.ConstraintName = constraint.Name;
            this.IsConstrained = true;
        }

        /// <summary>
        /// Sets the constraint name of the node
        /// </summary>
        /// <param name="constraintName"></param>
        public void SetConstraintName(string constraintName)
        {
            this.ConstraintName = constraintName;
            this.IsConstrained = true;
        }

        /// <summary>
        /// Sets a default plane as coordinate system
        /// </summary>
        public void SetCartesianCoordinatesystemAsDefault()
        {
            this.Plane = new Plane(Point);
        }

        /// <summary>
        /// Sets coordinate system as plane
        /// </summary>
        /// <param name="plane"></param>
        public void SetPlane(Plane plane)
        {
            this.Plane = plane;
        }

        /// <summary>
        /// Resets the topology by removing connected bars and setting valence to 0
        /// </summary>
        public void ResetTopology()
        {
            this.ConnectedBars = new List<Bar>();
            Valence = 0;
        }


        /// <summary>
        /// Add a bar instance into connected bar list
        /// </summary>
        /// <param name="b"></param>
        public void AddBar(Bar b)
        {
            ConnectedBars.Add(b);
        }

        /// <summary>
        /// Add a face instance into connected face list
        /// </summary>
        /// <param name="f"></param>
        public void AddFace(Face f)
        {
            ConnectedFaces.Add(f);
        }


        /// <summary>
        /// WIP
        /// </summary>
        /// <returns></returns>
        public bool SortConnectedBars()
        {
            Valence = ConnectedBars.Count;
            if (Valence < 2) return false;


            List<Node> nodeRing = new List<Node>(Valence);
            foreach (Bar b in ConnectedBars)
                nodeRing.Add(b.GetOppositeNode(this));

            List<double> angleAccumulator = new List<double>(Valence);
            Vector v0 = nodeRing[0].Point - this.Point;
            angleAccumulator.Add(0.0);
            for (int i = 1; i < Valence; i++)
            {
                Vector v1 = nodeRing[i].Point - this.Point;
                double angle = Vector.VectorAngle(v0, v1, Plane.Normal);
                if (angle < 0) angle += 2.0 * Math.PI;
                angleAccumulator.Add(angle);
            }

            SortBarsByAngle(ConnectedBars, angleAccumulator);
            SetAngles(angleAccumulator);

            return true;
        }


        /// <summary>
        /// WIP - change to use sorted dictionary
        /// Sort the bars by angle, smallest angle first.
        /// Assumes one to one mapping of bars to angles in two lists
        /// </summary>
        /// <param name="bars"></param>
        /// <param name="angles"></param>
        /// <returns></returns>
        private static bool SortBarsByAngle(List<Bar> bars, List<double> angles)
        {
            while (true)
            {
                bool swapped = false;
                for (int i = 0; i < angles.Count - 1; i++)
                    if (angles[i] > angles[i + 1])
                    {
                        double temp = angles[i];
                        angles[i] = angles[i + 1];
                        angles[i + 1] = temp;

                        Bar tempBar = bars[i];
                        bars[i] = bars[i + 1];
                        bars[i + 1] = tempBar;

                        swapped = true;
                    }
                if (!swapped)
                    break;
            }

            return true;
        }

        /// <summary>
        /// WIP
        /// </summary>
        /// <returns></returns>
        private void SetAngles(List<double> angleAccumulator)
        {
            BarDeltaAngles = new List<double>(angleAccumulator.Count);
            for (int i = 1; i < angleAccumulator.Count; i++)
                BarDeltaAngles.Add(angleAccumulator[i] - angleAccumulator[i - 1]);

            BarDeltaAngles.Add(2.0 * Math.PI - angleAccumulator.Last());
        }

   }
}
    
/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System.ComponentModel;
using BH.oM.Quantities.Attributes;

namespace BH.oM.Structure.Results
{
    [Description("General modal dynamics result of the structure.")]
    public class ModalDynamics : StructuralGlobalResult
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        
        public virtual int ModeNumber { get; set; } = 0;

        [Frequency]
        public virtual double Frequency { get; set; } = 0.0;

        [Mass]
        public virtual double ModalMass { get; set; } = 0.0;

        [ForcePerUnitLength]
        public virtual double ModalStiffness { get; set; } = 0.0;

        public virtual double ModalDamping { get; set; } = 0.0;

        [Ratio]
        public virtual double MassRatioX { get; set; } = 0.0;

        [Ratio]
        public virtual double MassRatioY { get; set; } = 0.0;

        [Ratio]
        public virtual double MassRatioZ { get; set; } = 0.0;

        [Ratio]
        public virtual double InertiaRatioX { get; set; } = 0.0;

        [Ratio]
        public virtual double InertiaRatioY { get; set; } = 0.0;

        [Ratio]
        public virtual double InertiaRatioZ { get; set; } = 0.0;

        /***************************************************/
    }
}


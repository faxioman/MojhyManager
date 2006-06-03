/* PlayArea.cs, FABIO MASINI
 * La classe definisce la singola area di gioco del campo */

using System;
using System.Collections.Generic;
using System.Text;
using Mojhy.Engine;
using Mojhy.Utils.DrawingExt;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines a single play area of the fields
    /// </summary>
    public class PlayArea
    {
        //rettangolo dell'area
        private RectangleObject l_objAreaRect = new RectangleObject(0,0,0,0);
        //indice dell'area
        public int l_intIndex;
        //il parent di PlayArea � l'oggetto PlayAreas
        private PlayAreas l_objPlayAreas;
        /// <summary>
        /// Gets the PlayAreas.
        /// </summary>
        /// <value>The parent.</value>
        public PlayAreas parent
        {
            get { return l_objPlayAreas; }
        }
        /// <summary>
        /// Gets the rectangle of the this <see cref="T:PlayArea"/>.
        /// </summary>
        public RectangleObject AreaRect
        {
            get { return l_objAreaRect; }
        }
        /// <summary>
        /// Get the area index
        /// </summary>
        public int Index
        {
            get { return l_intIndex; }
            set { l_intIndex = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PlayArea"/> class.
        /// </summary>
        /// <param name="objPlayAreas">The PlayAreas object.</param>
        public PlayArea(PlayAreas objPlayAreas)
        {
            l_objPlayAreas = objPlayAreas;
        }
    }
}

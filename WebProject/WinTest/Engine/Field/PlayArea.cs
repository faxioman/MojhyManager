/* PlayArea.cs, FABIO MASINI
 * La classe definisce la singola area di gioco del campo */

using System;
using System.Collections.Generic;
using System.Text;
using Mojhy.Engine;
using System.Drawing;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines a single play area of the fields
    /// </summary>
    public class PlayArea
    {
        /// <summary>
        /// Defines the rect of the area
        /// </summary>
        public Rectangle AreaRect;
        /// <summary>
        /// Get the area index
        /// </summary>
        public int Index;
        //il parent di PlayArea è l'oggetto PlayAreas
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
        /// Initializes a new instance of the <see cref="T:PlayArea"/> class.
        /// </summary>
        /// <param name="objPlayAreas">The PlayAreas object.</param>
        public PlayArea(PlayAreas objPlayAreas)
        {
            l_objPlayAreas = objPlayAreas;
        }
    }
}

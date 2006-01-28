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
    }
}

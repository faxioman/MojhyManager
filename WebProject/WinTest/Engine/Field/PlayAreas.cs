/* PlayAreas.cs, FABIO MASINI
 * La classe definisce le aree di gioco del campo che il sistema considera.
 * Esistono 20 aree dove il pallone può trovarsi. Per ognuna di queste
 * saranno definite le posizioni dei giocatori sul campo. */

using System;
using System.Collections.Generic;
using System.Text;
using Mojhy.Engine;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines all the twenty areas of the field
    /// </summary>
    class PlayAreas
    {
        private PlayArea[] l_arrAreas;
        private Field l_objField;
        /// <summary>
        /// Gets the areas array.
        /// </summary>
        /// <value>The array of PlayArea.</value>
        public PlayArea[] Areas
        {
            get { return l_arrAreas; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PlayAreas"/> class.
        /// </summary>
        /// <param name="FieldObject">The field object.</param>
        public PlayAreas(Field FieldObject)
        {
            l_objField = FieldObject;
            l_arrAreas = new PlayArea[20];
        }
    }
}

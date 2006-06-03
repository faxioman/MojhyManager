/* PlayingPositions.cs, FABIO MASINI
 * Sono definite le posizioni che, per ogni area in cui si trova il pallone, il
 * giocatore dovrà assumere in fase di attacco, difesa, palla ala centro, angolo, ecc...
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mojhy.Utils.DrawingExt;
namespace Mojhy.Engine
{
    /// <summary>
    /// Defines all the positions of field for every area and event.
    /// </summary>
    public class PlayingPositions
    {
        //posizioni di attacco e di difesa che il giocatore deve assumere per area
        private PointObject[] l_arrAttackPositions;
        private PointObject[] l_arrDefensePositions;
        //posizioni speciali che il giocatore dovrà assumere per evento

        //TODO//

        /// <summary>
        /// Gets or sets the list of attack positions.
        /// </summary>
        /// <value>The attack positions array.</value>
        public PointObject[] AttackPositions
        {
            get { return l_arrAttackPositions; }
            set { l_arrAttackPositions = value; }
        }
        /// <summary>
        /// Gets or sets the list of  defense positions.
        /// </summary>
        /// <value>The defense positions array.</value>
        public PointObject[] DefensePositions
        {
            get { return l_arrDefensePositions; }
            set { l_arrDefensePositions = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PlayingPositions"/> class.
        /// </summary>
        public PlayingPositions()
        {
            int i = 0;
            l_arrAttackPositions = new PointObject[20];
            l_arrDefensePositions = new PointObject[20];
            for (i = 0; i < 20; i++)
            {
                l_arrAttackPositions[i] = new PointObject();
                l_arrDefensePositions[i] = new PointObject();
            }
        }
    }
}
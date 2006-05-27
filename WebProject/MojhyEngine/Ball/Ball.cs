/* Ball.cs, FABIO MASINI
 * La classe definisce l'oggetto pallone */

using System;
using System.Collections.Generic;
using System.Text;
using Mojhy.Utils.DrawingExt;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines the ball object used on field.
    /// </summary>
    public class Ball
    {
        //oggetto campo ove � collocato il pallone
        private Field l_objField;
        //posizione del pallone in campo
        private Point3D l_ptzBallPosition = new Point3D(0,0,0);
        /// <summary>
        /// Gets the parent Field Object.
        /// </summary>
        /// <value>The parent Field Object.</value>
        public Field parent
        {
            get { return l_objField; }
        }
        /// <summary>
        /// Gets the ball position on field.
        /// </summary>
        /// <value>The ball position on field as a 3-dimensional point.</value>
        public Point3D PositionOnField
        {
            get { return l_ptzBallPosition; }
            set { l_ptzBallPosition = value; }
        }
        /// <summary>
        /// Initialize the ball on field.
        /// </summary>
        /// <param name="objField">The referenced field object.</param>
        public void PutOnField(Field objField)
        {
            if (objField != null)
            {
                //imposto il riferimento all'oggetto campo  
                l_objField = objField;
            }
            else
            {
                throw new Exception("The Field Object cannot be null");
            }
        }
    }
}

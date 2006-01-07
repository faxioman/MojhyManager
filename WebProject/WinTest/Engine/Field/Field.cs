/* Field.cs, FABIO MASINI
 * La classe definisce l'oggetto 'campo di calcio'.
 * Tutte le misure sono espresse in millimetri. */

using System;
using System.Collections.Generic;
using System.Text;
using Mojhy.Engine;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines the soccer field class used by 'play algorithm'.
    /// </summary>
    class Field
    {
        #region Private variables
        private PlayAreas l_objAreas;
        #endregion
        
        #region Private Const
        //spessore righe del campo
        private const int SPESSORELINEE = 110;
        //spessore dei punti disegnati sul campo
        private const int SPESSOREPUNTI = 800;
        //dimensioni del campo in mm
        private const int CAMPOWIDTH = 105000;
        private const int CAMPOHEIGHT = 70000;
        private const int PORTAWIDTH = 7320;
        //misura area di rigore
        private const int AREARIGORE = 16500;
        //misura area di porta
        private const int AREAPORTA = 5500;
        //distanza dischetto di rigore
        private const int DISCHETTORIGORE = 11000;
        //raggio lunette area di rigore e metà campo
        private const int RAGGIOCIRCCAMPO = 9150;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the field's width.
        /// </summary>
        /// <value>The width in mm.</value>
        public int Width
        {
            get { return CAMPOWIDTH; }
        }
        /// <summary>
        /// Gets the field's height.
        /// </summary>
        /// <value>The height in mm.</value>
        public int Height
        {
            get { return CAMPOHEIGHT; }
        }
        /// <summary>
        /// Gets the field's lines thickness.
        /// </summary>
        /// <value>The lines thickness in mm.</value>
        public int LinesThickness
        {
            get { return SPESSORELINEE; }
        }
        /// <summary>
        /// Gets the field's points thickness.
        /// </summary>
        /// <value>The points thickness in mm.</value>
        public int PointsThickness
        {
            get { return SPESSOREPUNTI; }
        }
        /// <summary>
        /// Gets the width of the goal.
        /// </summary>
        /// <value>The width of the goal in mm.</value>
        public int GoalWidth
        {
            get { return PORTAWIDTH; }
        }
        /// <summary>
        /// Gets the penalty area size.
        /// </summary>
        /// <value>The penalty area size in mm.</value>
        public int PenaltyAreaSize
        {
            get { return AREARIGORE; }
        }
        /// <summary>
        /// Gets the size of the goal area.
        /// </summary>
        /// <value>The size of the goal area in mm.</value>
        public int GoalAreaSize
        {
            get { return AREAPORTA; }
        }
        /// <summary>
        /// Gets the penalty spot distance from goal.
        /// </summary>
        /// <value>The penalty spot distance from goal in mm.</value>
        public int PenaltySpotDistance
        {
            get { return DISCHETTORIGORE; }
        }
        /// <summary>
        /// Gets the radius of all circles in a field (penalty arc and centre circle).
        /// </summary>
        /// <value>The radius in mm.</value>
        public int CirclesRadius
        {
            get { return RAGGIOCIRCCAMPO; }
        } 
        public PlayAreas Areas
        {
            get
            {
                return l_objAreas;
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Field"/> class.
        /// </summary>
        public Field()
        {
            //Inizializzo l'oggetto interno l_objAreas che contiene le aree
            //di gioco definite dal sistema
            l_objAreas = new PlayAreas(this);
        }
    }
}

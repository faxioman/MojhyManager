/* Field.cs, FABIO MASINI
 * La classe definisce l'oggetto 'campo di calcio'.
 * Tutte le misure sono espresse in millimetri. */

using System;
using System.Collections.Generic;
using System.Text;
using Mojhy.Engine;
using System.Drawing;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines the soccer field class used by 'play algorithm'.
    /// </summary>
    public class Field
    {
        #region Private variables
        //aree sensibili del campo
        private PlayAreas l_objAreas;
        //misure porta 1
        private Rectangle l_rctPortaLeft;
        //misure porta 2
        private Rectangle l_rctPortaRight;
        //misure area di rigore 1
        private Rectangle l_rctAreaRigoreLeft;
        //misure area di rigore 2
        private Rectangle l_rctAreaRigoreRight;
        //misure area piccola 1
        private Rectangle l_rctAreaPiccolaLeft;
        //misure area piccola 2
        private Rectangle l_rctAreaPiccolaRight;
        //posizione dischetto di rigore 1
        private Point l_ptDischettoRigoreLeft;
        //posizione dischetto di rigore 2
        private Point l_ptDischettoRigoreRight;
        //posizione del dischetto di metà campo
        private Point l_ptDischettoMetaCampo;
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
        /// <summary>
        /// Gets the rect of the first goal.
        /// </summary>
        /// <returns>The Rectangle of the first goal</returns>
        public Rectangle GoalLeftRect
        {
            get { return l_rctPortaLeft; }
        }
        /// <summary>
        /// Gets the rect of the second goal.
        /// </summary>
        /// <returns>The Rectangle of the second goal</returns>
        public Rectangle GoalRightRect
        {
            get { return l_rctPortaRight; }
        }
        /// <summary>
        /// Gets the rect of the first penalty area.
        /// </summary>
        /// <returns>The Rectangle of the first penalty area</returns>
        public Rectangle PenaltyAreaLeft
        {
            get { return l_rctAreaRigoreLeft; }
        }
        /// <summary>
        /// Gets the rect of the second penalty area.
        /// </summary>
        /// <returns>The Rectangle of the second penalty area</returns>
        public Rectangle PenaltyAreaRight
        {
            get { return l_rctAreaRigoreRight; }
        }
        /// <summary>
        /// Gets the rect of the first goal area.
        /// </summary>
        /// <returns>The Rectangle of the first goal area</returns>
        public Rectangle GoalAreaLeft
        {
            get { return l_rctAreaPiccolaLeft; }
        }
        /// <summary>
        /// Gets the rect of the second goal area.
        /// </summary>
        /// <returns>The Rectangle of the second goal area</returns>
        public Rectangle GoalAreaRight
        {
            get { return l_rctAreaPiccolaRight; }
        }
        /// <summary>
        /// Gets the position of the first penalty spot.
        /// </summary>
        /// <returns>The Point of the first penalty spot</returns>
        public Point PenaltySpotLeft
        {
            get { return l_ptDischettoRigoreLeft; }
        }
        /// <summary>
        /// Gets the position of the second penalty spot.
        /// </summary>
        /// <returns>The Point of the second penalty spot</returns>
        public Point PenaltySpotRight
        {
            get { return l_ptDischettoRigoreRight; }
        }
        /// <summary>
        /// Gets the position of the centre spot.
        /// </summary>
        /// <returns>The Point of the centre spot</returns>
        public Point CentreSpot
        {
            get { return l_ptDischettoMetaCampo; }
        }
        /// <summary>
        /// Gets the areas of field.
        /// </summary>
        /// <value>The areas.</value>
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
            //calcolo il rect delle due porte
            l_rctPortaLeft.X = 0;
            l_rctPortaLeft.Y = (this.Height - this.GoalWidth) / 2;
            l_rctPortaLeft.Width = 0;
            l_rctPortaLeft.Height = this.GoalWidth;
            l_rctPortaRight.X = this.Width;
            l_rctPortaRight.Y = (this.Height - this.GoalWidth) / 2;
            l_rctPortaRight.Width = 0;
            l_rctPortaRight.Height = this.GoalWidth;
            //calcolo il rect delle aree di rigore
            l_rctAreaRigoreLeft.X = 0;
            l_rctAreaRigoreLeft.Y = this.GoalLeftRect.Top - this.PenaltyAreaSize;
            l_rctAreaRigoreLeft.Width = this.PenaltyAreaSize;
            l_rctAreaRigoreLeft.Height = this.GoalLeftRect.Height + this.PenaltyAreaSize*2;
            l_rctAreaRigoreRight.X = this.Width - this.PenaltyAreaSize;
            l_rctAreaRigoreRight.Y = this.GoalRightRect.Top - this.PenaltyAreaSize;
            l_rctAreaRigoreRight.Width = this.PenaltyAreaSize;
            l_rctAreaRigoreRight.Height = this.GoalRightRect.Height + this.PenaltyAreaSize * 2;
            //calcolo il rect delle aree piccole
            l_rctAreaPiccolaLeft.X = 0;
            l_rctAreaPiccolaLeft.Y = this.GoalLeftRect.Top - this.GoalAreaSize;
            l_rctAreaPiccolaLeft.Width = this.GoalAreaSize;
            l_rctAreaPiccolaLeft.Height = this.GoalLeftRect.Height + this.GoalAreaSize * 2;
            l_rctAreaPiccolaRight.X = this.Width - this.GoalAreaSize;
            l_rctAreaPiccolaRight.Y = this.GoalRightRect.Top - this.GoalAreaSize;
            l_rctAreaPiccolaRight.Width = this.GoalAreaSize;
            l_rctAreaPiccolaRight.Height = this.GoalRightRect.Height + this.GoalAreaSize * 2;
            //calcolo la posizione dei dischetti del rigore
            l_ptDischettoRigoreLeft.X = this.PenaltySpotDistance;
            l_ptDischettoRigoreLeft.Y = this.Height / 2;
            l_ptDischettoRigoreRight.X = this.Width - this.PenaltySpotDistance;
            l_ptDischettoRigoreRight.Y = this.Height / 2;
            //calcolo la posizione del dischetto di metà campo
            l_ptDischettoMetaCampo.X = this.Width / 2;
            l_ptDischettoMetaCampo.Y = this.Height / 2;
        }
    }
}

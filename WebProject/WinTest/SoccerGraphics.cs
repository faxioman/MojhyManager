using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinTest
{
    /// <summary>
    /// Graphics utility for soccer testbed
    /// </summary>
    class SoccerGraphics
    {

        //unit� di misura in millimetri e scala di disegno
        private const GraphicsUnit UNIT = GraphicsUnit.Millimeter;
        private const float SCALE = (float)0.002;
        //oggetto Pen per il disegno delle linee del campo
        Pen l_penLinee;
        //oggetto campo interno
        Mojhy.Engine.Field l_objField;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SoccerGraphics"/> class.
        /// </summary>
        public SoccerGraphics(Mojhy.Engine.Field objField){
            l_objField = objField;
            l_penLinee = new Pen(Color.White, l_objField.LinesThickness);
        }
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:WinTest.SoccerGraphics"/> is reclaimed by garbage collection.
        /// </summary>
        ~SoccerGraphics()
        {
            l_penLinee.Dispose();
        }
        /// <summary>
        /// Sets the graphics properties of the Graphics object used by SoccerGraphics.
        /// </summary>
        /// <param name="g">The Graphics object.</param>
        private void setGraphicsProperties(Graphics g){
            //attivo l'antialias
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //imposto l'unit� di misura
            g.PageUnit = UNIT;
            //imposto la scala, visto che considero il campo con le sue misure reali
            g.PageScale = SCALE;
        }
        /// <summary>
        /// Render the base soccer field.
        /// </summary>
        /// <param name="g">The graphics object where the soccer field must be rendered</param>
		public void RenderField(Graphics g, Boolean blShowAreas)
        {
            setGraphicsProperties(g);
            //disegno il fondo
            g.FillRectangle(Brushes.Green, 0, 0, l_objField.Width, l_objField.Height);
            g.DrawRectangle(l_penLinee, 0, 0, l_objField.Width, l_objField.Height);
            //disegno le aree di rigore
            ////prima
            g.DrawRectangle(l_penLinee, l_objField.PenaltyAreaLeft);
            ////seconda
            g.DrawRectangle(l_penLinee, l_objField.PenaltyAreaRight);            
            //disegno l'area piccola
            ////prima
            g.DrawRectangle(l_penLinee, l_objField.GoalAreaLeft);
            ////seconda
            g.DrawRectangle(l_penLinee, l_objField.GoalAreaRight);
            //disegno le porte
            l_penLinee.Color = Color.Red;
            l_penLinee.Width = l_objField.LinesThickness * 4;
            g.DrawLine(l_penLinee, l_objField.GoalLeftRect.Left, l_objField.GoalLeftRect.Top, l_objField.GoalLeftRect.Right, l_objField.GoalLeftRect.Bottom);
            g.DrawLine(l_penLinee, l_objField.GoalRightRect.Left, l_objField.GoalRightRect.Top, l_objField.GoalRightRect.Right, l_objField.GoalRightRect.Bottom);
            l_penLinee.Color = Color.White;
            l_penLinee.Width = l_objField.LinesThickness;
            //disegno i dischetti di rigore
            g.FillEllipse(Brushes.White, l_objField.PenaltySpotLeft.X - l_objField.PointsThickness / 2, l_objField.PenaltySpotLeft.Y - l_objField.PointsThickness / 2, l_objField.PointsThickness, l_objField.PointsThickness);
            g.FillEllipse(Brushes.White, l_objField.PenaltySpotRight.X - l_objField.PointsThickness / 2, l_objField.PenaltySpotRight.Y - l_objField.PointsThickness / 2, l_objField.PointsThickness, l_objField.PointsThickness);
            //disegno la riga di met� campo.
            g.DrawLine(l_penLinee, l_objField.CentreSpot.X, 0, l_objField.CentreSpot.X, l_objField.Height);
            //disegno il punto di met� campo
            g.FillEllipse(Brushes.White, l_objField.CentreSpot.X - l_objField.PointsThickness / 2, l_objField.CentreSpot.Y - l_objField.PointsThickness / 2, l_objField.PointsThickness, l_objField.PointsThickness);
            //disegno il cerchio di met� campo
            g.DrawEllipse(l_penLinee, l_objField.CentreSpot.X - l_objField.CirclesRadius, l_objField.CentreSpot.Y - l_objField.CirclesRadius, l_objField.CirclesRadius * 2, l_objField.CirclesRadius * 2);
            //disegno le lunette dell'area.
            //troppo a digiuno in geometria per calcolare l'intersezione del cerchio con l'area di rigore.
            //ho fissato l'angolo di partenza a -53� (e il suo complemento a 180�) e l'ampiezza dell'angolo a 106� :-(
            g.DrawArc(l_penLinee, l_objField.PenaltySpotLeft.X - l_objField.CirclesRadius, l_objField.PenaltySpotLeft.Y - l_objField.CirclesRadius, l_objField.CirclesRadius * 2, l_objField.CirclesRadius * 2, -53, 106);
            g.DrawArc(l_penLinee, l_objField.PenaltySpotRight.X - l_objField.CirclesRadius, l_objField.PenaltySpotRight.Y - l_objField.CirclesRadius, l_objField.CirclesRadius * 2, l_objField.CirclesRadius * 2, 127, 106);
            //se richiesto, disegno le aree di gioco
            if (blShowAreas)
            {
                Pen penAreeGioco = new Pen(Color.YellowGreen);
                for (int i = 0; i < l_objField.Areas.AreasList.Length; i++)
                {
                    g.DrawRectangle(penAreeGioco, l_objField.Areas.AreasList[i].AreaRect);
                }
                penAreeGioco.Dispose();
            }
        }
    }
}

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

        //unità di misura in millimetri e scala di disegno
        private const GraphicsUnit enUnit = GraphicsUnit.Millimeter;
        private const float Scale = (float)0.002;
        /// <summary>
        /// Render the base soccer field.
        /// </summary>
        /// <param name="g">The graphics object where the soccer field must be rendered</param>
		public void RenderField(Graphics g)
        {
            Mojhy.Engine.Field objField = new Mojhy.Engine.Field();
            g.PageUnit = enUnit;
            //imposto la scala, visto che considero il campo con le sue misure reali
            g.PageScale = Scale;
            //disegno il fondo
            g.FillRectangle(Brushes.Green, 0, 0, objField.Width, objField.Height);
            //creo il Pen per il disegno delle linee
            Pen penLinee = new Pen(Color.White, objField.LinesThickness);
            //disegno le porte
            PointF ptPorta1A = new PointF(0, (objField.Height - objField.GoalWidth) / 2);
            PointF ptPorta1B = new PointF(0, (objField.Height - objField.GoalWidth) / 2 + objField.GoalWidth);
            g.DrawLine(penLinee, ptPorta1A, ptPorta1B);
            PointF ptPorta2A = new PointF(objField.Width, (objField.Height - objField.GoalWidth) / 2);
            PointF ptPorta2B = new PointF(objField.Width, (objField.Height - objField.GoalWidth) / 2 + objField.GoalWidth);
            g.DrawLine(penLinee, ptPorta2A.X - objField.LinesThickness / 2, ptPorta2A.Y, ptPorta2B.X - objField.LinesThickness / 2, ptPorta2B.Y);
            //disegno le aree di rigore
            ////prima
            g.DrawLine(penLinee, new PointF(0, ptPorta1A.Y - objField.PenaltyAreaSize), new PointF(objField.PenaltyAreaSize, ptPorta1A.Y - objField.PenaltyAreaSize));
            g.DrawLine(penLinee, new PointF(0, ptPorta1B.Y + objField.PenaltyAreaSize), new PointF(objField.PenaltyAreaSize, ptPorta1B.Y + objField.PenaltyAreaSize));
            g.DrawLine(penLinee, new PointF(objField.PenaltyAreaSize, ptPorta1A.Y - objField.PenaltyAreaSize), new PointF(objField.PenaltyAreaSize, ptPorta1B.Y + objField.PenaltyAreaSize));
            ////seconda
            g.DrawLine(penLinee, new PointF(objField.Width - objField.LinesThickness / 2, ptPorta2A.Y - objField.PenaltyAreaSize), new PointF(objField.Width - objField.PenaltyAreaSize - objField.LinesThickness / 2, ptPorta2A.Y - objField.PenaltyAreaSize));
            g.DrawLine(penLinee, new PointF(objField.Width - objField.LinesThickness / 2, ptPorta2B.Y + objField.PenaltyAreaSize), new PointF(objField.Width - objField.PenaltyAreaSize - objField.LinesThickness / 2, ptPorta2B.Y + objField.PenaltyAreaSize));
            g.DrawLine(penLinee, new PointF(objField.Width - objField.PenaltyAreaSize - objField.LinesThickness / 2, ptPorta2A.Y - objField.PenaltyAreaSize), new PointF(objField.Width - objField.PenaltyAreaSize - objField.LinesThickness / 2, ptPorta2B.Y + objField.PenaltyAreaSize));
            //disegno l'area piccola
            ////prima
            g.DrawLine(penLinee, new PointF(0, ptPorta1A.Y - objField.GoalAreaSize), new PointF(objField.GoalAreaSize, ptPorta1A.Y - objField.GoalAreaSize));
            g.DrawLine(penLinee, new PointF(0, ptPorta1B.Y + objField.GoalAreaSize), new PointF(objField.GoalAreaSize, ptPorta1B.Y + objField.GoalAreaSize));
            g.DrawLine(penLinee, new PointF(objField.GoalAreaSize, ptPorta1A.Y - objField.GoalAreaSize), new PointF(objField.GoalAreaSize, ptPorta1B.Y + objField.GoalAreaSize));
            ////seconda
            g.DrawLine(penLinee, new PointF(objField.Width - objField.LinesThickness/2, ptPorta2A.Y - objField.GoalAreaSize), new PointF(objField.Width - objField.GoalAreaSize - objField.LinesThickness/2, ptPorta2A.Y - objField.GoalAreaSize));
            g.DrawLine(penLinee, new PointF(objField.Width - objField.LinesThickness/2, ptPorta2B.Y + objField.GoalAreaSize), new PointF(objField.Width - objField.GoalAreaSize - objField.LinesThickness/2, ptPorta2B.Y + objField.GoalAreaSize));
            g.DrawLine(penLinee, new PointF(objField.Width - objField.GoalAreaSize - objField.LinesThickness/2, ptPorta2A.Y - objField.GoalAreaSize), new PointF(objField.Width - objField.GoalAreaSize - objField.LinesThickness/2, ptPorta2B.Y + objField.GoalAreaSize));
            //disegno i dischetti di rigore
            PointF dischettoRigore1 = new PointF(objField.PenaltySpotDistance, objField.Height / 2);
            PointF dischettoRigore2 = new PointF(objField.Width - objField.PenaltySpotDistance, objField.Height / 2);
            g.FillEllipse(Brushes.White, dischettoRigore1.X - objField.PointsThickness / 2, dischettoRigore1.Y - objField.PointsThickness / 2, objField.PointsThickness, objField.PointsThickness);
            g.FillEllipse(Brushes.White, dischettoRigore2.X - objField.PointsThickness / 2 - objField.LinesThickness / 2, dischettoRigore2.Y - objField.PointsThickness / 2, objField.PointsThickness, objField.PointsThickness);
            //disegno le lunette dell'area.
            //troppo a digiuno in geometria per calcolare l'intersezione del cerchio con l'area di rigore.
            //ho fissato l'angolo di partenza a -53° (e il suo complemento a 180°) e l'ampiezza dell'angolo a 106° :-(
            g.DrawArc(penLinee, dischettoRigore1.X - objField.CirclesRadius, dischettoRigore1.Y - objField.CirclesRadius, objField.CirclesRadius * 2, objField.CirclesRadius * 2, -53, 106);
            g.DrawArc(penLinee, dischettoRigore2.X - objField.CirclesRadius, dischettoRigore2.Y - objField.CirclesRadius, objField.CirclesRadius * 2, objField.CirclesRadius * 2, 127, 106);
            //calcolo la metà del campo
            PointF ptMetaCampo = new PointF(objField.Width / 2, objField.Height / 2);
            //disegno la riga di metà campo.
            g.DrawLine(penLinee, ptMetaCampo.X - objField.LinesThickness / 2, 0, ptMetaCampo.X - objField.LinesThickness / 2, objField.Height);
            //disegno il punto di metà campo
            g.FillEllipse(Brushes.White, ptMetaCampo.X - objField.PointsThickness / 2, ptMetaCampo.Y - objField.PointsThickness / 2, objField.PointsThickness, objField.PointsThickness);
            //disegno il cerchio di metà campo
            g.DrawEllipse(penLinee, ptMetaCampo.X - objField.CirclesRadius - objField.LinesThickness / 2, ptMetaCampo.Y - objField.CirclesRadius, objField.CirclesRadius * 2, objField.CirclesRadius * 2);
        }
    }
}

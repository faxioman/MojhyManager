using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Mojhy.Utils.DrawingExt;

namespace WinTest
{
    /// <summary>
    /// Graphics utility for soccer testbed
    /// </summary>
    class SoccerGraphics
    {
        //oggetto Graphics interno
        private Graphics l_objGraphics;
        //dpi dell'oggetto grafico
        private float l_fltDpiX;
        private float l_fltDpiY;
        //unità di misura in millimetri e scala di disegno
        private const GraphicsUnit UNIT = GraphicsUnit.Millimeter;
        private const float SCALE = (float)0.002;
        //millimetri per pollice
        private const float MM_PER_INCH = (float)25.4;
        //questo è il numero 'magico' (un giorno capirò come calcolarlo) da
        //usare per calcolare il moltiplicatore utilizzato nella conversione
        //da pixel a millimetri del campo
        private const float MAGICNUMBERCONVERSION = (float)0.07;
        //oggetto Pen per il disegno delle linee del campo
        Pen l_penLinee;
        //bitmap del palloncino
        private Image l_imgPalloncino;
        //bitmap base delleposizioni di attacco
        private Image l_imgAttacco;
        //bitmap base delleposizioni di difesa
        private Image l_imgDifesa;
        //bitmap base della posizione corrente
        private Image l_imgCurrent;
        public enum PlayerPositionType
        {
            Attack, Defense, Current
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SoccerGraphics"/> class.
        /// </summary>
        public SoccerGraphics(Graphics g){
            l_objGraphics = g;
            //inizializzo l'oggetto graphics
            SetGraphicsObject(g);
            //imposto i DPI dello schermo
            l_fltDpiX = g.DpiX;
            l_fltDpiY = g.DpiY;
            //carico la bitmap del palloncino
            l_imgPalloncino = Bitmap.FromFile("./Images/Palloncino.png", true);
            //carico la bitmap dell'attacco
            l_imgAttacco = Bitmap.FromFile("./Images/Attacco.png", true);
            //carico la bitmap della difesa
            l_imgDifesa = Bitmap.FromFile("./Images/Difesa.png", true);
            //carico la bitmpa di posizione corrente
            l_imgCurrent = Bitmap.FromFile("./Images/Current.png", true);
        }
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:WinTest.SoccerGraphics"/> is reclaimed by garbage collection.
        /// </summary>
        ~SoccerGraphics()
        {
            //distruggo la penna per disegnare le linee del campo
            l_penLinee.Dispose();
            //distruggo l'immagine del palloncino
            l_imgPalloncino.Dispose();
            //distruggo l'immagine attacco
            l_imgAttacco.Dispose();
            //distruggo l'immagine difesa
            l_imgDifesa.Dispose();
            //distruggo l'immagine current
            l_imgCurrent.Dispose();
        }
        /// <summary>
        /// Sets the graphics object parameters.
        /// </summary>
        /// <param name="g">The referenced Graphics object.</param>
        public void SetGraphicsObject(Graphics g)
        {
            l_objGraphics = g;
            //attivo l'antialias
            l_objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            l_objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            //imposto l'unità di misura
            l_objGraphics.PageUnit = UNIT;
            //imposto la scala, visto che considero il campo con le sue misure reali
            l_objGraphics.PageScale = SCALE;

        }
        /// <summary>
        /// Converts a point pixel location to the point as millimeter.
        /// </summary>
        /// <param name="PixelLoc">The point defined as pixel X and pixel Y.</param>
        /// <returns></returns>
        public PointObject PixelToMM(PointObject PixelLoc)
        {
            PointObject ptLocMM = new PointObject();
            ptLocMM.X = (int)(l_fltDpiX / MM_PER_INCH * PixelLoc.X * (MAGICNUMBERCONVERSION / SCALE));
            ptLocMM.Y = (int)(l_fltDpiY / MM_PER_INCH * PixelLoc.Y * (MAGICNUMBERCONVERSION / SCALE));
            return ptLocMM;
        }
        /// <summary>
        /// Converts a millimeter point location to the point as pixel.
        /// </summary>
        /// <param name="PixelLoc">The point defined as mm X and mm Y.</param>
        /// <returns></returns>
        public PointObject MMToPixel(PointObject PixelLoc)
        {
            PointObject ptLocPixel = new PointObject();
            ptLocPixel.X = (int)(MM_PER_INCH / l_fltDpiX * PixelLoc.X / (MAGICNUMBERCONVERSION / SCALE));
            ptLocPixel.Y = (int)(MM_PER_INCH / l_fltDpiY * PixelLoc.Y / (MAGICNUMBERCONVERSION / SCALE));
            return ptLocPixel;
        }
        /// <summary>
        /// Converts a millimeter point (3D) location to the point as pixel.
        /// </summary>
        /// <param name="PixelLoc">The pixel loc.</param>
        /// <returns></returns>
        public PointObject MMToPixel(Point3D PixelLoc)
        {
            PointObject ptLocPixel = new PointObject();
            ptLocPixel.X = (int)(MM_PER_INCH / l_fltDpiX * PixelLoc.X / (MAGICNUMBERCONVERSION / SCALE));
            ptLocPixel.Y = (int)(MM_PER_INCH / l_fltDpiY * PixelLoc.Y / (MAGICNUMBERCONVERSION / SCALE));
            return ptLocPixel;
        }
        /// <summary>
        /// Draws the selected play area.
        /// </summary>
        /// <param name="intSelectedAreaIndex">Index of the selected area.</param>
        /// <param name="objField">The referenced field object.</param>
        public void DrawSelectedArea(int intSelectedAreaIndex, Mojhy.Engine.Field objField)
        {
            //cerco in quale area mi trovo
            Mojhy.Engine.PlayArea objPlayArea = objField.Areas.AreasList[intSelectedAreaIndex];
            if (objPlayArea != null)
            {
                //disegno l'area selezionata
                SolidBrush colorBrush = new SolidBrush(Color.FromArgb(50, 12, 12,12));
                l_objGraphics.FillRectangle(colorBrush, new Rectangle(objPlayArea.AreaRect.X, objPlayArea.AreaRect.Y,objPlayArea.AreaRect.Width,objPlayArea.AreaRect.Height));
                colorBrush.Dispose();
                //calcolo il centro del rettangolo
                Point ptAreaCentre = new Point(objPlayArea.AreaRect.Right - (objPlayArea.AreaRect.Width / 2), objPlayArea.AreaRect.Bottom - (objPlayArea.AreaRect.Height / 2));
                l_objGraphics.DrawImage(l_imgPalloncino, ptAreaCentre);
            }
        }
        
        /// <summary>
        /// Draws the player.
        /// </summary>
        /// <param name="objPlayingPlayer">The PlayingPlayer object.</param>
        /// <param name="intAreaIndex">Index of the selected area.</param>
        /// <param name="enPositionType">Type of the player position (Attack, Defense, ...).</param>
        public void DrawPlayer(Mojhy.Engine.PlayingPlayer objPlayingPlayer, int intAreaIndex, PlayerPositionType enPositionType)
        {
            int intNumPlayer = objPlayingPlayer.Index + 1;
            Point ptCenteredPosition;
            //disegno inattacco
            //calcolo la posizione per centrare l'immagine
            Font fntNumber = new Font("Tahoma", 6, FontStyle.Bold);
            switch (enPositionType)
            {
                case PlayerPositionType.Attack:
                    ptCenteredPosition = new Point(objPlayingPlayer.PositionsOnField.AttackPositions[intAreaIndex].X - 1200, objPlayingPlayer.PositionsOnField.AttackPositions[intAreaIndex].Y - 1500);
                    l_objGraphics.DrawImage(l_imgAttacco, ptCenteredPosition);
                    if (intNumPlayer > 9)
                    {
                        l_objGraphics.DrawString(intNumPlayer.ToString(), fntNumber, Brushes.White, new Point(objPlayingPlayer.PositionsOnField.AttackPositions[intAreaIndex].X - 1000, objPlayingPlayer.PositionsOnField.AttackPositions[intAreaIndex].Y - 600));
                    }
                    else
                    {
                        l_objGraphics.DrawString(intNumPlayer.ToString(), fntNumber, Brushes.White, new Point(objPlayingPlayer.PositionsOnField.AttackPositions[intAreaIndex].X - 500, objPlayingPlayer.PositionsOnField.AttackPositions[intAreaIndex].Y - 600));
                    }
                    break;
                case PlayerPositionType.Defense:
                    //disegno in difesa
                    //calcolo la posizione per centrare l'immagine
                    ptCenteredPosition = new Point(objPlayingPlayer.PositionsOnField.DefensePositions[intAreaIndex].X - 1800, objPlayingPlayer.PositionsOnField.DefensePositions[intAreaIndex].Y - 1500);
                    l_objGraphics.DrawImage(l_imgDifesa, ptCenteredPosition);
                    if (intNumPlayer > 9)
                    {
                        l_objGraphics.DrawString(intNumPlayer.ToString(), fntNumber, Brushes.White, new Point(objPlayingPlayer.PositionsOnField.DefensePositions[intAreaIndex].X - 450, objPlayingPlayer.PositionsOnField.DefensePositions[intAreaIndex].Y - 600));
                    }
                    else
                    {
                        l_objGraphics.DrawString(intNumPlayer.ToString(), fntNumber, Brushes.White, new Point(objPlayingPlayer.PositionsOnField.DefensePositions[intAreaIndex].X - 100, objPlayingPlayer.PositionsOnField.DefensePositions[intAreaIndex].Y - 600));
                    }
                    break;
                case PlayerPositionType.Current:
                    //disegno in difesa
                    //calcolo la posizione per centrare l'immagine
                    ptCenteredPosition = new Point(objPlayingPlayer.CurrentPositionOnField.X - 1800, objPlayingPlayer.CurrentPositionOnField.Y - 1500);
                    l_objGraphics.DrawImage(l_imgCurrent, ptCenteredPosition);
                    if (intNumPlayer > 9)
                    {
                        l_objGraphics.DrawString(intNumPlayer.ToString(), fntNumber, Brushes.White, new Point(objPlayingPlayer.CurrentPositionOnField.X - 1000, objPlayingPlayer.CurrentPositionOnField.Y - 450));
                    }
                    else
                    {
                        l_objGraphics.DrawString(intNumPlayer.ToString(), fntNumber, Brushes.White, new Point(objPlayingPlayer.CurrentPositionOnField.X - 600, objPlayingPlayer.CurrentPositionOnField.Y - 450));
                    }
                    break;
            }
        }

     

        /// <summary>
        /// Render the base soccer field.
        /// </summary>
        /// <param name="objField">The Field object.</param>
        /// <param name="blShowAreas">if set to <c>true</c> shows play areas.</param>
		public void RenderField(Mojhy.Engine.Field objField, Boolean blShowAreas)
        {
            if (l_penLinee == null)
            {
                //creo l'oggetto di disegno per le righe campo
                l_penLinee = new Pen(Color.White, objField.LinesThickness);
            }
            //disegno il fondo
            l_objGraphics.FillRectangle(Brushes.Green, 0, 0, objField.Width, objField.Height);
            l_objGraphics.DrawRectangle(l_penLinee, 0, 0, objField.Width, objField.Height);
            //disegno le aree di rigore
            ////prima
            l_objGraphics.DrawRectangle(l_penLinee, objField.PenaltyAreaLeft.GetDrawingRectangle());
            ////seconda
            l_objGraphics.DrawRectangle(l_penLinee, objField.PenaltyAreaRight.GetDrawingRectangle());            
            //disegno l'area piccola
            ////prima
            l_objGraphics.DrawRectangle(l_penLinee, objField.GoalAreaLeft.GetDrawingRectangle());
            ////seconda
            l_objGraphics.DrawRectangle(l_penLinee, objField.GoalAreaRight.GetDrawingRectangle());
            //disegno le porte
            l_penLinee.Color = Color.Red;
            l_penLinee.Width = objField.LinesThickness * 4;
            l_objGraphics.DrawLine(l_penLinee, objField.GoalLeftRect.Left, objField.GoalLeftRect.Top, objField.GoalLeftRect.Right, objField.GoalLeftRect.Bottom);
            l_objGraphics.DrawLine(l_penLinee, objField.GoalRightRect.Left, objField.GoalRightRect.Top, objField.GoalRightRect.Right, objField.GoalRightRect.Bottom);
            l_penLinee.Color = Color.White;
            l_penLinee.Width = objField.LinesThickness;
            //disegno i dischetti di rigore
            l_objGraphics.FillEllipse(Brushes.White, objField.PenaltySpotLeft.X - objField.PointsThickness / 2, objField.PenaltySpotLeft.Y - objField.PointsThickness / 2, objField.PointsThickness, objField.PointsThickness);
            l_objGraphics.FillEllipse(Brushes.White, objField.PenaltySpotRight.X - objField.PointsThickness / 2, objField.PenaltySpotRight.Y - objField.PointsThickness / 2, objField.PointsThickness, objField.PointsThickness);
            //disegno la riga di metà campo.
            l_objGraphics.DrawLine(l_penLinee, objField.CentreSpot.X, 0, objField.CentreSpot.X, objField.Height);
            //disegno il punto di metà campo
            l_objGraphics.FillEllipse(Brushes.White, objField.CentreSpot.X - objField.PointsThickness / 2, objField.CentreSpot.Y - objField.PointsThickness / 2, objField.PointsThickness, objField.PointsThickness);
            //disegno il cerchio di metà campo
            l_objGraphics.DrawEllipse(l_penLinee, objField.CentreSpot.X - objField.CirclesRadius, objField.CentreSpot.Y - objField.CirclesRadius, objField.CirclesRadius * 2, objField.CirclesRadius * 2);
            //disegno le lunette dell'area.
            //troppo a digiuno in geometria per calcolare l'intersezione del cerchio con l'area di rigore.
            //ho fissato l'angolo di partenza a -53° (e il suo complemento a 180°) e l'ampiezza dell'angolo a 106° :-(
            l_objGraphics.DrawArc(l_penLinee, objField.PenaltySpotLeft.X - objField.CirclesRadius, objField.PenaltySpotLeft.Y - objField.CirclesRadius, objField.CirclesRadius * 2, objField.CirclesRadius * 2, -53, 106);
            l_objGraphics.DrawArc(l_penLinee, objField.PenaltySpotRight.X - objField.CirclesRadius, objField.PenaltySpotRight.Y - objField.CirclesRadius, objField.CirclesRadius * 2, objField.CirclesRadius * 2, 127, 106);
            //se richiesto, disegno le aree di gioco
            if (blShowAreas)
            {
                Pen penAreeGioco = new Pen(Color.YellowGreen);
                for (int i = 0; i < objField.Areas.AreasList.Length; i++)
                {
                    RectangleObject objAreaRectAux = objField.Areas.AreasList[i].AreaRect;
                    l_objGraphics.DrawRectangle(penAreeGioco, new Rectangle(objAreaRectAux.X,objAreaRectAux.Y,objAreaRectAux.Width,objAreaRectAux.Height));
                }
                penAreeGioco.Dispose();
            }
        }



        /// <summary>
        /// Draws the moving player.
        /// </summary>
        /// <param name="objPlayingPlayer">The obj playing player.</param>
        public void DrawMovingPlayer(Mojhy.Engine.PlayingPlayer objPlayingPlayer)
        {
            int intNumPlayer = objPlayingPlayer.Index + 1;
            Point ptCenteredPosition = new Point(objPlayingPlayer.CurrentPositionOnField.X, objPlayingPlayer.CurrentPositionOnField.Y);

            //disegno inattacco
            //calcolo la posizione per centrare l'immagine
            ptCenteredPosition = new Point(ptCenteredPosition.X - 1200, ptCenteredPosition.Y - 1500);

            l_objGraphics.DrawImage(l_imgAttacco, ptCenteredPosition);
            if (intNumPlayer > 9)
            {
                l_objGraphics.DrawString(intNumPlayer.ToString(), new Font("Tahoma", 7), Brushes.White, new Point(objPlayingPlayer.CurrentPositionOnField.X - 900, objPlayingPlayer.CurrentPositionOnField.Y - 600));
            }
            else
            {
                l_objGraphics.DrawString(intNumPlayer.ToString(), new Font("Tahoma", 7), Brushes.White, new Point(objPlayingPlayer.CurrentPositionOnField.X - 500, objPlayingPlayer.CurrentPositionOnField.Y - 600));
            }

        }

        /// <summary>
        /// Draws the ball.
        /// </summary>
        /// <param name="objBall">The ball object.</param>
        public void DrawBall(Mojhy.Engine.Ball objBall)
        {
            if (objBall != null)
            {
                l_objGraphics.DrawImage(l_imgPalloncino, new Point(objBall.PositionOnField.X-500, objBall.PositionOnField.Y-500));
            }
        }

        /// <summary>
        /// Draws the selected play area without drawing the ball in the center
        /// </summary>
        /// <param name="intSelectedAreaIndex">Index of the selected area.</param>
        /// <param name="objField">The referenced field object.</param>
        public void DrawSelectedAreaNoBall(int intSelectedAreaIndex, Mojhy.Engine.Field objField)
        {
            //cerco in quale area mi trovo
            Mojhy.Engine.PlayArea objPlayArea = objField.Areas.AreasList[intSelectedAreaIndex];
            if (objPlayArea != null)
            {
                //disegno l'area selezionata
                SolidBrush colorBrush = new SolidBrush(Color.FromArgb(50, 12, 12, 12));
                l_objGraphics.FillRectangle(colorBrush, new Rectangle(objPlayArea.AreaRect.X,objPlayArea.AreaRect.Y,objPlayArea.AreaRect.Width,objPlayArea.AreaRect.Height));
                colorBrush.Dispose();
            }
        }

    }
}

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
        }
        public void SetGraphicsObject(Graphics g)
        {
            l_objGraphics = g;
            //attivo l'antialias
            l_objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //imposto l'unità di misura
            l_objGraphics.PageUnit = UNIT;
            //imposto la scala, visto che considero il campo con le sue misure reali
            l_objGraphics.PageScale = SCALE;

        }
        /// <summary>
        /// Converts a point pixel location to the point as millimeter.
        /// </summary>
        /// <param name="PixelLoc">The point defined as pixel X and pixel Y</param>
        /// <returns></returns>
        public Point PixelToMM(Point PixelLoc)
        {
            Point ptLocMM = new Point();
            ptLocMM.X = (int)(l_fltDpiX / MM_PER_INCH * PixelLoc.X * (MAGICNUMBERCONVERSION / SCALE));
            ptLocMM.Y = (int)(l_fltDpiY / MM_PER_INCH * PixelLoc.Y * (MAGICNUMBERCONVERSION / SCALE));
            return ptLocMM;
        }
        public Point MMToPixel(Point PixelLoc)
        {
            Point ptLocPixel = new Point();
            ptLocPixel.X = (int)(MM_PER_INCH / l_fltDpiX * PixelLoc.X / (MAGICNUMBERCONVERSION / SCALE));
            ptLocPixel.Y = (int)(MM_PER_INCH / l_fltDpiY * PixelLoc.Y / (MAGICNUMBERCONVERSION / SCALE));
            return ptLocPixel;
        }
        /// <summary>
        /// Draws the selected play area.
        /// </summary>
        /// <param name="BallLoc">The selecte point inside the field</param>
        public void DrawSelectedArea(Point BallLoc, Mojhy.Engine.Field objField)
        {
            BallLoc = this.PixelToMM(BallLoc);
            //cerco in quale area mi trovo
            Mojhy.Engine.PlayArea objPlayArea = objField.Areas.GetAreaFromLoc(BallLoc);
            if (objPlayArea != null)
            {
                //disegno l'area selezionata
                SolidBrush colorBrush = new SolidBrush(Color.FromArgb(50, 12, 12,12));
                l_objGraphics.FillRectangle(colorBrush, objPlayArea.AreaRect);
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
        /// <param name="enPositionType">Type of player position.</param>
        public void DrawPlayer(Mojhy.Engine.PlayingPlayer objPlayingPlayer, Mojhy.Engine.PlayingPlayer.PlayerPositionType enPositionType)
        {
            //calcolo la posizione per centrare l'immagine
            Point ptCenteredPosition = new Point(objPlayingPlayer.PositionOnField.X - 1800, objPlayingPlayer.PositionOnField.Y - 1500);
            l_objGraphics.DrawImage(l_imgDifesa, ptCenteredPosition);
            int intNumPlayer = objPlayingPlayer.Index+1;
            if (intNumPlayer > 9) {
                l_objGraphics.DrawString(intNumPlayer.ToString(), new Font("Tahoma", 7), Brushes.White, new Point(objPlayingPlayer.PositionOnField.X -450, objPlayingPlayer.PositionOnField.Y -600));
            }else{
                l_objGraphics.DrawString(intNumPlayer.ToString(), new Font("Tahoma", 7), Brushes.White, new Point(objPlayingPlayer.PositionOnField.X - 100, objPlayingPlayer.PositionOnField.Y - 600));
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
            l_objGraphics.DrawRectangle(l_penLinee, objField.PenaltyAreaLeft);
            ////seconda
            l_objGraphics.DrawRectangle(l_penLinee, objField.PenaltyAreaRight);            
            //disegno l'area piccola
            ////prima
            l_objGraphics.DrawRectangle(l_penLinee, objField.GoalAreaLeft);
            ////seconda
            l_objGraphics.DrawRectangle(l_penLinee, objField.GoalAreaRight);
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
                    l_objGraphics.DrawRectangle(penAreeGioco, objField.Areas.AreasList[i].AreaRect);
                }
                penAreeGioco.Dispose();
            }
        }
    }
}

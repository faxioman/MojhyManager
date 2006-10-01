/* Ball.cs, FABIO MASINI
 * La classe definisce l'oggetto pallone */

using System;
using System.Collections.Generic;
using System.Text;
using Mojhy.Utils.DrawingExt;
using System.Threading;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines the ball object used on field.
    /// </summary>
    public class Ball
    {
        //oggetto campo ove è collocato il pallone
        private Field l_objField;
        //posizione del pallone in campo
        private Point3D l_ptzBallPosition = new Point3D(0,0,0);

        //posizione che il pallone deve raggiungere
        private Point3D l_ptzBallEndPosition = new Point3D(0, 0, 0);

        //thread AI
        private Thread l_objAIThread;

        //indica la velocità del pallone che deve essere proporzionale alla potenza impressa alla palla
        private Single l_sglVelocity = 100;

        //potenza impressa alla palla, in base alla potenza vengono impostate velocità e distanza da raggiungere
        private Single l_sglShootPower = 30;

        //distanza che deve raggiugnere la palla
        private Single l_sglShootDistance = 500;

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
        }

        /// <summary>
        /// Sets the ball Shoot Power
        /// </summary>
        /// <value>The Shoot Power.</value>
        public Single ShootPower
        {
            set { l_sglShootPower = value; }
        }

        /// <summary>
        /// Sets the Final ball position on field.
        /// </summary>
        /// <value>The position where the ball go.</value>
        public Point3D EndPositionOnField
        {
            set { l_ptzBallEndPosition = value; }
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


        /// <summary>
        /// Enables the movement of ball
        /// </summary>
        public void EnableBall()
        {            
            
            // in base alla forza impressa la pallone
            // stabilisco una velocita e una distanza
            // bisognerebbeun algoritmo per calcolare velocità 
            // e distanza in base alla forza !

            if (l_sglShootPower == 1)
            {
                l_sglVelocity = 10;
                l_sglShootDistance = 500;
            } 
            if (l_sglShootPower == 2)
            {
                l_sglVelocity = 20;
                l_sglShootDistance = 1000;
            }
            if (l_sglShootPower == 3)
            {
                l_sglVelocity = 30;
                l_sglShootDistance = 2000;
            }
            if (l_sglShootPower == 4)
            {
                l_sglVelocity = 40;
                l_sglShootDistance = 5000;
            }
            if (l_sglShootPower == 5)
            {
                l_sglVelocity = 50;
                l_sglShootDistance = 10000;
            }
            if (l_sglShootPower == 6)
            {
                l_sglVelocity = 60;
                l_sglShootDistance = 20000;
            }
            if (l_sglShootPower == 7)
            {
                l_sglVelocity = 70;
                l_sglShootDistance = 30000;
            }
            if (l_sglShootPower == 8)
            {
                l_sglVelocity = 80;
                l_sglShootDistance = 50000;
            }
            if (l_sglShootPower == 9)
            {
                l_sglVelocity = 90;
                l_sglShootDistance = 70000;
            }
            if (l_sglShootPower == 10)
            {
                l_sglVelocity = 100;
                l_sglShootDistance = 100000;
            }
                        
            //creo il nuovo thread di posizionamento del giocatore, che segue il pallone.
            if (l_objAIThread == null)
            {
                l_objAIThread = new Thread(this.TheBrain);
                l_objAIThread.Start();
            }
        }
        /// <summary>
        /// Disables the movement of ball
        /// </summary>
        public void DisableBall()
        {
            //distruggo il thread
            if (l_objAIThread != null)
            {
                l_objAIThread.Abort();
                l_objAIThread = null;
            }
        }

        /// <summary>
        /// It's the ball brain. ??!?!?
        /// </summary>
        private void TheBrain()
        {
            double dblCos = 0;
            double dblSin = 0;
            int intMoveX = 0;
            int intMoveY = 0;
            
            //angolo di spostmento del giocatore
            double dblMoveAngle;

            //parte il ciclo guidato dal Thread
            while (System.Threading.Thread.CurrentThread.ThreadState == ThreadState.Running)
            {
                //durante il ciclo diminuisco la distanza che deve percorrere il pallone
                l_sglShootDistance -= 50;
                if (l_sglShootDistance >= 50)
                {
                    //calcolo l'angolo di spostamento del pallone
                    dblMoveAngle = Mojhy.Utils.Math.Angle(this.PositionOnField.X, this.PositionOnField.Y, l_ptzBallEndPosition.X, l_ptzBallEndPosition.Y);
                    dblCos = Math.Cos(dblMoveAngle);
                    dblSin = Math.Sin(dblMoveAngle);
                    intMoveX = (int)Math.Round(l_sglVelocity * dblCos);
                    intMoveY = (int)Math.Round(l_sglVelocity * dblSin);

                    System.Threading.Thread.Sleep(4); //TODO. SOLO PER TESTING...POI NON SARà IL CASO DI RALLENTARE NULLA!!!!
                    
                    //muovo il pallone nella sua posizione (verifico che non sia già arrivato 'nei pressi')
                    if ((Math.Abs(this.PositionOnField.X - l_ptzBallEndPosition.X) > 500) || (Math.Abs(this.PositionOnField.Y - l_ptzBallEndPosition.Y) > 500))
                    {
                        this.PositionOnField.X += intMoveX;
                        this.PositionOnField.Y -= intMoveY;
                    }
                    else
                    {
                        this.DisableBall();
                    }
                }
                else
                {
                    this.DisableBall();
                }
            }
        }            
    }
}

/* PlayAreas.cs, FABIO MASINI
 * La classe definisce le aree di gioco del campo che il sistema considera.
 * Esistono 20 aree dove il pallone può trovarsi. Per ognuna di queste
 * saranno definite le posizioni dei giocatori sul campo. */

using System;
using System.Collections.Generic;
using System.Text;
using Mojhy.Engine;
using Mojhy.Utils.DrawingExt;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines all the twenty areas of the field
    /// </summary>
    public class PlayAreas
    {
        private PlayArea[] l_arrAreas;
        private Field l_objField;
        /// <summary>
        /// Gets the areas array.
        /// </summary>
        /// <value>The array of PlayArea.</value>
        public PlayArea[] AreasList
        {
            get { return l_arrAreas; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PlayAreas"/> class.
        /// </summary>
        /// <param name="FieldObject">The field object.</param>
        public PlayAreas(Field FieldObject)
        {
            //imposto l'oggetto campo interno
            l_objField = FieldObject;
            //altezza e larghezza dei settori relativi le fasce
            int intAltezzaAreaFasce = ((l_objField.Height - l_objField.GoalWidth) / 2) - l_objField.PenaltyAreaSize;
            int intLarghezzaAreaFasce = l_objField.Width / 4;
            //altezza e larghezza dei settori relativi il centrocampo
            int intAltezzaAreaCentrocampo = (l_objField.GoalWidth + (l_objField.PenaltyAreaSize*2)) / 2;
            int intLarghezzaAreaCentrocampo = (l_objField.Width - (l_objField.PenaltyAreaSize * 2)) / 4;
            //larghezza dei settori dell'area di rigore
            int intLargezzaAreaAreeRigore = l_objField.PenaltyAreaSize;
            //creo l'array di aree
            l_arrAreas = new PlayArea[20];
            for (int i = 0; i < l_arrAreas.Length; i++)
            {
                l_arrAreas[i] = new PlayArea(this);
                l_arrAreas[i].Index = i;
                if (i < 4)
                {
                    //fascia sinistra
                    l_arrAreas[i].AreaRect.X = intLarghezzaAreaFasce * i;
                    l_arrAreas[i].AreaRect.Y = 0;
                    l_arrAreas[i].AreaRect.Width = intLarghezzaAreaFasce;
                    l_arrAreas[i].AreaRect.Height = intAltezzaAreaFasce;
                }
                else if (i == 4)
                {
                    //prima area dell'area di rigore per la prima porta
                    l_arrAreas[i].AreaRect.X = 0;
                    l_arrAreas[i].AreaRect.Y = intAltezzaAreaFasce;
                    l_arrAreas[i].AreaRect.Width = intLargezzaAreaAreeRigore;
                    l_arrAreas[i].AreaRect.Height = intAltezzaAreaCentrocampo;
                }
                else if ((i > 4) && (i < 9))
                {
                    //prima fascia di centrocampo
                    l_arrAreas[i].AreaRect.X = (intLarghezzaAreaCentrocampo * (i - 5)) + intLargezzaAreaAreeRigore;
                    l_arrAreas[i].AreaRect.Y = intAltezzaAreaFasce;
                    l_arrAreas[i].AreaRect.Width = intLarghezzaAreaCentrocampo;
                    l_arrAreas[i].AreaRect.Height = intAltezzaAreaCentrocampo;
                }
                else if (i == 9)
                {
                    //prima area dell'area di rigore per la seconda porta
                    l_arrAreas[i].AreaRect.X = l_objField.Width - intLargezzaAreaAreeRigore;
                    l_arrAreas[i].AreaRect.Y = intAltezzaAreaFasce;
                    l_arrAreas[i].AreaRect.Width = intLargezzaAreaAreeRigore;
                    l_arrAreas[i].AreaRect.Height = intAltezzaAreaCentrocampo;
                }
                else if (i == 10)
                {
                    //seconda area dell'area di rigore per la prima porta
                    l_arrAreas[i].AreaRect.X = 0;
                    l_arrAreas[i].AreaRect.Y = intAltezzaAreaFasce + intAltezzaAreaCentrocampo;
                    l_arrAreas[i].AreaRect.Width = intLargezzaAreaAreeRigore;
                    l_arrAreas[i].AreaRect.Height = intAltezzaAreaCentrocampo;
                }
                else if ((i > 10) && (i < 15))
                {
                    //seconda fascia di centrocampo
                    l_arrAreas[i].AreaRect.X = (intLarghezzaAreaCentrocampo * (i - 11)) + intLargezzaAreaAreeRigore;
                    l_arrAreas[i].AreaRect.Y = intAltezzaAreaFasce + intAltezzaAreaCentrocampo;
                    l_arrAreas[i].AreaRect.Width = intLarghezzaAreaCentrocampo;
                    l_arrAreas[i].AreaRect.Height = intAltezzaAreaCentrocampo;
                }
                else if (i == 15)
                {
                    //prima area dell'area di rigore per la seconda porta
                    l_arrAreas[i].AreaRect.X = l_objField.Width - intLargezzaAreaAreeRigore;
                    l_arrAreas[i].AreaRect.Y = intAltezzaAreaFasce + intAltezzaAreaCentrocampo;
                    l_arrAreas[i].AreaRect.Width = intLargezzaAreaAreeRigore;
                    l_arrAreas[i].AreaRect.Height = intAltezzaAreaCentrocampo;
                }
                else
                {
                    //fascia destra
                    l_arrAreas[i].AreaRect.X = intLarghezzaAreaFasce * (i - 16);
                    l_arrAreas[i].AreaRect.Y = intAltezzaAreaFasce + (intAltezzaAreaCentrocampo * 2);
                    l_arrAreas[i].AreaRect.Width = intLarghezzaAreaFasce;
                    l_arrAreas[i].AreaRect.Height = intAltezzaAreaFasce;
                }
            }
        }
        /// <summary>
        /// Gets the Area identified by the point location.
        /// </summary>
        /// <param name="Loc">The Point of the requested location.</param>
        /// <returns></returns>
        public PlayArea GetAreaFromLoc(PointObject Loc)
        {
            foreach (PlayArea objPlayAreaAux in this.AreasList)
            {
                if (objPlayAreaAux.AreaRect.Contains(Loc))
                {
                    return objPlayAreaAux;
                }
            }
            //non ho trovato nessuna area
            return null;
        }
        /// <summary>
        /// Gets the Area identified by the 3D point location.
        /// </summary>
        /// <param name="Loc">The 3D point of the requested location.</param>
        /// <returns></returns>
        public PlayArea GetAreaFromLoc(Point3D Loc)
        {
            //converto il punto tridimensionale in bidimensionale
            PointObject ptLocAux = new PointObject(Loc.X, Loc.Y);
            return this.GetAreaFromLoc(ptLocAux);
        }
    }
}

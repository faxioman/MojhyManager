/* League.cs, MARCO CECCARELLI
 * La classe definisce le leghe in cui sono presenti i diversi campionati */

namespace Mojhy.Calendar
{
    /// <summary>
    /// Defines all the twenty areas of the field
    /// </summary>
    class League
    {
        private string l_strName;       
        
        /// <summary>
        /// Gets the Name of the League.
        /// </summary>        
        public string Name
        {
            get { return l_strName; }
            set { l_strName = value; }
        }        
        
    }
}

using System;
using System.Diagnostics;
using System.Xml;
using System.Reflection;
using System.Globalization;

namespace Mojhy.Engine.FiniteStateMachine
{
    using System;
    using System.Diagnostics;
    using System.Xml;
    using System.Reflection;
    using System.Globalization;
    using System.Collections;
    struct FsmXmlAlpha
    {
        /// <summary>
        /// 'The state machine executes objects that implements this interface
        /// </summary>
        public interface IFsm
        {
            void Start();
            void Terminate();
            void External();
        }
        public class Fsm
        {
            private XMLStateMachine FsmXml;
            private Type appType;
            private object appObj;
            CultureInfo culture;

            public Fsm(ref object _appObj, string XMLfile)
            {
                FsmXml = new XMLStateMachine();
                FsmXml.StateTable = XMLfile;
                //imposto lo stato di default 'Start'
                FsmXml.CurrentState = "Start";
                culture = CultureInfo.CurrentUICulture;
                //ottengo a quale classe appartiene l'oggetto
                appObj = _appObj;
                appType = appObj.GetType();
            }

            public string NextState()
            {
                //Prima di passare al prossimo stato, verifico eventuali eventi esterni
                ((IFsm)(appObj)).External();
                string[] args = { };
                //Chiamo un metodo della classe 'appType'
                //La classe sarà principalmente proprio il PlayingPlayer
                //Il nome del metodo è in in 'fsm.CurrentState'
                //Lo stato e il metodo DEVONO avere lo stesso nome
                //Dato che viene utilizzata la reflection, va considerata che questa è case sensitive.
                //Lo stato 'Start' cerca quindi un metodo 'Start', e non 'start'
                appType.InvokeMember(FsmXml.CurrentState, System.Reflection.BindingFlags.InvokeMethod, null, appObj, args, culture);
                //prossimo stato
                FsmXml.GetNextState();
                return CurrentState;
            }

            /// <summary>
            /// Gets or sets the current state.
            /// </summary>
            /// <value>The current state</value>
            public string CurrentState
            {
                get
                {
                    return FsmXml.CurrentState;
                }
                set
                {
                    FsmXml.CurrentState = value;
                }
            }
        }
        /// <summary>
        /// This class implements a table-driven finite state machine.
        /// The table is defined by an XML document.
        /// </summary>
        private class XMLStateMachine
        {
            private XmlTextReader m_tableParser;
            public string m_stateCurrent;
            private string m_stateTable;

            public XMLStateMachine()
            {
                CurrentState = string.Empty;
                StateTable = string.Empty;
            }
            /// <summary>
            /// Gets or sets the current state in the table
            /// </summary>
            public string CurrentState
            {
                get
                {
                    return m_stateCurrent;
                }
                set
                {
                    m_stateCurrent = value;
                }
            }
            /// <summary>
            /// Gets or sets the the state table file name.
            /// </summary>
            public string StateTable
            {
                get
                {
                    return m_stateTable;
                }
                set
                {
                    m_stateTable = value;
                }
            }
            /// <summary>
            /// Gets the next valid state given the current state and the supplied input.
            /// </summary>
            /// <param name="inputArg">The input used to trigger a state transition.</param>
            /// <param name="tag">Are we looking for a state or a item</param>
            /// <returns>A string that identifies the next state</returns>
            public string GetNextState()
            {
                string nextState = string.Empty;
                if (CurrentState != string.Empty)
                {
                    try
                    {
                        m_tableParser = new XmlTextReader(StateTable);
                        while (true == m_tableParser.Read())
                        {
                            if (XmlNodeType.Element == m_tableParser.NodeType)
                            {
                                if (true == m_tableParser.HasAttributes)
                                {
                                    string state = m_tableParser.GetAttribute("name");
                                    if (state == CurrentState)
                                    {
                                        while (true == m_tableParser.Read())
                                        {
                                            if (XmlNodeType.Element == m_tableParser.NodeType)
                                            {
                                                //leggo i dati della transazione
                                                if ("transition" == m_tableParser.Name)
                                                {
                                                    if (true == m_tableParser.HasAttributes)
                                                    {
                                                        CurrentState = m_tableParser.GetAttribute("next");
                                                        nextState = CurrentState;
                                                        return nextState;
                                                    }
                                                }
                                                //Si tratta di uno stato senza transazione
                                                //Mi sposto direttamente allo stato successivo
                                                else if ("state" == m_tableParser.Name)
                                                {
                                                    CurrentState = m_tableParser.GetAttribute("name");
                                                    nextState = CurrentState;
                                                    return nextState;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        //distruggo completamente l'oggetto
                        m_tableParser.Close();
                        m_tableParser = null;
                        CurrentState = string.Empty;
                    }
                }
                return nextState;
            }
        }
    }
}

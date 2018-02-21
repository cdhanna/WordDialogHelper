using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dialog.Client
{
    public interface IDialogRuleClient
    {
 
        event EventHandler<OnConnectionEventArgs> OnConnection;
        event EventHandler<OnDisconnectionEventArgs> OnDisconnection;

        event EventHandler<NewRulesEventArgs> OnRulePosted;
        event EventHandler OnRulesCleared;

        event EventHandler<VariablePostedEventArgs> OnVariablePosted;
        event EventHandler OnVariablesCleared;

        event EventHandler OnValuesUpdated;

        VariableCollection Variables { get; }
        List<DialogRule> Rules { get; }
        Dictionary<string, object> Values { get; }

        bool IsConnected { get; }

        void StartConnection(string host="localhost", int port=9090, string protocol="ws");
        void CloseConnection();

       
        void PostVariables(params Variable[] variables);
        void ClearVariables();

        void PostRules(params DialogRule[] rules);
        void ClearRules();

        void PostValues(Dictionary<string, object> values);

    }
    
    public class OnConnectionEventArgs : EventArgs
    {

    }

    public class OnDisconnectionEventArgs : EventArgs
    {

    }

    public class VariablePostedEventArgs : EventArgs
    {
        public Variable[] Variables { get; set; }
        public VariablePostedEventArgs(ICollection<Variable> vars)
        {
            Variables = vars.ToArray();
        }
    }

    public class NewRulesEventArgs : EventArgs
    {
        public DialogRule[] Rules { get; set; } 
        public NewRulesEventArgs(ICollection<DialogRule> rules)
        {
            Rules = rules.ToArray();
        }
    }
   

}

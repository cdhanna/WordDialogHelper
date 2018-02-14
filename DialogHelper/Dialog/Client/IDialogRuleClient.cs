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
        event EventHandler<NewRulesEventArgs> OnNewRules;
        

        void StartConnection(string host="localhost", int port=9090, string protocol="ws");
        void CloseConnection();
        void SendVariables(VariableCollection collection);
        void SendActiveRule(DialogRule activeRule);

    }
    
    public class OnConnectionEventArgs : EventArgs
    {

    }

    public class OnDisconnectionEventArgs : EventArgs
    {

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

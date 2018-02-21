using Dialog;
using Dialog.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.VariableVisual
{
    public class DialogActionPaneViewModel : INotifyPropertyChanged
    {

        public VariableCollection Variables { get; set; } = new VariableCollection();

        public DialogRuleClient Client { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DialogActionPaneViewModel(DialogRuleClient client)
        {
            Client = client;

            client.OnConnection += (s, a) =>
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsClientConnected)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectToggleText)));
            };
            client.OnDisconnection += (s, a) =>
            {

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsClientConnected)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectToggleText)));
            };

        }

        public bool IsClientConnected { get { return Client.IsConnected; } }
        public string ConnectToggleText { get { return IsClientConnected ? "Disconnect" : "Connect"; } }

    }
}

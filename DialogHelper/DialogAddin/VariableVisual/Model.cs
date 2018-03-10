using Dialog;
using Dialog.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DialogAddin.VariableVisual
{
    public class DialogActionPaneViewModel : INotifyPropertyChanged
    {

        public VariableCollection Variables { get; set; } = new VariableCollection();

        public DialogRuleClient Client { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = (s, a) => { };

       // private Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

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

        public string LoadedVariableFile { get; private set; }
        public bool IsClientConnected { get { return Client.IsConnected; } }
        public string ConnectToggleText { get { return IsClientConnected ? "Disconnect" : "Connect"; } }

        public string ManifestName { get { return "Tuna"; } }
        public int ManifestFileCount { get { return 43; } }

        public Config Config { get { return ConfigHelper.Config; } }


        public void LoadVariablesFromFile(string fileName)
        {
            var lines = new List<string>();

            if (File.Exists(fileName) == false)
            {
                return; // nothing to do.
            }

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                while (!sr.EndOfStream)
                    lines.Add(sr.ReadLine());
            }

            Variables.Clear();

            LoadedVariableFile = fileName;
            if (lines.Count > 0)
            {
                var headers = lines.First().ToLower().Split(',');
                for (var i = 1; i < lines.Count; i++)
                {
                    var data = lines[i].ToLower().Split(',');
                    var type = "unknown";
                    var name = "unknown";
                    for (var eIndex = 0; eIndex < data.Length && eIndex < headers.Length; eIndex++)
                    {
                        switch (headers[eIndex])
                        {
                            case "type":
                                type = data[eIndex];
                                break;
                            case "name":
                                name = data[eIndex];
                                break;
                            default:
                                throw new InvalidOperationException("Invalid parse column " + headers[eIndex]);
                        }
                    }
                    Variables.Add(type, name);
                    //VariableGrid.Items.Refresh();
                }
                ConfigHelper.Config.DefaultCSVPath = fileName;

               // _dispatcher.Invoke(() =>
               //{

               //});
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Variables)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LoadedVariableFile)));

            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DialogAddin.VariableVisual.Controls
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        public Main()
        {
            InitializeComponent();
        }

        public DialogActionPaneViewModel Model { get { return (DialogActionPaneViewModel)DataContext; } }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Variable Definition Files (*.csv)|*.csv";
            dialog.Multiselect = false;
            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var fileName = dialog.FileName;
                LoadedFile.Content = fileName;

                var lines = new List<string>();
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                        lines.Add(sr.ReadLine());
                }
                
                Model.Variables.Clear();

                if (lines.Count > 0)
                {
                    var headers = lines.First().ToLower().Split(',');
                    for (var i = 1; i < lines.Count; i++)
                    {
                        var data = lines[i].ToLower().Split(',');
                        var type = "unknown";
                        var name = "unknown";
                        for (var eIndex = 0; eIndex < data.Length && eIndex < headers.Length; eIndex ++)
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
                        Model.Variables.Add(type, name);
                        VariableGrid.Items.Refresh();
                    }
                }

            }
      
        }

        private void ConnectionToggle_Click(object sender, RoutedEventArgs e)
        {
            if (Model.IsClientConnected)
            {
                Model.Client.CloseConnection();
            } else
            {
                Model.Client.StartConnection();
            }
        }
    }
}

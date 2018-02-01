using DialogAddin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new DialogRibbon();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace DialogAddin
{
    [ComVisible(true)]
    public class DialogRibbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;
        private DialogService _srvc;
        SaveFileDialog saveDialog = new SaveFileDialog();

        public DialogRibbon(DialogService srvc)
        {
            _srvc = srvc;
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("DialogAddin.DialogRibbon.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit http://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
            
        }

        public void OnAddRule(Office.IRibbonControl ribbon)
        {
            _srvc.AddEmptyRule();

        }

        public void OnSave(Office.IRibbonControl ribbon)
        {
            //var scanned = _srvc.Scan();
            //var jsonModels = scanned.ToJsonRules();
            //var json = JsonConvert.SerializeObject(jsonModels, Formatting.Indented);

            saveDialog.Filter = "JSON Files (*.json)|*.json";
            saveDialog.RestoreDirectory = true;
            saveDialog.AddExtension = true;

            var result = saveDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _srvc.SaveAsJson(_srvc.ActiveDocument, saveDialog.FileName);
                //File.WriteAllText(saveDialog.FileName, json);
            }
             
        }

        public void OnValidate(Office.IRibbonControl ribbon)
        {
            _srvc.EraseComments();
            _srvc.Validate();
        }

        public void OnAntlrTest(Office.IRibbonControl ribbon)
        {
            _srvc.EraseComments();
            _srvc.ScanAndValidate();
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}

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
            //Globals.ThisAddIn.Application.ActiveDocument.Characters.Last.Select();
            //Globals.ThisAddIn.Application.ActiveDocument.Range()

            //Word.Range currentRange = Globals.ThisAddIn.Application.Selection.Range;
            //currentRange.Text = "Rule:New\n";
            //currentRange.set_Style(Word.WdBuiltinStyle.wdStyleHeading1);

            //var end = currentRange.End ;
            //var nextRange = Globals.ThisAddIn.Application.ActiveDocument.Range(end);
            //nextRange.Text = "\nnext";
            //nextRange.set_Style(Word.WdBuiltinStyle.wdStyleNormal);

        }

        public void OnSave(Office.IRibbonControl ribbon)
        {
            _srvc.ScanDocument();
            

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

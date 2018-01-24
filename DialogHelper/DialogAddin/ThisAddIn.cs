using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;

namespace DialogAddin
{
    public partial class ThisAddIn
    {
        private DialogService _srvc = new DialogService();
        private WordSaveHandler wsh = null;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            // attach the save handler
            wsh = new WordSaveHandler(Application);
            wsh.AfterAutoSaveEvent += Wsh_AfterAutoSaveEvent;
            wsh.AfterSaveEvent += Wsh_AfterAutoSaveEvent;
            wsh.AfterUiSaveEvent += Wsh_AfterAutoSaveEvent;
        }

        private void Wsh_AfterAutoSaveEvent(Word.Document doc, bool isClosed)
        {
            var filename = doc.FullName;
        }

        private void Application_DocumentBeforeSave(Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {
            _srvc.SaveAsJson(Doc);
            
            //throw new NotImplementedException();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }



        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new DialogRibbon(_srvc);
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}

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
        

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.Application.DocumentChange += Application_DocumentChange;
            Application.DocumentOpen += Application_DocumentOpen;


        }

        private void Application_DocumentOpen(Word.Document Doc)
        {
            
        }

        private void Application_DocumentChange()
        {
            var doc = this.Application.ActiveDocument;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new DialogRibbon();
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

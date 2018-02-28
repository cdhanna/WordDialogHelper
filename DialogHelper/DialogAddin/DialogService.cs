using Dialog;
using Dialog.Client;
using DialogAddin.Models;
using DialogAddin.VariableVisual;
using DialogAddin.WordLang;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
namespace DialogAddin
{
    public class DialogService
    {
        public const string SECTION_DISPLAYAS = "DisplayAs";
        public const string SECTION_PRECONDITIONS = "Conditions";
        public const string SECTION_DIALOGS = "Dialogs";
        public const string SECTION_OUTCOMES = "Outcomes";

        public const string SYSTEM_NAME = "DialogToolkit";

        public Word.Document ActiveDocument { get { return Globals.ThisAddIn.Application.ActiveDocument; } }
        public Word.Range EndOfDocument { get { return ActiveDocument.Range(ActiveDocument.Characters.Last.Start); } }

        public Word.Style RuleHeadingStyle { get { return ActiveDocument.Styles[Word.WdBuiltinStyle.wdStyleHeading1]; } }
        public Word.Style RuleSectionStyle { get { return ActiveDocument.Styles[Word.WdBuiltinStyle.wdStyleHeading2]; } }
        public Word.Style RuleSubSectionStyle { get { return ActiveDocument.Styles[Word.WdBuiltinStyle.wdStyleHeading3]; } }
        public Word.Style RuleNormalStyle { get { return ActiveDocument.Styles[Word.WdBuiltinStyle.wdStyleNormal]; } }


        public DialogRuleClient Client { get; set; } = new DialogRuleClient();
        public DialogActionPaneViewModel Model { get; set; }
        public List<Word.Comment> Comments { get; set; } = new List<Word.Comment>();

        //public Word.Range NextRange(Word.Range toFinish=null)
        //{
        //    if (toFinish == null) toFinish = EndOfDocument;
        //    toFinish.Text = toFinish.Text + "\n";
        //    var nextRange = ActiveDocument.Range(toFinish.End - 1);
        //    return nextRange;
        //}


        public DialogService()
        {
            Model = new DialogActionPaneViewModel(Client);
            Model.LoadVariablesFromFile(ConfigHelper.Config.DefaultCSVPath);
            
           

        }



        public void AddEmptyRule()
        {

            var title = ActiveDocument.Paragraphs.Add(EndOfDocument);
            title.Range.Text = "Rule";
            title.set_Style(RuleHeadingStyle);
            title.LeftIndent = -12;
            title.Range.Text += "\n";
            var displayAs = ActiveDocument.Paragraphs.Add(EndOfDocument);
            displayAs.Range.Text = SECTION_DISPLAYAS;
            displayAs.set_Style(RuleSectionStyle);
            displayAs.Range.Text += "\n";

            var preConditions = ActiveDocument.Paragraphs.Add(EndOfDocument);
            preConditions.Range.Text = SECTION_PRECONDITIONS;
            preConditions.set_Style(RuleSectionStyle);
            preConditions.Range.Text += "\n";

            var bullets = ActiveDocument.Paragraphs.Add(EndOfDocument);
            bullets.set_Style(RuleNormalStyle);
            bullets.Range.ListFormat.ApplyBulletDefault();
            bullets.Range.InsertBefore("a is b");
            
            var dialogs = ActiveDocument.Paragraphs.Add(EndOfDocument);
            dialogs.Range.Text = SECTION_DIALOGS;
            dialogs.set_Style(RuleSectionStyle);
            dialogs.Range.Text += "\n";

            var outcomes = ActiveDocument.Paragraphs.Add(EndOfDocument);
            outcomes.Range.Text = SECTION_OUTCOMES;
            outcomes.set_Style(RuleSectionStyle);
            outcomes.Range.Text += "\n";
            var outcomeBullets = ActiveDocument.Paragraphs.Add(EndOfDocument);
            outcomeBullets.set_Style(RuleNormalStyle);
            outcomeBullets.Range.ListFormat.ApplyBulletDefault();
            outcomeBullets.Range.InsertBefore("set a to b");
            

        }


        public string ScanForJson(Word.Document doc=null)
        {
            if (doc == null) doc = ActiveDocument;

            EraseComments(doc);

            var allText = doc.Range().Text;
            var compiler = new WordLangCompiler();
            var compilerResults = compiler.Compile(allText, Model.Variables);

            //var result = new WordLangResults(allText);

            

            var line2RangeStart = new List<int>();
            line2RangeStart.Add(0);
            for (var i = 0; i < allText.Length; i++)
            {
                var c = allText[i];
                if (c == '\r')
                {
                    line2RangeStart.Add(i);
                }
            }

            compilerResults.Errors.ForEach(err =>
            {
                //allLines[err.Line]
                var rangeStart = line2RangeStart[err.Line-1] + err.CharPosition ;
                var rangeEnd = line2RangeStart[err.EndLine - 1] + err.EndCharPosition + 1;

                rangeEnd = Math.Min(rangeEnd, doc.Range().End - 1);
                rangeStart = Math.Min(rangeStart, rangeEnd);



                object message = err.Message;
                var comment = doc.Comments.Add(doc.Range(rangeStart, rangeEnd), ref message);
                comment.Author = SYSTEM_NAME;
                comment.ShowTip = true;

            });

            if (compilerResults.Errors.Count == 0)
            {
                return compilerResults.JSON;
                //var v = new RulesVisitor();
                //var rules = v.VisitProg(result.ProgramContext);
            }
            return null;
           

            //var treeVisitor = new WordLangStringVisitor();
            //var treeText = treeVisitor.Visit(result.ProgramContext);

        }



        //public void SaveAsJson(Word.Document doc=null, string filePath=null)
        //{
        //    var scanned = Scan(doc);
        //    var jsonModels = scanned.ToJsonRules();
        //    var json = JsonConvert.SerializeObject(jsonModels, Formatting.Indented);

        //    if (filePath == null)
        //    {
        //        filePath = doc.FullName.Replace(".docx", ".json");
        //    }

        //    File.WriteAllText(filePath, json);

        //}

        public void EraseComments(Word.Document doc = null)
        {
            if (doc == null)
            {
                doc = ActiveDocument;
            }

            var toDelete = new List<Word.Comment>();
            foreach(Word.Comment comment in doc.Comments)
            {
                if (comment.Author.Equals(SYSTEM_NAME))
                {
                    toDelete.Add(comment);
                }
            }
            toDelete.ForEach(c => c.DeleteRecursively());

        }

       
       
    }
}

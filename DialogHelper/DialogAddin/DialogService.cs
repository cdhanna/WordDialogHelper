using System;
using System.Collections.Generic;
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
        public const string SECTION_PRECONDITIONS = "PreConditions";
        public const string SECTION_DIALOGS = "Dialogs";
        public const string SECTION_OUTCOMES = "Outcomes";


        public Word.Document ActiveDocument { get { return Globals.ThisAddIn.Application.ActiveDocument; } }
        public Word.Range EndOfDocument { get { return ActiveDocument.Range(ActiveDocument.Characters.Last.Start); } }

        public Word.Style RuleHeadingStyle { get { return ActiveDocument.Styles[Word.WdBuiltinStyle.wdStyleHeading1]; } }
        public Word.Style RuleSectionStyle { get { return ActiveDocument.Styles[Word.WdBuiltinStyle.wdStyleHeading2]; } }
        public Word.Style RuleSubSectionStyle { get { return ActiveDocument.Styles[Word.WdBuiltinStyle.wdStyleHeading3]; } }
        public Word.Style RuleNormalStyle { get { return ActiveDocument.Styles[Word.WdBuiltinStyle.wdStyleNormal]; } }

        
        //public Word.Range NextRange(Word.Range toFinish=null)
        //{
        //    if (toFinish == null) toFinish = EndOfDocument;
        //    toFinish.Text = toFinish.Text + "\n";
        //    var nextRange = ActiveDocument.Range(toFinish.End - 1);
        //    return nextRange;
        //}



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
            bullets.Range.InsertBefore("if");
            
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
            outcomeBullets.Range.InsertBefore("Exit()");
            

        }

        public void ScanDocument()
        {
            var paragraphs = ActiveDocument.Paragraphs;

            var enumerator = paragraphs.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current as Word.Paragraph;
                if (current == null)
                {
                    throw new InvalidOperationException("Could not parse bad paragraph");
                }
                var next = new Action(() =>
               {
                   if (!enumerator.MoveNext())
                   {
                       throw new InvalidOperationException("Ran out of content to consume, while epxecting displayAs");
                   }
                   current = enumerator.Current as Word.Paragraph;
                   if (current == null)
                   {
                       throw new InvalidOperationException("Could not parse bad paragraph");
                   }
               });

                var getNormal = new Func<string>(() =>
               {
                   var output = "";
                   next();

                   var b = GetBundle(current);
                   while (b.IsNormal || (!b.IsHeader && !b.IsSection))
                   {
                       output += b.Text;
                       next();
                       b = GetBundle(current);
                   }

                   return output;
               });

                var header = GetBundle(current);
                
                if (string.IsNullOrEmpty(header.Text))
                {
                    continue;
                }

                if (!header.IsHeader)
                {
                    throw new InvalidOperationException("Was expecting a headline");
                }

                next();
                var displayAs = GetBundle(current);
                var displayText = getNormal();

                var preConditions = GetBundle(current);
                var conditions = getNormal();

                var dialogLabel = GetBundle(current);
                var dialogs = getNormal();

                var outcomeLabel = GetBundle(current);
                var outcomes = getNormal();
            }
            

            //var dialog = new SaveFileDialog();
            //dialog.DefaultExt = ".json";
            //dialog.AddExtension = true;
            //dialog.Filter = "JSON Files|*.json";
            //var result = dialog.ShowDialog();

            //if (result == DialogResult.OK)
            //{
            //    var savePath = dialog.FileName;
            //}
        }

        private Bundle GetBundle(Word.Paragraph p)
        {
            var text = p.Range.Text;
            var style = p.Range.get_Style();

            var isHeader = style.NameLocal == RuleHeadingStyle.NameLocal;
            var isNormal = style.NameLocal == RuleNormalStyle.NameLocal;
            var isSubSection = style.NameLocal == RuleSectionStyle.NameLocal;

            return new Bundle()
            {
                Text = text,
                IsHeader = isHeader,
                IsNormal = isNormal,
                IsSection = isSubSection
            };
        }

        struct Bundle
        {
            public string Text;
            public bool IsHeader, IsNormal, IsSection;
        }
    }
}

using DialogAddin.Models;
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

        public List<ScannedRule> Scan(Word.Document doc=null)
        {
            var paragraphcs = (doc != null ? doc : ActiveDocument).Paragraphs;
            var iterator = paragraphcs.GetEnumerator();

            var state = ScanState.IGNORE;
            var allRules = new List<ScannedRule>();
            ScannedRule buildingRule = null;
            ScannedRuleSection buildingSection = null;

            while (iterator.MoveNext())
            {
                var current = iterator.Current as Word.Paragraph;
                var bundle = GetBundle(current);

                var handleRule = new Action(() =>
               {
                    if (bundle.IsHeader)
                   {
                       if (buildingRule != null)
                       {
                           if (buildingSection != null)
                           {
                               buildingRule.Sections.Add(buildingSection);
                           }
                           allRules.Add(buildingRule);
                       }
                       buildingRule = new ScannedRule();
                       buildingRule.Name = bundle.Text.RemoveWordNewLines();
                       state = ScanState.RULE;
                   }
               });

                var handleSection = new Action(() =>
                {
                    if (bundle.IsSection)
                    {
                        if (buildingSection != null && buildingRule != null)
                        {
                            buildingRule.Sections.Add(buildingSection);
                        }
                        buildingSection = new ScannedRuleSection();
                        buildingSection.Name = bundle.Text.RemoveWordNewLines();
                        state = ScanState.SECTION;
                    }
                });

                switch (state)
                {
                    case ScanState.SECTION:

                        if (!bundle.IsSection && !bundle.IsHeader)
                        {
                            if (buildingSection != null)
                            {
                                buildingSection.Content += bundle.Text;
                                

                            }
                        }
                        handleSection();
                        handleRule();
                      
                        break;
                    case ScanState.RULE:
                        handleSection();
                        break;
                    default:
                        handleRule();
                        break;
                }

            }
            if (buildingRule != null)
            {
                if (buildingSection != null)
                {
                    buildingRule.Sections.Add(buildingSection);
                }
                allRules.Add(buildingRule);
            }

            return allRules;
        }

        
        public void SaveAsJson(Word.Document doc=null, string filePath=null)
        {
            var scanned = Scan(doc);
            var jsonModels = scanned.ToJsonRules();
            var json = JsonConvert.SerializeObject(jsonModels, Formatting.Indented);

            if (filePath == null)
            {
                filePath = doc.FullName.Replace(".docx", ".json");
            }

            File.WriteAllText(filePath, json);

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

        enum ScanState {
            IGNORE,
            RULE,
            SECTION
        }
    }
}

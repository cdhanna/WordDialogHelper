using Dialog;
using Dialog.Validation;
using DialogAddin.Models;
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


        public List<Word.Comment> Comments { get; set; } = new List<Word.Comment>();

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
            if (doc == null)
            {
                doc = ActiveDocument;
            }
            var paragraphcs = doc.Paragraphs;
            var iterator = paragraphcs.GetEnumerator();

            var allText = doc.Range().Text;
            var result = new WordLangResults(allText);
            var treeVisitor = new WordLangStringVisitor();
            var treeText = treeVisitor.Visit(result.ProgramContext);


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
                       buildingRule.Start = current.Range.Start;
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
                        buildingSection.Start = current.Range.Start;
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

            allRules.ForEach(r => r.Document = doc);

            return allRules;
        }

        public void ScanAndValidate()
        {
            ActiveDocument.Range(0, 5).Bold = 1;

            var allText = ActiveDocument.Range().Text;
            var result = new WordLangResults(allText);

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

            result.ParserErrors.Errors.ForEach(err =>
            {
                //allLines[err.Line]
                var rangeStart = line2RangeStart[err.Line-1] + err.CharPosition ;

                object message = err.Message;
                var comment = ActiveDocument.Comments.Add(ActiveDocument.Range(rangeStart, rangeStart + 1), ref message);
                comment.Author = SYSTEM_NAME;
                comment.ShowTip = true;

            });

            if (result.ParserErrors.AnyErrors == false)
            {
                var v = new RulesVisitor();
                var rules = v.VisitProg(result.ProgramContext);
            }

           

            //var treeVisitor = new WordLangStringVisitor();
            //var treeText = treeVisitor.Visit(result.ProgramContext);

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

        public void Validate(List<ScannedRule> rules=null)
        {
            if (rules == null)
            {
                rules = Scan();
            }

            var jsonRules = rules.ToJsonRules();
            var id2ScanRule = rules.ToDictionary(rule => rule.Id);
            var id2JsonRule = jsonRules.ToDictionary(rule => rule.Id);

            var validator = new RuleValidator();
            var result = validator.ValidateRules(jsonRules);

            AddValidationComments(id2ScanRule, id2JsonRule, result.NameErrors, "");
            AddValidationComments(id2ScanRule, id2JsonRule, result.DialogErrors, SECTION_DIALOGS);
            AddValidationComments(id2ScanRule, id2JsonRule, result.DisplayAsErrors, SECTION_DISPLAYAS);

        }

        private void AddValidationComments(Dictionary<Guid, ScannedRule> id2ScanRule, Dictionary<Guid, JsonRule> id2JsonRule, List<ValidationError> errors, string sectionTitle)
        {
            var noSection = new ScannedRuleSection()
            {
                Content = "",
                Name = "",
                Start = 0
            };

            foreach (var error in errors)
            {
                var scanRule = id2ScanRule[error.RuleId];
                var jsonRule = id2JsonRule[error.RuleId];
                var scanSection = scanRule.Sections.FirstOrDefault(s => s.Name.Equals(sectionTitle));
                if (scanSection == null)
                {
                    scanSection = noSection;
                }


                var rangeStart = scanSection.Start + error.Offset;

                if (error.Offset < 0)
                {
                    rangeStart = scanSection.Start + Math.Abs(error.Offset);
                } else
                {
                    rangeStart = scanSection.Start + error.Offset + sectionTitle.Length; // add one for the new line
                }
                //rangeStart += 1;

                var rangeEnd = rangeStart + error.Length;
                var errorRange = scanRule.Document.Range(rangeStart, rangeEnd);

                //var x = new wordLangVisitor

                object message = error.Message;
                var comment = scanRule.Document.Comments.Add(errorRange, ref message);
                comment.Author = SYSTEM_NAME;
                comment.ShowTip = true;
            }
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

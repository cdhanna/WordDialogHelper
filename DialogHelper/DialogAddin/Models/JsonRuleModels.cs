using Dialog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.Models
{
    

    public static class ScanConverterExtensions
    {
        public static string GetContentFromSection(this ScannedRule rule, string sectionTitle)
        {
            var section = rule.Sections.FirstOrDefault(s => s.Name.Equals(sectionTitle));
            if (section == null)
            {
                return "__notfound__";
            }
            else return section.Content == null ? "" : section.Content;
        }
        

        //public static DialogRule ToJsonRule(this ScannedRule rule)
        //{

        //    /*
        //     * :name:
        //     * content
        //     * 
        //     * :othername:
        //     * more content
        //     * 
        //     * 
        //     */

        //    var rawDialog = rule.GetContentFromSection(DialogService.SECTION_DIALOGS);
        //    if (rawDialog == null) rawDialog = "";

        //    var rawDialogLines = rawDialog.Split('\r');
        //    string speaker = null;
        //    var speakerLines = "";
        //    var dialogParts = new List<DialogRule.DialogPart>();
        //    for(var i = 0; i < rawDialogLines.Length; i++)
        //    {
        //        var line = rawDialogLines[i];
        //        if (line.StartsWith(":"))
        //        {
        //            if (speaker != null)
        //            {
        //                dialogParts.Add(new DialogRule.DialogPart() {
        //                    Id = Guid.NewGuid(),
        //                    Speaker = speaker,
        //                    Content = speakerLines
        //                });
        //            }


        //            speaker = line;
        //            speakerLines = "";
        //        } else
        //        {
        //            speakerLines += line + '\n';
        //        }
        //    }
        //    if (speaker != null)
        //    {
        //        dialogParts.Add(new DialogRule.DialogPart()
        //        {
        //            Id = Guid.NewGuid(),
        //            Speaker = speaker,
        //            Content = speakerLines
        //        });
        //    }

        //    var output = new DialogRule()
        //    {
        //        Id = rule.Id,
        //        Name = rule.Name,
        //        DisplayAs = rule.GetContentFromSection(DialogService.SECTION_DISPLAYAS).Replace('\r', '\n'),
        //        Conditions = rule.GetContentFromSection(DialogService.SECTION_PRECONDITIONS).Replace('\r', '\n').Split('\n'),
        //        Outcomes = rule.GetContentFromSection(DialogService.SECTION_OUTCOMES).Replace('\r', '\n').Split('\n'),
        //        Dialog = dialogParts.ToArray()
        //    };
        //    return output;
        //}

        //public static List<DialogRule> ToJsonRules(this List<ScannedRule> rules)
        //{
        //    return rules.Select(r => r.ToJsonRule()).ToList();
        //}

    }
}

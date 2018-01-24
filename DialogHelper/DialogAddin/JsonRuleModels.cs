using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin
{
    public class DialogRule
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayAs")]
        public string DisplayAs { get; set; }

        [JsonProperty("conditions")]
        public string[] Conditions { get; set; }

        [JsonProperty("outcomes")]
        public string[] Outcomes { get; set; }

        [JsonProperty("dialog")]
        public DialogPart[] Dialog { get; set; }

        public class DialogPart
        {
            [JsonProperty("speaker")]
            public string Speaker { get; set; }

            [JsonProperty("text")]
            public string Content { get; set; }
        }
    }

    public static class ScanConverterExtensions
    {
        public static string GetContentFromSection(this ScannedRule rule, string sectionTitle)
        {
            var section = rule.Sections.FirstOrDefault(s => s.Name.Equals(sectionTitle));
            if (section == null)
            {
                return "__notfound__";
            }
            else return section.Content;
        }

        public static DialogRule ToJsonRule(this ScannedRule rule)
        {
            var output = new DialogRule()
            {
                Name = rule.Name,
                DisplayAs = rule.GetContentFromSection(DialogService.SECTION_DISPLAYAS),
                Conditions = rule.GetContentFromSection(DialogService.SECTION_PRECONDITIONS).Split('\r'),
                Outcomes = rule.GetContentFromSection(DialogService.SECTION_OUTCOMES).Split('\r'),
                Dialog = new DialogRule.DialogPart[]{
                     new DialogRule.DialogPart() {
                        Content = rule.GetContentFromSection(DialogService.SECTION_DIALOGS),
                        Speaker = "Universe"
                     }
                }
            };
            return output;
        }
    }
}

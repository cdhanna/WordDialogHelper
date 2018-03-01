using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dialog
{
    public class DialogRule
    {
        //[JsonProperty("guid")]
        //public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayAs")]
        public string DisplayAs { get; set; }

        [JsonProperty("conditions")]
        public DialogCondition[] Conditions { get; set; }

        [JsonProperty("dialog")]
        public DialogPart[] Dialog { get; set; }

        [JsonProperty("outcomes")]
        public DialogOutcome[] Outcomes { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as DialogRule);
        }

        public bool Equals(DialogRule other)
        {
            if (other == null) return false;
            return other.Name.Equals(Name)
                && other.DisplayAs.Equals(DisplayAs)
                && (other.Conditions != null && other.Conditions.SequenceEqual(Conditions))
                && (other.Outcomes != null && other.Outcomes.SequenceEqual(Outcomes))
                && (other.Dialog != null && other.Dialog.SequenceEqual(Dialog));
        }

        public class DialogPart
        {
            //[JsonProperty("guid")]
            //public Guid Id { get; set; }

            [JsonProperty("speaker")]
            public string Speaker { get; set; }

            [JsonProperty("content")]
            public string Content { get; set; }

            [JsonProperty("parts")]
            public string[] ContentParts { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as DialogPart);
            }

            public bool Equals(DialogPart other)
            {
                if (other == null) return false;
                return other.Speaker.Equals(Speaker)
                    && other.Content.Equals(Content)
                    && other.ContentParts.SequenceEqual(ContentParts);
            }
        }

        public class DialogCondition
        {
            [JsonProperty("op")]
            public string Op { get; set; }
            [JsonProperty("left")]
            public string Left { get; set; }

            [JsonProperty("right")]
            public string Right { get; set; }
            public override bool Equals(object obj)
            {
                return Equals(obj as DialogCondition);
            }

            public bool Equals(DialogCondition other)
            {
                if (other == null) return false;
                return other.Op.Equals(Op)
                    && other.Left.Equals(Left)
                    && other.Right.Equals(Right);
            }
        }

        public class DialogOutcome
        {
            [JsonProperty("command")]
            public string Command { get; set; }

            [JsonProperty("target")]
            public string Target { get; set; }

            [JsonProperty("arguments")]
            public Dictionary<string, string> Arguments { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as DialogOutcome);
            }

            public bool Equals(DialogOutcome other)
            {
                if (other == null) return false;
                return other.Command.Equals(Command)
                    && other.Target.Equals(Target)
                    && other.Arguments.SequenceEqual(Arguments);
            }
        }
    }
}

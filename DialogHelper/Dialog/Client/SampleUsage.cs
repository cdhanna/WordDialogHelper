using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dialog.Client
{
    class SampleUsage
    {

        void Test()
        {

            IDialogRuleClient client = null;

            client.StartConnection();

            client.OnNewRules += Client_OnNewRules;


        }

        private void Client_OnNewRules(object sender, NewRulesEventArgs e)
        {



        }
    }
}

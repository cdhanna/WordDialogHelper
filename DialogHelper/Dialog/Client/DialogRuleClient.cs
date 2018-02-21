using Dialog.Client.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Dialog.Client
{
    public class DialogRuleClient : IDialogRuleClient
    {
        public VariableCollection Variables { get; private set; }

        public List<DialogRule> Rules { get; private set; }

        public Dictionary<string, object> Values { get; private set; }

        public event EventHandler<OnConnectionEventArgs> OnConnection = (s, a) => { };
        public event EventHandler<OnDisconnectionEventArgs> OnDisconnection = (s, a) => { };
        public event EventHandler<NewRulesEventArgs> OnRulePosted = (s, a) => { };
        public event EventHandler OnRulesCleared = (s, a) => { };
        public event EventHandler<VariablePostedEventArgs> OnVariablePosted = (s, a) => { };
        public event EventHandler OnVariablesCleared = (s, a) => { };
        public event EventHandler OnValuesUpdated = (s, a) => { };

        public bool IsConnected { get { return _ws != null && _ws.IsAlive; } }

        private WebSocket _ws;


        public DialogRuleClient()
        {

        }

        public void ClearRules()
        {
            Send(new ClearRulesMessage());
        }

        public void ClearVariables()
        {
            Send(new ClearVariablesMessage());
        }

        public void PostRules(params DialogRule[] rules)
        {
            Send(new PostRuleMessage() { Rules = rules });
        }

        public void PostValues(Dictionary<string, object> values)
        {
            Send(new PostValueMessage() { Values = values });
        }
        
        public void PostVariables(params Variable[] variables)
        {
            Send(new PostVariableMessage() { Variables = variables });
        }


        public void CloseConnection()
        {
            _ws.Close(CloseStatusCode.Normal, "OK");
        }
        public void StartConnection(string host = "localhost", int port = 9090, string protocol = "ws")
        {
            if (_ws != null)
            {
                throw new InvalidOperationException("Socket already exists!");
            }

            _ws = new WebSocket($"{protocol}://{host}:{port}/dialog");

            _ws.OnOpen += _ws_OnOpen;
            _ws.OnClose += _ws_OnClose;

            _ws.OnError += _ws_OnError;
            _ws.OnMessage += _ws_OnMessage;

            _ws.Connect();
        }

        private void _wsGotClearVariables()
        {
            Variables.Clear();
            OnVariablesCleared(this, new EventArgs());
        }
        private void _wsGotClearRules()
        {
            Rules.Clear();
            OnRulesCleared(this, new EventArgs());
        }
        private void _wsGotPostVariables(PostVariableMessage msg)
        {
            for (var i = 0; i < msg.Variables.Length; i++)
            {
                Variables.Add(msg.Variables[i]);
            }
            OnVariablePosted(this, new VariablePostedEventArgs(msg.Variables));
        }
        private void _wsGotPostValues(PostValueMessage msg)
        {
            foreach (var kv in msg.Values)
            {
                if (Values.ContainsKey(kv.Key))
                {
                    Values[kv.Key] = kv.Value;
                } else
                {
                    Values.Add(kv.Key, kv.Value);
                }
            }
            OnValuesUpdated(this, new EventArgs());
        }
        private void _wsGotPostRules(PostRuleMessage msg)
        {
            for (var i = 0; i < msg.Rules.Length; i++)
            {
                Rules.Add(msg.Rules[i]);
            }
            OnRulePosted(this, new NewRulesEventArgs(msg.Rules));
        }

        private void Send(DialogRuleClientMessage msg)
        {
            if (_ws == null || !_ws.IsAlive)
            {
                throw new Exception("not connected");
            }

            var json = JsonConvert.SerializeObject(msg);
            _ws.Send(json);
        }

        private void _ws_OnMessage(object sender, MessageEventArgs e)
        {
            if (e == null) return;
            if (e.Data == null) return;

            var parsed = JsonConvert.DeserializeObject<DialogRuleClientMessage>(e.Data);

            switch (parsed.Type) {

                case ClearVariablesMessage.TYPE:
                    _wsGotClearVariables();
                    break;
                case ClearRulesMessage.TYPE:
                    _wsGotClearRules();
                    break;
                case PostVariableMessage.TYPE:
                    _wsGotPostVariables(JsonConvert.DeserializeObject<PostVariableMessage>(e.Data));
                    break;
                case PostValueMessage.TYPE:
                    _wsGotPostValues(JsonConvert.DeserializeObject<PostValueMessage>(e.Data));
                    break;
                case PostRuleMessage.TYPE:
                    _wsGotPostRules(JsonConvert.DeserializeObject<PostRuleMessage>(e.Data));
                    break;

                default:
                    throw new NotImplementedException("Unknown message type");

            }


        }

        private void _ws_OnError(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _ws_OnOpen(object sender, EventArgs e)
        {
            OnConnection(this, new OnConnectionEventArgs());
        }
        private void _ws_OnClose(object sender, CloseEventArgs e)
        {
            _ws = null;
            OnDisconnection(this, new OnDisconnectionEventArgs());
        }



    }
}

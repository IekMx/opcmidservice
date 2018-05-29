using Newtonsoft.Json;
using opclibrary.Mappings;
using opclibrary.Services;
using opclibrary.Services.Impl;
using OpcSocket.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace OpcSocket
{
    public class WebSocketsServer : IWebSocketsServer
    {
        private AbstractOpcManager _gaugeXlManager = Module1.GaugeXlManager;
        private AbstractOpcManager _festoManager = Module1.FestoManager;
        private List<OperationContext> _operationContexts = new List<OperationContext>();

        public WebSocketsServer()
        {
            var opcx = OperationContext.Current;
            if (!_operationContexts.Contains(opcx))
            {
                _operationContexts.Add(opcx);
                _festoManager.Config.ClientTags.ForEach(x =>
                {
                    ReportProgress(new
                    {
                        server = _festoManager.ServerName,
                        handle = x.Handle,
                        value = x.Value,
                        name = x.Name
                    });
                });
            }
            _gaugeXlManager.DataReceived += _opcManager_DataReceived;
            _festoManager.DataReceived += _opcManager_DataReceived;
        }

        private void _opcManager_DataReceived(object sender, opclibrary.Mappings.OpcEventArgs e)
        {
            ReportProgress(new
            {
                server = ((AbstractOpcManager)sender).ServerName,
                handle = e.ItemHandle,
                value = e.ItemValue?.ToString(),
                name = ((AbstractOpcManager)sender).Config.ClientTags.FirstOrDefault(y => y.Handle == e.ItemHandle).Name
            });
        }

        public void MessageReceived(Message msg)
        {
            if (msg.IsEmpty) return;
            var callback = OperationContext.Current.GetCallbackChannel<IProgressContext>();
            try
            {
                var bytes = msg.GetBody<byte[]>();
                var tag = JsonConvert.DeserializeObject<OpcTag>(Encoding.ASCII.GetString(bytes));
                var gaugeTag = _gaugeXlManager.Config.ClientTags.FirstOrDefault(x => x.Name == tag.Name);
                var festoTag = _festoManager.Config.ClientTags.FirstOrDefault(x => x.Name == tag.Name);

                _festoManager.Config.ClientTags.Where(x => x.Name == tag.Name).FirstOrDefault().Value = tag.Value;
                _festoManager.Write(tag.Handle);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERR-IEK151: " + e.Message);
                ReportProgress(new
                {
                    error = "IEK151",
                    message = e.Message
                });
            }
        }

        private Message CreateMessage(string msgText)
        {
            Message msg = ByteStreamMessage.CreateMessage(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(msgText)));

            msg.Properties["WebSocketMessageProperty"] = new WebSocketMessageProperty
            {
                MessageType = WebSocketMessageType.Text
            };

            return msg;
        }

        private void ReportProgress(dynamic obj)
        {
            _operationContexts.ForEach(x =>
            {
                var callback = x.GetCallbackChannel<IProgressContext>();
                if (((IChannel)callback).State != CommunicationState.Opened) { return; }
                callback.ReportProgress(CreateMessage(JsonConvert.SerializeObject(obj)));
            });
        }
    }

}

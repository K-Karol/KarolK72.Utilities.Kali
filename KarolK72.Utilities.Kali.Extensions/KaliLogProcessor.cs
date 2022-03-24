using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarolK72.Utilities.Kali.Client.Library;
using KarolK72.Utilities.Kali.Proto;

namespace KarolK72.Utilities.Kali.Extensions
{
    internal class KaliLogProcessor : IDisposable
    {
        private const int _maxQueuedMessages = 1024;

        private readonly BlockingCollection<KaliLog> _messageQueue = new BlockingCollection<KaliLog>(_maxQueuedMessages);
        private readonly Thread _outputThread;
        private readonly KaliClientService _client;

        public KaliLogProcessor(KaliClientService client)
        {
            _client = client;
            _outputThread = new Thread(ProcessLogQueue)
            {
                IsBackground = true,
                Name = "Console logger queue processing thread"
            };
            _outputThread.Start();
        }

        public void EnqueueMessage(KaliLog message)
        {
            if (!_messageQueue.IsAddingCompleted)
            {
                try
                {
                    _messageQueue.Add(message);
                    return;
                }
                catch (InvalidOperationException) { }
            }

            // Adding is completed so just log the message
            try
            {
                SendLog(message);
            }
            catch (Exception) { }
        }

        private void ProcessLogQueue()
        {
            try
            {
                foreach (KaliLog message in _messageQueue.GetConsumingEnumerable())
                {
                    SendLog(message);
                }
            }
            catch
            {
                try
                {
                    _messageQueue.CompleteAdding();
                }
                catch { }
            }
        }

        internal virtual void SendLog(KaliLog entry)
        {
            _client.Log(new LogRequest()
            {
                Category = entry.Category,
                EventId = entry.EventId.Id,
                EventName = entry.EventId.Name ?? "",
                Exception = Newtonsoft.Json.JsonConvert.SerializeObject(entry.Exception),
                //Exception = System.Text.Json.JsonSerializer.Serialize(entry.Exception),
                LogLevel = (int)entry.LogLevel,
                RenderedMessage = entry.RenderedMessage,
                Scopes = Newtonsoft.Json.JsonConvert.SerializeObject(entry.Scopes)
                //Scopes = System.Text.Json.JsonSerializer.Serialize(entry.Scopes)
            });
        }


        public void Dispose()
        {
            _messageQueue.CompleteAdding();

            try
            {
                _outputThread.Join(1500);
            }
            catch (ThreadStateException) { }

            foreach (KaliLog message in _messageQueue.GetConsumingEnumerable())
            {
                SendLog(message);
            }

        }

    }
}

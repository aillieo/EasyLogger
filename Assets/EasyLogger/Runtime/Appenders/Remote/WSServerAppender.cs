using System;
using System.Net.WebSockets;
using UnityEngine;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AillieoUtils.EasyLogger
{
    public class WSServerAppender : IAppender
    {
        public event Action<byte[]> OnReceive;

        public IFormatter formatter { get; set; }

        private ClientWebSocket clientWebSocket;
        private Uri uri;

        public WSServerAppender(Uri uri)
        {
            ApplicationEvents.onApplicationQuit += OnApplicationQuit;
            this.uri = uri;
            this.ConnectAndListen();
        }

        public void OnReceiveLogItem(ref LogItem logItem)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(logItem.message);
            this.SendAsync(bytes);
        }

        private void OnApplicationQuit()
        {
            if (this.clientWebSocket != null)
            {
                this.clientWebSocket.Dispose();
                this.clientWebSocket = null;
            }
        }

        private async void ConnectAndListen()
        {
            clientWebSocket = new ClientWebSocket();
            try
            {
                clientWebSocket
                    .ConnectAsync(this.uri, CancellationToken.None)
                    .Wait();
            }
            catch (AggregateException ae) when (ae.InnerException is WebSocketException webSocketException)
            {
                Debug.Log(webSocketException.Message);
                await CloseAsync();
                await Task.Delay(TimeSpan.FromSeconds(60));
                ConnectAndListen();
                return;
            }

            var buffer = new byte[1024];
            while (true)
            {
                try
                {
                    var asBuffer = new ArraySegment<byte>(buffer);
                    var result = await clientWebSocket.ReceiveAsync(asBuffer, CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        byte[] resultBuffer = new byte[result.Count];
                        Array.Copy(buffer, 0, resultBuffer, 0, result.Count);
                        OnReceive?.Invoke(resultBuffer);
                    }
                }
                catch (ObjectDisposedException objectDisposedException)
                {
                }
                catch (OperationCanceledException operationCanceledException)
                {
                }
                catch (WebSocketException webSocketException)
                {
                    await CloseAsync();
                    await Task.Delay(1000);
                    ConnectAndListen();
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    return;
                }
            }
        }

        private async void SendAsync(byte[] bytes)
        {
            if (clientWebSocket == null)
            {
                return;
            }

            if (clientWebSocket.State != WebSocketState.Open)
            {
                return;
            }

            try
            {
                clientWebSocket.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    endOfMessage: true,
                    CancellationToken.None)
                    .Wait();
            }
            catch (ObjectDisposedException objectDisposedException)
            {
            }
            catch (OperationCanceledException operationCanceledException)
            {
            }
            catch (WebSocketException webSocketException)
            {
                await CloseAsync();
                await Task.Delay(1000);
                ConnectAndListen();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task CloseAsync()
        {
            if (clientWebSocket == null || clientWebSocket.State != WebSocketState.Open)
            {
                return;
            }

            try
            {
                var task = clientWebSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    string.Empty,
                    CancellationToken.None);
                task.Wait();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            clientWebSocket.Dispose();
        }
    }
}

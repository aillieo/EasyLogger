// -----------------------------------------------------------------------
// <copyright file="WSServerAppender.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public class WSServerAppender : IAppender
    {
        public event Action<byte[]> OnReceive;

        public IFormatter formatter { get; set; }

        private ClientWebSocket clientWebSocket;
        private Uri uri;

        public WSServerAppender(Uri uri)
        {
            ApplicationEvents.onApplicationQuit += this.OnApplicationQuit;
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
            this.clientWebSocket = new ClientWebSocket();
            try
            {
                this.clientWebSocket
                    .ConnectAsync(this.uri, CancellationToken.None)
                    .Wait();
            }
            catch (AggregateException ae) when (ae.InnerException is WebSocketException webSocketException)
            {
                Debug.Log(webSocketException.Message);
                await this.CloseAsync();
                await Task.Delay(TimeSpan.FromSeconds(60));
                this.ConnectAndListen();
                return;
            }

            var buffer = new byte[1024];
            while (true)
            {
                try
                {
                    var asBuffer = new ArraySegment<byte>(buffer);
                    var result = await this.clientWebSocket.ReceiveAsync(asBuffer, CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        byte[] resultBuffer = new byte[result.Count];
                        Array.Copy(buffer, 0, resultBuffer, 0, result.Count);
                        this.OnReceive?.Invoke(resultBuffer);
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
                    await this.CloseAsync();
                    await Task.Delay(1000);
                    this.ConnectAndListen();
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
            if (this.clientWebSocket == null)
            {
                return;
            }

            if (this.clientWebSocket.State != WebSocketState.Open)
            {
                return;
            }

            try
            {
                this.clientWebSocket.SendAsync(
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
                await this.CloseAsync();
                await Task.Delay(1000);
                this.ConnectAndListen();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task CloseAsync()
        {
            if (this.clientWebSocket == null || this.clientWebSocket.State != WebSocketState.Open)
            {
                return;
            }

            try
            {
                await this.clientWebSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    string.Empty,
                    CancellationToken.None);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            this.clientWebSocket.Dispose();
        }
    }
}

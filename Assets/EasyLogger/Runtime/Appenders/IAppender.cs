namespace AillieoUtils.EasyLogger
{
    public interface IAppender
    {
        void OnReceiveLogItem(ref LogItem logItem);
    }
}

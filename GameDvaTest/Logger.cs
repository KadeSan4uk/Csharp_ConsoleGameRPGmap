public class Logger
{
    private const int MaxLogElements = 10;
    private Queue<string> _queue;
    private Action<string, int> _printAction; 

    public Logger(Action<string, int> printAction)
    {
        _queue = new Queue<string>();
        _printAction = printAction;
    }

    public void AddLog(string log)
    {
        _queue.Enqueue(log);

        while (_queue.Count > MaxLogElements)
            _queue.Dequeue();

        ShowLog();
    }

    public void ShowLog()
    {
        int line = 0; 
        foreach (var str in _queue)
        {
            _printAction?.Invoke(str, line++);
        }
    }

    public void Clear()
    {
        _queue.Clear();
        ShowLog();
    }
}

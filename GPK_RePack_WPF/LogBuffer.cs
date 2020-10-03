using System;
using System.Collections.Concurrent;
using System.Windows.Threading;

namespace GPK_RePack_WPF
{
    /// <summary>
    /// Used to avoid spamming UI with log messages updates.
    /// Fires an update every 100ms and merges queued lines in a single message.
    /// </summary>
    public class LogBuffer
    {
        private readonly ConcurrentQueue<string> _buffer;
        private readonly DispatcherTimer _timer;

        public event Action<string> LinesFlushed;

        public LogBuffer()
        {
            _buffer = new ConcurrentQueue<string>();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += OnTick;
        }

        public void AppendLine(string line)
        {
            _buffer.Enqueue(line);
            if (!_timer.IsEnabled) _timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (_buffer.Count == 0)
            {
                _timer.Stop();
                return;
            }

            var lines = "";

            while (_buffer.TryDequeue(out var line))
            {
                lines += line + Environment.NewLine;
            }

            _timer.Stop();
            LinesFlushed?.Invoke(lines);


        }
    }
}
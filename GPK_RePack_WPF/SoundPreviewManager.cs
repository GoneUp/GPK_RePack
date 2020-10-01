using NAudio.Vorbis;
using NAudio.Wave;
using Nostrum;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Nostrum.Extensions;
using WaveFormRendererLib;
using Image = System.Drawing.Image;

namespace GPK_RePack_WPF
{
    public class SoundPreviewManager : TSPropertyChanged, IDisposable
    {
        private VorbisWaveReader _waveReader;
        private readonly WaveOut _waveOut;
        private readonly Timer _timer;
        private readonly WaveFormRenderer _renderer;
        private Image _bmp;
        private ImageSource _waveForm;
        private DateTime _startTime;

        public ImageSource WaveForm
        {
            get => _waveForm;
            set
            {
                if (_waveForm == value) return;
                _waveForm = value;
                N();
            }
        }

        public double CurrentPosition
        {
            get
            {
                if (_waveReader == null) return 0;
                //if (_waveReader != null)
                //    return _waveReader.CurrentTime.TotalMilliseconds / _waveReader.TotalTime.TotalMilliseconds;
                return (DateTime.Now - _startTime).TotalMilliseconds * 1000 / _waveReader.TotalTime.TotalMilliseconds;
                //return 0;
            }
            set
            {
                if (_waveReader == null) return;
                var totalMs = _waveReader.TotalTime.TotalMilliseconds;
                var setMs = (value / 1000D) * totalMs;
                if (setMs < totalMs)
                {
                    var wasPlaying = _waveOut.PlaybackState == PlaybackState.Playing;
                    _waveOut.Pause();
                    _waveReader.CurrentTime = TimeSpan.FromMilliseconds(setMs);
                    if(wasPlaying) _waveOut.Play();
                }
                else
                {
                    _waveOut.Stop();
                }
                N();
                N(nameof(CurrentTime));
                _startTime = DateTime.Now - _waveReader.CurrentTime;
            }
        }

        public string CurrentTime
        {
            get
            {
                if (_waveReader == null) return TimeSpan.Zero.ToString(@"mm\:ss\.ff");
                return (DateTime.Now - _startTime).ToString(@"mm\:ss\.ff");
            }
        }

        public PlaybackState PlaybackState => _waveOut.PlaybackState;

        public SoundPreviewManager()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
            _waveOut = new WaveOut();
            _waveOut.PlaybackStopped += OnPlaybackStopped;
            _timer = new Timer { Interval = 10 };
            _timer.Tick += OnTimerTick;
            //https://github.com/naudio/NAudio.WaveFormRenderer
            _renderer = new WaveFormRenderer();

        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                N(nameof(CurrentPosition));
                N(nameof(CurrentTime));

            }, DispatcherPriority.DataBind);
            if ((DateTime.Now - _startTime).TotalMilliseconds >= _waveReader.TotalTime.TotalMilliseconds) _waveOut.Stop();
            if (PlaybackState == PlaybackState.Playing) return;
            _timer.Stop();

        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            _timer.Stop();
            _startTime = DateTime.Now;
            N(nameof(CurrentPosition));
            N(nameof(CurrentTime));

            //ResetOggPreview();
        }

        public void Setup(byte[] soundwave)
        {
            ResetOggPreview();
            _waveReader = new VorbisWaveReader(new MemoryStream(soundwave));
            RenderWaveForm();
            _waveOut.Init(_waveReader);
        }

        private void RenderWaveForm()
        {
            Task.Run(() =>
            {
                var col = ((System.Windows.Media.Color)App.Current.FindResource("SelectionColor")).ToDrawingColor();
                var rendererSettings = new StandardWaveFormRendererSettings
                {
                    Width = 1200,
                    TopHeight = 128,
                    BottomHeight = 128,
                    BackgroundColor = System.Drawing.Color.Transparent,
                    TopPeakPen = new System.Drawing.Pen(col),
                    BottomPeakPen = new System.Drawing.Pen(col)
                };
                var maxPeakProvider = new RmsPeakProvider(200);
                this._bmp = _renderer.Render(_waveReader, maxPeakProvider, rendererSettings);

                Dispatcher.InvokeAsync(() =>
                {
                    using (var ms = new MemoryStream())
                    {
                        _bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;

                        var bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.StreamSource = ms;
                        bi.EndInit();
                        WaveForm = bi;
                    }
                    _waveReader.Position = 0;
                });

            });
        }

        public void PlaySound()
        {
            if (PlaybackState == PlaybackState.Paused)
            {
                _startTime = DateTime.Now - _waveReader.CurrentTime;
                _waveOut.Resume();
            }
            else
            {
                _startTime = DateTime.Now;
                _waveReader.Position = 0;
                _waveOut.Play();
            }
            _timer.Start();
            //OggPreviewButtonText = "Stop Preview";
        }

        public void ResetOggPreview()
        {
            if (_waveOut != null && _waveOut.PlaybackState == PlaybackState.Playing)
            {
                _waveOut.Stop();
                _waveReader?.Close();
                _waveReader?.Dispose();
                WaveForm = null;
            }

            _waveReader = null;
            _bmp?.Dispose();
            //OggPreviewButtonText = "Ogg Preview";
        }

        public void Dispose()
        {
            _waveReader?.Dispose();
            _waveOut?.Dispose();
            _bmp?.Dispose();
        }

        public void PauseSound()
        {
            if (PlaybackState == PlaybackState.Playing)
            {
                _waveOut?.Pause();
                _timer.Stop();

            }
        }
    }
}
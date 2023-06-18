using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mcce22.SmartOffice.Simulator.Messages;
using Mcce22.SmartOffice.Simulator.Services;
using Newtonsoft.Json;

namespace Mcce22.SmartOffice.Simulator.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private const double DEFAULT_DESK_CANVAS_TOP = 0;
        private const double MAX_DESK_HEIGHT = 120;
        private const double MIN_DESK_HEIGHT = 70;
        private const double DEFAULT_LEG_CANVAS_TOP = 420;
        private const double DEFAULT_LEG_HEIGHT = 80;
        private const double DEFAULT_IMAGEFRAME_CANVAS_TOP = 336;
        private const double DEFAULT_WIFI_CANVAS_TOP = 315;

        private readonly IMqttService _mqttService;
        private readonly AppSettings _appSettings;
        private readonly Timer _delayTimer;

        [ObservableProperty]
        private double _deskCanvasTop = DEFAULT_DESK_CANVAS_TOP;

        [ObservableProperty]
        private double _legCanvasTop = DEFAULT_LEG_CANVAS_TOP;

        [ObservableProperty]
        private double _legHeight = DEFAULT_LEG_HEIGHT;

        [ObservableProperty]
        private double _deskHeight = MIN_DESK_HEIGHT;

        [ObservableProperty]
        private double _imageFrameCanvasTop = DEFAULT_IMAGEFRAME_CANVAS_TOP;

        [ObservableProperty]
        private double _wifiCanvasTop = DEFAULT_WIFI_CANVAS_TOP;

        [ObservableProperty]
        private string _imageUrl;

        [ObservableProperty]
        private double _temperature = 20;

        [ObservableProperty]
        private double _noiseLevel = 20;

        [ObservableProperty]
        private double _co2Level = 700;

        [ObservableProperty]
        private string _messageLog;

        public MainViewModel(IMqttService mqttService, AppSettings appSettings)
        {
            _mqttService = mqttService;
            _appSettings = appSettings;
            _delayTimer = new Timer();
            _delayTimer.Interval = 5000;
            _delayTimer.Elapsed += OnDelayTimerElpased;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Temperature) ||
                e.PropertyName == nameof(NoiseLevel) ||
                e.PropertyName == nameof(Co2Level))
            {
                _delayTimer.Stop();
                _delayTimer.Start();
            }

            base.OnPropertyChanged(e);
        }

        private async void OnDelayTimerElpased(object sender, ElapsedEventArgs e)
        {
            var msg = new DataIngressMessage
            {
                WorkspaceNumber = _appSettings.WorkspaceNumber,
                Temperature = Temperature,
                NoiseLevel = NoiseLevel,
                Co2Level = Co2Level,
            };

            await _mqttService.PublishMessage(msg);

            MessageLog += $"[OUT] {JsonConvert.SerializeObject(msg)}" + Environment.NewLine;

            _delayTimer.Stop();
        }

        public async Task Connect()
        {
            await _mqttService.Connect();

            _mqttService.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, MessageReceivedArgs e)
        {
            if (e.Message.WorkspaceNumber == _appSettings.WorkspaceNumber)
            {
                if (e.Message.DeskHeight > MAX_DESK_HEIGHT)
                {
                    DeskHeight = MAX_DESK_HEIGHT;
                }
                else if (e.Message.DeskHeight < MIN_DESK_HEIGHT)
                {
                    DeskHeight = MIN_DESK_HEIGHT;
                }
                else
                {
                    DeskHeight = e.Message.DeskHeight;
                }

                ImageUrl = e.Message.UserImageUrl;

                MessageLog += $"[IN ] {JsonConvert.SerializeObject(e.Message)}" + Environment.NewLine;

                UpdateDeskParams(DeskHeight);
            }
        }

        private void UpdateDeskParams(double deskHeight)
        {
            LegCanvasTop = DEFAULT_LEG_CANVAS_TOP - (deskHeight - MIN_DESK_HEIGHT) * 2;
            LegHeight = DEFAULT_LEG_HEIGHT + (deskHeight - MIN_DESK_HEIGHT) * 2;
            ImageFrameCanvasTop = DEFAULT_IMAGEFRAME_CANVAS_TOP - (deskHeight - MIN_DESK_HEIGHT) * 2;
            WifiCanvasTop = DEFAULT_WIFI_CANVAS_TOP - (deskHeight - MIN_DESK_HEIGHT) * 2;
            DeskCanvasTop = DEFAULT_DESK_CANVAS_TOP - (deskHeight - MIN_DESK_HEIGHT) * 2;
        }

        [RelayCommand]
        private void ClearMessageLog()
        {
            MessageLog = string.Empty;
        }
    }
}

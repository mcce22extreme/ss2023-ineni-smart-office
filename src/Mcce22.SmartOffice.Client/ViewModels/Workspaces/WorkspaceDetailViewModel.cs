using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class WorkspaceDetailViewModel : DialogViewModelBase
    {
        private readonly IWorkspaceManager _workspaceManager;

        public int WorkspaceId { get; set; }

        private string _workspaceNumber;
        public string WorkspaceNumber
        {
            get { return _workspaceNumber; }
            set { SetProperty(ref _workspaceNumber, value); }
        }

        private string _roomNumber;
        public string RoomNumber
        {
            get { return _roomNumber; }
            set { SetProperty(ref _roomNumber, value); }
        }

        private int _top;
        public int Top
        {
            get { return _top; }
            set { SetProperty(ref _top, value); }
        }

        private int _left;
        public int Left
        {
            get { return _left; }
            set { SetProperty(ref _left, value); }
        }

        private int _width;
        public int Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        private int _height;
        public int Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        public WorkspaceDetailViewModel(IWorkspaceManager workspaceManager, IDialogService dialogService)
            : base(dialogService)
        {
            Title = "Create workspace";
            _workspaceManager = workspaceManager;
        }

        public WorkspaceDetailViewModel(WorkspaceModel model, IWorkspaceManager workspaceManager, IDialogService dialogService)
            : this(workspaceManager, dialogService)
        {
            Title = "Edit workspace";

            WorkspaceId = model.Id;
            WorkspaceNumber = model.WorkspaceNumber;
            RoomNumber = model.RoomNumber;
            Top = model.Top;
            Left = model.Left;
            Width = model.Width;
            Height = model.Height;
        }

        protected override async Task OnSave()
        {
            await _workspaceManager.Save(new WorkspaceModel
            {
                Id = WorkspaceId,
                WorkspaceNumber = WorkspaceNumber,
                RoomNumber = RoomNumber,
                Top = Top,
                Left = Left,
                Width = Width,
                Height = Height,
            });
        }
    }
}

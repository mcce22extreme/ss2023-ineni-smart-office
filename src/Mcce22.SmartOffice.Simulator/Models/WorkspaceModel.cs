using CommunityToolkit.Mvvm.ComponentModel;

namespace Mcce22.SmartOffice.Simulator
{
    public partial class WorkspaceModel : ObservableObject
    {
        public string Id { get; set; }

        public string WorkspaceNumber { get; set; }
    }
}

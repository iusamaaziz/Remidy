using CommunityToolkit.Mvvm.Input;
using Remidy.Models;

namespace Remidy.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}
#nullable disable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Remidy.Data;
using Remidy.Models;
using Remidy.Services;
using System.Linq;

namespace Remidy.PageModels
{
    public partial class ProjectListPageModel : ObservableObject
    {
        private readonly ProjectRepository _projectRepository;

        [ObservableProperty]
        private List<Project> _projects = [];

        [ObservableProperty]
        private List<Project> _filteredProjects = [];

        [ObservableProperty]
        private string _searchText = string.Empty;

        public ProjectListPageModel(ProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        partial void OnSearchTextChanged(string value)
        {
            PerformSearch();
        }

        [RelayCommand]
        private async Task Appearing()
        {
            Projects = await _projectRepository.ListAsync();
            FilteredProjects = new List<Project>(Projects);
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredProjects = new List<Project>(Projects);
                return;
            }

            var searchTerm = SearchText.ToLowerInvariant();
            FilteredProjects = Projects.Where(project =>
                (!string.IsNullOrEmpty(project.RegistrationNo) && project.RegistrationNo.ToLowerInvariant().Contains(searchTerm)) ||
                (!string.IsNullOrEmpty(project.Name) && project.Name.ToLowerInvariant().Contains(searchTerm))
            ).ToList();
        }

        [RelayCommand]
        Task NavigateToProject(Project project)
            => Shell.Current.GoToAsync($"project?id={project.ID}");

        [RelayCommand]
        async Task AddProject()
        {
            await Shell.Current.GoToAsync($"project");
        }
    }
}
#nullable disable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Remidy.Data;
using Remidy.Models;
using Remidy.Services;
using System.Linq;

namespace Remidy.PageModels
{
    /// <summary>
    /// ViewModel for the Project List Page that displays all patient case records.
    /// Provides functionality to view, search, and navigate to individual case records.
    /// Implements real-time search filtering by Registration Number and Patient Name.
    /// </summary>
    /// <remarks>
    /// Features:
    /// - Load and display all patient cases from database
    /// - Real-time search functionality (Registration # and Name)
    /// - Navigate to individual case details
    /// - Add new patient cases
    /// - Responsive UI with filtered results
    /// 
    /// Search Implementation:
    /// - Case-insensitive partial matching
    /// - Searches both Registration Number and Patient Name fields
    /// - Auto-updates results as user types
    /// - Maintains original list for efficient filtering
    /// </remarks>
    public partial class ProjectListPageModel : ObservableObject
    {
        private readonly ProjectRepository _projectRepository;

        /// <summary>
        /// Complete list of all patient projects/cases loaded from database.
        /// Used as source data for filtering operations.
        /// </summary>
        [ObservableProperty]
        private List<Project> _projects = [];

        /// <summary>
        /// Filtered list of projects based on current search criteria.
        /// This collection is bound to the UI and updates in real-time during search.
        /// </summary>
        [ObservableProperty]
        private List<Project> _filteredProjects = [];

        /// <summary>
        /// Current search text entered by user for filtering projects.
        /// Triggers automatic filtering when changed.
        /// </summary>
        [ObservableProperty]
        private string _searchText = string.Empty;

        /// <summary>
        /// Initializes a new instance of ProjectListPageModel with required dependencies.
        /// </summary>
        /// <param name="projectRepository">Repository for accessing project data from database</param>
        public ProjectListPageModel(ProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        /// <summary>
        /// Automatically triggered when SearchText property changes.
        /// Initiates search filtering to update the displayed results.
        /// </summary>
        /// <param name="value">New search text value</param>
        partial void OnSearchTextChanged(string value)
        {
            PerformSearch();
        }

        /// <summary>
        /// Loads all patient projects from database when page appears.
        /// Initializes both the main projects list and filtered results.
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        [RelayCommand]
        private async Task Appearing()
        {
            Projects = await _projectRepository.ListAsync();
            FilteredProjects = new List<Project>(Projects);
        }

        /// <summary>
        /// Performs search filtering on the projects list based on current search text.
        /// Searches both Registration Number and Patient Name fields with case-insensitive matching.
        /// </summary>
        /// <remarks>
        /// Search Logic:
        /// - Empty/whitespace search text: Shows all projects
        /// - Non-empty search: Filters projects where Registration Number OR Name contains search term
        /// - Uses case-insensitive partial matching for user-friendly search experience
        /// </remarks>
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

        /// <summary>
        /// Navigates to the detailed view of a selected patient project.
        /// </summary>
        /// <param name="project">The project to view in detail</param>
        /// <returns>Task representing the navigation operation</returns>
        [RelayCommand]
        Task NavigateToProject(Project project)
            => Shell.Current.GoToAsync($"project?id={project.ID}");

        /// <summary>
        /// Navigates to create a new patient project/case record.
        /// Opens the project detail page in "add new" mode.
        /// </summary>
        /// <returns>Task representing the navigation operation</returns>
        [RelayCommand]
        async Task AddProject()
        {
            await Shell.Current.GoToAsync($"project");
        }
    }
}
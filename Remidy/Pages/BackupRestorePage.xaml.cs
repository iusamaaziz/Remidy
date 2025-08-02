namespace Remidy.Pages
{
    /// <summary>
    /// Code-behind for the BackupRestorePage.
    /// Handles the UI initialization and lifecycle events.
    /// </summary>
    public partial class BackupRestorePage : ContentPage
    {
        public BackupRestorePage(BackupRestorePageModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
} 
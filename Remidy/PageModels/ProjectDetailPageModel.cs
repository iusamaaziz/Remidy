using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Remidy.Models;

namespace Remidy.PageModels
{
    /// <summary>
    /// ViewModel for the Project Detail Page that handles comprehensive patient case management.
    /// Manages all aspects of a patient case including demographics, medical history, examination findings,
    /// vital signs, medical classifications, and treatment tracking.
    /// </summary>
    /// <remarks>
    /// Core Functionality:
    /// - Patient demographic information (Name, Description, Registration details)
    /// - Vital signs tracking (BP, Temperature, Pulse, Respiration, Height, Weight)
    /// - Medical classifications using lookup types (BMI, Constitution, Nature, etc.)
    /// - Physical examination findings (Face, Hands, Eyes, Hair, etc.)
    /// - Case categorization and tagging system
    /// - Task/treatment tracking integration
    /// - Present complaints documentation
    /// 
    /// Data Flow:
    /// 1. Load existing case data or initialize new case
    /// 2. Populate dropdown lists from lookup repositories
    /// 3. Bind UI controls to observable properties
    /// 4. Handle user input and validation
    /// 5. Save changes back to database
    /// 
    /// Special Features:
    /// - String-based vital signs properties for seamless UI binding
    /// - Automatic index management for picker controls
    /// - Comprehensive lookup type support (20+ medical classifications)
    /// - Error handling and user feedback
    /// - Navigation parameter handling for new vs existing cases
    /// 
    /// Architecture:
    /// - Implements IQueryAttributable for navigation parameters
    /// - Implements IProjectTaskPageModel for task management integration
    /// - Uses Repository Pattern for data access
    /// - Follows MVVM pattern with observable properties
    /// - Leverages CommunityToolkit.Mvvm for code generation
    /// </remarks>
    public partial class ProjectDetailPageModel : ObservableObject, IQueryAttributable, IProjectTaskPageModel
    {
        private Project? _project;
        private readonly ProjectRepository _projectRepository;
        private readonly TaskRepository _taskRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly LookupRepositoryFactory _lookupFactory;
        private readonly TagRepository _tagRepository;
        private readonly ModalErrorHandler _errorHandler;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private List<ProjectTask> _tasks = [];

        [ObservableProperty]
        private List<Category> _categories = [];

        [ObservableProperty]
        private Category? _category;

        [ObservableProperty]
        private int _categoryIndex = -1;

        // BMI
        [ObservableProperty]
        private List<BMICategoryType> _bmiCategories = [];

        [ObservableProperty]
        private BMICategoryType? _bmiCategory;

        [ObservableProperty]
        private int _bmiCategoryIndex = -1;

        // Constitution
        [ObservableProperty]
        private List<ConstitutionType> _constitutionTypes = [];

        [ObservableProperty]
        private ConstitutionType? _constitutionType;

        [ObservableProperty]
        private int _constitutionTypeIndex = -1;

        // Nature
        [ObservableProperty]
        private List<NatureType> _natureTypes = [];

        [ObservableProperty]
        private NatureType? _natureType;

        [ObservableProperty]
        private int _natureTypeIndex = -1;

        // Look
        [ObservableProperty]
        private List<LookType> _lookTypes = [];

        [ObservableProperty]
        private LookType? _lookType;

        [ObservableProperty]
        private int _lookTypeIndex = -1;

        // Complexion
        [ObservableProperty]
        private List<ComplexionType> _complexionTypes = [];

        [ObservableProperty]
        private ComplexionType? _complexionType;

        [ObservableProperty]
        private int _complexionTypeIndex = -1;

        // Temperament
        [ObservableProperty]
        private List<TemperamentType> _temperamentTypes = [];

        [ObservableProperty]
        private TemperamentType? _temperamentType;

        [ObservableProperty]
        private int _temperamentTypeIndex = -1;

        // Hand
        [ObservableProperty]
        private List<HandType> _handTypes = [];

        [ObservableProperty]
        private HandType? _handType;

        [ObservableProperty]
        private int _handTypeIndex = -1;

        // FaceType
        [ObservableProperty]
        private List<FaceType> _faceTypes = [];

        [ObservableProperty]
        private FaceType? _faceType;

        [ObservableProperty]
        private int _faceTypeIndex = -1;

        // NailType
        [ObservableProperty]
        private List<NailType> _nailTypes = [];

        [ObservableProperty]
        private NailType? _nailType;

        [ObservableProperty]
        private int _nailTypeIndex = -1;

        // SpeakingType
        [ObservableProperty]
        private List<SpeakingType> _speakingTypes = [];

        [ObservableProperty]
        private SpeakingType? _speakingType;

        [ObservableProperty]
        private int _speakingTypeIndex = -1;

        // TongueType
        [ObservableProperty]
        private List<TongueType> _tongueTypes = [];

        [ObservableProperty]
        private TongueType? _tongueType;

        [ObservableProperty]
        private int _tongueTypeIndex = -1;

        // HairType
        [ObservableProperty]
        private List<HairType> _hairTypes = [];

        [ObservableProperty]
        private HairType? _hairType;

        [ObservableProperty]
        private int _hairTypeIndex = -1;

        // EyeType
        [ObservableProperty]
        private List<EyeType> _eyeTypes = [];

        [ObservableProperty]
        private EyeType? _eyeType;

        [ObservableProperty]
        private int _eyeTypeIndex = -1;

        // Cause of Disease
        [ObservableProperty] private List<CauseOfDiseaseType> _causeOfDiseaseTypes = new();
        [ObservableProperty] private CauseOfDiseaseType? _causeOfDiseaseType;
        [ObservableProperty] private int _causeOfDiseaseTypeIndex = -1;

        // Sensation
        [ObservableProperty] private List<SensationType> _sensationTypes = new();
        [ObservableProperty] private SensationType? _sensationType;
        [ObservableProperty] private int _sensationTypeIndex = -1;

        // Treatment Often Done
        [ObservableProperty] private List<TreatmentType> _treatmentTypes = new();
        [ObservableProperty] private TreatmentType? _treatmentType;
        [ObservableProperty] private int _treatmentTypeIndex = -1;

        // Responded to Treatment
        [ObservableProperty] private List<RespondedToTreatmentType> _respondedToTreatmentTypes = new();
        [ObservableProperty] private RespondedToTreatmentType? _respondedToTreatmentType;
        [ObservableProperty] private int _respondedToTreatmentTypeIndex = -1;

        // Blood Group
        [ObservableProperty] private List<BloodGroupType> _bloodGroupTypes = new();
        [ObservableProperty] private BloodGroupType? _bloodGroupType;
        [ObservableProperty] private int _bloodGroupTypeIndex = -1;

        // Case Condition
        [ObservableProperty] private List<CaseConditionType> _caseConditionTypes = new();
        [ObservableProperty] private CaseConditionType? _caseConditionType;
        [ObservableProperty] private int _caseConditionTypeIndex = -1;

        // Personal Habit
        [ObservableProperty] private List<PersonalHabitType> _personalHabitTypes = new();
        [ObservableProperty] private PersonalHabitType? _personalHabitType;
        [ObservableProperty] private int _personalHabitTypeIndex = -1;

        [ObservableProperty]
        private List<Tag> _allTags = [];

        [ObservableProperty]
        private IconData _icon;

        [ObservableProperty]
        bool _isBusy;

        // General Information
        [ObservableProperty] private string _registrationNo;
        [ObservableProperty] private DateOnly _date;
        [ObservableProperty] private TimeSpan _time;
        [ObservableProperty] private string _motherName;
        [ObservableProperty] private string _maritalStatus;
        [ObservableProperty] private int? _children;
        [ObservableProperty] private string _caste;
        [ObservableProperty] private int? _age;
        [ObservableProperty] private string _religion;
        [ObservableProperty] private string _occupation;
        [ObservableProperty] private string _address;
        [ObservableProperty] private string _contactNo;

        #region Vital Signs
        /// <summary>
        /// Blood Pressure measurement as string for seamless UI binding.
        /// Converted from/to nullable double in Project model during load/save operations.
        /// </summary>
        [ObservableProperty] private string _bp = string.Empty;
        
        /// <summary>
        /// Body temperature measurement as string for seamless UI binding.
        /// Converted from/to nullable double in Project model during load/save operations.
        /// </summary>
        [ObservableProperty] private string _temperature = string.Empty;
        
        /// <summary>
        /// Pulse rate measurement as string for seamless UI binding.
        /// Converted from/to nullable int in Project model during load/save operations.
        /// </summary>
        [ObservableProperty] private string _pulse = string.Empty;
        
        /// <summary>
        /// Respiration rate measurement as string for seamless UI binding.
        /// Converted from/to nullable int in Project model during load/save operations.
        /// </summary>
        [ObservableProperty] private string _respiration = string.Empty;
        
        /// <summary>
        /// Patient height measurement as string for seamless UI binding.
        /// Converted from/to nullable double in Project model during load/save operations.
        /// </summary>
        [ObservableProperty] private string _height = string.Empty;
        
        /// <summary>
        /// Patient weight measurement as string for seamless UI binding.
        /// Converted from/to nullable double in Project model during load/save operations.
        /// </summary>
        [ObservableProperty] private string _weight = string.Empty;
        #endregion

        /// <summary>
        /// Patient's present complaints documented in chronological order.
        /// Includes location, sensation, modalities, amelioration, and other relevant symptoms.
        /// Critical field for case assessment and treatment planning.
        /// </summary>
        [ObservableProperty] private string _presentComplaints = string.Empty;

        [ObservableProperty] private string _pecular;
        [ObservableProperty] private string _queer;
        [ObservableProperty] private string _strange;
        [ObservableProperty] private string _likes;
        [ObservableProperty] private string _dislikes;
        [ObservableProperty] private string _head;
        [ObservableProperty] private string _eyes;
        [ObservableProperty] private string _ear;
        [ObservableProperty] private string _nose;
        [ObservableProperty] private string _face;
        [ObservableProperty] private string _teeth;
        [ObservableProperty] private string _mouth;
        [ObservableProperty] private string _throat;
        [ObservableProperty] private string _thirst;
        [ObservableProperty] private string _diet;
        [ObservableProperty] private string _abdomen;
        [ObservableProperty] private string _stool;
        [ObservableProperty] private string _urine;
        [ObservableProperty] private string _lungs;
        [ObservableProperty] private string _heart;
        [ObservableProperty] private string _spine;
        [ObservableProperty] private string _maleGenitalOrganic;
        [ObservableProperty] private string _maleGenitalFunctional;
        [ObservableProperty] private string _femaleGenitalOrganic;
        [ObservableProperty] private string _femaleGenitalFunctional;
        [ObservableProperty] private string _sleep;
        [ObservableProperty] private string _skin;
        [ObservableProperty] private string _fever;
        [ObservableProperty] private string _dreams;
        [ObservableProperty] private string _aversions;
        [ObservableProperty] private string _sweat;
        [ObservableProperty] private string _side;
        [ObservableProperty] private string _thermally;
        [ObservableProperty] private string _xRayFindings;
        [ObservableProperty] private string _usgOpinion;
        [ObservableProperty] private string _ctScanFindings;
        [ObservableProperty] private string _mriFindings;

        [ObservableProperty]
        private List<IconData> _icons = new List<IconData>
        {
            new IconData { Icon = FluentUI.ribbon_24_regular, Description = "Ribbon Icon" },
            new IconData { Icon = FluentUI.ribbon_star_24_regular, Description = "Ribbon Star Icon" },
            new IconData { Icon = FluentUI.trophy_24_regular, Description = "Trophy Icon" },
            new IconData { Icon = FluentUI.badge_24_regular, Description = "Badge Icon" },
            new IconData { Icon = FluentUI.book_24_regular, Description = "Book Icon" },
            new IconData { Icon = FluentUI.people_24_regular, Description = "People Icon" },
            new IconData { Icon = FluentUI.bot_24_regular, Description = "Bot Icon" }
        };

        public bool HasCompletedTasks
            => _project?.Tasks.Any(t => t.IsCompleted) ?? false;

        public ProjectDetailPageModel(
            ProjectRepository projectRepository,
            TaskRepository taskRepository,
            CategoryRepository categoryRepository,
            LookupRepositoryFactory lookupFactory,
            TagRepository tagRepository,
            ModalErrorHandler errorHandler)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
            _lookupFactory = lookupFactory;
            _tagRepository = tagRepository;
            _errorHandler = errorHandler;
            _icon = _icons.First();
            Tasks = [];
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("id"))
            {
                int id = Convert.ToInt32(query["id"]);
                LoadData(id).FireAndForgetSafeAsync(_errorHandler);
            }
            else if (query.ContainsKey("refresh"))
            {
                RefreshData().FireAndForgetSafeAsync(_errorHandler);
            }
            else
            {
                Task.WhenAll(LoadCategories(), LoadTags(), LoadHandTypes(), LoadBMITypes(), LoadPersonalHabitTypes()).FireAndForgetSafeAsync(_errorHandler);
                _project = new();
                _project.Tags = [];
                _project.Tasks = [];
                Tasks = _project.Tasks;
            }
        }

        private async Task LoadCategories() =>
            Categories = await _categoryRepository.ListAsync();

        private async Task LoadBMITypes() =>
            BmiCategories = await _lookupFactory.GetRepository<BMICategoryType>().ListAsync();

        private async Task LoadHandTypes() =>
            HandTypes = await _lookupFactory.GetRepository<HandType>().ListAsync();

        private async Task LoadTags() =>
            AllTags = await _tagRepository.ListAsync();

        private async Task LoadPersonalHabitTypes() =>
            PersonalHabitTypes = await _lookupFactory.GetRepository<PersonalHabitType>().ListAsync();

        private async Task RefreshData()
        {
            if (_project.IsNullOrNew())
            {
                if (_project is not null)
                    Tasks = new(_project.Tasks);

                return;
            }

            Tasks = await _taskRepository.ListAsync(_project.ID);
            _project.Tasks = Tasks;
        }

        private async Task LoadData(int id)
        {
            try
            {
                IsBusy = true;

                _project = await _projectRepository.GetAsync(id);

                if (_project.IsNullOrNew())
                {
                    _errorHandler.HandleError(new Exception($"Project with id {id} could not be found."));
                    return;
                }

                // Basic Info
                Name = _project.Name;
                Description = _project.Description;
                RegistrationNo = _project.RegistrationNo;
                Date = DateOnly.FromDateTime(_project.Date);
                Time = _project.Time;
                MotherName = _project.MotherName;
                MaritalStatus = _project.MaritalStatus;
                Children = _project.Children;
                Caste = _project.Caste;
                Age = _project.Age;
                Religion = _project.Religion;
                Occupation = _project.Occupation;
                Address = _project.Address;
                ContactNo = _project.ContactNo;

                // Vital Signs
                Bp = _project.BP?.ToString("F1") ?? string.Empty;
                Temperature = _project.Temperature?.ToString("F1") ?? string.Empty;
                Pulse = _project.Pulse?.ToString() ?? string.Empty;
                Respiration = _project.Respiration?.ToString() ?? string.Empty;
                Height = _project.Height?.ToString("F1") ?? string.Empty;
                Weight = _project.Weight?.ToString("F1") ?? string.Empty;

                TemperamentTypes = await _lookupFactory.GetRepository<TemperamentType>().ListAsync();
                TemperamentType = TemperamentTypes?.FirstOrDefault(h => h.Id == _project.TemperamentTypeId);
                TemperamentTypeIndex = TemperamentTypes?.FindIndex(h => h.Id == _project.TemperamentTypeId) ?? -1;

                BmiCategories = await _lookupFactory.GetRepository<BMICategoryType>().ListAsync();
                BmiCategory = BmiCategories?.FirstOrDefault(h => h.Id == _project.BmiCategoryId);
                BmiCategoryIndex = BmiCategories?.FindIndex(h => h.Id == _project.BmiCategoryId) ?? -1;

                // Constitution
                ConstitutionTypes = await _lookupFactory.GetRepository<ConstitutionType>().ListAsync();
                ConstitutionType = ConstitutionTypes.FirstOrDefault(c => c.Id == _project.ConstitutionTypeId);
                ConstitutionTypeIndex = ConstitutionTypes.FindIndex(c => c.Id == _project.ConstitutionTypeId);

                // Nature
                NatureTypes = await _lookupFactory.GetRepository<NatureType>().ListAsync();
                NatureType = NatureTypes.FirstOrDefault(c => c.Id == _project.NatureTypeId);
                NatureTypeIndex = NatureTypes.FindIndex(c => c.Id == _project.NatureTypeId);

                // Look
                LookTypes = await _lookupFactory.GetRepository<LookType>().ListAsync();
                LookType = LookTypes.FirstOrDefault(c => c.Id == _project.LookTypeId);
                LookTypeIndex = LookTypes.FindIndex(c => c.Id == _project.LookTypeId);

                // Complexion
                ComplexionTypes = await _lookupFactory.GetRepository<ComplexionType>().ListAsync();
                ComplexionType = ComplexionTypes.FirstOrDefault(c => c.Id == _project.ComplexionTypeId);
                ComplexionTypeIndex = ComplexionTypes.FindIndex(c => c.Id == _project.ComplexionTypeId);

                // Temperament
                TemperamentTypes = await _lookupFactory.GetRepository<TemperamentType>().ListAsync();
                TemperamentType = TemperamentTypes.FirstOrDefault(c => c.Id == _project.TemperamentTypeId);
                TemperamentTypeIndex = TemperamentTypes.FindIndex(c => c.Id == _project.TemperamentTypeId);

                // Face Type
                FaceTypes = await _lookupFactory.GetRepository<FaceType>().ListAsync();
                FaceType = FaceTypes.FirstOrDefault(c => c.Id == _project.FaceTypeId);
                FaceTypeIndex = FaceTypes.FindIndex(c => c.Id == _project.FaceTypeId);

                // Nail
                NailTypes = await _lookupFactory.GetRepository<NailType>().ListAsync();
                NailType = NailTypes.FirstOrDefault(c => c.Id == _project.NailTypeId);
                NailTypeIndex = NailTypes.FindIndex(c => c.Id == _project.NailTypeId);

                // Speaking
                SpeakingTypes = await _lookupFactory.GetRepository<SpeakingType>().ListAsync();
                SpeakingType = SpeakingTypes.FirstOrDefault(c => c.Id == _project.SpeakingTypeId);
                SpeakingTypeIndex = SpeakingTypes.FindIndex(c => c.Id == _project.SpeakingTypeId);

                // Tongue
                TongueTypes = await _lookupFactory.GetRepository<TongueType>().ListAsync();
                TongueType = TongueTypes.FirstOrDefault(c => c.Id == _project.TongueTypeId);
                TongueTypeIndex = TongueTypes.FindIndex(c => c.Id == _project.TongueTypeId);

                // Hair
                HairTypes = await _lookupFactory.GetRepository<HairType>().ListAsync();
                HairType = HairTypes.FirstOrDefault(c => c.Id == _project.HairTypeId);
                HairTypeIndex = HairTypes.FindIndex(c => c.Id == _project.HairTypeId);

                // Eye
                EyeTypes = await _lookupFactory.GetRepository<EyeType>().ListAsync();
                EyeType = EyeTypes.FirstOrDefault(c => c.Id == _project.EyeColoryId);
                EyeTypeIndex = EyeTypes.FindIndex(c => c.Id == _project.EyeColoryId);

                HandTypes = await _lookupFactory.GetRepository<HandType>().ListAsync();
                HandType = HandTypes?.FirstOrDefault(h => h.Id == _project.HairTypeId);
                HandTypeIndex = HandTypes?.FindIndex(h => h.Id == _project.HairTypeId) ?? -1;

                CauseOfDiseaseTypes = await _lookupFactory.GetRepository<CauseOfDiseaseType>().ListAsync();
                CauseOfDiseaseType = CauseOfDiseaseTypes.FirstOrDefault(c => c.Id == _project.CauseOfDiseaseTypeId);
                CauseOfDiseaseTypeIndex = CauseOfDiseaseTypes.FindIndex(c => c.Id == _project.CauseOfDiseaseTypeId);

                SensationTypes = await _lookupFactory.GetRepository<SensationType>().ListAsync();
                SensationType = SensationTypes.FirstOrDefault(c => c.Id == _project.SensationTypeId);
                SensationTypeIndex = SensationTypes.FindIndex(c => c.Id == _project.SensationTypeId);

                TreatmentTypes = await _lookupFactory.GetRepository<TreatmentType>().ListAsync();
                TreatmentType = TreatmentTypes.FirstOrDefault(c => c.Id == _project.TreatmentTypeId);
                TreatmentTypeIndex = TreatmentTypes.FindIndex(c => c.Id == _project.TreatmentTypeId);

                RespondedToTreatmentTypes = await _lookupFactory.GetRepository<RespondedToTreatmentType>().ListAsync();
                RespondedToTreatmentType = RespondedToTreatmentTypes.FirstOrDefault(c => c.Id == _project.RespondedToTreatmentTypeId);
                RespondedToTreatmentTypeIndex = RespondedToTreatmentTypes.FindIndex(c => c.Id == _project.RespondedToTreatmentTypeId);

                BloodGroupTypes = await _lookupFactory.GetRepository<BloodGroupType>().ListAsync();
                BloodGroupType = BloodGroupTypes.FirstOrDefault(c => c.Id == _project.BloodGroupTypeId);
                BloodGroupTypeIndex = BloodGroupTypes.FindIndex(c => c.Id == _project.BloodGroupTypeId);

                CaseConditionTypes = await _lookupFactory.GetRepository<CaseConditionType>().ListAsync();
                CaseConditionType = CaseConditionTypes.FirstOrDefault(c => c.Id == _project.CaseConditionTypeId);
                CaseConditionTypeIndex = CaseConditionTypes.FindIndex(c => c.Id == _project.CaseConditionTypeId);

                PersonalHabitTypes = await _lookupFactory.GetRepository<PersonalHabitType>().ListAsync();
                PersonalHabitType = PersonalHabitTypes.FirstOrDefault(c => c.Id == _project.PersonalHabitTypeId);
                PersonalHabitTypeIndex = PersonalHabitTypes.FindIndex(c => c.Id == _project.PersonalHabitTypeId);

                PresentComplaints = _project.PresentComplaints;
                Pecular = _project.Pecular;
                Queer = _project.Queer;
                Strange = _project.Strange;
                Likes = _project.Likes;
                Dislikes = _project.Dislikes;
                Head = _project.Head;
                Eyes = _project.Eyes;
                Ear = _project.Ear;
                Nose = _project.Nose;
                Face = _project.Face;
                Teeth = _project.Teeth;
                Mouth = _project.Mouth;
                Throat = _project.Throat;
                Thirst = _project.Thirst;
                Diet = _project.Diet;
                Abdomen = _project.Abdomen;
                Stool = _project.Stool;
                Urine = _project.Urine;
                Lungs = _project.Lungs;
                Heart = _project.Heart;
                Spine = _project.Spine;
                MaleGenitalOrganic = _project.MaleGenitalOrganic;
                MaleGenitalFunctional = _project.MaleGenitalFunctional;
                FemaleGenitalOrganic = _project.FemaleGenitalOrganic;
                FemaleGenitalFunctional = _project.FemaleGenitalFunctional;
                Sleep = _project.Sleep;
                Skin = _project.Skin;
                Fever = _project.Fever;
                Dreams = _project.Dreams;
                Aversions = _project.Aversions;
                Sweat = _project.Sweat;
                Side = _project.Side;
                Thermally = _project.Thermally;
                XRayFindings = _project.XRayFindings;
                UsgOpinion = _project.USGOpinion;
                CtScanFindings = _project.CTScanFindings;
                MriFindings = _project.MRIFindings;

                Categories = await _categoryRepository.ListAsync();
                Category = Categories?.FirstOrDefault(c => c.ID == _project.CategoryID);
                CategoryIndex = Categories?.FindIndex(c => c.ID == _project.CategoryID) ?? -1;

                Tasks = _project.Tasks;

                Icon.Icon = _project.Icon;

                var allTags = await _tagRepository.ListAsync();
                foreach (var tag in allTags)
                {
                    tag.IsSelected = _project.Tags.Any(t => t.ID == tag.ID);
                }
                AllTags = new(allTags);
            }
            catch (Exception e)
            {
                _errorHandler.HandleError(e);
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(HasCompletedTasks));
            }
        }

        [RelayCommand]
        private async Task TaskCompleted(ProjectTask task)
        {
            await _taskRepository.SaveItemAsync(task);
            OnPropertyChanged(nameof(HasCompletedTasks));
        }


        [RelayCommand]
        private async Task Save()
        {
            if (_project is null)
            {
                _errorHandler.HandleError(
                    new Exception("Project is null. Cannot Save."));

                return;
            }

            _project.Name = Name;
            _project.Description = Description;
            _project.RegistrationNo = RegistrationNo;
            _project.Date = Date.ToDateTime(TimeOnly.MinValue); // Combine with time for full DateTime
            _project.Time = Time;
            _project.MotherName = MotherName;
            _project.MaritalStatus = MaritalStatus;
            _project.Children = Children;
            _project.Caste = Caste;
            _project.Age = Age;
            _project.Religion = Religion;
            _project.Occupation = Occupation;
            _project.Address = Address;
            _project.ContactNo = ContactNo;

            // Vital signs
            _project.BP = double.TryParse(Bp, out var bp) ? bp : null;
            _project.Temperature = double.TryParse(Temperature, out var temp) ? temp : null;
            _project.Pulse = int.TryParse(Pulse, out var pulse) ? pulse : null;
            _project.Respiration = int.TryParse(Respiration, out var respiration) ? respiration : null;
            _project.Height = double.TryParse(Height, out var height) ? height : null;
            _project.Weight = double.TryParse(Weight, out var weight) ? weight : null;

            _project.BmiCategoryId = BmiCategory?.Id ?? 0;
            _project.ConstitutionTypeId = ConstitutionType?.Id ?? 0;
            _project.NatureTypeId = NatureType?.Id ?? 0;
            _project.LookTypeId = LookType?.Id ?? 0;
            _project.ComplexionTypeId = ComplexionType?.Id ?? 0;
            _project.TemperamentTypeId = TemperamentType?.Id ?? 0;
            _project.FaceTypeId = FaceType?.Id ?? 0;
            _project.NailTypeId = NailType?.Id ?? 0;
            _project.SpeakingTypeId = SpeakingType?.Id ?? 0;
            _project.TongueTypeId = TongueType?.Id ?? 0;
            _project.HairTypeId = HairType?.Id ?? 0;
            _project.EyeColoryId = EyeType?.Id ?? 0;

            _project.CauseOfDiseaseTypeId = CauseOfDiseaseType?.Id ?? 0;
            _project.SensationTypeId = SensationType?.Id ?? 0;
            _project.TreatmentTypeId = TreatmentType?.Id ?? 0;
            _project.RespondedToTreatmentTypeId = RespondedToTreatmentType?.Id ?? 0;
            _project.BloodGroupTypeId = BloodGroupType?.Id ?? 0;
            _project.CaseConditionTypeId = CaseConditionType?.Id ?? 0;
            _project.PersonalHabitTypeId = PersonalHabitType?.Id ?? 0;

            _project.PresentComplaints = PresentComplaints;
            _project.Pecular = Pecular;
            _project.Queer = Queer;
            _project.Strange = Strange;
            _project.Likes = Likes;
            _project.Dislikes = Dislikes;
            _project.Head = Head;
            _project.Eyes = Eyes;
            _project.Ear = Ear;
            _project.Nose = Nose;
            _project.Face = Face;
            _project.Teeth = Teeth;
            _project.Mouth = Mouth;
            _project.Throat = Throat;
            _project.Thirst = Thirst;
            _project.Diet = Diet;
            _project.Abdomen = Abdomen;
            _project.Stool = Stool;
            _project.Urine = Urine;
            _project.Lungs = Lungs;
            _project.Heart = Heart;
            _project.Spine = Spine;
            _project.MaleGenitalOrganic = MaleGenitalOrganic;
            _project.MaleGenitalFunctional = MaleGenitalFunctional;
            _project.FemaleGenitalOrganic = FemaleGenitalOrganic;
            _project.FemaleGenitalFunctional = FemaleGenitalFunctional;
            _project.Sleep = Sleep;
            _project.Skin = Skin;
            _project.Fever = Fever;
            _project.Dreams = Dreams;
            _project.Aversions = Aversions;
            _project.Sweat = Sweat;
            _project.Side = Side;
            _project.Thermally = Thermally;
            _project.XRayFindings = XRayFindings;
            _project.USGOpinion = UsgOpinion;
            _project.CTScanFindings = CtScanFindings;
            _project.MRIFindings = MriFindings;

            _project.CategoryID = Category?.ID ?? 0;
            _project.Icon = Icon.Icon ?? FluentUI.ribbon_24_regular;

            await _projectRepository.SaveItemAsync(_project);

            if (_project.IsNullOrNew())
            {
                foreach (var tag in AllTags)
                {
                    if (tag.IsSelected)
                    {
                        await _tagRepository.SaveItemAsync(tag, _project.ID);
                    }
                }
            }

            foreach (var task in _project.Tasks)
            {
                if (task.ID == 0)
                {
                    task.ProjectID = _project.ID;
                    await _taskRepository.SaveItemAsync(task);
                }
            }

            await Shell.Current.GoToAsync("..");
            await AppShell.DisplayToastAsync("Project saved");
        }

        [RelayCommand]
        private async Task AddTask()
        {
            if (_project is null)
            {
                _errorHandler.HandleError(
                    new Exception("Project is null. Cannot navigate to task."));

                return;
            }

            // Pass the project so if this is a new project we can just add
            // the tasks to the project and then save them all from here.
            await Shell.Current.GoToAsync($"task",
                new ShellNavigationQueryParameters(){
                    {TaskDetailPageModel.ProjectQueryKey, _project}
                });
        }

        [RelayCommand]
        private async Task Delete()
        {
            if (_project.IsNullOrNew())
            {
                await Shell.Current.GoToAsync("..");
                return;
            }

            await _projectRepository.DeleteItemAsync(_project);
            await Shell.Current.GoToAsync("..");
            await AppShell.DisplayToastAsync("Project deleted");
        }

        [RelayCommand]
        private Task NavigateToTask(ProjectTask task) =>
            Shell.Current.GoToAsync($"task?id={task.ID}");

        [RelayCommand]
        private async Task ToggleTag(Tag tag)
        {
            tag.IsSelected = !tag.IsSelected;

            if (!_project.IsNullOrNew())
            {
                if (tag.IsSelected)
                {
                    await _tagRepository.SaveItemAsync(tag, _project.ID);
                }
                else
                {
                    await _tagRepository.DeleteItemAsync(tag, _project.ID);
                }
            }

            AllTags = new(AllTags);
        }

        [RelayCommand]
        private async Task CleanTasks()
        {
            var completedTasks = Tasks.Where(t => t.IsCompleted).ToArray();
            foreach (var task in completedTasks)
            {
                await _taskRepository.DeleteItemAsync(task);
                Tasks.Remove(task);
            }

            Tasks = new(Tasks);
            OnPropertyChanged(nameof(HasCompletedTasks));
            await AppShell.DisplayToastAsync("All cleaned up!");
        }
    }
}

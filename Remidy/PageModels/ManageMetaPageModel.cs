using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Remidy.Models;

namespace Remidy.PageModels
{
    public partial class ManageMetaPageModel(
        CategoryRepository categoryRepository,
        LookupRepositoryFactory lookupFactory,
        TagRepository tagRepository,
        SeedDataService seedDataService) : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Category> _categories = [];

        [ObservableProperty]
        private ObservableCollection<BMICategoryType> _bmiCategories = [];

        [ObservableProperty]
        private ObservableCollection<ConstitutionType> _constitutionTypes = [];

        [ObservableProperty]
        private ObservableCollection<NatureType> _natureTypes = [];

        [ObservableProperty]
        private ObservableCollection<LookType> _lookTypes = [];

        [ObservableProperty]
        private ObservableCollection<ComplexionType> _complexionTypes = [];

        [ObservableProperty]
        private ObservableCollection<TemperamentType> _temperamentTypes = [];

        [ObservableProperty]
        private ObservableCollection<HandType> _handTypes = [];

        [ObservableProperty]
        private ObservableCollection<FaceType> _faceTypes = [];

        [ObservableProperty]
        private ObservableCollection<NailType> _nailTypes = [];

        [ObservableProperty]
        private ObservableCollection<SpeakingType> _speakingTypes = [];

        [ObservableProperty]
        private ObservableCollection<TongueType> _tongueTypes = [];

        [ObservableProperty]
        private ObservableCollection<HairType> _hairTypes = [];

        [ObservableProperty]
        private ObservableCollection<EyeType> _eyeTypes = [];

        [ObservableProperty]
        private ObservableCollection<CauseOfDiseaseType> _causeOfDiseaseTypes = [];

        [ObservableProperty]
        private ObservableCollection<SensationType> _sensationTypes = [];

        [ObservableProperty]
        private ObservableCollection<TreatmentType> _treatmentTypes = [];

        [ObservableProperty]
        private ObservableCollection<RespondedToTreatmentType> _respondedToTreatmentTypes = [];

        [ObservableProperty]
        private ObservableCollection<BloodGroupType> _bloodGroupTypes = [];

        [ObservableProperty]
        private ObservableCollection<CaseConditionType> _caseConditionTypes = [];

        [ObservableProperty]
        private ObservableCollection<PersonalHabitType> _personalHabitTypes = [];

        [ObservableProperty]
        private ObservableCollection<Tag> _tags = [];

        private async Task LoadData()
        {
            var categoriesList = await categoryRepository.ListAsync();
            Categories = new ObservableCollection<Category>(categoriesList);
            
            var bmiList = await lookupFactory.GetRepository<BMICategoryType>().ListAsync();
            BmiCategories = new ObservableCollection<BMICategoryType>(bmiList);

            var constitutionList = await lookupFactory.GetRepository<ConstitutionType>().ListAsync();
            ConstitutionTypes = new ObservableCollection<ConstitutionType>(constitutionList);

            var natureList = await lookupFactory.GetRepository<NatureType>().ListAsync();
            NatureTypes = new ObservableCollection<NatureType>(natureList);

            var lookList = await lookupFactory.GetRepository<LookType>().ListAsync();
            LookTypes = new ObservableCollection<LookType>(lookList);

            var temperamentList = await lookupFactory.GetRepository<TemperamentType>().ListAsync();
            TemperamentTypes = new ObservableCollection<TemperamentType>(temperamentList);

            var complexionList = await lookupFactory.GetRepository<ComplexionType>().ListAsync();
            ComplexionTypes = new ObservableCollection<ComplexionType>(complexionList);

            var handList = await lookupFactory.GetRepository<HandType>().ListAsync();
            HandTypes = new ObservableCollection<HandType>(handList);

            var faceList = await lookupFactory.GetRepository<FaceType>().ListAsync();
            FaceTypes = new ObservableCollection<FaceType>(faceList);

            var nailList = await lookupFactory.GetRepository<NailType>().ListAsync();
            NailTypes = new ObservableCollection<NailType>(nailList);

            var speakingList = await lookupFactory.GetRepository<SpeakingType>().ListAsync();
            SpeakingTypes = new ObservableCollection<SpeakingType>(speakingList);

            var tongueList = await lookupFactory.GetRepository<TongueType>().ListAsync();
            TongueTypes = new ObservableCollection<TongueType>(tongueList);

            var hairList = await lookupFactory.GetRepository<HairType>().ListAsync();
            HairTypes = new ObservableCollection<HairType>(hairList);

            var eyeList = await lookupFactory.GetRepository<EyeType>().ListAsync();
            EyeTypes = new ObservableCollection<EyeType>(eyeList);

            var causeList = await lookupFactory.GetRepository<CauseOfDiseaseType>().ListAsync();
            CauseOfDiseaseTypes = new ObservableCollection<CauseOfDiseaseType>(causeList);

            var sensationList = await lookupFactory.GetRepository<SensationType>().ListAsync();
            SensationTypes = new ObservableCollection<SensationType>(sensationList);

            var treatmentList = await lookupFactory.GetRepository<TreatmentType>().ListAsync();
            TreatmentTypes = new ObservableCollection<TreatmentType>(treatmentList);

            var respondedList = await lookupFactory.GetRepository<RespondedToTreatmentType>().ListAsync();
            RespondedToTreatmentTypes = new ObservableCollection<RespondedToTreatmentType>(respondedList);

            var bloodList = await lookupFactory.GetRepository<BloodGroupType>().ListAsync();
            BloodGroupTypes = new ObservableCollection<BloodGroupType>(bloodList);

            var caseList = await lookupFactory.GetRepository<CaseConditionType>().ListAsync();
            CaseConditionTypes = new ObservableCollection<CaseConditionType>(caseList);

            var habitList = await lookupFactory.GetRepository<PersonalHabitType>().ListAsync();
            PersonalHabitTypes = new ObservableCollection<PersonalHabitType>(habitList);

            var tagsList = await tagRepository.ListAsync();
            Tags = new ObservableCollection<Tag>(tagsList);
        }

        [RelayCommand]
        private Task Appearing()
            => LoadData();

        #region Categories
        [RelayCommand]
        private async Task SaveCategories()
        {
            foreach (var category in Categories)
            {
                await categoryRepository.SaveItemAsync(category);
            }

            await AppShell.DisplayToastAsync("Categories saved");
        }

        [RelayCommand]
        private async Task DeleteCategory(Category category)
        {
            Categories.Remove(category);
            await categoryRepository.DeleteItemAsync(category);
            await AppShell.DisplayToastAsync("Category deleted");
        }

        [RelayCommand]
        private async Task AddCategory()
        {
            var category = new Category();
            Categories.Add(category);
            await categoryRepository.SaveItemAsync(category);
            await AppShell.DisplayToastAsync("Category added");
        }
        #endregion

        #region BMI
        [RelayCommand]
        private async Task SaveBMICategories()
        {
            foreach (var bmiCategory in BmiCategories)
            {
                await lookupFactory.GetRepository<BMICategoryType>().SaveItemAsync(bmiCategory);
            }

            await AppShell.DisplayToastAsync("BMI Categories saved");
        }

        [RelayCommand]
        private async Task DeleteBmiCategory(BMICategoryType category)
        {
            BmiCategories.Remove(category);
            await lookupFactory.GetRepository<BMICategoryType>().DeleteItemAsync(category);
            await AppShell.DisplayToastAsync("BMI Category deleted");
        }

        [RelayCommand]
        private async Task AddBmiCategory()
        {
            var category = new BMICategoryType();
            BmiCategories.Add(category);
            await lookupFactory.GetRepository<BMICategoryType>().SaveItemAsync(category);
            await AppShell.DisplayToastAsync("BMI Category added");
        }
        #endregion

        #region Constitution
        [RelayCommand]
        private async Task SaveConstitutionTypes()
        {
            foreach (var constitutionType in ConstitutionTypes)
            {
                await lookupFactory.GetRepository<ConstitutionType>().SaveItemAsync(constitutionType);
            }

            await AppShell.DisplayToastAsync("Constitutions saved");
        }

        [RelayCommand]
        private async Task DeleteConstitutionType(ConstitutionType category)
        {
            ConstitutionTypes.Remove(category);
            await lookupFactory.GetRepository<ConstitutionType>().DeleteItemAsync(category);
            await AppShell.DisplayToastAsync("Constitution deleted");
        }

        [RelayCommand]
        private async Task AddConstitutionType()
        {
            var category = new ConstitutionType();
            ConstitutionTypes.Add(category);
            await lookupFactory.GetRepository<ConstitutionType>().SaveItemAsync(category);
            await AppShell.DisplayToastAsync("Constitution added");
        }
        #endregion

        #region Nature
        [RelayCommand]
        private async Task SaveNatureTypes()
        {
            foreach (var natureType in NatureTypes)
            {
                await lookupFactory.GetRepository<NatureType>().SaveItemAsync(natureType);
            }

            await AppShell.DisplayToastAsync("Natures saved");
        }

        [RelayCommand]
        private async Task DeleteNatureType(NatureType nature)
        {
            NatureTypes.Remove(nature);
            await lookupFactory.GetRepository<NatureType>().DeleteItemAsync(nature);
            await AppShell.DisplayToastAsync("Nature deleted");
        }

        [RelayCommand]
        private async Task AddNatureType()
        {
            var nature = new NatureType();
            NatureTypes.Add(nature);
            await lookupFactory.GetRepository<NatureType>().SaveItemAsync(nature);
            await AppShell.DisplayToastAsync("Nature added");
        }
        #endregion

        #region Complexion
        [RelayCommand]
        private async Task SaveComplexionTypes()
        {
            foreach (var complexionType in ComplexionTypes)
            {
                await lookupFactory.GetRepository<ComplexionType>().SaveItemAsync(complexionType);
            }

            await AppShell.DisplayToastAsync("Complexions saved");
        }

        [RelayCommand]
        private async Task DeleteComplexionType(ComplexionType complexion)
        {
            ComplexionTypes.Remove(complexion);
            await lookupFactory.GetRepository<ComplexionType>().DeleteItemAsync(complexion);
            await AppShell.DisplayToastAsync("Complexion deleted");
        }

        [RelayCommand]
        private async Task AddComplexionType()
        {
            var complexion = new ComplexionType();
            ComplexionTypes.Add(complexion);
            await lookupFactory.GetRepository<ComplexionType>().SaveItemAsync(complexion);
            await AppShell.DisplayToastAsync("Complexion added");
        }
        #endregion

        #region Look
        [RelayCommand]
        private async Task SaveLookTypes()
        {
            foreach (var lookType in LookTypes)
            {
                await lookupFactory.GetRepository<LookType>().SaveItemAsync(lookType);
            }

            await AppShell.DisplayToastAsync("Looks saved");
        }

        [RelayCommand]
        private async Task DeleteLookType(LookType look)
        {
            LookTypes.Remove(look);
            await lookupFactory.GetRepository<LookType>().DeleteItemAsync(look);
            await AppShell.DisplayToastAsync("Look deleted");
        }

        [RelayCommand]
        private async Task AddLookType()
        {
            var look = new LookType();
            LookTypes.Add(look);
            await lookupFactory.GetRepository<LookType>().SaveItemAsync(look);
            await AppShell.DisplayToastAsync("Look added");
        }
        #endregion

        #region Temperament
        [RelayCommand]
        private async Task SaveTemperamentTypes()
        {
            foreach (var temperamentType in TemperamentTypes)
            {
                await lookupFactory.GetRepository<TemperamentType>().SaveItemAsync(temperamentType);
            }

            await AppShell.DisplayToastAsync("Temperaments saved");
        }

        [RelayCommand]
        private async Task DeleteTemperamentType(TemperamentType temperament)
        {
            TemperamentTypes.Remove(temperament);
            await lookupFactory.GetRepository<TemperamentType>().DeleteItemAsync(temperament);
            await AppShell.DisplayToastAsync("Temperament deleted");
        }

        [RelayCommand]
        private async Task AddTemperamentType()
        {
            var temperament = new TemperamentType();
            TemperamentTypes.Add(temperament);
            await lookupFactory.GetRepository<TemperamentType>().SaveItemAsync(temperament);
            await AppShell.DisplayToastAsync("Temperament added");
        }
        #endregion

        #region Hand
        [RelayCommand]
        private async Task SaveHandTypes()
        {
            foreach (var handType in HandTypes)
            {
                await lookupFactory.GetRepository<HandType>().SaveItemAsync(handType);
            }

            await AppShell.DisplayToastAsync("Hands saved");
        }

        [RelayCommand]
        private async Task DeleteHandType(HandType hand)
        {
            HandTypes.Remove(hand);
            await lookupFactory.GetRepository<HandType>().DeleteItemAsync(hand);
            await AppShell.DisplayToastAsync("Hand deleted");
        }

        [RelayCommand]
        private async Task AddHandType()
        {
            var hand = new HandType();
            HandTypes.Add(hand);
            await lookupFactory.GetRepository<HandType>().SaveItemAsync(hand);
            await AppShell.DisplayToastAsync("Hand added");
        }
        #endregion

        #region Face
        [RelayCommand]
        private async Task SaveFaceTypes()
        {
            foreach (var faceType in FaceTypes)
            {
                await lookupFactory.GetRepository<FaceType>().SaveItemAsync(faceType);
            }

            await AppShell.DisplayToastAsync("Faces saved");
        }

        [RelayCommand]
        private async Task DeleteFaceType(FaceType face)
        {
            FaceTypes.Remove(face);
            await lookupFactory.GetRepository<FaceType>().DeleteItemAsync(face);
            await AppShell.DisplayToastAsync("Face deleted");
        }

        [RelayCommand]
        private async Task AddFaceType()
        {
            var face = new FaceType();
            FaceTypes.Add(face);
            await lookupFactory.GetRepository<FaceType>().SaveItemAsync(face);
            await AppShell.DisplayToastAsync("Face added");
        }
        #endregion

        #region Nail
        [RelayCommand]
        private async Task SaveNailTypes()
        {
            foreach (var nailType in NailTypes)
            {
                await lookupFactory.GetRepository<NailType>().SaveItemAsync(nailType);
            }

            await AppShell.DisplayToastAsync("Nails saved");
        }

        [RelayCommand]
        private async Task DeleteNailType(NailType nail)
        {
            NailTypes.Remove(nail);
            await lookupFactory.GetRepository<NailType>().DeleteItemAsync(nail);
            await AppShell.DisplayToastAsync("Nail deleted");
        }

        [RelayCommand]
        private async Task AddNailType()
        {
            var nail = new NailType();
            NailTypes.Add(nail);
            await lookupFactory.GetRepository<NailType>().SaveItemAsync(nail);
            await AppShell.DisplayToastAsync("Nail added");
        }
        #endregion

        #region Speaking
        [RelayCommand]
        private async Task SaveSpeakingTypes()
        {
            foreach (var speakingType in SpeakingTypes)
            {
                await lookupFactory.GetRepository<SpeakingType>().SaveItemAsync(speakingType);
            }

            await AppShell.DisplayToastAsync("Speakings saved");
        }

        [RelayCommand]
        private async Task DeleteSpeakingType(SpeakingType speaking)
        {
            SpeakingTypes.Remove(speaking);
            await lookupFactory.GetRepository<SpeakingType>().DeleteItemAsync(speaking);
            await AppShell.DisplayToastAsync("Speaking deleted");
        }

        [RelayCommand]
        private async Task AddSpeakingType()
        {
            var speaking = new SpeakingType();
            SpeakingTypes.Add(speaking);
            await lookupFactory.GetRepository<SpeakingType>().SaveItemAsync(speaking);
            await AppShell.DisplayToastAsync("Speaking added");
        }
        #endregion

        #region Tongue
        [RelayCommand]
        private async Task SaveTongueTypes()
        {
            foreach (var tongueType in TongueTypes)
            {
                await lookupFactory.GetRepository<TongueType>().SaveItemAsync(tongueType);
            }

            await AppShell.DisplayToastAsync("Tongues saved");
        }

        [RelayCommand]
        private async Task DeleteTongueType(TongueType tongue)
        {
            TongueTypes.Remove(tongue);
            await lookupFactory.GetRepository<TongueType>().DeleteItemAsync(tongue);
            await AppShell.DisplayToastAsync("Tongue deleted");
        }

        [RelayCommand]
        private async Task AddTongueType()
        {
            var tongue = new TongueType();
            TongueTypes.Add(tongue);
            await lookupFactory.GetRepository<TongueType>().SaveItemAsync(tongue);
            await AppShell.DisplayToastAsync("Tongue added");
        }
        #endregion

        #region Hair
        [RelayCommand]
        private async Task SaveHairTypes()
        {
            foreach (var hairType in HairTypes)
            {
                await lookupFactory.GetRepository<HairType>().SaveItemAsync(hairType);
            }

            await AppShell.DisplayToastAsync("Hairs saved");
        }

        [RelayCommand]
        private async Task DeleteHairType(HairType hair)
        {
            HairTypes.Remove(hair);
            await lookupFactory.GetRepository<HairType>().DeleteItemAsync(hair);
            await AppShell.DisplayToastAsync("Hair deleted");
        }

        [RelayCommand]
        private async Task AddHairType()
        {
            var hair = new HairType();
            HairTypes.Add(hair);
            await lookupFactory.GetRepository<HairType>().SaveItemAsync(hair);
            await AppShell.DisplayToastAsync("Hair added");
        }
        #endregion

        #region Eye
        [RelayCommand]
        private async Task SaveEyeTypes()
        {
            foreach (var eyeType in EyeTypes)
            {
                await lookupFactory.GetRepository<EyeType>().SaveItemAsync(eyeType);
            }

            await AppShell.DisplayToastAsync("Eyes saved");
        }

        [RelayCommand]
        private async Task DeleteEyeType(EyeType eye)
        {
            EyeTypes.Remove(eye);
            await lookupFactory.GetRepository<EyeType>().DeleteItemAsync(eye);
            await AppShell.DisplayToastAsync("Eye deleted");
        }

        [RelayCommand]
        private async Task AddEyeType()
        {
            var eye = new EyeType();
            EyeTypes.Add(eye);
            await lookupFactory.GetRepository<EyeType>().SaveItemAsync(eye);
            await AppShell.DisplayToastAsync("Eye added");
        }
        #endregion

        #region CauseOfDisease
        [RelayCommand]
        private async Task SaveCauseOfDiseaseTypes()
        {
            foreach (var causeType in CauseOfDiseaseTypes)
            {
                await lookupFactory.GetRepository<CauseOfDiseaseType>().SaveItemAsync(causeType);
            }

            await AppShell.DisplayToastAsync("Causes of Disease saved");
        }

        [RelayCommand]
        private async Task DeleteCauseOfDiseaseType(CauseOfDiseaseType cause)
        {
            CauseOfDiseaseTypes.Remove(cause);
            await lookupFactory.GetRepository<CauseOfDiseaseType>().DeleteItemAsync(cause);
            await AppShell.DisplayToastAsync("Cause of Disease deleted");
        }

        [RelayCommand]
        private async Task AddCauseOfDiseaseType()
        {
            var cause = new CauseOfDiseaseType();
            CauseOfDiseaseTypes.Add(cause);
            await lookupFactory.GetRepository<CauseOfDiseaseType>().SaveItemAsync(cause);
            await AppShell.DisplayToastAsync("Cause of Disease added");
        }
        #endregion

        #region Sensation
        [RelayCommand]
        private async Task SaveSensationTypes()
        {
            foreach (var sensationType in SensationTypes)
            {
                await lookupFactory.GetRepository<SensationType>().SaveItemAsync(sensationType);
            }

            await AppShell.DisplayToastAsync("Sensations saved");
        }

        [RelayCommand]
        private async Task DeleteSensationType(SensationType sensation)
        {
            SensationTypes.Remove(sensation);
            await lookupFactory.GetRepository<SensationType>().DeleteItemAsync(sensation);
            await AppShell.DisplayToastAsync("Sensation deleted");
        }

        [RelayCommand]
        private async Task AddSensationType()
        {
            var sensation = new SensationType();
            SensationTypes.Add(sensation);
            await lookupFactory.GetRepository<SensationType>().SaveItemAsync(sensation);
            await AppShell.DisplayToastAsync("Sensation added");
        }
        #endregion

        #region Treatment
        [RelayCommand]
        private async Task SaveTreatmentTypes()
        {
            foreach (var treatmentType in TreatmentTypes)
            {
                await lookupFactory.GetRepository<TreatmentType>().SaveItemAsync(treatmentType);
            }

            await AppShell.DisplayToastAsync("Treatments saved");
        }

        [RelayCommand]
        private async Task DeleteTreatmentType(TreatmentType treatment)
        {
            TreatmentTypes.Remove(treatment);
            await lookupFactory.GetRepository<TreatmentType>().DeleteItemAsync(treatment);
            await AppShell.DisplayToastAsync("Treatment deleted");
        }

        [RelayCommand]
        private async Task AddTreatmentType()
        {
            var treatment = new TreatmentType();
            TreatmentTypes.Add(treatment);
            await lookupFactory.GetRepository<TreatmentType>().SaveItemAsync(treatment);
            await AppShell.DisplayToastAsync("Treatment added");
        }
        #endregion

        #region RespondedToTreatment
        [RelayCommand]
        private async Task SaveRespondedToTreatmentTypes()
        {
            foreach (var respondedType in RespondedToTreatmentTypes)
            {
                await lookupFactory.GetRepository<RespondedToTreatmentType>().SaveItemAsync(respondedType);
            }

            await AppShell.DisplayToastAsync("Treatment Responses saved");
        }

        [RelayCommand]
        private async Task DeleteRespondedToTreatmentType(RespondedToTreatmentType responded)
        {
            RespondedToTreatmentTypes.Remove(responded);
            await lookupFactory.GetRepository<RespondedToTreatmentType>().DeleteItemAsync(responded);
            await AppShell.DisplayToastAsync("Treatment Response deleted");
        }

        [RelayCommand]
        private async Task AddRespondedToTreatmentType()
        {
            var responded = new RespondedToTreatmentType();
            RespondedToTreatmentTypes.Add(responded);
            await lookupFactory.GetRepository<RespondedToTreatmentType>().SaveItemAsync(responded);
            await AppShell.DisplayToastAsync("Treatment Response added");
        }
        #endregion

        #region BloodGroup
        [RelayCommand]
        private async Task SaveBloodGroupTypes()
        {
            foreach (var bloodGroupType in BloodGroupTypes)
            {
                await lookupFactory.GetRepository<BloodGroupType>().SaveItemAsync(bloodGroupType);
            }

            await AppShell.DisplayToastAsync("Blood Groups saved");
        }

        [RelayCommand]
        private async Task DeleteBloodGroupType(BloodGroupType bloodGroup)
        {
            BloodGroupTypes.Remove(bloodGroup);
            await lookupFactory.GetRepository<BloodGroupType>().DeleteItemAsync(bloodGroup);
            await AppShell.DisplayToastAsync("Blood Group deleted");
        }

        [RelayCommand]
        private async Task AddBloodGroupType()
        {
            var bloodGroup = new BloodGroupType();
            BloodGroupTypes.Add(bloodGroup);
            await lookupFactory.GetRepository<BloodGroupType>().SaveItemAsync(bloodGroup);
            await AppShell.DisplayToastAsync("Blood Group added");
        }
        #endregion

        #region CaseCondition
        [RelayCommand]
        private async Task SaveCaseConditionTypes()
        {
            foreach (var caseConditionType in CaseConditionTypes)
            {
                await lookupFactory.GetRepository<CaseConditionType>().SaveItemAsync(caseConditionType);
            }

            await AppShell.DisplayToastAsync("Case Conditions saved");
        }

        [RelayCommand]
        private async Task DeleteCaseConditionType(CaseConditionType caseCondition)
        {
            CaseConditionTypes.Remove(caseCondition);
            await lookupFactory.GetRepository<CaseConditionType>().DeleteItemAsync(caseCondition);
            await AppShell.DisplayToastAsync("Case Condition deleted");
        }

        [RelayCommand]
        private async Task AddCaseConditionType()
        {
            var caseCondition = new CaseConditionType();
            CaseConditionTypes.Add(caseCondition);
            await lookupFactory.GetRepository<CaseConditionType>().SaveItemAsync(caseCondition);
            await AppShell.DisplayToastAsync("Case Condition added");
        }
        #endregion

        #region PersonalHabit
        [RelayCommand]
        private async Task SavePersonalHabitTypes()
        {
            foreach (var personalHabitType in PersonalHabitTypes)
            {
                await lookupFactory.GetRepository<PersonalHabitType>().SaveItemAsync(personalHabitType);
            }

            await AppShell.DisplayToastAsync("Personal Habits saved");
        }

        [RelayCommand]
        private async Task DeletePersonalHabitType(PersonalHabitType personalHabit)
        {
            PersonalHabitTypes.Remove(personalHabit);
            await lookupFactory.GetRepository<PersonalHabitType>().DeleteItemAsync(personalHabit);
            await AppShell.DisplayToastAsync("Personal Habit deleted");
        }

        [RelayCommand]
        private async Task AddPersonalHabitType()
        {
            var personalHabit = new PersonalHabitType();
            PersonalHabitTypes.Add(personalHabit);
            await lookupFactory.GetRepository<PersonalHabitType>().SaveItemAsync(personalHabit);
            await AppShell.DisplayToastAsync("Personal Habit added");
        }
        #endregion

        #region Tags
        [RelayCommand]
        private async Task SaveTags()
        {
            foreach (var tag in Tags)
            {
                await tagRepository.SaveItemAsync(tag);
            }

            await AppShell.DisplayToastAsync("Tags saved");
        }

        [RelayCommand]
        private async Task DeleteTag(Tag tag)
        {
            Tags.Remove(tag);
            await tagRepository.DeleteItemAsync(tag);
            await AppShell.DisplayToastAsync("Tag deleted");
        }

        [RelayCommand]
        private async Task AddTag()
        {
            var tag = new Tag();
            Tags.Add(tag);
            await tagRepository.SaveItemAsync(tag);
            await AppShell.DisplayToastAsync("Tag added");
        }
        #endregion

        [RelayCommand]
        private async Task Reset()
        {
            Preferences.Default.Remove("is_seeded");
            await seedDataService.LoadSeedDataAsync();
            Preferences.Default.Set("is_seeded", true);
            await Shell.Current.GoToAsync("//main");
        }
    }
}

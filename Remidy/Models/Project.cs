using System.Text.Json.Serialization;

namespace Remidy.Models
{
    /// <summary>
    /// Core data model representing a patient case record in the medical management system.
    /// Contains comprehensive patient information including demographics, vital signs, 
    /// medical classifications, examination findings, and treatment details.
    /// </summary>
    /// <remarks>
    /// Data Categories:
    /// - Core Identity: ID, Name, Description, Registration details
    /// - Demographics: Age, Gender, Contact information, Family details
    /// - Vital Signs: BP, Temperature, Pulse, Respiration, Height, Weight
    /// - Medical Classifications: 20+ lookup type references (BMI, Constitution, etc.)
    /// - Physical Examination: Face, Hands, Eyes, Hair, and other body systems
    /// - Medical History: Present complaints, previous treatments, conditions
    /// - Case Management: Categories, dates, status tracking
    /// 
    /// Database Design:
    /// - Primary entity in the medical case management system
    /// - Foreign key relationships to all lookup types via [TypeName]Id properties
    /// - Navigation properties for complex types (marked with [JsonIgnore])
    /// - Nullable fields to accommodate partial data entry during case development
    /// 
    /// Usage:
    /// - Created and managed through ProjectDetailPageModel
    /// - Persisted via ProjectRepository using SQLite
    /// - Displayed in lists via ProjectListPageModel
    /// - Supports comprehensive search and filtering capabilities
    /// 
    /// Architecture Notes:
    /// - Uses int foreign keys for lookup type relationships
    /// - Navigation properties are JsonIgnore'd to prevent serialization issues
    /// - Designed for both new case creation and existing case modification
    /// - Supports partial completion with nullable properties
    /// </remarks>
    public class Project
    {
        /// <summary>Primary key identifier for the patient case record.</summary>
        public int ID { get; set; }
        
        /// <summary>Patient's full name.</summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>Additional description or relationship information (SO/DO/WO).</summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>Icon identifier for UI display (currently unused).</summary>
        public string Icon { get; set; } = string.Empty;

        #region General Information
        /// <summary>Unique registration number for the patient case.</summary>
        public string RegistrationNo { get; set; }
        
        /// <summary>Date of case registration or last visit.</summary>
        public DateTime Date { get; set; }
        
        /// <summary>Time of case registration or last visit.</summary>
        public TimeSpan Time { get; set; }
        
        /// <summary>Patient's mother's name for additional identification.</summary>
        public string MotherName { get; set; }
        
        /// <summary>Marital status (Single, Divorced, Widowed, Other).</summary>
        public string MaritalStatus { get; set; } // Could be enum: S, D, W, O
        
        /// <summary>Number of children (applicable for married patients).</summary>
        public int? Children { get; set; }
        
        /// <summary>Patient's caste information.</summary>
        public string Caste { get; set; }
        
        /// <summary>Patient's age in years.</summary>
        public int? Age { get; set; }
        
        /// <summary>Patient's religious affiliation.</summary>
        public string Religion { get; set; }
        
        /// <summary>Patient's occupation or profession.</summary>
        public string Occupation { get; set; }
        
        /// <summary>Patient's residential address.</summary>
        public string Address { get; set; }
        
        /// <summary>Patient's contact phone number.</summary>
        public string ContactNo { get; set; }
        #endregion

        #region Vital Signs
        /// <summary>Blood pressure measurement (could be enhanced to support systolic/diastolic separately).</summary>
        public double? BP { get; set; }
        
        /// <summary>Body temperature measurement.</summary>
        public double? Temperature { get; set; }
        
        /// <summary>Pulse rate per minute.</summary>
        public int? Pulse { get; set; }
        
        /// <summary>Respiration rate per minute.</summary>
        public int? Respiration { get; set; }
        
        /// <summary>Patient height in centimeters.</summary>
        public double? Height { get; set; }
        
        /// <summary>Patient weight in kilograms.</summary>
        public double? Weight { get; set; }
        
        /// <summary>Calculated or manually entered BMI value.</summary>
        public double? BMI { get; set; }
        #endregion

        [JsonIgnore]
        public int BmiCategoryId { get; set; }
        public BMICategoryType BmiCategory { get; set; }

        // ConstitutionType
        [JsonIgnore]
        public int ConstitutionTypeId { get; set; }

        public ConstitutionType? ConstitutionType { get; set; }

        // Nature
        [JsonIgnore]
        public int NatureTypeId { get; set; }
        public NatureType? NatureType { get; set; }

        // Look
        [JsonIgnore]
        public int LookTypeId { get; set; }
        public LookType? LookType { get; set; }

        // Complexion
        [JsonIgnore]
        public int ComplexionTypeId { get; set; }
        public ComplexionType? ComplexionType { get; set; }

        // Temperament
        [JsonIgnore]
        public int TemperamentTypeId { get; set; }
        public TemperamentType TemperamentType { get; set; }

        // FaceType
        [JsonIgnore]
        public int FaceTypeId { get; set; }
        public FaceType FaceType { get; set; }

        // NailType
        [JsonIgnore]
        public int NailTypeId { get; set; }
        public NailType NailType { get; set; }

        // SpeakingType
        [JsonIgnore]
        public int SpeakingTypeId { get; set; }
        public SpeakingType SpeakingType { get; set; }

        // TongueType
        [JsonIgnore]
        public int TongueTypeId { get; set; }
        public TongueType TongueType { get; set; }

        // HairType
        [JsonIgnore]
        public int HairTypeId { get; set; }
        public HairType? HairType { get; set; }

        // Eyes
        [JsonIgnore]
        public int EyeColoryId { get; set; }
        public EyeType? EyeColor { get; set; }

        // CauseOfDisease
        [JsonIgnore]
        public int CauseOfDiseaseTypeId { get; set; }
        public CauseOfDiseaseType? CauseOfDiseaseType { get; set; }

        // Sensation
        [JsonIgnore]
        public int SensationTypeId { get; set; }
        public SensationType? SensationType { get; set; }

        // Treatment
        [JsonIgnore]
        public int TreatmentTypeId { get; set; }
        public TreatmentType? TreatmentType { get; set; }

        // Responded To Treatment
        [JsonIgnore]
        public int RespondedToTreatmentTypeId { get; set; }
        public RespondedToTreatmentType? RespondedToTreatmentType { get; set; }

        // Blood Group
        [JsonIgnore]
        public int BloodGroupTypeId { get; set; }
        public BloodGroupType? BloodGroupType { get; set; }

        // Case Condition
        [JsonIgnore]
        public int CaseConditionTypeId { get; set; }
        public CaseConditionType? CaseConditionType { get; set; }

        // Personal Habit
        /// <summary>Foreign key reference to personal habit classification.</summary>
        [JsonIgnore]
        public int PersonalHabitTypeId { get; set; }
        
        /// <summary>Navigation property for personal habit classification (smoking, drinking, etc.).</summary>
        public PersonalHabitType? PersonalHabitType { get; set; }

        #region Medical History & Complaints
        /// <summary>
        /// Patient's present complaints documented in chronological order.
        /// Critical field containing location, sensation, modalities, amelioration, and other symptoms.
        /// Used for case assessment, diagnosis, and treatment planning.
        /// </summary>
        public string PresentComplaints { get; set; }
        #endregion

        public string Pecular { get; set; }

        public string Queer { get; set; }

        public string Strange { get; set; }

        public string Likes { get; set; }

        public string Dislikes { get; set; }

        public string Head { get; set; }

        public string Eyes { get; set; }

        public string Ear { get; set; }
        public string Nose { get; set; }

        public string Face { get; set; }

        public string Teeth { get; set; }

        public string Mouth { get; set; }

        public string Throat { get; set; }

        public string Thirst { get; set; }

        public string Diet { get; set; }

        public string Abdomen { get; set; }

        public string Stool { get; set; }

        public string Urine { get; set; }

        public string Lungs { get; set; }

        public string Heart { get; set; }

        public string Spine { get; set; }

        public string MaleGenitalOrganic { get; set; }

        public string MaleGenitalFunctional { get; set; }

        public string FemaleGenitalOrganic { get; set; }

        public string FemaleGenitalFunctional { get; set; }

        public string Sleep { get; set; }

        public string Skin { get; set; }

        public string Fever { get; set; }

        public string Dreams { get; set; }

        public string Aversions { get; set; }

        public string Sweat { get; set; }

        public string Side { get; set; }

        public string Thermally { get; set; }

        public string XRayFindings { get; set; }

        public string USGOpinion { get; set; }

        public string CTScanFindings { get; set; }

        public string MRIFindings { get; set; }

        [JsonIgnore]
        public int CategoryID { get; set; }

        public Category? Category { get; set; }

        public List<ProjectTask> Tasks { get; set; } = [];

        public List<Tag> Tags { get; set; } = [];

        public override string ToString() => $"{Name}";
    }

    public class ProjectsJson
    {
        public List<Project> Projects { get; set; } = [];
    }
}
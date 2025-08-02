using System.Text.Json.Serialization;

namespace Remidy.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        // General Information
        public string RegistrationNo { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string MotherName { get; set; }
        public string MaritalStatus { get; set; } // Could be enum: S, D, W, O
        public int? Children { get; set; }
        public string Caste { get; set; }
        public int? Age { get; set; }
        public string Religion { get; set; }
        public string Occupation { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }

        // Vital Signs
        public double? BP { get; set; } // Can use a separate class if systolic/diastolic needed
        public double? Temperature { get; set; }
        public int? Pulse { get; set; }
        public int? Respiration { get; set; }
        public double? Height { get; set; } // in cm
        public double? Weight { get; set; } // in kg
        public double? BMI { get; set; }

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
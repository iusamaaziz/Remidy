using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Remidy.Models;

namespace Remidy.Data
{
    /// <summary>
    /// Repository class for managing projects in the database.
    /// </summary>
    public class ProjectRepository
    {
        private bool _hasBeenInitialized = false;
        private readonly ILogger _logger;
        private readonly TaskRepository _taskRepository;
        private readonly TagRepository _tagRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectRepository"/> class.
        /// </summary>
        /// <param name="taskRepository">The task repository instance.</param>
        /// <param name="tagRepository">The tag repository instance.</param>
        /// <param name="logger">The logger instance.</param>
        public ProjectRepository(TaskRepository taskRepository, TagRepository tagRepository, ILogger<ProjectRepository> logger)
        {
            _taskRepository = taskRepository;
            _tagRepository = tagRepository;
            _logger = logger;
        }

        /// <summary>
        /// Initializes the database connection and creates the Project table if it does not exist.
        /// </summary>
        private async Task Init()
        {
            if (_hasBeenInitialized)
                return;

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            try
            {
                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Project (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Description TEXT NOT NULL,
                Icon TEXT NOT NULL,

                -- General Information
                RegistrationNo TEXT,
                Date TEXT, -- stored in ISO8601 string format
                Time TEXT, -- stored as HH:mm:ss
                MotherName TEXT,
                MaritalStatus TEXT,
                Children INTEGER,
                Caste TEXT,
                Age INTEGER,
                Religion TEXT,
                Occupation TEXT,
                Address TEXT,
                ContactNo TEXT,

                -- Vital Signs
                BP REAL,
                Temperature REAL,
                Pulse INTEGER,
                Respiration INTEGER,
                Height REAL,
                Weight REAL,
                BMI REAL,
                BmiCategoryID INTEGER,
                ConstitutionTypeId INTEGER,
                NatureTypeId INTEGER,
                LookTypeId INTEGER,
                ComplexionTypeId INTEGER,
                TemperamentTypeId INTEGER,
                FaceTypeId INTEGER,
                NailTypeId INTEGER,
                SpeakingTypeId INTEGER,
                TongueTypeId INTEGER,
                HairTypeId INTEGER,
                EyeColoryId INTEGER,
                CategoryID INTEGER,
                Pecular TEXT,
                Queer TEXT,
                Strange TEXT,
                Likes TEXT,
                Dislikes TEXT,
                Head TEXT,
                Eyes TEXT,
                Ear TEXT,
                Face TEXT,
                Teeth TEXT,
                Mouth TEXT,
                Throat TEXT,
                Thirst TEXT,
                Diet TEXT,
                Abdomen TEXT,
                Stool TEXT,
                Urine TEXT,
                Lungs TEXT,
                Heart TEXT,
                Spine TEXT,
                MaleGenitalOrganic TEXT,
                MaleGenitalFunctional TEXT,
                FemaleGenitalOrganic TEXT,
                FemaleGenitalFunctional TEXT,
                Sleep TEXT,
                Skin TEXT,
                Fever TEXT,
                Dreams TEXT,
                Aversions TEXT,
                Sweat TEXT,
                Side TEXT,
                Thermally TEXT,
                XRayFindings TEXT,
                USGOpinion TEXT,
                CTScanFindings TEXT,
                MRIFindings TEXT,
                Nose TEXT
            );";
                await createTableCmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating Project table");
                throw;
            }

            _hasBeenInitialized = true;
        }

        /// <summary>
        /// Retrieves a list of all projects from the database.
        /// </summary>
        /// <returns>A list of <see cref="Project"/> objects.</returns>
        public async Task<List<Project>> ListAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM Project";
            var projects = new List<Project>();

            await using var reader = await selectCmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                projects.Add(new Project
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Icon = reader.GetString(3),
                    RegistrationNo = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Date = reader.IsDBNull(5) ? DateTime.MinValue : DateTime.Parse(reader.GetString(5)),
                    Time = reader.IsDBNull(6) ? TimeSpan.Zero : TimeSpan.Parse(reader.GetString(6)),
                    MotherName = reader.IsDBNull(7) ? null : reader.GetString(7),
                    MaritalStatus = reader.IsDBNull(8) ? null : reader.GetString(8),
                    Children = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                    Caste = reader.IsDBNull(10) ? null : reader.GetString(10),
                    Age = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                    Religion = reader.IsDBNull(12) ? null : reader.GetString(12),
                    Occupation = reader.IsDBNull(13) ? null : reader.GetString(13),
                    Address = reader.IsDBNull(14) ? null : reader.GetString(14),
                    ContactNo = reader.IsDBNull(15) ? null : reader.GetString(15),

                    BP = reader.IsDBNull(16) ? null : reader.GetDouble(16),
                    Temperature = reader.IsDBNull(17) ? null : reader.GetDouble(17),
                    Pulse = reader.IsDBNull(18) ? null : reader.GetInt32(18),
                    Respiration = reader.IsDBNull(19) ? null : reader.GetInt32(19),
                    Height = reader.IsDBNull(20) ? null : reader.GetDouble(20),
                    Weight = reader.IsDBNull(21) ? null : reader.GetDouble(21),
                    BMI = reader.IsDBNull(22) ? null : reader.GetDouble(22),

                    BmiCategoryId = reader.GetInt32(23),
                    ConstitutionTypeId = reader.IsDBNull(24) ? 0 : reader.GetInt32(24),
                    NatureTypeId = reader.IsDBNull(25) ? 0 : reader.GetInt32(25),
                    LookTypeId = reader.IsDBNull(26) ? 0 : reader.GetInt32(26),
                    ComplexionTypeId = reader.IsDBNull(27) ? 0 : reader.GetInt32(27),
                    TemperamentTypeId = reader.IsDBNull(28) ? 0 : reader.GetInt32(28),
                    FaceTypeId = reader.IsDBNull(29) ? 0 : reader.GetInt32(29),
                    NailTypeId = reader.IsDBNull(30) ? 0 : reader.GetInt32(30),
                    SpeakingTypeId = reader.IsDBNull(31) ? 0 : reader.GetInt32(31),
                    TongueTypeId = reader.IsDBNull(32) ? 0 : reader.GetInt32(32),
                    HairTypeId = reader.IsDBNull(33) ? 0 : reader.GetInt32(33),
                    EyeColoryId = reader.IsDBNull(34) ? 0 : reader.GetInt32(34),
                    CategoryID = reader.GetInt32(35),

                    Pecular = reader.IsDBNull(36) ? null : reader.GetString(36),
                    Queer = reader.IsDBNull(37) ? null : reader.GetString(37),
                    Strange = reader.IsDBNull(38) ? null : reader.GetString(38),
                    Likes = reader.IsDBNull(39) ? null : reader.GetString(39),
                    Dislikes = reader.IsDBNull(40) ? null : reader.GetString(40),
                    Head = reader.IsDBNull(41) ? null : reader.GetString(41),
                    Eyes = reader.IsDBNull(42) ? null : reader.GetString(42),
                    Ear = reader.IsDBNull(43) ? null : reader.GetString(43),
                    Face = reader.IsDBNull(44) ? null : reader.GetString(44),
                    Teeth = reader.IsDBNull(45) ? null : reader.GetString(45),
                    Mouth = reader.IsDBNull(46) ? null : reader.GetString(46),
                    Throat = reader.IsDBNull(47) ? null : reader.GetString(47),
                    Thirst = reader.IsDBNull(48) ? null : reader.GetString(48),
                    Diet = reader.IsDBNull(49) ? null : reader.GetString(49),
                    Abdomen = reader.IsDBNull(50) ? null : reader.GetString(50),
                    Stool = reader.IsDBNull(51) ? null : reader.GetString(51),
                    Urine = reader.IsDBNull(52) ? null : reader.GetString(52),
                    Lungs = reader.IsDBNull(53) ? null : reader.GetString(53),
                    Heart = reader.IsDBNull(54) ? null : reader.GetString(54),
                    Spine = reader.IsDBNull(55) ? null : reader.GetString(55),
                    MaleGenitalOrganic = reader.IsDBNull(56) ? null : reader.GetString(56),
                    MaleGenitalFunctional = reader.IsDBNull(57) ? null : reader.GetString(57),
                    FemaleGenitalOrganic = reader.IsDBNull(58) ? null : reader.GetString(58),
                    FemaleGenitalFunctional = reader.IsDBNull(59) ? null : reader.GetString(59),
                    Sleep = reader.IsDBNull(60) ? null : reader.GetString(60),
                    Skin = reader.IsDBNull(61) ? null : reader.GetString(61),
                    Fever = reader.IsDBNull(62) ? null : reader.GetString(62),
                    Dreams = reader.IsDBNull(63) ? null : reader.GetString(63),
                    Aversions = reader.IsDBNull(64) ? null : reader.GetString(64),
                    Sweat = reader.IsDBNull(65) ? null : reader.GetString(65),
                    Side = reader.IsDBNull(66) ? null : reader.GetString(66),
                    Thermally = reader.IsDBNull(67) ? null : reader.GetString(67),
                    XRayFindings = reader.IsDBNull(68) ? null : reader.GetString(68),
                    USGOpinion = reader.IsDBNull(69) ? null : reader.GetString(69),
                    CTScanFindings = reader.IsDBNull(70) ? null : reader.GetString(70),
                    MRIFindings = reader.IsDBNull(71) ? null : reader.GetString(71),
                    Nose = reader.IsDBNull(72) ? null : reader.GetString(72),
                });
            }

            foreach (var project in projects)
            {
                project.Tags = await _tagRepository.ListAsync(project.ID);
                project.Tasks = await _taskRepository.ListAsync(project.ID);
            }

            return projects;
        }

        /// <summary>
        /// Retrieves a specific project by its ID.
        /// </summary>
        /// <param name="id">The ID of the project.</param>
        /// <returns>A <see cref="Project"/> object if found; otherwise, null.</returns>
        public async Task<Project?> GetAsync(int id)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM Project WHERE ID = @id";
            selectCmd.Parameters.AddWithValue("@id", id);

            await using var reader = await selectCmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var project = new Project
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Icon = reader.GetString(3),
                    RegistrationNo = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Date = reader.IsDBNull(5) ? DateTime.MinValue : DateTime.Parse(reader.GetString(5)),
                    Time = reader.IsDBNull(6) ? TimeSpan.Zero : TimeSpan.Parse(reader.GetString(6)),
                    MotherName = reader.IsDBNull(7) ? null : reader.GetString(7),
                    MaritalStatus = reader.IsDBNull(8) ? null : reader.GetString(8),
                    Children = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                    Caste = reader.IsDBNull(10) ? null : reader.GetString(10),
                    Age = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                    Religion = reader.IsDBNull(12) ? null : reader.GetString(12),
                    Occupation = reader.IsDBNull(13) ? null : reader.GetString(13),
                    Address = reader.IsDBNull(14) ? null : reader.GetString(14),
                    ContactNo = reader.IsDBNull(15) ? null : reader.GetString(15),

                    BP = reader.IsDBNull(16) ? null : reader.GetDouble(16),
                    Temperature = reader.IsDBNull(17) ? null : reader.GetDouble(17),
                    Pulse = reader.IsDBNull(18) ? null : reader.GetInt32(18),
                    Respiration = reader.IsDBNull(19) ? null : reader.GetInt32(19),
                    Height = reader.IsDBNull(20) ? null : reader.GetDouble(20),
                    Weight = reader.IsDBNull(21) ? null : reader.GetDouble(21),
                    BMI = reader.IsDBNull(22) ? null : reader.GetDouble(22),

                    BmiCategoryId = reader.GetInt32(23),
                    ConstitutionTypeId = reader.IsDBNull(24) ? 0 : reader.GetInt32(24),
                    NatureTypeId = reader.IsDBNull(25) ? 0 : reader.GetInt32(25),
                    LookTypeId = reader.IsDBNull(26) ? 0 : reader.GetInt32(26),
                    ComplexionTypeId = reader.IsDBNull(27) ? 0 : reader.GetInt32(27),
                    TemperamentTypeId = reader.IsDBNull(28) ? 0 : reader.GetInt32(28),
                    FaceTypeId = reader.IsDBNull(29) ? 0 : reader.GetInt32(29),
                    NailTypeId = reader.IsDBNull(30) ? 0 : reader.GetInt32(30),
                    SpeakingTypeId = reader.IsDBNull(31) ? 0 : reader.GetInt32(31),
                    TongueTypeId = reader.IsDBNull(32) ? 0 : reader.GetInt32(32),
                    HairTypeId = reader.IsDBNull(33) ? 0 : reader.GetInt32(33),
                    EyeColoryId = reader.IsDBNull(34) ? 0 : reader.GetInt32(34),
                    CategoryID = reader.GetInt32(35),

                    Pecular = reader.IsDBNull(36) ? null : reader.GetString(36),
                    Queer = reader.IsDBNull(37) ? null : reader.GetString(37),
                    Strange = reader.IsDBNull(38) ? null : reader.GetString(38),
                    Likes = reader.IsDBNull(39) ? null : reader.GetString(39),
                    Dislikes = reader.IsDBNull(40) ? null : reader.GetString(40),
                    Head = reader.IsDBNull(41) ? null : reader.GetString(41),
                    Eyes = reader.IsDBNull(42) ? null : reader.GetString(42),
                    Ear = reader.IsDBNull(43) ? null : reader.GetString(43),
                    Face = reader.IsDBNull(44) ? null : reader.GetString(44),
                    Teeth = reader.IsDBNull(45) ? null : reader.GetString(45),
                    Mouth = reader.IsDBNull(46) ? null : reader.GetString(46),
                    Throat = reader.IsDBNull(47) ? null : reader.GetString(47),
                    Thirst = reader.IsDBNull(48) ? null : reader.GetString(48),
                    Diet = reader.IsDBNull(49) ? null : reader.GetString(49),
                    Abdomen = reader.IsDBNull(50) ? null : reader.GetString(50),
                    Stool = reader.IsDBNull(51) ? null : reader.GetString(51),
                    Urine = reader.IsDBNull(52) ? null : reader.GetString(52),
                    Lungs = reader.IsDBNull(53) ? null : reader.GetString(53),
                    Heart = reader.IsDBNull(54) ? null : reader.GetString(54),
                    Spine = reader.IsDBNull(55) ? null : reader.GetString(55),
                    MaleGenitalOrganic = reader.IsDBNull(56) ? null : reader.GetString(56),
                    MaleGenitalFunctional = reader.IsDBNull(57) ? null : reader.GetString(57),
                    FemaleGenitalOrganic = reader.IsDBNull(58) ? null : reader.GetString(58),
                    FemaleGenitalFunctional = reader.IsDBNull(59) ? null : reader.GetString(59),
                    Sleep = reader.IsDBNull(60) ? null : reader.GetString(60),
                    Skin = reader.IsDBNull(61) ? null : reader.GetString(61),
                    Fever = reader.IsDBNull(62) ? null : reader.GetString(62),
                    Dreams = reader.IsDBNull(63) ? null : reader.GetString(63),
                    Aversions = reader.IsDBNull(64) ? null : reader.GetString(64),
                    Sweat = reader.IsDBNull(65) ? null : reader.GetString(65),
                    Side = reader.IsDBNull(66) ? null : reader.GetString(66),
                    Thermally = reader.IsDBNull(67) ? null : reader.GetString(67),
                    XRayFindings = reader.IsDBNull(68) ? null : reader.GetString(68),
                    USGOpinion = reader.IsDBNull(69) ? null : reader.GetString(69),
                    CTScanFindings = reader.IsDBNull(70) ? null : reader.GetString(70),
                    MRIFindings = reader.IsDBNull(71) ? null : reader.GetString(71),
                    Nose = reader.IsDBNull(72) ? null : reader.GetString(72),
                };

                project.Tags = await _tagRepository.ListAsync(project.ID);
                project.Tasks = await _taskRepository.ListAsync(project.ID);

                return project;
            }

            return null;
        }

        /// <summary>
        /// Saves a project to the database. If the project ID is 0, a new project is created; otherwise, the existing project is updated.
        /// </summary>
        /// <param name="item">The project to save.</param>
        /// <returns>The ID of the saved project.</returns>
        public async Task<int> SaveItemAsync(Project item)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var saveCmd = connection.CreateCommand();
            if (item.ID == 0)
            {
                saveCmd.CommandText = @"
                    INSERT INTO Project (
                        Name, Description, Icon, RegistrationNo, Date, Time, MotherName, MaritalStatus, Children, Caste, Age,
                        Religion, Occupation, Address, ContactNo,
                        BP, Temperature, Pulse, Respiration, Height, Weight, BMI,
                        BmiCategoryId, ConstitutionTypeId, NatureTypeId, LookTypeId, ComplexionTypeId,
                        TemperamentTypeId, FaceTypeId, NailTypeId, SpeakingTypeId, TongueTypeId,
                        HairTypeId, EyeColoryId, CategoryID,
                        Pecular, Queer, Strange, Likes, Dislikes,
                        Head, Eyes, Ear, Face, Teeth, Mouth, Throat, Thirst, Diet, Abdomen, Stool, Urine,
                        Lungs, Heart, Spine, MaleGenitalOrganic, MaleGenitalFunctional,
                        FemaleGenitalOrganic, FemaleGenitalFunctional, Sleep, Skin, Fever,
                        Dreams, Aversions, Sweat, Side, Thermally,
                        XRayFindings, USGOpinion, CTScanFindings, MRIFindings, Nose
                    ) VALUES (
                        @Name, @Description, @Icon, @RegistrationNo, @Date, @Time, @MotherName, @MaritalStatus, @Children, @Caste, @Age,
                        @Religion, @Occupation, @Address, @ContactNo,
                        @BP, @Temperature, @Pulse, @Respiration, @Height, @Weight, @BMI,
                        @BmiCategoryId, @ConstitutionTypeId, @NatureTypeId, @LookTypeId, @ComplexionTypeId,
                        @TemperamentTypeId, @FaceTypeId, @NailTypeId, @SpeakingTypeId, @TongueTypeId,
                        @HairTypeId, @EyeColoryId, @CategoryID,
                        @Pecular, @Queer, @Strange, @Likes, @Dislikes,
                        @Head, @Eyes, @Ear, @Face, @Teeth, @Mouth, @Throat, @Thirst, @Diet, @Abdomen, @Stool, @Urine,
                        @Lungs, @Heart, @Spine, @MaleGenitalOrganic, @MaleGenitalFunctional,
                        @FemaleGenitalOrganic, @FemaleGenitalFunctional, @Sleep, @Skin, @Fever,
                        @Dreams, @Aversions, @Sweat, @Side, @Thermally,
                        @XRayFindings, @USGOpinion, @CTScanFindings, @MRIFindings, @Nose
                    );
                    SELECT last_insert_rowid();";
                        }
                        else
                        {
                            saveCmd.CommandText = @"
                    UPDATE Project SET
                    Name = @Name, Description = @Description, Icon = @Icon, RegistrationNo = @RegistrationNo,
                    Date = @Date, Time = @Time, MotherName = @MotherName, MaritalStatus = @MaritalStatus,
                    Children = @Children, Caste = @Caste, Age = @Age, Religion = @Religion,
                    Occupation = @Occupation, Address = @Address, ContactNo = @ContactNo,
                    BP = @BP, Temperature = @Temperature, Pulse = @Pulse, Respiration = @Respiration,
                    Height = @Height, Weight = @Weight, BMI = @BMI,
                    BmiCategoryId = @BmiCategoryId, ConstitutionTypeId = @ConstitutionTypeId,
                    NatureTypeId = @NatureTypeId, LookTypeId = @LookTypeId, ComplexionTypeId = @ComplexionTypeId,
                    TemperamentTypeId = @TemperamentTypeId, FaceTypeId = @FaceTypeId, NailTypeId = @NailTypeId,
                    SpeakingTypeId = @SpeakingTypeId, TongueTypeId = @TongueTypeId, HairTypeId = @HairTypeId,
                    EyeColoryId = @EyeColoryId, CategoryID = @CategoryID,
                    Pecular = @Pecular, Queer = @Queer, Strange = @Strange, Likes = @Likes, Dislikes = @Dislikes,
                    Head = @Head, Eyes = @Eyes, Ear = @Ear, Face = @Face, Teeth = @Teeth, Mouth = @Mouth,
                    Throat = @Throat, Thirst = @Thirst, Diet = @Diet, Abdomen = @Abdomen, Stool = @Stool, Urine = @Urine,
                    Lungs = @Lungs, Heart = @Heart, Spine = @Spine, MaleGenitalOrganic = @MaleGenitalOrganic, MaleGenitalFunctional = @MaleGenitalFunctional,
                    FemaleGenitalOrganic = @FemaleGenitalOrganic, FemaleGenitalFunctional = @FemaleGenitalFunctional, Sleep = @Sleep,
                    Skin = @Skin, Fever = @Fever, Dreams = @Dreams, Aversions = @Aversions,
                    Sweat = @Sweat, Side = @Side, Thermally = @Thermally,
                    XRayFindings = @XRayFindings, USGOpinion = @USGOpinion, CTScanFindings = @CTScanFindings, MRIFindings = @MRIFindings, Nose = @Nose
                    WHERE ID = @ID";

                saveCmd.Parameters.AddWithValue("@ID", item.ID);
            }

            saveCmd.Parameters.AddWithValue("@Name", item.Name);
            saveCmd.Parameters.AddWithValue("@Description", item.Description);
            saveCmd.Parameters.AddWithValue("@Icon", item.Icon);
            saveCmd.Parameters.AddWithValue("@RegistrationNo", (object?)item.RegistrationNo ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Date", item.Date == default ? DBNull.Value : item.Date.ToString("yyyy-MM-dd"));
            saveCmd.Parameters.AddWithValue("@Time", item.Time == default ? DBNull.Value : item.Time.ToString("hh\\:mm\\:ss"));
            saveCmd.Parameters.AddWithValue("@MotherName", (object?)item.MotherName ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@MaritalStatus", (object?)item.MaritalStatus ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Children", (object?)item.Children ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Caste", (object?)item.Caste ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Age", (object?)item.Age ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Religion", (object?)item.Religion ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Occupation", (object?)item.Occupation ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Address", (object?)item.Address ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@ContactNo", (object?)item.ContactNo ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@BP", (object?)item.BP ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Temperature", (object?)item.Temperature ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Pulse", (object?)item.Pulse ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Respiration", (object?)item.Respiration ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Height", (object?)item.Height ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Weight", (object?)item.Weight ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@BMI", (object?)item.BMI ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@BmiCategoryId", item.BmiCategoryId);
            saveCmd.Parameters.AddWithValue("@ConstitutionTypeId", item.ConstitutionTypeId);
            saveCmd.Parameters.AddWithValue("@NatureTypeId", item.NatureTypeId);
            saveCmd.Parameters.AddWithValue("@LookTypeId", item.LookTypeId);
            saveCmd.Parameters.AddWithValue("@ComplexionTypeId", item.ComplexionTypeId);
            saveCmd.Parameters.AddWithValue("@TemperamentTypeId", item.TemperamentTypeId);
            saveCmd.Parameters.AddWithValue("@FaceTypeId", item.FaceTypeId);
            saveCmd.Parameters.AddWithValue("@NailTypeId", item.NailTypeId);
            saveCmd.Parameters.AddWithValue("@SpeakingTypeId", item.SpeakingTypeId);
            saveCmd.Parameters.AddWithValue("@TongueTypeId", item.TongueTypeId);
            saveCmd.Parameters.AddWithValue("@HairTypeId", item.HairTypeId);
            saveCmd.Parameters.AddWithValue("@EyeColoryId", item.EyeColoryId);
            saveCmd.Parameters.AddWithValue("@CategoryID", item.CategoryID);
            saveCmd.Parameters.AddWithValue("@Pecular", (object?)item.Pecular ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Queer", (object?)item.Queer ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Strange", (object?)item.Strange ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Likes", (object?)item.Likes ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Dislikes", (object?)item.Dislikes ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Head", (object?)item.Head ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Eyes", (object?)item.Eyes ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Ear", (object?)item.Ear ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Face", (object?)item.Face ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Teeth", (object?)item.Teeth ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Mouth", (object?)item.Mouth ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Throat", (object?)item.Throat ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Thirst", (object?)item.Thirst ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Diet", (object?)item.Diet ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Abdomen", (object?)item.Abdomen ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Stool", (object?)item.Stool ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Urine", (object?)item.Urine ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Lungs", (object?)item.Lungs ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Heart", (object?)item.Heart ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Spine", (object?)item.Spine ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@MaleGenitalOrganic", (object?)item.MaleGenitalOrganic ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@MaleGenitalFunctional", (object?)item.MaleGenitalFunctional ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@FemaleGenitalOrganic", (object?)item.FemaleGenitalOrganic ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@FemaleGenitalFunctional", (object?)item.FemaleGenitalFunctional ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Sleep", (object?)item.Sleep ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Skin", (object?)item.Skin ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Fever", (object?)item.Fever ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Dreams", (object?)item.Dreams ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Aversions", (object?)item.Aversions ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Sweat", (object?)item.Sweat ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Side", (object?)item.Side ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Thermally", (object?)item.Thermally ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@XRayFindings", (object?)item.XRayFindings ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@USGOpinion", (object?)item.USGOpinion ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@CTScanFindings", (object?)item.CTScanFindings ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@MRIFindings", (object?)item.MRIFindings ?? DBNull.Value);
            saveCmd.Parameters.AddWithValue("@Nose", (object?)item.Nose ?? DBNull.Value);

            var result = await saveCmd.ExecuteScalarAsync();
            if (item.ID == 0)
            {
                item.ID = Convert.ToInt32(result);
            }

            return item.ID;
        }

        /// <summary>
        /// Deletes a project from the database.
        /// </summary>
        /// <param name="item">The project to delete.</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> DeleteItemAsync(Project item)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM Project WHERE ID = @ID";
            deleteCmd.Parameters.AddWithValue("@ID", item.ID);

            return await deleteCmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Drops the Project table from the database.
        /// </summary>
        public async Task DropTableAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var dropCmd = connection.CreateCommand();
            dropCmd.CommandText = "DROP TABLE IF EXISTS Project";
            await dropCmd.ExecuteNonQueryAsync();

            await _taskRepository.DropTableAsync();
            await _tagRepository.DropTableAsync();
            _hasBeenInitialized = false;
        }
    }
}
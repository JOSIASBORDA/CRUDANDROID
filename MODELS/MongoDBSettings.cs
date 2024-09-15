namespace CRUDALUMNOS.MODELS
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string AlumnosCollectionName { get; set; } = null!;
    }
}

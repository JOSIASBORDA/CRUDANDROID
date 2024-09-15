using CRUDALUMNOS.MODELS;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CRUDALUMNOS.SERVICIO
{
    public class AlumnoService
    {
        private readonly IMongoCollection<Alumno> _alumnosCollection;

        public AlumnoService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var settings = mongoDBSettings.Value;
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "MongoDBSettings is null");
            }

            var mongoClient = new MongoClient(settings.ConnectionString); // Cadena de conexión
            var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName); // Nombre de la base de datos
            _alumnosCollection = mongoDatabase.GetCollection<Alumno>(settings.AlumnosCollectionName); // Nombre de la colección
        }

        public async Task<List<Alumno>> GetAsync() =>
            await _alumnosCollection.Find(_ => true).ToListAsync();

        public async Task<Alumno?> GetAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            return await _alumnosCollection.Find(x => x.Id == objectId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Alumno nuevoAlumno)
        {
            await _alumnosCollection.InsertOneAsync(nuevoAlumno);
        }

        public async Task UpdateAsync(string id, Alumno alumnoActualizado)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ID format", nameof(id));
            }

            alumnoActualizado.Id = objectId;
            await _alumnosCollection.ReplaceOneAsync(x => x.Id == objectId, alumnoActualizado);
        }

        public async Task RemoveAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ID format", nameof(id));
            }

            await _alumnosCollection.DeleteOneAsync(x => x.Id == objectId);
        }
    }
}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CRUDALUMNOS.MODELS
{
    public class Alumno
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("carrera")]
        public string Carrera { get; set; }
    }
}

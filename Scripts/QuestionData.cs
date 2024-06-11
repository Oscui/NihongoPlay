using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

[Serializable]
public class QuestionData
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    [BsonElement("id")]
    public int IdPregunta { get; set; }
    
    [BsonElement("pregunta")]
    public string Pregunta { get; set; }
    
    [BsonElement("respuesta1")]
    public string Respuesta1 { get; set; }
    
    [BsonElement("respuesta2")]
    public string Respuesta2 { get; set; }
    
    [BsonElement("respuesta3")]
    public string Respuesta3 { get; set; }
    
    [BsonElement("respuesta4")]
    public string Respuesta4 { get; set; }
    
    [BsonElement("respuesta_correcta")]
    public int RespuestaCorrecta { get; set; }
    
    [BsonElement("id_leccion")]
    public int IdLeccion { get; set; }
}

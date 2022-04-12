using CAS_API.Helper;
using CAS_API.Models;
using CAS_API.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CAS_API.Controllers
{
    /// <summary>
    /// Controller for the Documentoperations
    /// </summary>
    [ApiController]
    [Route("Document")]
    public class DocumentController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly string _fileFolder;

        /// <summary>
        /// Constructor of the Documentoperations Controller
        /// </summary>
        /// <param name="apiSettings">Settings of the API</param>
        public DocumentController(IApiSettings apiSettings)
        {
            _connectionString = apiSettings.ConnectionString;
            _fileFolder = apiSettings.FileFolder;
        }

        /// <summary>
        /// Get a Document by id
        /// </summary>
        /// <param name="id">Unique id of Document</param>
        /// <returns>A Document</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Document>> Get([FromRoute] string id)
        {
            try
            {
                Document? document = null;
                var con = new SqlConnection(_connectionString);
                con.Open();
                var cmd = new SqlCommand($"SELECT * FROM dbo.Documents where Id = {id}", con);
                var dataReader = await cmd.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    document = new Document() { Id = (int)dataReader.GetValue(dataReader.GetOrdinal("Id")), Dateiname = (string)dataReader.GetValue(dataReader.GetOrdinal("Dateiname")), Pfad = (string)dataReader.GetValue(dataReader.GetOrdinal("Pfad")), LetzteAenderung = (DateTime)dataReader.GetValue(dataReader.GetOrdinal("LetzteAenderung")) };
                }
                con.Close();
                if (document != null)
                {
                    return Ok(document);
                }
                return NotFound($"Es konnte kein Dokument mit Id '{id}' gefunden");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Get a list of documents without content
        /// </summary>
        /// <returns>A list of documents</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Document>>> GetDocuments()
        {
            try
            {
                var result = new List<Document>();
                var con = new SqlConnection(_connectionString);
                con.Open();
                var cmd = new SqlCommand("SELECT TOP (100) * FROM dbo.Documents", con);
                var dataReader = await cmd.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    result.Add(new Document() { Id = (int)dataReader.GetValue(dataReader.GetOrdinal("Id")), Dateiname = (string)dataReader.GetValue(dataReader.GetOrdinal("Dateiname")), Pfad = (string)dataReader.GetValue(dataReader.GetOrdinal("Pfad")), LetzteAenderung = (DateTime)dataReader.GetValue(dataReader.GetOrdinal("LetzteAenderung")) });
                }
                con.Close();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Save a File
        /// </summary>
        /// <param name="file">File to save</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        [HttpPost("Content")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Document>> Upload([FromForm] IFormFile file)
        {
            try
            {
                var filePath = "";
                if (file.Length > 0)
                {
                    filePath = Path.Combine(_fileFolder, file.FileName);
                    await using var memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);
                    byte[] documentContent = memoryStream.ToArray();               
                    await System.IO.File.WriteAllBytesAsync(filePath, documentContent);
                }

                var con = new SqlConnection(_connectionString);
                con.Open();
                var cmd = new SqlCommand($"INSERT INTO dbo.Documents (Dateiname, Pfad, LetzteAenderung) VALUES ('{file.FileName}', '{filePath}', '{DateTime.Now}')", con);
                var count = await cmd.ExecuteNonQueryAsync();
                con.Close();
                if (count > 0)
                {
                    return Ok();
                }
                return BadRequest("Es konnte kein Dokument in der Datenbank erstellt werden");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Get a File by Id
        /// </summary>
        /// <param name="id">Unique id of Document</param>
        /// <returns>the File matched to the Id</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request</response>
        [HttpGet("{id}/Content")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Download([FromRoute] string id)
        {
            try
            {
                var document = await DocumentHelper.GetDocumentById(id, _connectionString);
                if (document == null)
                {
                    return BadRequest($"Es konnte kein Dokument mit Id {id} gefunden werden");
                }

                var file = await System.IO.File.ReadAllBytesAsync(document.Pfad);
                return new FileContentResult(file, "application/pdf")
                {
                    FileDownloadName = document.Dateiname
                };
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Create a Document
        /// </summary>
        /// <param name="document">Document to create</param>
        /// <response code="200">Created</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Document>> Create([FromBody] Document document)
        {
            try
            {
                var count = 0;
                var con = new SqlConnection(_connectionString);
                con.Open();

                var cmd = new SqlCommand($"INSERT INTO dbo.Documents (Dateiname, Pfad, LetzteAenderung) VALUES ('{document.Dateiname}', '{document.Pfad}', '{DateTime.Now}')", con);
                count = await cmd.ExecuteNonQueryAsync();

                con.Close();
                if (count > 0)
                {
                    return Ok($"Es wurde ein Dokument angelegt");
                }
                return BadRequest($"Es konnte kein Dokument erstellt werden");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Update an existing Postbox
        /// </summary>
        /// <param name="id">Unique id of Document</param>
        /// <param name="document">new Document</param>
        /// <response code="2040">Updated</response>
        /// <response code="400">Bad request</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Document>> UpdateDocument([FromRoute] string id, [FromBody] Document document)
        {
            try
            {
                var con = new SqlConnection(_connectionString);
                con.Open();     
                
                var updateCmd = new SqlCommand($"UPDATE dbo.Documents set Dateiname = '{document.Dateiname}', Pfad = '{document.Pfad}', LetzteAenderung = '{DateTime.Now}' where Id = {id}", con);
                var count = await updateCmd.ExecuteNonQueryAsync();

                con.Close();
                return Ok($"Es wurde {count} Dokument aktualisiert");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Deletes a Document
        /// </summary>
        /// <param name="id">Unique id of Document</param>
        /// <response code="201">Deleted</response>
        /// <response code="400">Bad request</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Document>> Delete([FromRoute] string id)
        {
            try
            {
                var con = new SqlConnection(_connectionString);
                con.Open();

                var selectCmd = new SqlCommand($"SELECT * FROM dbo.Documents where Id = {id}", con);
                var dataReader = await selectCmd.ExecuteReaderAsync();
                Document? document = null;
                while (await dataReader.ReadAsync())
                {
                    document = new Document() { Id = (int)dataReader.GetValue(dataReader.GetOrdinal("Id")), Dateiname = (string)dataReader.GetValue(dataReader.GetOrdinal("Dateiname")), Pfad = (string)dataReader.GetValue(dataReader.GetOrdinal("Pfad")), LetzteAenderung = (DateTime)dataReader.GetValue(dataReader.GetOrdinal("LetzteAenderung")) };
                }
                if (document == null)
                {
                    return NotFound($"Es konnte kein Dokument zur Id {id} gefunden werden");
                }
                dataReader.Close();
                var deleteCmd = new SqlCommand($"DELETE FROM dbo.Documents where Id = {id}", con);
                var count = await deleteCmd.ExecuteNonQueryAsync();
                con.Close();
                if (count > 0)
                {
                    return Ok($"Es wurde {count} Dokument gelöscht");
                }
                return BadRequest("Es konnte kein Dokument gelöscht werden");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}

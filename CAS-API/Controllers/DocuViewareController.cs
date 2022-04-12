using CAS_API.Helper;
using CAS_API.Models;
using CAS_API.Models.Interfaces;
using GdPicture14.WEB;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CAS_API.Controllers
{
#pragma warning disable 1591
    [ApiController]
    [Route("DocuVieware")]
    public class DocuViewareController : ControllerBase
    {
        private readonly string _connectionString;
        public DocuViewareController(IApiSettings apiSettings)
        {
            _connectionString = apiSettings.ConnectionString;
        }

        [HttpPost("GetDocuViewareControl")]
        public DocuViewareOutputResponse GetDocuViewareControl(string? documentId, DocuViewareConfiguration controlConfiguration)
        {
            if (!DocuViewareManager.IsSessionAlive(controlConfiguration.SessionId))
            {
                if (!string.IsNullOrEmpty(controlConfiguration.SessionId) && !string.IsNullOrEmpty(controlConfiguration.ControlId))
                {
                    DocuViewareManager.CreateDocuViewareSession(controlConfiguration.SessionId, controlConfiguration.ControlId, 20);
                }
                else
                {
                    throw new Exception("Ungültige Session oder ControlId");
                }
            }

            using (DocuViewareControl docuVieware = new DocuViewareControl(controlConfiguration.SessionId))
            {
                docuVieware.AllowPrint = controlConfiguration.AllowPrint;
                docuVieware.EnablePrintButton = controlConfiguration.EnablePrintButton;
                docuVieware.AllowUpload = controlConfiguration.AllowUpload;
                docuVieware.EnableFileUploadButton = controlConfiguration.EnableFileUploadButton;
                docuVieware.CollapsedSnapIn = controlConfiguration.CollapsedSnapIn;
                docuVieware.ShowAnnotationsSnapIn = controlConfiguration.ShowAnnotationsSnapIn;
                docuVieware.EnableRotateButtons = controlConfiguration.EnableRotateButtons;
                docuVieware.EnableZoomButtons = controlConfiguration.EnableZoomButtons;
                docuVieware.EnablePageViewButtons = controlConfiguration.EnablePageViewButtons;
                docuVieware.EnableMultipleThumbnailSelection = controlConfiguration.EnableMultipleThumbnailSelection;
                docuVieware.EnableMouseModeButtons = controlConfiguration.EnableMouseModeButtons;
                docuVieware.EnableFormFieldsEdition = controlConfiguration.EnableFormFieldsEdition;
                docuVieware.EnableTwainAcquisitionButton = controlConfiguration.EnableTwainAcquisitionButton;
                docuVieware.MaxUploadSize = 36700160; // 35MB
                docuVieware.EnableThumbnailDragDrop = controlConfiguration.EnableThumbnailDragDrop;
                docuVieware.Locale = DocuViewareLocale.De;
                
                if (documentId != null) {
                    var document = DocumentHelper.GetDocumentById(documentId, _connectionString).GetAwaiter().GetResult();
                    if (document != null)
                    {
                        docuVieware.LoadFromFile(document.Pfad);
                    }
                }

                using (StringWriter controlOutput = new StringWriter())
                {
                    docuVieware.RenderControl(controlOutput);
                    DocuViewareOutputResponse output = new DocuViewareOutputResponse
                    {
                        HtmlContent = controlOutput.ToString()
                    };
                    return output;
                }
            }
        }

        [HttpPost("baserequest")]
        public string baserequest([FromBody] object jsonString)
        {
            return DocuViewareControllerActionsHandler.baserequest(jsonString);
        }


        [HttpGet("print")]
        public HttpResponseMessage Print(string sessionID, string pageRange, bool printAnnotations)
        {
            return DocuViewareControllerActionsHandler.print(sessionID, pageRange, printAnnotations);
        }

        [HttpGet("save")]
        public IActionResult Save(string sessionID, string fileName, string format, string pageRange, bool dropAnnotations, bool flattenAnnotations)
        {
            DocuViewareControllerActionsHandler.save(sessionID, ref fileName, format, pageRange, dropAnnotations, flattenAnnotations, out HttpStatusCode statusCode, out string reasonPhrase, out byte[] content, out string contentType);
            if (statusCode == HttpStatusCode.OK)
            {
                return File(content, contentType, fileName);
            }
            else
            {
                return StatusCode((int)statusCode, reasonPhrase);
            }
        }


        [HttpGet("twainservicesetupdownload")]
        public IActionResult TwainServiceSetupDownload(string sessionID)
        {
            DocuViewareControllerActionsHandler.twainservicesetupdownload(sessionID, out HttpStatusCode statusCode, out byte[] content, out string contentType, out string fileName, out string reasonPhrase);
            if (statusCode == HttpStatusCode.OK)
            {
                return File(content, contentType, fileName);
            }
            else
            {
                return StatusCode((int)statusCode, reasonPhrase);
            }
        }

        [HttpPost("formfieldupdate")]
        public string FormfieldUpdate([FromBody] object jsonString)
        {
            return DocuViewareControllerActionsHandler.formfieldupdate(jsonString);
        }

        [HttpPost("annotupdate")]
        public string AnnotUpdate([FromBody] object jsonString)
        {
            return DocuViewareControllerActionsHandler.annotupdate(jsonString);
        }

        [HttpPost("loadfromfile")]
        public string LoadFromFile([FromBody] object jsonString)
        {
            return DocuViewareControllerActionsHandler.loadfromfile(jsonString);
        }

        [HttpPost("loadfromfilemultipart")]
        public string LoadFromFileMultipart()
        {
            return DocuViewareControllerActionsHandler.loadfromfilemultipart(Request);
        }
    }
#pragma warning restore 1591
}


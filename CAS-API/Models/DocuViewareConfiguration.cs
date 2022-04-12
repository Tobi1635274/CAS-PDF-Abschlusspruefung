namespace CAS_API.Models
{
#pragma warning disable 1591
    public class DocuViewareConfiguration
    {
        public string SessionId { get; set; } = string.Empty;   
        public string ControlId { get; set; } = string.Empty;
        public bool AllowPrint { get; set; } = false;
        public bool EnablePrintButton { get; set; } = false;
        public bool AllowUpload { get; set; } = false;
        public bool EnableFileUploadButton { get; set; } = false;
        public bool CollapsedSnapIn { get; set; } = false;
        public bool ShowAnnotationsSnapIn { get; set; } = false;
        public bool EnableRotateButtons { get; set; } = false;
        public bool EnableZoomButtons { get; set; } = false;
        public bool EnablePageViewButtons { get; set; } = false;
        public bool EnableMultipleThumbnailSelection { get; set; } = false;
        public bool EnableMouseModeButtons { get; set; } = false;
        public bool EnableFormFieldsEdition { get; set; } = false;
        public bool EnableTwainAcquisitionButton { get; set; } = false;
        public bool EnableThumbnailDragDrop { get; set; } = false;
    }
#pragma warning restore 1591
}

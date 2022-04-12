import axios from "axios";

export const getDocuViewareMarkup = async (documentId?: string) => {
    try {
        let url: string = "http://localhost:5018/DocuVieware/GetDocuViewareControl";
        if (documentId) {
            url += `?documentId=${documentId}`;
        }

        let config = {
            SessionId: "mySessionId",
            ControlId: "DocuVieware1",
            AllowPrint: true,
            EnablePrintButton: true,
            AllowUpload: false,
            EnableFileUploadButton: false,
            CollapsedSnapIn: true,
            ShowAnnotationsSnapIn: true,
            EnableRotateButtons: true,
            EnableZoomButtons: true,
            EnablePageViewButtons: true,
            EnableMultipleThumbnailSelection: true,
            EnableMouseModeButtons: true,
            EnableFormFieldsEdition: true,
            EnableTwainAcquisitionButton: true,
            EnableThumbnailDragDrop: true
        };

        const markup = await axios.post(
            url,
            JSON.stringify(config),
            {
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );
        return markup.data["htmlContent"];
    }
    catch (error) {
        console.log("Error: " + error)
        return error;
    }
};

export const insertMarkup = (markup: any, id: string) => {
    const fragment = document
        .createRange()
        .createContextualFragment(markup);
    (document.getElementById(id) as HTMLDivElement).appendChild(fragment);
};
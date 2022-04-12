import * as React from "react";
import WebViewer, { WebViewerInstance, Core, UI } from '@pdftron/webviewer'
import axios, { AxiosResponse } from "axios";
import { IPageState } from "../interfaces/Interfaces";
import { useLocation } from "react-router-dom";

function PdfTron() {
    const viewerDiv = React.useRef<HTMLDivElement>(null);
    const [instance, setInstance] = React.useState<WebViewerInstance>();
    const location = useLocation<IPageState>();

    React.useEffect(() => {
        async function fetchDocumentContent() {
            if (instance && location?.state?.documentId) {
                axios.defaults.headers.common["Access-Control-Allow-Origin"] = "*";
                axios.defaults.headers.get['Accepts'] = 'application/json';
                try {
                    const documentContentResponse: AxiosResponse = await axios.get(`http://localhost:5018/Document/${location.state.documentId}/Content`, { responseType: 'arraybuffer' });
                    const documentContent = new Blob([documentContentResponse.data], { type: 'application/pdf' });
                    const documentResponse: AxiosResponse = await axios.get(`http://localhost:5018/Document/${location.state.documentId}`);
                    instance.UI.loadDocument(documentContent, { filename: documentResponse.data.dateiname, extension: 'pdf' });
                    return documentContent;
                }
                catch (error) {
                    console.log("Error: " + error)
                }
            }
        }

        fetchDocumentContent();
    }, [location?.state?.documentId, instance]);

    React.useEffect(() => {
        async function createPdfTron() {
            if (instance === undefined) {
                const newInstance = await WebViewer(
                    {
                        path: 'pdftronLib',
                        initialDoc: '',
                    },
                    viewerDiv.current as HTMLDivElement,
                )
                setInstance(newInstance);
            }
            else {
                instance.UI.setLanguage('de');
                setInstance(instance);
            }
        }
        createPdfTron()
    }, [instance]);

    return (
        <div className="pdftrondiv">
            <div className="pdftron">
                <div className="webviewer webviewerStyle" ref={viewerDiv}></div>
            </div>
        </div>
    );
};

export default PdfTron;
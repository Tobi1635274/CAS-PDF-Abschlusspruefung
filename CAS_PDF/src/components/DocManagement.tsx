import * as React from "react";
import axios, { AxiosResponse } from "axios";
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs";
import DocumentGrid from "./DocumentGrid";
import { IDocument } from "../interfaces/Interfaces";
import { useHistory } from 'react-router-dom';

function DocMangement() {
    const [documents, setDocuments] = React.useState<IDocument[]>([])
    const [activeId, setActiveId] = React.useState<number>();
    const [visible, setVisible] = React.useState<boolean>(false);
    const history = useHistory();

    React.useEffect(() => {
        async function fetchDocuments() {
            axios.defaults.headers.common["Access-Control-Allow-Origin"] = "*";
            axios.defaults.headers.get['Accepts'] = 'application/json';
            try {
                const response: AxiosResponse = await axios.get("http://localhost:5018/Document");
                const documentList: IDocument[] = response.data.map((documentList: IDocument[]): IDocument[] => {
                    return (documentList);
                });
                return setDocuments(documentList);
            }
            catch (error) {
                console.log("Error: " + error)
            }
        }

        fetchDocuments();
    }, []);

    const toggleDialog = () => {
        setVisible(!visible);
    };

    return (
        <div>
            <DocumentGrid setActiveId={setActiveId} toggleDialog={toggleDialog} documents={documents} />
            {visible && (
                <Dialog key="DocumentClickDialog" title={"Bitte waehlen"} onClose={toggleDialog}>
                    <p key="DialogText" className="documentClickDialogStyle">
                        Welches Tool soll benutzt werden?
                    </p>
                    <DialogActionsBar key="DocumentClickDialogActionsBar">
                        <button
                            key="PdfTronButton"
                            className="k-button k-button-md k-rounded-md k-button-solid k-button-solid-base"
                            onClick={() => {
                                history.push('/PdfTron', {
                                    documentId: activeId
                                })
                            }}>
                            PDFTron
                        </button>
                        <button
                            key="DocuViewareButton"
                            className="k-button k-button-md k-rounded-md k-button-solid k-button-solid-base"
                            onClick={() => { history.push('/DocuVieware', { documentId: activeId }); }}>
                            DocuVieware
                        </button>
                    </DialogActionsBar>
                </Dialog>
            )}
        </div>
    );
};

export default DocMangement;

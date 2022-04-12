import '../App.css';
import React from "react";
import { useHistory } from 'react-router-dom';
import { AppBar, AppBarSection, AppBarSpacer } from "@progress/kendo-react-layout";
import { Tooltip } from "@progress/kendo-react-tooltip";
import { BadgeContainer } from '@progress/kendo-react-indicators';
import { Dialog } from "@progress/kendo-react-dialogs";
import { Upload } from "@progress/kendo-react-upload";

function NavBar() {
    const [visible, setVisible] = React.useState<boolean>(false);
    const history = useHistory();

    const toggleDialog = () => {
        setVisible(!visible);
    };

    return (
        <>
            <AppBar className="Navbar">
                <AppBarSpacer style={{ width: 4 }} />
                <AppBarSection>
                    <span className="appBarSectionSpanStyle" onClick={() => { history.push('/'); }}>
                        <h1>CAS-PDF</h1>
                    </span>
                </AppBarSection>

                <AppBarSpacer />

                <AppBarSection className="actions">
                    <Tooltip anchorElement="target" position="bottom">
                        <button title="Dokumentenupload" className="k-button k-button-clear dokumentenUploadStyle" onClick={toggleDialog}>
                            <BadgeContainer>
                                <span title="Dokumentenupload" className="k-icon k-i-upload k-icon-16" />
                            </BadgeContainer>
                        </button>

                        <button title="DocuVieware" className="k-button k-button-clear docuViewareStyle" onClick={() => { history.push('/DocuVieware'); }}>
                            <BadgeContainer>
                                <span title="DocuVieware" className="k-icon k-i-track-changes-enable k-icon-32" />
                            </BadgeContainer>
                        </button>

                        <button title="PdfTron" className="k-button k-button-clear pdfTronStyle" onClick={() => { history.push('/PdfTron'); }}>
                            <BadgeContainer>
                                <span title="PdfTron" className="k-icon k-i-change-manually k-icon-32" />
                            </BadgeContainer>
                        </button>
                    </Tooltip>
                </AppBarSection>
                {visible && (
                    <Dialog title={"Upload"} onClose={toggleDialog}>
                        <Upload
                            defaultFiles={[]}
                            withCredentials={false}
                            restrictions={{ allowedExtensions: ['.pdf'] }}
                            saveUrl={"http://localhost:5018/Document/Content"}
                            saveField={"file"}
                        />
                    </Dialog>
               
                )}
            </AppBar>
        </>
    );
};

export default NavBar;

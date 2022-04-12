import React from "react";
import * as Api from "./DocuViewareApi";
import { IPageState } from "../interfaces/Interfaces";
import { useLocation } from "react-router-dom";

function DocuVieware() {
    const location = useLocation<IPageState>();

    React.useEffect(() => {
        if (location?.state?.documentId) {
            Api
                .getDocuViewareMarkup(location.state.documentId)
                .then((markup: any) => Api.insertMarkup(markup, "dvContainer"));
        }
        else {
            Api
                .getDocuViewareMarkup()
                .then((markup: any) => Api.insertMarkup(markup, "dvContainer"));
        }
    }, [location?.state?.documentId]);
    return (
        <div id="dvContainer" className="dvContainerStyle" ></div >
    );
};
export default DocuVieware;
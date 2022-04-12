import { Route, Switch } from 'react-router-dom';
import DocuVieware from "./components/DocuVieware"
import PdfTron from "./components/PdfTron"
import DocManagement from "./components/DocManagement"

function Pages() {

    return (
        <div>
            <Switch>
                <Route exact path="/">
                    <DocManagement />
                </Route>
                <Route path="/DocuVieware">
                    <DocuVieware />
                </Route>
                <Route path="/PdfTron">
                    <PdfTron />
                </Route>
            </Switch>
        </div>
    )
};

export default Pages;
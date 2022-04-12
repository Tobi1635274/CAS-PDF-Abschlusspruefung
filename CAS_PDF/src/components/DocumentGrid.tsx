import { Grid, GridColumn, GridRowClickEvent } from "@progress/kendo-react-grid";
import { GridProps } from "../interfaces/Interfaces";

function DocumentGrid(props: GridProps) {
    const onRowClick = (event: GridRowClickEvent) => {
        props.setActiveId(event.dataItem.id);
        props.toggleDialog();
    };

    return (
        <Grid key="documentListGridStyle" onRowClick={onRowClick} selectable={{ enabled: true }} className="DocumentListGridStyle" data={props.documents}>
            <GridColumn key="DocumentListGridId" field="id" title="ID" width="40px" />
            <GridColumn key="DocumentListGridDateiname" field="dateiname" title="Dateiname" width="250px" />
            <GridColumn key="DocumentListGridPfad" field="pfad" title="Pfad" />
            <GridColumn key="DocumentListGridLetzteAenderung" field="letzteAenderung" title="Letzte Aenderung" />
        </Grid>
    )
}

export default DocumentGrid;

export interface IDocument {
    id: number,
    dateiname: string,
    pfad: string,
    letzteaenderung: Date
}

export interface IPageState {
    documentId: string;
}

export interface GridProps {
    setActiveId: React.Dispatch<React.SetStateAction<number | undefined>>,
    toggleDialog: () => void,
    documents: IDocument[]
}
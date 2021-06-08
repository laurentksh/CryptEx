export interface SnackBar {
    Title: string;
    Message: string;
    Severity: AlertType;
    CloseAfter: number;

    ShowCloseButton: boolean;
    OnCloseActionCallback: () => void;

    ShowContinueActionBtn: boolean;
    ContinueActionBtnString: string;
    OnContinueActionCallback: () => void;

    ShowPrimaryActionBtn: boolean;
    PrimaryActionBtnString: string;
    OnPrimaryActionCallback: () => void;

    ShowSecondaryActionBtn: boolean;
    SecondaryActionBtnString: string;
    OnSecondaryActionCallback: () => void;
}

export class SnackBarUI implements SnackBar {
    Title: string;
    Message: string;
    Severity: AlertType;
    CloseAfter: number;

    ShowCloseButton: boolean;
    OnCloseActionCallback: () => void;

    ShowContinueActionBtn: boolean;
    ContinueActionBtnString: string;
    OnContinueActionCallback: () => void;

    ShowPrimaryActionBtn: boolean;
    PrimaryActionBtnString: string;
    OnPrimaryActionCallback: () => void;

    ShowSecondaryActionBtn: boolean;
    SecondaryActionBtnString: string;
    OnSecondaryActionCallback: () => void;

    Show: boolean = false;
    Hide: boolean = false;
}

export class SnackBarCreate implements SnackBar {
    constructor(title: string, message: string = null, severity: AlertType = AlertType.Info, closeAfter: number = 5000) {
        this.Title = title;
        this.Message = message;
        this.Severity = severity;
        this.CloseAfter = closeAfter;
    }
    
    
    

    Title: string;
    Message: string;
    Severity: AlertType;
    CloseAfter: number;

    ShowCloseButton: boolean = true;
    OnCloseActionCallback: () => void = null;

    ShowContinueActionBtn: boolean = false;
    ContinueActionBtnString: string = "Continue";
    OnContinueActionCallback: () => void = null;

    ShowPrimaryActionBtn: boolean = false;
    PrimaryActionBtnString: string = "Primary";
    OnPrimaryActionCallback: () => void = null;
    
    ShowSecondaryActionBtn: boolean = false;
    SecondaryActionBtnString: string = "Secondary";
    OnSecondaryActionCallback: () => void = null;

}

export enum AlertType {
    Info,
    Success,
    Warning,
    Error
  }
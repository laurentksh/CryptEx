export interface SnackBar {
    Title: string;
    Message: string;
    Severity: AlertType;
    CloseAfter: number;

    ShowCloseButton: boolean;
    OnCloseActionCallback: () => void;

    ShowContinueActionBtn: boolean;
    OnContinueActionCallback: () => void;

    ShowPrimaryActionBtn: boolean;
    OnPrimaryActionCallback: () => void;

    ShowSecondaryActionBtn: boolean;
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
    OnContinueActionCallback: () => void;

    ShowPrimaryActionBtn: boolean;
    OnPrimaryActionCallback: () => void;

    ShowSecondaryActionBtn: boolean;
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
    OnContinueActionCallback: () => void = null;
    ShowPrimaryActionBtn: boolean = false;
    OnPrimaryActionCallback: () => void = null;
    ShowSecondaryActionBtn: boolean = false;
    OnSecondaryActionCallback: () => void = null;

}

export enum AlertType {
    Info,
    Success,
    Warning,
    Error
  }
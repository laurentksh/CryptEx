export interface ChangePasswordDto {
    /**
     * Reset token, obtained by email after calling /resetPassword
     */
    Token: string;

    /**
     * The new password
     */
    NewPassword: string;
}

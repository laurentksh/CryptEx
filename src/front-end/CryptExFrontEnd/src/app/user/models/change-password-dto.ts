export interface ChangePasswordDto {
    /**
     * Reset token, obtained by email after calling /resetPassword
     */
    token: string;

    /**
     * The user's email, provide only if unauthenticated.
     */
    email: string;

    /**
     * The new password
     */
    newPassword: string;
}

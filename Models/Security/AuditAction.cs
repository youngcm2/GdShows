namespace Models.Security
{
	public enum AuditAction
	{
		Unknown,
		LoginSuccess,
		LoginFailed,
		PasswordReset,
		ChangePasswordRequest,
		ChangePassword,
		UnlockAccount,
		CreateUser
	}
}

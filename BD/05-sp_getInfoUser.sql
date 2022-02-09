USE testarchdb
GO

CREATE PROCEDURE [dbo].[sp_getInfoUser](
	@email NVARCHAR(150) = ''
	, @pwd NVARCHAR(150) = ''
)
AS
BEGIN
	DECLARE 
		@userExist			INT = 0
		, @errorMsg			NVARCHAR(500) = ''
		, @errorNumber		INT = 0
		, @errorSeverity	INT = 0
		, @errorState		INT = 0

	BEGIN TRY
		SELECT
			@userExist = ISNULL(COUNT(*), 0)
		FROM dbo.usuario u
		WHERE u.userEmail = @email
			AND u.userPass = @pwd
			AND u.userStatus = 1

		IF @userExist > 0 BEGIN
			SELECT
				u.userId AS id
				, u.userNombre + ' ' + u.userPapellido + ' ' + u.userSapellido AS nombre
				, u.userEmail as nombreUsuario
			FROM dbo.usuario u
			WHERE u.userEmail = @email
				AND u.userPass = @pwd
				AND u.userStatus = 1
		END
		ELSE BEGIN
			SET @errorMsg = 'Credenciales incorrectas'
			SET @errorSeverity = 16
			SET @errorState = 1
			RAISERROR(@errorMsg,@errorSeverity,@errorState)
		END

	END TRY
	BEGIN CATCH
		
		IF @errorMsg = '' BEGIN
			SET @errorMsg = ERROR_MESSAGE()
			SET @errorSeverity = ERROR_SEVERITY()
			SET @errorState = ERROR_STATE()
		END
		RAISERROR(@errorMsg,@errorSeverity,@errorState)
	END CATCH 
END
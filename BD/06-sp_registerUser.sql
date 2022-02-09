USE testarchdb
GO

CREATE PROCEDURE [dbo].[sp_registerUser](
	@nombre			NVARCHAR(150) = ''
	,@pApellido		NVARCHAR(150) = ''
	,@sApellido		NVARCHAR(150) = ''
	,@email			NVARCHAR(150) = ''
	,@pwd			NVARCHAR(150) = ''
)
AS
BEGIN
	DECLARE 
		@errorMsg			NVARCHAR(500) = ''
		, @errorNumber		INT = 0
		, @errorSeverity	INT = 0
		, @errorState		INT = 0

	BEGIN TRAN
	BEGIN TRY

		INSERT INTO dbo.usuario (
			userNombre
			,userPapellido
			,userSapellido
			,userEmail
			,userPass
		) VALUES (
			@nombre
			,@pApellido
			,@sApellido
			,@email
			,@pwd
		)

		IF @@TRANCOUNT > 0 BEGIN
			COMMIT TRAN
		END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 BEGIN
			ROLLBACK TRAN
		END

		SET @errorMsg = ERROR_MESSAGE()
		SET @errorNumber = ERROR_NUMBER()
		SET @errorSeverity = ERROR_SEVERITY()
		SET @errorState = ERROR_STATE()

		IF @errorNumber = 2627 BEGIN
			SET @errorMsg = 'Usuario existente'
		END

		RAISERROR(@errorMsg,@errorSeverity,@errorState)

	END CATCH
END
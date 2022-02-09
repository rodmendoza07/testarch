USE testarchdb
GO

CREATE PROCEDURE [dbo].[sp_registerTask] (
	@taskTitle		NVARCHAR(500) = ''
	, @taskDesc		NVARCHAR(1000) = ''
	, @taskStatus	INT = 0
	, @taskUser		INT = 0
	, @taskId		NVARCHAR(255) = 0
	, @operationOpt INT = 0
)
AS
BEGIN
	DECLARE 
		@errorMsg			NVARCHAR(500) = ''
		, @errorNumber		INT = 0
		, @errorSeverity	INT = 0
		, @errorState		INT = 0
		, @tId				INT = 0

	BEGIN TRAN
	BEGIN TRY
		
		IF @operationOpt = 0 BEGIN
			
			INSERT INTO dbo.tarea(
				tareaTitulo
				, tareaUserId
				, tareaEstado
				, tareaGuid
			) VALUES (
				@taskTitle
				, @taskUser
				, 1
				, NEWID()
			)

			SET @tId = @@IDENTITY

			INSERT INTO dbo.tareaDetalle (
				tareaId
				, tareaDdesc
			) VALUES (
				@tId
				, @taskDesc
			)

		END
		ELSE IF @operationOpt = 1 BEGIN
			
			SELECT
				@tId = t.tareaId
			FROM dbo.tarea t
			WHERE t.tareaGuid = @taskId

			UPDATE dbo.tarea SET
				tareaFechaMod = GETDATE()
				,tareaTitulo = @taskTitle
				,tareaEstado = @taskStatus
			WHERE tareaGuid = @taskId

			UPDATE dbo.tareaDetalle SET
				tareaDstatus = 0
			WHERE tareaId = @tId

			INSERT INTO tareaDetalle (
				tareaId
				, tareaDdesc
			) VALUES (
				@tId
				, @taskDesc
			)
		END
		ELSE IF @operationOpt = 2 BEGIN
			
			UPDATE dbo.tarea SET
				tareaStatus = 0
			WHERE tareaGuid = @taskId

		END
		ELSE BEGIN
			SET @errorMsg = 'Opción inválida'
			SET @errorSeverity = 16
			SET @errorState = 1
			RAISERROR(@errorMsg,@errorSeverity,@errorState)
		END

		IF @@TRANCOUNT > 0 BEGIN
			COMMIT TRAN
		END

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 BEGIN
			ROLLBACK TRAN
		END
		IF @errorMsg = '' BEGIN
			SET @errorMsg = ERROR_MESSAGE()
			SET @errorSeverity = ERROR_SEVERITY()
			SET @errorState = ERROR_STATE()
		END
		RAISERROR(@errorMsg,@errorSeverity,@errorState)
	END CATCH
END
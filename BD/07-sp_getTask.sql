USE testarchdb
GO

CREATE PROCEDURE [dbo].[sp_getTask] (
	@taskId			NVARCHAR(255) = ''
	, @taskuserId	INT = 0
	, @operationOpt INT = 0
)
AS
BEGIN
	DECLARE 
		@errorMsg			NVARCHAR(500) = ''
		, @errorNumber		INT = 0
		, @errorSeverity	INT = 0
		, @errorState		INT = 0

	BEGIN TRY

		IF @operationOpt = 0 BEGIN

			SELECT
				t.tareaId
				,CONVERT(NVARCHAR(255),t.tareaGuid) AS [GUID]
				,t.tareaTitulo AS nombre
				,CONVERT(VARCHAR, t.tareaFechaCreacion, 103) AS fecha
				,t.tareaUserId AS userId
				,td.tareaDdesc AS descripcion
				,t.tareaEstado AS estadoId
				,ce.catEstadoValor AS estadoDescripcion
				,ce.catEstadoCss AS heading 
			FROM dbo.tarea t
				INNER JOIN dbo.tareaDetalle td ON (t.tareaId = td.tareaId AND td.tareaDstatus = 1)
				INNER JOIN dbo.catEstado ce ON (t.tareaEstado = ce.catEstadoId AND ce.catEstadoStatus = 1)
			WHERE t.tareaUserId = @taskuserId
				AND t.tareaStatus = 1
			ORDER BY t.tareaId

		END
		ELSE IF @operationOpt = 1 BEGIN
			SELECT
				t.tareaId
				,CONVERT(NVARCHAR(255),t.tareaGuid) AS [GUID]
				,t.tareaTitulo AS nombre
				,CONVERT(VARCHAR, t.tareaFechaCreacion, 103) AS fecha
				,t.tareaUserId AS userId
				,td.tareaDdesc AS descripcion
				,t.tareaEstado AS estadoId
				,ce.catEstadoValor AS estadoDescripcion
				,ce.catEstadoCss AS heading 
			FROM dbo.tarea t
				INNER JOIN dbo.tareaDetalle td ON (t.tareaId = td.tareaId AND td.tareaDstatus = 1)
				INNER JOIN dbo.catEstado ce ON (t.tareaEstado = ce.catEstadoId AND ce.catEstadoStatus = 1)
			WHERE t.tareaUserId = @taskuserId
				AND CONVERT(NVARCHAR(255), t.tareaGuid) = @taskId
				AND t.tareaStatus = 1
		END
		ELSE IF @operationOpt = 2 BEGIN

			SELECT
				t.tareaId
				,CONVERT(NVARCHAR(255),t.tareaGuid) AS [GUID]
				,t.tareaTitulo AS nombre
				,CONVERT(VARCHAR, t.tareaFechaCreacion, 103) AS fecha
				,t.tareaUserId AS userId
				,td.tareaDdesc AS descripcion
				,t.tareaEstado AS estadoId
				,ce.catEstadoValor AS estadoDescripcion
				,ce.catEstadoCss AS heading 
			FROM dbo.tarea t
				INNER JOIN dbo.tareaDetalle td ON (t.tareaId = td.tareaId AND td.tareaDstatus = 1)
				INNER JOIN dbo.catEstado ce ON (t.tareaEstado = ce.catEstadoId AND ce.catEstadoStatus = 1)
			WHERE t.tareaUserId = @taskuserId
				AND t.tareaStatus = 1
			ORDER BY CONVERT(VARCHAR, t.tareaFechaCreacion, 112) ASC

		END
		ELSE IF @operationOpt = 3 BEGIN

			SELECT
				t.tareaId
				,CONVERT(NVARCHAR(255),t.tareaGuid) AS [GUID]
				,t.tareaTitulo AS nombre
				,CONVERT(VARCHAR, t.tareaFechaCreacion, 103) AS fecha
				,t.tareaUserId AS userId
				,td.tareaDdesc AS descripcion
				,t.tareaEstado AS estadoId
				,ce.catEstadoValor AS estadoDescripcion
				,ce.catEstadoCss AS heading 
			FROM dbo.tarea t
				INNER JOIN dbo.tareaDetalle td ON (t.tareaId = td.tareaId AND td.tareaDstatus = 1)
				INNER JOIN dbo.catEstado ce ON (t.tareaEstado = ce.catEstadoId AND ce.catEstadoStatus = 1)
			WHERE t.tareaUserId = @taskuserId
				AND t.tareaStatus = 1
			ORDER BY t.tareaTitulo ASC

		END
		ELSE BEGIN
			SET @errorMsg = 'Opción inválida'
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
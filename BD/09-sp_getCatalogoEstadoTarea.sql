USE testarchdb
GO

CREATE PROCEDURE [dbo].[sp_getCatalogoEstadoTarea]
AS
BEGIN
	DECLARE
		@errorMsg NVARCHAR(500) = ''
	BEGIN TRY
		SELECT
			c.catEstadoId AS estadoId
			,c.catEstadoValor AS estadoDesc
		FROM dbo.catEstado c
	END TRY
	BEGIN CATCH
		SET @errorMsg = ERROR_MESSAGE()
		RAISERROR(@errorMsg, 16, 1)
	END CATCH
END
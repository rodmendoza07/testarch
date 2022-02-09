USE testarchdb
GO

CREATE PROCEDURE [dbo].[sp_getAesParams]
AS
BEGIN
	DECLARE
		@errorMsg NVARCHAR(500) = ''

	BEGIN TRY
	
		SELECT
			c.configValor AS IV
		FROM dbo.configuracion c
		WHERE c.configDescripcion = 'IV'

		SELECT
			c.configValor AS [Key]
		FROM dbo.configuracion c
		WHERE c.configDescripcion = 'key'

	END TRY
	BEGIN CATCH
		
		SET @errorMsg = ERROR_MESSAGE()
		RAISERROR(@errorMsg, 16, 1)

	END CATCH
END
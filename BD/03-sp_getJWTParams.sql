USE testarchdb
GO

CREATE PROCEDURE [dbo].[sp_getJWTParams]
AS
BEGIN
	DECLARE
		@errorMsg NVARCHAR(500) = ''

	BEGIN TRY
		SELECT
			c.configValor AS apiSecret
		FROM dbo.configuracion c
		WHERE c.configDescripcion = 'apiSecret'

		SELECT 
			c.configValor AS expirationT
		FROM dbo.configuracion c
		WHERE c.configDescripcion = 'expiration'
	END TRY
	BEGIN CATCH

		SET @errorMsg = ERROR_MESSAGE()
		RAISERROR(@errorMsg, 16, 1)

	END CATCH
END
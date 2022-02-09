USE testarchdb
GO

INSERT INTO dbo.catEstado (
	catEstadoValor,
	catEstadoCss
) VALUES 
('Pendiente', 'panel panel-warning'),
('Completada', 'panel panel-success')

INSERT INTO dbo.configuracion (
	configDescripcion,
	configValor
) VALUES 
('IV', '460fbef60d8015f70b2543d92bce31e9')
,('key', 'efd2aa1a9db6ec805190a139e981b9c1')
,('apiSecret', '4b0bf2ac0a9f9a86360e27fe35e35b4f')
,('expiration', '1440')
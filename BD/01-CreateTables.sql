USE testarchdb
GO

CREATE TABLE configuracion (
	configId INT NOT NULL IDENTITY(1,1) CONSTRAINT pk_configId PRIMARY KEY,
	configDescripcion NVARCHAR(500) NOT NULL CONSTRAINT configDescripcion DEFAULT '',
	configValor NVARCHAR(500) NOT NULL CONSTRAINT df_configValor DEFAULT '',
	configFechaCreacion DATETIME NOT NULL CONSTRAINT df_configFechaCreacion DEFAULT GETDATE(),
	configStatus INT NOT NULL CONSTRAINT df_configStatus DEFAULT 1
)

CREATE TABLE usuario (
	userId INT NOT NULL IDENTITY(1,1) CONSTRAINT pk_userId PRIMARY KEY,
	userNombre NVARCHAR(150) NOT NULL CONSTRAINT df_userNombre DEFAULT '',
	userPapellido NVARCHAR(150) NOT NULL CONSTRAINT df_userPapellido DEFAULT '',
	userSapellido NVARCHAR(150) NOT NULL CONSTRAINT df_userSapellido DEFAULT '',
	userEmail NVARCHAR(150) NOT NULL CONSTRAINT uq_userEmail UNIQUE,
	userPass NVARCHAR(150) NOT NULL CONSTRAINT df_userPass DEFAULT '',
	userFechaCreacion DATETIME NOT NULL CONSTRAINT df_userFechaCreacion DEFAULT GETDATE(),
	userStatus INT NOT NULL CONSTRAINT df_userStatus DEFAULT 1,
)

CREATE TABLE catEstado (
	catEstadoId INT NOT NULL IDENTITY(1,1) CONSTRAINT pk_catEstadoId PRIMARY KEY,
	catEstadoValor NVARCHAR(500) NOT NULL CONSTRAINT df_catEstadoValor DEFAULT '',
	catEstadoCss NVARCHAR(500) NOT NULL CONSTRAINT df_catEstadoCss DEFAULT 'panel panel-warning',
	catEstadoFechaCreacion DATETIME NOT NULL CONSTRAINT df_catEstadoFechaCreacion DEFAULT GETDATE(),
	catEstadoStatus INT NOT NULL CONSTRAINT df_catEstadoStatus DEFAULT 1
)

CREATE TABLE tarea (
	tareaId INT NOT NULL IDENTITY(1,1) CONSTRAINT pk_tareaId PRIMARY KEY,
	tareaGuid UNIQUEIDENTIFIER NOT NULL CONSTRAINT uq_tareaGuid UNIQUE,
	tareaTitulo NVARCHAR(500) NOT NULL CONSTRAINT df_tareaTitulo DEFAULT '',
	tareaFechaCreacion DATETIME NOT NULL CONSTRAINT df_tareaFechaCreacion DEFAULT GETDATE(),
	tareaFechaMod DATETIME NOT NULL CONSTRAINT df_tareaFechaMod DEFAULT GETDATE(),
	tareaUserId INT FOREIGN KEY REFERENCES usuario(userId),
	tareaEstado INT FOREIGN KEY REFERENCES catEstado(catEstadoId),
	tareaStatus INT NOT NULL CONSTRAINT df_tareaStatus DEFAULT 1
)

CREATE TABLE tareaDetalle (
	tareaDId INT NOT NULL IDENTITY(1,1) CONSTRAINT pk_tareaDId PRIMARY KEY,
	tareaId INT FOREIGN KEY REFERENCES tarea(tareaId),
	tareaDdesc NVARCHAR(1000) NOT NULL CONSTRAINT df_tareaDdesc DEFAULT '',
	tareaDfechaCreacion DATETIME NOT NULL CONSTRAINT df_tareaDfechaCreacion DEFAULT GETDATE(),
	tareaDstatus INT NOT NULL CONSTRAINT df_tareaDstatus DEFAULT 1
)
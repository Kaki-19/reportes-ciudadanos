IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Categorias] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(60) NOT NULL,
    [Descripcion] nvarchar(200) NULL,
    [Activa] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Estados] (
    [Id] int NOT NULL,
    [Nombre] nvarchar(30) NOT NULL,
    [Color] varchar(7) NOT NULL,
    CONSTRAINT [PK_Estados] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Roles] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(30) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Usuarios] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    [Correo] nvarchar(150) NOT NULL,
    [PasswordHash] nvarchar(255) NOT NULL,
    [RolId] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL DEFAULT (SYSDATETIME()),
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Usuarios_Roles_RolId] FOREIGN KEY ([RolId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Reportes] (
    [Id] int NOT NULL IDENTITY,
    [Folio] varchar(20) NOT NULL,
    [Titulo] nvarchar(120) NOT NULL,
    [Descripcion] nvarchar(1000) NOT NULL,
    [UsuarioId] int NOT NULL,
    [CategoriaId] int NOT NULL,
    [EstadoId] int NOT NULL DEFAULT 1,
    [Latitud] decimal(9,6) NOT NULL,
    [Longitud] decimal(9,6) NOT NULL,
    [Direccion] nvarchar(250) NULL,
    [FotoUrl] nvarchar(300) NOT NULL,
    [FechaCreacion] datetime2 NOT NULL DEFAULT (SYSDATETIME()),
    [FechaActualizacion] datetime2 NULL,
    CONSTRAINT [PK_Reportes] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_Reportes_Latitud] CHECK ([Latitud] BETWEEN -90 AND 90),
    CONSTRAINT [CK_Reportes_Longitud] CHECK ([Longitud] BETWEEN -180 AND 180),
    CONSTRAINT [FK_Reportes_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Reportes_Estados_EstadoId] FOREIGN KEY ([EstadoId]) REFERENCES [Estados] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Reportes_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [HistorialEstados] (
    [Id] int NOT NULL IDENTITY,
    [ReporteId] int NOT NULL,
    [EstadoAnteriorId] int NULL,
    [EstadoNuevoId] int NOT NULL,
    [UsuarioId] int NOT NULL,
    [Comentario] nvarchar(500) NULL,
    [Fecha] datetime2 NOT NULL DEFAULT (SYSDATETIME()),
    CONSTRAINT [PK_HistorialEstados] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_HistorialEstados_Estados_EstadoAnteriorId] FOREIGN KEY ([EstadoAnteriorId]) REFERENCES [Estados] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_HistorialEstados_Estados_EstadoNuevoId] FOREIGN KEY ([EstadoNuevoId]) REFERENCES [Estados] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_HistorialEstados_Reportes_ReporteId] FOREIGN KEY ([ReporteId]) REFERENCES [Reportes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_HistorialEstados_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activa', N'Descripcion', N'Nombre') AND [object_id] = OBJECT_ID(N'[Categorias]'))
    SET IDENTITY_INSERT [Categorias] ON;
INSERT INTO [Categorias] ([Id], [Activa], [Descripcion], [Nombre])
VALUES (1, CAST(1 AS bit), N'Baches y daños en el pavimento', N'Baches'),
(2, CAST(1 AS bit), N'Luminarias apagadas o dañadas', N'Alumbrado'),
(3, CAST(1 AS bit), N'Fugas en la red de agua potable', N'Fugas de agua'),
(4, CAST(1 AS bit), N'Basura acumulada en la vía pública', N'Basura');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activa', N'Descripcion', N'Nombre') AND [object_id] = OBJECT_ID(N'[Categorias]'))
    SET IDENTITY_INSERT [Categorias] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Color', N'Nombre') AND [object_id] = OBJECT_ID(N'[Estados]'))
    SET IDENTITY_INSERT [Estados] ON;
INSERT INTO [Estados] ([Id], [Color], [Nombre])
VALUES (1, '#F59E0B', N'Pendiente'),
(2, '#3B82F6', N'En revisión'),
(3, '#8B5CF6', N'En proceso'),
(4, '#10B981', N'Resuelto'),
(5, '#EF4444', N'Rechazado');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Color', N'Nombre') AND [object_id] = OBJECT_ID(N'[Estados]'))
    SET IDENTITY_INSERT [Estados] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([Id], [Nombre])
VALUES (1, N'Ciudadano'),
(2, N'Administrador');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] OFF;
GO

CREATE UNIQUE INDEX [IX_Categorias_Nombre] ON [Categorias] ([Nombre]);
GO

CREATE UNIQUE INDEX [IX_Estados_Nombre] ON [Estados] ([Nombre]);
GO

CREATE INDEX [IX_HistorialEstados_EstadoAnteriorId] ON [HistorialEstados] ([EstadoAnteriorId]);
GO

CREATE INDEX [IX_HistorialEstados_EstadoNuevoId] ON [HistorialEstados] ([EstadoNuevoId]);
GO

CREATE INDEX [IX_HistorialEstados_ReporteId] ON [HistorialEstados] ([ReporteId]);
GO

CREATE INDEX [IX_HistorialEstados_UsuarioId] ON [HistorialEstados] ([UsuarioId]);
GO

CREATE INDEX [IX_Reportes_CategoriaId] ON [Reportes] ([CategoriaId]);
GO

CREATE INDEX [IX_Reportes_EstadoId] ON [Reportes] ([EstadoId]);
GO

CREATE UNIQUE INDEX [IX_Reportes_Folio] ON [Reportes] ([Folio]);
GO

CREATE INDEX [IX_Reportes_UsuarioId] ON [Reportes] ([UsuarioId]);
GO

CREATE UNIQUE INDEX [IX_Roles_Nombre] ON [Roles] ([Nombre]);
GO

CREATE UNIQUE INDEX [IX_Usuarios_Correo] ON [Usuarios] ([Correo]);
GO

CREATE INDEX [IX_Usuarios_RolId] ON [Usuarios] ([RolId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261127002106_InitialCreate', N'8.0.11');
GO

COMMIT;
GO


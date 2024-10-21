/****** TABLAS ******/

/** USUARIO **/
CREATE TABLE Usuario(
	ID_Usuario INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100) NOT NULL,
	Apellido VARCHAR(100) NOT NULL,
	Fecha_Nacimiento DATE NOT NULL,
	Teléfono VARCHAR(20) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Contraseña VARCHAR(100) NOT NULL,
	ID_Rol INT NULL,
	Estado BIT NOT NULL
	);

/** CATEGORIA **/
CREATE TABLE Categoria (
    ID_Categoria INT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(100) NOT NULL,
);

/** PRODUCTOS **/
CREATE TABLE Productos(
	ID_Producto INT PRIMARY KEY,
	Nombre_Producto VARCHAR(100) NOT NULL,
	Descripcion VARCHAR(100) NOT NULL,
	Precio DECIMAL(10,2) NOT NULL,
	ID_Categoria INT,
	Stock INT DEFAULT 0,
	Talla VARCHAR(20),
	Color VARCHAR(20),
	FOREIGN KEY (ID_Categoria) REFERENCES Categoria(ID_Categoria)
	);

/** PEDIDOS **/
CREATE TABLE Pedidos(
	ID_Pedido INT PRIMARY KEY,
	ID_Usuario INT,
	Fecha_Pedido DATE NOT NULL,
	Monto_Total DECIMAL(10,2) NOT NULL,
	Metodo_Pedido VARCHAR(50),
	Metodo_Pago VARCHAR(50),
	FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID_Usuario)
	);

/** DETALLES PEDIDO **/
CREATE TABLE Detalles_Pedido(
	ID_Detalle_Pedido INT PRIMARY KEY,
    ID_Pedido INT,
    ID_Producto INT,
    Cantidad INT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    FOREIGN   
 KEY (ID_Pedido) REFERENCES Pedidos(ID_Pedido),
    FOREIGN KEY (ID_Producto) REFERENCES Productos(ID_Producto)
	);

/****** PROCEDIMIENTOS ALMACENADOS ******/

/** REGISTRAR USUARIO **/
CREATE PROCEDURE RegistrarUsuario
	@Nombre				varchar(100),
	@Apellido			varchar(100),
	@Fecha_Nacimiento	date,
	@Teléfono			varchar(20),
	@Email				varchar(100),
	@Contraseña			varchar(100),
	@Identificacion		varchar(50)
AS
BEGIN

	DECLARE @Rol	TINYINT = 1,
			@Estado	BIT		= 1

	IF NOT EXISTS(SELECT 1 FROM dbo.Usuario WHERE Email = @Email OR Identificacion = @Identificacion)
	BEGIN

		INSERT INTO dbo.Usuario(Nombre,Apellido,Fecha_Nacimiento,Teléfono,Email,Contraseña,Identificacion,ID_Rol,Estado)
		VALUES (@Nombre,@Apellido,@Fecha_Nacimiento,@Teléfono,@Email,@Contraseña,@Identificacion,@Rol,@Estado)

	END

END;

/**EXEC RegistrarUsuario 'Danny', 'Rojas', '1995-08-27', '60015555', 'crojas1@chemasfutcr.com', '$Prueba1234*', '116160205'
Error Msg 547, Level 16, State 0, Procedure RegistrarUsuario, Line 18 [Batch Start Line 85]
The INSERT statement conflicted with the FOREIGN KEY constraint "FK_Usuario_Rol". The conflict occurred in database "ChemasfutCR", table "dbo.Rol", column 'ID_Rol'. **//

/** ACTUALIZAR USUARIO **/
CREATE PROCEDURE ActualizarUsuario
    @ID_Usuario INT,
    @Nombre VARCHAR(100),
    @Apellido VARCHAR(100),
    @Fecha_Nacimiento DATE,
    @Teléfono VARCHAR(20),
    @ID_Rol INT,
    @Estado BIT
AS
BEGIN
    UPDATE Usuario
    SET Nombre = @Nombre,
        Apellido = @Apellido,
        Fecha_Nacimiento = @Fecha_Nacimiento,
        Teléfono = @Teléfono,
        ID_Rol = @ID_Rol,
        Estado = @Estado
    WHERE ID_Usuario = @ID_Usuario;
END;

/** INACTIVAR USUARIO **/
CREATE PROCEDURE InactivarUsuario
    @ID_Usuario INT
AS
BEGIN
    UPDATE Usuario
    SET Estado = 0
    WHERE ID_Usuario = @ID_Usuario;
END;

/** INICIAR SESION **/
CREATE PROCEDURE IniciarSesion
    @Email       VARCHAR(100),
    @Contraseña  VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.ID_Usuario,
        U.Nombre,
        U.Email,
        U.ID_Rol,
        R.Descripcion AS Rol_Descripcion,
        U.Estado
    FROM 
        dbo.Usuario U
    INNER JOIN 
        dbo.Rol R ON U.ID_Rol = R.ID_Rol
    WHERE 
        U.Email = @Email
        AND U.Contraseña = @Contraseña
        AND U.Estado = 1;
END

/** INSERTAR USUARIO **/
CREATE PROCEDURE InsertarUsuario
    @Nombre VARCHAR(100),
    @Apellido VARCHAR(100),
    @Fecha_Nacimiento DATE,
    @Teléfono VARCHAR(20),
    @Email VARCHAR(100),
    @Contraseña VARCHAR(100),
    @ID_Rol INT,
    @Estado BIT
AS
BEGIN
    INSERT INTO Usuario (Nombre, Apellido, Fecha_Nacimiento, Teléfono, Email, Contraseña, ID_Rol, Estado)
    VALUES (@Nombre, @Apellido, @Fecha_Nacimiento, @Teléfono, @Email, @Contraseña, @ID_Rol, @Estado);
END;

/** OBTENER TODOS LOS USUARIOS **/
CREATE PROCEDURE ObtenerTodosLosUsuarios
AS
BEGIN
    SELECT ID_Usuario,
        Nombre,
        Apellido,
        Fecha_Nacimiento,
        Teléfono,
        Email,
        Contraseña,
        ID_Rol,
        Estado
	FROM Usuario
END;

/** OBTENER USUARIO POR ID **/
CREATE PROCEDURE ObtenerUsuarioPorId
    @ID_Usuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ID_Usuario,
        Nombre,
        Apellido,
        Fecha_Nacimiento,
        Teléfono,
        Email,
        Contraseña,
        ID_Rol,
        Estado
    FROM 
        Usuario
    WHERE 
        ID_Usuario = @ID_Usuario;
END;

/** UK tabla usuario campo correo **/
ALTER TABLE Usuario
ADD CONSTRAINT UK_Usuario UNIQUE (Email);


use master

--if exists(SELECT * FROM sys.databases WHERE name = 'ClasesDeSalsa')
--BEGIN
--	DROP DATABASE ClasesDeSalsa
--END
--go
 
create database  ClasesDeSalsa
go


use ClasesDeSalsa
go

create table Empleado (
    Usuario nvarchar(50) not null primary key,
    Contraseña nvarchar(255) not null
)
go

create table Miembro (
    IdMiembro int identity(1,1) primary key,
    NombreCompleto nvarchar(100) not null,
    Telefono nvarchar(15),
    Mail nvarchar(100)
    )
go
create table FinanzasMensuales (
    IdFinanzaMensual int identity(1,1) primary key,
    Año int not null,
    Mes int not null,
    Rent decimal(10,2) not null default 0, 
    Observacion nvarchar(255),
    
    constraint UQ_FinanzasMensuales_Año_Mes unique (Año, Mes),
    constraint CK_FinanzasMensuales_Mes check (Mes between 1 and 12)

)

CREATE TABLE TipoMembresia
(
    IdTipoMembresia INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    DuracionDias INT NULL,
    CantidadClases INT ,
    EsIlimitada BIT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL)
go

create table Membresia (
    IdMembresia int identity(1,1) primary key,
    IdMiembro int not null foreign key references Miembro(IdMiembro),
    IdTipoMembresia int not null foreign key references TipoMembresia(IdTipoMembresia),
    
    FechaInicio date not null,
    FechaExpiracion date not null,
    Costo decimal(10,2) not null,
    Observacion nvarchar(255),
    CantClases int not null,
    Pago varchar(10),
   Actbt bit )
go



insert into TipoMembresia (Nombre, DuracionDias, CantidadClases, EsIlimitada, Precio)
values 
    ('Week Intro', 7, 1, 0, 10), --7 days,cost $10,1 classe
    ('Unlimited/week', 7, null, 1, 50),--7 days, $50 , unlimited
    ('Casual', 180, 1, 0, 24),--180 days,cost $24,1 classe
    ('2 times/week', 7, 2, 0, 39),--7 days,$39, 2 classes
    ('Free trial', 7, 1, 0, 0),--180 days
    ('10 pack', 84, 10, 0, 210), --84 days,$210 ,10 classes
    ('Unlimited/month', 28, null, 1, 170),--28 days,$170
    ('4 Pack', 28, 4, 0, 80)  --28 days , cost $80, 4 classes
    


go

--  Empleado
insert into empleado (usuario, contraseña) values ('Saba', 'Dxvk9VURcR3/LFOqpr7KKA==:UZFTsDX1Nxx8goQNmbvKxYArVZ8dp3iYMQPyVlrVxto=')

go

------sp-------
create procedure logueo
    @usul nvarchar(50)
as
begin
    select usuario, contraseña
    from empleado
    where usuario = @usul
end

--ERRORES 30/09


-- si preciono alta no hay algo que me diga que los campos peude estar vacios 

go

create procedure BuscarMiembro
 @IdMiembro INT
as 
begin
select *
from Miembro
where IdMiembro=@IdMiembro
end

go

create procedure AltaMiembro
    @Nom nvarchar(100),
    @Telefono nvarchar(15),
    @Mail nvarchar(100)
as
begin
    SET NOCOUNT ON

    declare @IdMiembro int

    
    insert into Miembro (NombreCompleto, Telefono, Mail)
    values (@Nom, @Telefono, @Mail)

    set @IdMiembro = SCOPE_IDENTITY()

    
    declare @Hoy DATE = GETDATE()
    declare @Expiracion DATE = DATEADD(DAY, 7, @Hoy)
    declare @Mes NVARCHAR(15) = DATENAME(MONTH, @Hoy)

-- free trial is number 5
    insert into Membresia (IdMiembro,IdTipoMembresia, FechaInicio, FechaExpiracion,Costo, Observacion, CantClases, Pago,Actbt) 
        values (@IdMiembro,5,@Hoy, @Expiracion,0.00, NULL, 0, 'Cash',1)

    return 1
end
GO

create procedure modificarMiembro
    @IdMiembro int,
    @Nombre nvarchar(100),
    @Telefono nvarchar(15),
    @Mail nvarchar(100)
AS
begin

    IF NOT EXISTS (SELECT * FROM Miembro WHERE IdMiembro = @IdMiembro)
    begin
        return -1  
    end

    update Miembro
    set NombreCompleto = @Nombre, Telefono = @Telefono, Mail = @Mail
    where IdMiembro = @IdMiembro

  
    return 1
end



go

CREATE procedure Historial
AS 
begin
 
    select 
        m.IdMiembro,
        m.NombreCompleto,
        mem.IdMembresia,
        mem.IdTipoMembresia,
       tm.Nombre,
        mem.Actbt,
        mem.FechaInicio,
        mem.FechaExpiracion,
        mem.Costo,
        mem.Observacion,
        mem.CantClases,
        mem.Pago
    from 
        Miembro m
    inner join
        Membresia mem on m.IdMiembro = mem.IdMiembro  
                        --AND mem.Actbt = 0 -- Solo membresías inactivas o expiradas
    inner join
        TipoMembresia tm on mem.IdTipoMembresia = tm.IdTipoMembresia
    order by 
         mem.IdMembresia desc
end



go


create procedure ListarMiembrosActivos --arreglada
AS
begin
 exec actualizarvencidas
    select 
        m.IdMiembro, 
        m.NombreCompleto, 
        m.Telefono, --no necesito llevarlo
        m.Mail, --no necesito llevarlo
        me.IdMembresia,
        me.FechaInicio,
        me.FechaExpiracion,
        me.Costo,
        me.Observacion,
        me.CantClases,
        me.Pago,
        tm.IdTipoMembresia,
        tm.Nombre
        
    from Miembro m
    inner join Membresia me on m.IdMiembro = me.IdMiembro
    inner join TipoMembresia tm on me.IdTipoMembresia = tm.IdTipoMembresia
    where me.Actbt = 1
    order by me.FechaInicio desc
end
go

create procedure ListarMiembros
as
begin
   

    select
        m.IdMiembro,
        m.NombreCompleto,
        m.Telefono,
        m.Mail,
        me.IdMembresia,
        me.Actbt,
        me.FechaInicio,
        me.FechaExpiracion,
        me.IdTipoMembresia,
        tm.Nombre 
    from Miembro m
   left join Membresia me
        on m.IdMiembro = me.IdMiembro
        and me.Actbt = 1
    left join TipoMembresia tm
        on me.IdTipoMembresia = tm.IdTipoMembresia
   order by m.NombreCompleto
end

GO

create procedure BuscarMembresia
    @IdMembresia INT
as
begin
    select
        me.IdMembresia,
        me.IdMiembro,
        me.IdTipoMembresia,
        me.CantClases,
        me.FechaInicio,
        me.FechaExpiracion,
        me.Costo,
        me.Observacion,
        me.Pago,
        me.Actbt,
        tm.Nombre

    from Membresia me

    inner join TipoMembresia tm
        on me.IdTipoMembresia = tm.IdTipoMembresia

    where me.IdMembresia = @IdMembresia
end

GO

create procedure AltaMembresia
    @idMiembro int,
    @idTipoMembresia int,
    @observacion nvarchar(255),
    @pago varchar(10)
as
begin
    if not exists (
        select 1 from Miembro where IdMiembro = @idMiembro
    )
        return -1

    if not exists (
        select 1 from TipoMembresia where IdTipoMembresia = @idTipoMembresia
    )
        return -2

    if exists (
        select 1 from Membresia
        where IdMiembro = @idMiembro and Actbt = 1
    )
        return -3

    if @pago not in ('Cash', 'Card')
        return -4

    declare @precio decimal(10,2)
    declare @duracionDias int

    select
        @precio = Precio,
        @duracionDias = DuracionDias
    from TipoMembresia
    where IdTipoMembresia = @idTipoMembresia

    declare @fechaInicio date = cast(getdate() as date)
    declare @fechaExpiracion date = dateadd(day, @duracionDias, @fechaInicio)

    declare @cantClasesFinal int = 0

    insert into Membresia
    (
        IdMiembro,
        IdTipoMembresia,
        FechaInicio,
        FechaExpiracion,
        Costo,
        Observacion,
        CantClases,
        Pago,
        Actbt
    )
    values
    (
        @idMiembro,
        @idTipoMembresia,
        @fechaInicio,
        @fechaExpiracion,
        @precio,
        @observacion,
        @cantClasesFinal,
        @pago,
        1
    )

    return 1
end

go

create procedure actualizarMembresia
    @idMembresia int
as
begin
    set nocount on

    declare @cantidadClasesActual int
    declare @cantidadClasesMax int
    declare @esIlimitada bit

    select
        @cantidadClasesActual = m.CantClases,
        @cantidadClasesMax = t.CantidadClases,
        @esIlimitada = t.EsIlimitada
    from Membresia m
    inner join TipoMembresia t
        on m.IdTipoMembresia = t.IdTipoMembresia
    where m.IdMembresia = @idMembresia
      and m.Actbt = 1

    if @cantidadClasesActual is null
        return

    update Membresia
    set CantClases = CantClases + 1
    where IdMembresia = @idMembresia
      and Actbt = 1

    set @cantidadClasesActual = @cantidadClasesActual + 1

    if @esIlimitada = 0
       and @cantidadClasesActual >= @cantidadClasesMax
    begin
        update Membresia
        set Actbt = 0
        where IdMembresia = @idMembresia
    end
end

go

create procedure Eliminarmembresia
@idmembresia int
as
begin
    if not exists (select * from membresia where idmembresia = @idmembresia)
    begin
        return -1
    end

    delete from membresia where idmembresia = @idmembresia
    return 1
end

go

create procedure ModificarNotas
@Observacion  nvarchar(255),
 @IdMembresia int
 as
 begin
    
    IF NOT EXISTS (SELECT * FROM Membresia WHERE IdMembresia = @IdMembresia)
    begin
        return -1
    end
    
   
    update Membresia
    set Observacion=@Observacion
    where IdMembresia = @IdMembresia

    return 1  
end

go

create procedure actualizarvencidas
as
begin
    update membresia
    set actbt = 0
    where actbt = 1
    and fechaexpiracion < cast(getdate() as date)
end
go

-------------------------------factura----------------------------------------


create procedure listarfinanzasmensuales
 @año int
as
begin
    select * from FinanzasMensuales
     where Año = @año
    order by mes
end
go 




---------------------------------------------------------------------




insert into Miembro (NombreCompleto, Telefono, Mail)
values
    (N'Oliver Bennett', N'+61437182650', N'bluegum.482@gmail.com'),
    (N'Charlotte Mitchell', N'+61458301742', N'cassie.moon91@hotmail.com'),
    (N'Jack Thompson', N'+61412659083', N'riverstone.k7@gmail.com'),
    (N'Amelia Parker', N'+61490427136', N'lottie.bee204@hotmail.com'),
    (N'Noah Williams', N'+61463849217', N'koala.tracks58@gmail.com'),
    (N'Isla Campbell', N'+61427518394', N'peachyday.731@hotmail.com'),
    (N'William Anderson', N'+61481930625', N'cloudnine.fern@gmail.com'),
    (N'Mia Roberts', N'+61435092718', N'jellybean.402@hotmail.com'),
    (N'Lucas Walker', N'+61459274603', N'gumtree.radio@gmail.com'),
    (N'Grace Kelly', N'+61410738529', N'sunny_side.86@hotmail.com'),
    (N'Levi Harris', N'+61476301948', N'paperkite.529@gmail.com'),
    (N'Chloe Martin', N'+61423865170', N'cocoandmint14@hotmail.com'),
    (N'Henry Clark', N'+61492540731', N'quiet.ocean.63@gmail.com'),
    (N'Zoe Lewis', N'+61468123905', N'buttonmoon.718@hotmail.com'),
    (N'James Young', N'+61431486027', N'lanternfox.95@gmail.com'),
    (N'Ella Hall', N'+61457029316', N'marblepeach.320@hotmail.com'),
    (N'Benjamin Wright', N'+61419674852', N'foggy.morning77@gmail.com'),
    (N'Sophie King', N'+61484362509', N'tinyplanet.601@hotmail.com'),
    (N'Charlie Scott', N'+61426951738', N'wattlebird.28@gmail.com'),
    (N'Ruby Green', N'+61470538164', N'mango.sorbet42@hotmail.com'),
    (N'Alexander Baker', N'+61438206491', N'coppercloud.507@gmail.com'),
    (N'Evie Adams', N'+61491725308', N'violetpocket.33@hotmail.com'),
    (N'Leo Nelson', N'+61465190827', N'saltwater.radio@gmail.com'),
    (N'Harper Carter', N'+61420374615', N'lemon.drift.804@hotmail.com'),
    (N'Sebastian Morris', N'+61473851260', N'papercrane.119@gmail.com'),
    (N'Layla Cook', N'+61416982473', N'echidna.club7@hotmail.com'),
    (N'Ethan Bailey', N'+61482635091', N'goldenhour.266@gmail.com'),
    (N'Willow Richardson', N'+61439017284', N'blueberry.toast39@hotmail.com'),
    (N'Mason Cooper', N'+61454720836', N'afterglow.514@gmail.com'),
    (N'Isabella Murphy', N'+61428163957', N'cinnamoncloud.62@hotmail.com')
go


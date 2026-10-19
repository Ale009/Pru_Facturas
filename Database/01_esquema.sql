CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    CREATE TABLE `clientes` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nombre` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Nit` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `Direccion` varchar(200) CHARACTER SET utf8mb4 NULL,
        `Activo` tinyint(1) NOT NULL,
        CONSTRAINT `PK_clientes` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    CREATE TABLE `productos` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nombre` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `PrecioUnitario` decimal(18,2) NOT NULL,
        `Activo` tinyint(1) NOT NULL,
        CONSTRAINT `PK_productos` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    CREATE TABLE `usuarios` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `NombreUsuario` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `PasswordHash` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Activo` tinyint(1) NOT NULL,
        CONSTRAINT `PK_usuarios` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    CREATE TABLE `facturas` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Fecha` datetime(6) NOT NULL,
        `ClienteId` int NOT NULL,
        `UsuarioId` int NOT NULL,
        `Total` decimal(18,2) NOT NULL,
        `Estado` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `FechaAnulacion` datetime(6) NULL,
        `MotivoAnulacion` varchar(200) CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_facturas` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_facturas_clientes_ClienteId` FOREIGN KEY (`ClienteId`) REFERENCES `clientes` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_facturas_usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    CREATE TABLE `factura_detalles` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `FacturaId` int NOT NULL,
        `ProductoId` int NOT NULL,
        `Cantidad` int NOT NULL,
        `PrecioUnitario` decimal(18,2) NOT NULL,
        `Subtotal` decimal(18,2) NOT NULL,
        CONSTRAINT `PK_factura_detalles` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_factura_detalles_facturas_FacturaId` FOREIGN KEY (`FacturaId`) REFERENCES `facturas` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_factura_detalles_productos_ProductoId` FOREIGN KEY (`ProductoId`) REFERENCES `productos` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    CREATE INDEX `IX_factura_detalles_FacturaId` ON `factura_detalles` (`FacturaId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    CREATE INDEX `IX_factura_detalles_ProductoId` ON `factura_detalles` (`ProductoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    CREATE INDEX `IX_facturas_ClienteId` ON `facturas` (`ClienteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    CREATE INDEX `IX_facturas_UsuarioId` ON `facturas` (`UsuarioId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    CREATE UNIQUE INDEX `IX_usuarios_NombreUsuario` ON `usuarios` (`NombreUsuario`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261019175135_Inicial') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261019175135_Inicial', '8.0.32');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;


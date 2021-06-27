CREATE DATABASE RaspiDB;
GO

USE RaspiDB;
GO

/*
CREATE LOGIN appuser WITH PASSWORD='', DEFAULT_DATABASE=RaspiDB;
GO
*/

CREATE USER appuser FOR LOGIN appuser WITH DEFAULT_SCHEMA=dbo;
GO

ALTER ROLE db_owner ADD MEMBER appuser;
GO

CREATE TABLE dbo.tabDevice
(
    fId int NOT NULL,
    fName nvarchar(50) NOT NULL,
    fLatitude float NOT NULL,
    fLongitude float NOT NULL,
    fActive bit NOT NULL,
    CONSTRAINT PK_tabDevice_fId PRIMARY KEY CLUSTERED (fId)
)

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Represents the device Id which is defined by the system' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tabDevice', @level2type=N'COLUMN',@level2name=N'fId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Userfriendly device name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tabDevice', @level2type=N'COLUMN',@level2name=N'fName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Location parameter of the device' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tabDevice', @level2type=N'COLUMN',@level2name=N'fLatitude'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Location parameter of the device' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tabDevice', @level2type=N'COLUMN',@level2name=N'fLongitude'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Device is active' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tabDevice', @level2type=N'COLUMN',@level2name=N'fActive'
GO

INSERT INTO dbo.tabDevice(fId, fName, fLatitude, fLongitude, fActive)
VALUES
(1, 'MeidlingPi', 48.20849, 16.37208, 1),
(2, 'HietzingPi', 40.20849, 10.37208, 1)

CREATE SEQUENCE dbo.SQ_tabTemperatureHistory_fId
    START WITH 1
    INCREMENT BY 1
    NO CACHE;
GO

CREATE TABLE tabTemperatureHistory
(
    fId bigint NOT NULL DEFAULT (NEXT VALUE FOR dbo.SQ_tabTemperatureHistory_fId),
    fDate datetime2 NOT NULL,
    fDegreeCelsius float NOT NULL,
    frDeviceId int NOT NULL,
    CONSTRAINT PK_tabTemperatureHistory_fId PRIMARY KEY CLUSTERED (fId),
    CONSTRAINT FK_tabDevice_tabTemperatureHistory FOREIGN KEY (frDeviceId) REFERENCES tabDevice (fId)
)
GO

INSERT INTO dbo.tabTemperatureHistory(fDate, fDegreeCelsius, frDeviceId)
VALUES
('2020-06-19 00:00:00', 18.6, 1),
('2020-06-19 12:00:00', 30.6, 1),
('2020-06-20 00:00:00', 20.6, 1),
('2020-06-20 12:00:00', 31.6, 1),
('2020-06-21 00:00:00', 20.6, 1),
('2020-06-21 12:00:00', 28.6, 1),
('2020-06-19 00:00:00', 18.6, 2)

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Surrogate key for device temperature records' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tabTemperatureHistory', @level2type=N'COLUMN',@level2name=N'fId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of the recording' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE', @level1name=N'tabTemperatureHistory', @level2type=N'COLUMN',@level2name=N'fDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Actual measurement' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE', @level1name=N'tabTemperatureHistory', @level2type=N'COLUMN',@level2name=N'fDegreeCelsius'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Foreign key to related device' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE', @level1name=N'tabTemperatureHistory', @level2type=N'COLUMN',@level2name=N'frDeviceId'
GO




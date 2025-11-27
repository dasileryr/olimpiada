-- Скрипт создания базы данных "Олимпиада"
-- Сервер: DESKTOP-K3OP1CJ

USE master;
GO

-- Удаление базы данных, если она существует
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'Олимпиада')
    DROP DATABASE [Олимпиада];
GO

-- Создание базы данных
-- Используйте стандартный путь SQL Server или укажите свой путь
-- Если путь не указан, SQL Server использует путь по умолчанию
CREATE DATABASE [Олимпиада];
GO

USE [Олимпиада];
GO

-- Таблица стран
CREATE TABLE Countries (
    CountryId INT PRIMARY KEY IDENTITY(1,1),
    CountryName NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- Таблица видов спорта
CREATE TABLE Sports (
    SportId INT PRIMARY KEY IDENTITY(1,1),
    SportName NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- Таблица олимпиад
CREATE TABLE Olympics (
    OlympicsId INT PRIMARY KEY IDENTITY(1,1),
    Year INT NOT NULL,
    IsSummer BIT NOT NULL, -- 1 - летняя, 0 - зимняя
    HostCountryId INT NOT NULL,
    City NVARCHAR(100) NOT NULL,
    FOREIGN KEY (HostCountryId) REFERENCES Countries(CountryId),
    UNIQUE(Year, IsSummer)
);
GO

-- Таблица спортсменов
CREATE TABLE Athletes (
    AthleteId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50) NULL,
    CountryId INT NOT NULL,
    DateOfBirth DATE NOT NULL,
    Photo VARBINARY(MAX) NULL, -- Фото спортсмена
    FOREIGN KEY (CountryId) REFERENCES Countries(CountryId)
);
GO

-- Таблица связи олимпиады и видов спорта
CREATE TABLE OlympicSports (
    OlympicSportId INT PRIMARY KEY IDENTITY(1,1),
    OlympicsId INT NOT NULL,
    SportId INT NOT NULL,
    ParticipantCount INT NOT NULL DEFAULT 0,
    FOREIGN KEY (OlympicsId) REFERENCES Olympics(OlympicsId) ON DELETE CASCADE,
    FOREIGN KEY (SportId) REFERENCES Sports(SportId),
    UNIQUE(OlympicsId, SportId)
);
GO

-- Таблица результатов (медали)
CREATE TABLE Results (
    ResultId INT PRIMARY KEY IDENTITY(1,1),
    OlympicsId INT NOT NULL,
    SportId INT NOT NULL,
    AthleteId INT NOT NULL,
    MedalType INT NOT NULL, -- 1 - золото, 2 - серебро, 3 - бронза
    FOREIGN KEY (OlympicsId) REFERENCES Olympics(OlympicsId) ON DELETE CASCADE,
    FOREIGN KEY (SportId) REFERENCES Sports(SportId),
    FOREIGN KEY (AthleteId) REFERENCES Athletes(AthleteId),
    CHECK (MedalType IN (1, 2, 3))
);
GO

-- Создание индексов для улучшения производительности
CREATE INDEX IX_Olympics_Year ON Olympics(Year);
CREATE INDEX IX_Olympics_HostCountry ON Olympics(HostCountryId);
CREATE INDEX IX_Athletes_Country ON Athletes(CountryId);
CREATE INDEX IX_Results_Olympics ON Results(OlympicsId);
CREATE INDEX IX_Results_Sport ON Results(SportId);
CREATE INDEX IX_Results_Athlete ON Results(AthleteId);
CREATE INDEX IX_Results_Medal ON Results(MedalType);
GO

-- Вставка тестовых данных
INSERT INTO Countries (CountryName) VALUES
(N'Россия'),
(N'США'),
(N'Китай'),
(N'Германия'),
(N'Франция'),
(N'Великобритания'),
(N'Япония'),
(N'Канада'),
(N'Италия'),
(N'Австралия');
GO

INSERT INTO Sports (SportName) VALUES
(N'Плавание'),
(N'Легкая атлетика'),
(N'Гимнастика'),
(N'Биатлон'),
(N'Лыжные гонки'),
(N'Фигурное катание'),
(N'Хоккей'),
(N'Футбол'),
(N'Баскетбол'),
(N'Теннис');
GO

-- Пример олимпиады
INSERT INTO Olympics (Year, IsSummer, HostCountryId, City) VALUES
(2020, 1, 7, N'Токио'),
(2018, 0, 4, N'Пхёнчхан'),
(2016, 1, 5, N'Рио-де-Жанейро'),
(2014, 0, 1, N'Сочи');
GO


using Microsoft.Data.SqlClient;
using Olimpiada.Models;
using System.Data;

namespace Olimpiada.DataAccess
{
    public class DatabaseHelper
    {
        private readonly string connectionString;
        private readonly string masterConnectionString;

        public DatabaseHelper()
        {
            connectionString = @"Server=DESKTOP-K3OP1CJ;Database=Олимпиада;Integrated Security=True;TrustServerCertificate=True;";
            masterConnectionString = @"Server=DESKTOP-K3OP1CJ;Database=master;Integrated Security=True;TrustServerCertificate=True;";
            EnsureDatabaseExists();
        }

        private void EnsureDatabaseExists()
        {
            try
            {
                // Проверяем существование базы данных
                using (var connection = new SqlConnection(masterConnectionString))
                {
                    connection.Open();
                    var checkDbQuery = "SELECT COUNT(*) FROM sys.databases WHERE name = 'Олимпиада'";
                    using (var command = new SqlCommand(checkDbQuery, connection))
                    {
                        var exists = (int)command.ExecuteScalar() > 0;
                        if (!exists)
                        {
                            // Создаем базу данных
                            var createDbQuery = "CREATE DATABASE [Олимпиада]";
                            using (var createCommand = new SqlCommand(createDbQuery, connection))
                            {
                                createCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                // Проверяем существование таблиц
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var checkTableQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Olympics'";
                    using (var command = new SqlCommand(checkTableQuery, connection))
                    {
                        var tableExists = (int)command.ExecuteScalar() > 0;
                        if (!tableExists)
                        {
                            CreateTables(connection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    $"Ошибка при создании базы данных: {ex.Message}\n\n" +
                    "Убедитесь, что:\n" +
                    "1. SQL Server запущен\n" +
                    "2. У вас есть права на создание баз данных\n" +
                    "3. Сервер DESKTOP-K3OP1CJ доступен\n\n" +
                    "Или выполните скрипт CreateDatabase.sql вручную в SQL Server Management Studio.",
                    "Ошибка базы данных",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                throw;
            }
        }

        private void CreateTables(SqlConnection connection)
        {
            var queries = new[]
            {
                @"CREATE TABLE Countries (
                    CountryId INT PRIMARY KEY IDENTITY(1,1),
                    CountryName NVARCHAR(100) NOT NULL UNIQUE
                )",
                @"CREATE TABLE Sports (
                    SportId INT PRIMARY KEY IDENTITY(1,1),
                    SportName NVARCHAR(100) NOT NULL UNIQUE
                )",
                @"CREATE TABLE Olympics (
                    OlympicsId INT PRIMARY KEY IDENTITY(1,1),
                    Year INT NOT NULL,
                    IsSummer BIT NOT NULL,
                    HostCountryId INT NOT NULL,
                    City NVARCHAR(100) NOT NULL,
                    FOREIGN KEY (HostCountryId) REFERENCES Countries(CountryId),
                    UNIQUE(Year, IsSummer)
                )",
                @"CREATE TABLE Athletes (
                    AthleteId INT PRIMARY KEY IDENTITY(1,1),
                    FirstName NVARCHAR(50) NOT NULL,
                    LastName NVARCHAR(50) NOT NULL,
                    MiddleName NVARCHAR(50) NULL,
                    CountryId INT NOT NULL,
                    DateOfBirth DATE NOT NULL,
                    Photo VARBINARY(MAX) NULL,
                    FOREIGN KEY (CountryId) REFERENCES Countries(CountryId)
                )",
                @"CREATE TABLE OlympicSports (
                    OlympicSportId INT PRIMARY KEY IDENTITY(1,1),
                    OlympicsId INT NOT NULL,
                    SportId INT NOT NULL,
                    ParticipantCount INT NOT NULL DEFAULT 0,
                    FOREIGN KEY (OlympicsId) REFERENCES Olympics(OlympicsId) ON DELETE CASCADE,
                    FOREIGN KEY (SportId) REFERENCES Sports(SportId),
                    UNIQUE(OlympicsId, SportId)
                )",
                @"CREATE TABLE Results (
                    ResultId INT PRIMARY KEY IDENTITY(1,1),
                    OlympicsId INT NOT NULL,
                    SportId INT NOT NULL,
                    AthleteId INT NOT NULL,
                    MedalType INT NOT NULL,
                    FOREIGN KEY (OlympicsId) REFERENCES Olympics(OlympicsId) ON DELETE CASCADE,
                    FOREIGN KEY (SportId) REFERENCES Sports(SportId),
                    FOREIGN KEY (AthleteId) REFERENCES Athletes(AthleteId),
                    CHECK (MedalType IN (1, 2, 3))
                )",
                @"CREATE INDEX IX_Olympics_Year ON Olympics(Year)",
                @"CREATE INDEX IX_Olympics_HostCountry ON Olympics(HostCountryId)",
                @"CREATE INDEX IX_Athletes_Country ON Athletes(CountryId)",
                @"CREATE INDEX IX_Results_Olympics ON Results(OlympicsId)",
                @"CREATE INDEX IX_Results_Sport ON Results(SportId)",
                @"CREATE INDEX IX_Results_Athlete ON Results(AthleteId)",
                @"CREATE INDEX IX_Results_Medal ON Results(MedalType)"
            };

            foreach (var query in queries)
            {
                using (var command = new SqlCommand(query, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // Игнорируем ошибки, если объект уже существует
                        if (!ex.Message.Contains("уже существует") && !ex.Message.Contains("already exists"))
                        {
                            throw;
                        }
                    }
                }
            }

            // Вставляем тестовые данные, если их еще нет
            InsertTestData(connection);
        }

        private void InsertTestData(SqlConnection connection)
        {
            // Проверяем, есть ли уже данные
            using (var checkCommand = new SqlCommand("SELECT COUNT(*) FROM Countries", connection))
            {
                var count = (int)checkCommand.ExecuteScalar();
                if (count > 0) return; // Данные уже есть
            }

            var testData = new[]
            {
                @"INSERT INTO Countries (CountryName) VALUES
                (N'Россия'), (N'США'), (N'Китай'), (N'Германия'), (N'Франция'),
                (N'Великобритания'), (N'Япония'), (N'Канада'), (N'Италия'), (N'Австралия')",
                @"INSERT INTO Sports (SportName) VALUES
                (N'Плавание'), (N'Легкая атлетика'), (N'Гимнастика'), (N'Биатлон'), (N'Лыжные гонки'),
                (N'Фигурное катание'), (N'Хоккей'), (N'Футбол'), (N'Баскетбол'), (N'Теннис')"
            };

            foreach (var query in testData)
            {
                using (var command = new SqlCommand(query, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        // Игнорируем ошибки при вставке тестовых данных
                    }
                }
            }
        }

        // Методы для работы с олимпиадами
        public List<Olympics> GetAllOlympics()
        {
            var olympics = new List<Olympics>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"SELECT o.OlympicsId, o.Year, o.IsSummer, o.HostCountryId, o.City, c.CountryName 
                             FROM Olympics o 
                             INNER JOIN Countries c ON o.HostCountryId = c.CountryId 
                             ORDER BY o.Year DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            olympics.Add(new Olympics
                            {
                                OlympicsId = reader.GetInt32(0),
                                Year = reader.GetInt32(1),
                                IsSummer = reader.GetBoolean(2),
                                HostCountryId = reader.GetInt32(3),
                                City = reader.GetString(4),
                                HostCountryName = reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return olympics;
        }

        public void AddOlympics(Olympics olympics)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Olympics (Year, IsSummer, HostCountryId, City) VALUES (@Year, @IsSummer, @HostCountryId, @City)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Year", olympics.Year);
                    command.Parameters.AddWithValue("@IsSummer", olympics.IsSummer);
                    command.Parameters.AddWithValue("@HostCountryId", olympics.HostCountryId);
                    command.Parameters.AddWithValue("@City", olympics.City);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateOlympics(Olympics olympics)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Olympics SET Year = @Year, IsSummer = @IsSummer, HostCountryId = @HostCountryId, City = @City WHERE OlympicsId = @OlympicsId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OlympicsId", olympics.OlympicsId);
                    command.Parameters.AddWithValue("@Year", olympics.Year);
                    command.Parameters.AddWithValue("@IsSummer", olympics.IsSummer);
                    command.Parameters.AddWithValue("@HostCountryId", olympics.HostCountryId);
                    command.Parameters.AddWithValue("@City", olympics.City);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOlympics(int olympicsId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Olympics WHERE OlympicsId = @OlympicsId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OlympicsId", olympicsId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Методы для работы со странами
        public List<Country> GetAllCountries()
        {
            var countries = new List<Country>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT CountryId, CountryName FROM Countries ORDER BY CountryName";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            countries.Add(new Country
                            {
                                CountryId = reader.GetInt32(0),
                                CountryName = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return countries;
        }

        public void AddCountry(Country country)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Countries (CountryName) VALUES (@CountryName)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", country.CountryName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateCountry(Country country)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Countries SET CountryName = @CountryName WHERE CountryId = @CountryId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryId", country.CountryId);
                    command.Parameters.AddWithValue("@CountryName", country.CountryName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCountry(int countryId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Countries WHERE CountryId = @CountryId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryId", countryId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Методы для работы с видами спорта
        public List<Sport> GetAllSports()
        {
            var sports = new List<Sport>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT SportId, SportName FROM Sports ORDER BY SportName";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sports.Add(new Sport
                            {
                                SportId = reader.GetInt32(0),
                                SportName = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return sports;
        }

        public void AddSport(Sport sport)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Sports (SportName) VALUES (@SportName)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SportName", sport.SportName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateSport(Sport sport)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Sports SET SportName = @SportName WHERE SportId = @SportId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SportId", sport.SportId);
                    command.Parameters.AddWithValue("@SportName", sport.SportName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSport(int sportId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Sports WHERE SportId = @SportId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SportId", sportId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Методы для работы со спортсменами
        public List<Athlete> GetAllAthletes()
        {
            var athletes = new List<Athlete>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"SELECT a.AthleteId, a.FirstName, a.LastName, a.MiddleName, a.CountryId, 
                             a.DateOfBirth, a.Photo, c.CountryName 
                             FROM Athletes a 
                             INNER JOIN Countries c ON a.CountryId = c.CountryId 
                             ORDER BY a.LastName, a.FirstName";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            athletes.Add(new Athlete
                            {
                                AthleteId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                MiddleName = reader.IsDBNull(3) ? null : reader.GetString(3),
                                CountryId = reader.GetInt32(4),
                                DateOfBirth = reader.GetDateTime(5),
                                Photo = reader.IsDBNull(6) ? null : (byte[])reader[6],
                                CountryName = reader.GetString(7)
                            });
                        }
                    }
                }
            }
            return athletes;
        }

        public void AddAthlete(Athlete athlete)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"INSERT INTO Athletes (FirstName, LastName, MiddleName, CountryId, DateOfBirth, Photo) 
                             VALUES (@FirstName, @LastName, @MiddleName, @CountryId, @DateOfBirth, @Photo)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", athlete.FirstName);
                    command.Parameters.AddWithValue("@LastName", athlete.LastName);
                    command.Parameters.AddWithValue("@MiddleName", (object?)athlete.MiddleName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CountryId", athlete.CountryId);
                    command.Parameters.AddWithValue("@DateOfBirth", athlete.DateOfBirth);
                    command.Parameters.AddWithValue("@Photo", (object?)athlete.Photo ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateAthlete(Athlete athlete)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"UPDATE Athletes SET FirstName = @FirstName, LastName = @LastName, 
                             MiddleName = @MiddleName, CountryId = @CountryId, DateOfBirth = @DateOfBirth, 
                             Photo = @Photo WHERE AthleteId = @AthleteId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AthleteId", athlete.AthleteId);
                    command.Parameters.AddWithValue("@FirstName", athlete.FirstName);
                    command.Parameters.AddWithValue("@LastName", athlete.LastName);
                    command.Parameters.AddWithValue("@MiddleName", (object?)athlete.MiddleName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CountryId", athlete.CountryId);
                    command.Parameters.AddWithValue("@DateOfBirth", athlete.DateOfBirth);
                    command.Parameters.AddWithValue("@Photo", (object?)athlete.Photo ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteAthlete(int athleteId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Athletes WHERE AthleteId = @AthleteId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AthleteId", athleteId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Методы для работы с результатами
        public List<Result> GetAllResults()
        {
            var results = new List<Result>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"SELECT r.ResultId, r.OlympicsId, r.SportId, r.AthleteId, r.MedalType,
                             a.LastName + ' ' + a.FirstName + ISNULL(' ' + a.MiddleName, '') as AthleteName,
                             s.SportName
                             FROM Results r
                             INNER JOIN Athletes a ON r.AthleteId = a.AthleteId
                             INNER JOIN Sports s ON r.SportId = s.SportId
                             ORDER BY r.OlympicsId, s.SportName";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new Result
                            {
                                ResultId = reader.GetInt32(0),
                                OlympicsId = reader.GetInt32(1),
                                SportId = reader.GetInt32(2),
                                AthleteId = reader.GetInt32(3),
                                MedalType = reader.GetInt32(4),
                                AthleteName = reader.GetString(5),
                                SportName = reader.GetString(6)
                            });
                        }
                    }
                }
            }
            return results;
        }

        public void AddResult(Result result)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Results (OlympicsId, SportId, AthleteId, MedalType) VALUES (@OlympicsId, @SportId, @AthleteId, @MedalType)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OlympicsId", result.OlympicsId);
                    command.Parameters.AddWithValue("@SportId", result.SportId);
                    command.Parameters.AddWithValue("@AthleteId", result.AthleteId);
                    command.Parameters.AddWithValue("@MedalType", result.MedalType);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateResult(Result result)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Results SET OlympicsId = @OlympicsId, SportId = @SportId, AthleteId = @AthleteId, MedalType = @MedalType WHERE ResultId = @ResultId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ResultId", result.ResultId);
                    command.Parameters.AddWithValue("@OlympicsId", result.OlympicsId);
                    command.Parameters.AddWithValue("@SportId", result.SportId);
                    command.Parameters.AddWithValue("@AthleteId", result.AthleteId);
                    command.Parameters.AddWithValue("@MedalType", result.MedalType);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteResult(int resultId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Results WHERE ResultId = @ResultId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ResultId", resultId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Запросы и отчеты

        // 1. Медальный зачет по странам (по конкретной олимпиаде или за всю историю)
        public List<MedalStanding> GetMedalStanding(int? olympicsId = null)
        {
            var standings = new List<MedalStanding>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = olympicsId.HasValue
                    ? @"SELECT c.CountryName,
                       SUM(CASE WHEN r.MedalType = 1 THEN 1 ELSE 0 END) as Gold,
                       SUM(CASE WHEN r.MedalType = 2 THEN 1 ELSE 0 END) as Silver,
                       SUM(CASE WHEN r.MedalType = 3 THEN 1 ELSE 0 END) as Bronze
                       FROM Results r
                       INNER JOIN Athletes a ON r.AthleteId = a.AthleteId
                       INNER JOIN Countries c ON a.CountryId = c.CountryId
                       WHERE r.OlympicsId = @OlympicsId
                       GROUP BY c.CountryName
                       ORDER BY Gold DESC, Silver DESC, Bronze DESC"
                    : @"SELECT c.CountryName,
                       SUM(CASE WHEN r.MedalType = 1 THEN 1 ELSE 0 END) as Gold,
                       SUM(CASE WHEN r.MedalType = 2 THEN 1 ELSE 0 END) as Silver,
                       SUM(CASE WHEN r.MedalType = 3 THEN 1 ELSE 0 END) as Bronze
                       FROM Results r
                       INNER JOIN Athletes a ON r.AthleteId = a.AthleteId
                       INNER JOIN Countries c ON a.CountryId = c.CountryId
                       GROUP BY c.CountryName
                       ORDER BY Gold DESC, Silver DESC, Bronze DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    if (olympicsId.HasValue)
                        command.Parameters.AddWithValue("@OlympicsId", olympicsId.Value);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            standings.Add(new MedalStanding
                            {
                                CountryName = reader.GetString(0),
                                Gold = reader.GetInt32(1),
                                Silver = reader.GetInt32(2),
                                Bronze = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }
            return standings;
        }

        // 2. Медалисты по видам спорта
        public List<Result> GetMedalistsBySport(int? olympicsId, int? sportId)
        {
            var results = new List<Result>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"SELECT r.ResultId, r.OlympicsId, r.SportId, r.AthleteId, r.MedalType,
                            a.LastName + ' ' + a.FirstName + ISNULL(' ' + a.MiddleName, '') as AthleteName,
                            s.SportName
                            FROM Results r
                            INNER JOIN Athletes a ON r.AthleteId = a.AthleteId
                            INNER JOIN Sports s ON r.SportId = s.SportId
                            WHERE 1=1";

                if (olympicsId.HasValue)
                    query += " AND r.OlympicsId = @OlympicsId";
                if (sportId.HasValue)
                    query += " AND r.SportId = @SportId";

                query += " ORDER BY s.SportName, r.MedalType, a.LastName";

                using (var command = new SqlCommand(query, connection))
                {
                    if (olympicsId.HasValue)
                        command.Parameters.AddWithValue("@OlympicsId", olympicsId.Value);
                    if (sportId.HasValue)
                        command.Parameters.AddWithValue("@SportId", sportId.Value);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new Result
                            {
                                ResultId = reader.GetInt32(0),
                                OlympicsId = reader.GetInt32(1),
                                SportId = reader.GetInt32(2),
                                AthleteId = reader.GetInt32(3),
                                MedalType = reader.GetInt32(4),
                                AthleteName = reader.GetString(5),
                                SportName = reader.GetString(6)
                            });
                        }
                    }
                }
            }
            return results;
        }

        // 3. Страна с наибольшим количеством золотых медалей
        public MedalStanding? GetCountryWithMostGoldMedals(int? olympicsId = null)
        {
            var standings = GetMedalStanding(olympicsId);
            return standings.OrderByDescending(s => s.Gold).ThenByDescending(s => s.Silver).ThenByDescending(s => s.Bronze).FirstOrDefault();
        }

        // 4. Спортсмен с наибольшим количеством золотых медалей в конкретном виде спорта
        public List<Athlete> GetAthleteWithMostGoldMedalsInSport(int sportId)
        {
            var athletes = new List<Athlete>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"SELECT TOP 1 a.AthleteId, a.FirstName, a.LastName, a.MiddleName, a.CountryId,
                            a.DateOfBirth, a.Photo, c.CountryName,
                            COUNT(*) as GoldCount
                            FROM Results r
                            INNER JOIN Athletes a ON r.AthleteId = a.AthleteId
                            INNER JOIN Countries c ON a.CountryId = c.CountryId
                            WHERE r.SportId = @SportId AND r.MedalType = 1
                            GROUP BY a.AthleteId, a.FirstName, a.LastName, a.MiddleName, a.CountryId,
                            a.DateOfBirth, a.Photo, c.CountryName
                            ORDER BY GoldCount DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SportId", sportId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            athletes.Add(new Athlete
                            {
                                AthleteId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                MiddleName = reader.IsDBNull(3) ? null : reader.GetString(3),
                                CountryId = reader.GetInt32(4),
                                DateOfBirth = reader.GetDateTime(5),
                                Photo = reader.IsDBNull(6) ? null : (byte[])reader[6],
                                CountryName = reader.GetString(7)
                            });
                        }
                    }
                }
            }
            return athletes;
        }

        // 5. Страна, которая чаще всех была хозяйкой олимпиады
        public string? GetMostFrequentHostCountry()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"SELECT TOP 1 c.CountryName
                            FROM Olympics o
                            INNER JOIN Countries c ON o.HostCountryId = c.CountryId
                            GROUP BY c.CountryName
                            ORDER BY COUNT(*) DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }

        // 6. Состав олимпиадной команды спортсменов конкретной страны
        public List<Athlete> GetCountryTeam(int countryId)
        {
            var athletes = new List<Athlete>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"SELECT a.AthleteId, a.FirstName, a.LastName, a.MiddleName, a.CountryId,
                            a.DateOfBirth, a.Photo, c.CountryName
                            FROM Athletes a
                            INNER JOIN Countries c ON a.CountryId = c.CountryId
                            WHERE a.CountryId = @CountryId
                            ORDER BY a.LastName, a.FirstName";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryId", countryId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            athletes.Add(new Athlete
                            {
                                AthleteId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                MiddleName = reader.IsDBNull(3) ? null : reader.GetString(3),
                                CountryId = reader.GetInt32(4),
                                DateOfBirth = reader.GetDateTime(5),
                                Photo = reader.IsDBNull(6) ? null : (byte[])reader[6],
                                CountryName = reader.GetString(7)
                            });
                        }
                    }
                }
            }
            return athletes;
        }

        // 7. Статистика выступления конкретной страны
        public MedalStanding GetCountryStatistics(int countryId, int? olympicsId = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = olympicsId.HasValue
                    ? @"SELECT c.CountryName,
                       SUM(CASE WHEN r.MedalType = 1 THEN 1 ELSE 0 END) as Gold,
                       SUM(CASE WHEN r.MedalType = 2 THEN 1 ELSE 0 END) as Silver,
                       SUM(CASE WHEN r.MedalType = 3 THEN 1 ELSE 0 END) as Bronze
                       FROM Results r
                       INNER JOIN Athletes a ON r.AthleteId = a.AthleteId
                       INNER JOIN Countries c ON a.CountryId = c.CountryId
                       WHERE c.CountryId = @CountryId AND r.OlympicsId = @OlympicsId
                       GROUP BY c.CountryName"
                    : @"SELECT c.CountryName,
                       SUM(CASE WHEN r.MedalType = 1 THEN 1 ELSE 0 END) as Gold,
                       SUM(CASE WHEN r.MedalType = 2 THEN 1 ELSE 0 END) as Silver,
                       SUM(CASE WHEN r.MedalType = 3 THEN 1 ELSE 0 END) as Bronze
                       FROM Results r
                       INNER JOIN Athletes a ON r.AthleteId = a.AthleteId
                       INNER JOIN Countries c ON a.CountryId = c.CountryId
                       WHERE c.CountryId = @CountryId
                       GROUP BY c.CountryName";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryId", countryId);
                    if (olympicsId.HasValue)
                        command.Parameters.AddWithValue("@OlympicsId", olympicsId.Value);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new MedalStanding
                            {
                                CountryName = reader.GetString(0),
                                Gold = reader.GetInt32(1),
                                Silver = reader.GetInt32(2),
                                Bronze = reader.GetInt32(3)
                            };
                        }
                    }
                }
            }
            return new MedalStanding { CountryName = "Не найдено" };
        }
    }
}


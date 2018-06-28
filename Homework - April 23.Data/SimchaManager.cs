using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework___April_23.Data
{
    public class SimchaManager
    {
        public string _connectionString;
        public SimchaManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Simcha> GetSimchos()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Simchos ";
                connection.Open();
                var simchos = new List<Simcha>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var simcha = new Simcha
                    {
                        Id = (int)reader["Id"],
                        Date = (DateTime)reader["Date"],
                        SimchaName = (string)reader["SimchaName"]
                    };
                    SetSimchaTotal(simcha);
                    simchos.Add(simcha);
                }
                return simchos;
            }
        }

        private void SetSimchaTotal(Simcha simcha)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT ISNull(SUM(ContributionAmount), 0) as Total, " +
                                  "Count(*) as ContributorCount " +
                                  "FROM Contributions " +
                                  "WHERE SimchaId = @simchaId";
                cmd.Parameters.AddWithValue("@simchaId", simcha.Id);
                connection.Open();
                var reader = cmd.ExecuteReader();
                reader.Read();
                simcha.Total = (decimal)reader["Total"];
                simcha.ContributorCount = (int)reader["ContributorCount"];
            }
        }

        public void AddSimcha(Simcha simcha)
        {
            var cManager = new ContributorManager(_connectionString);
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var cmd = sqlConnection.CreateCommand())
            {
                sqlConnection.Open();
                cmd.CommandText = "INSERT INTO Simchos " +
                                  "VALUES (@simchaName, @date) " +
                                  "SELECT SCOPE_IDENTITY()";
                cmd.Parameters.AddWithValue("@simchaName", simcha.SimchaName);
                cmd.Parameters.AddWithValue("@date", simcha.Date);
                simcha.Id = (int)(decimal)cmd.ExecuteScalar();
            }
            var contributors = cManager.GetContributors();
            foreach (Contributor contributor in contributors)
            {
                if (contributor.AlwaysInclude)
                {
                    AddContribution(new Contribution
                    {
                        SimchaId = simcha.Id,
                        ContributorId = contributor.Id,
                        Amount = 5
                    });
                }
            }
        }

        public Simcha GetSimchaById(int simchaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Simchos " +
                                  "WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", simchaId);
                connection.Open();
                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
                var simcha = new Simcha
                {
                    Id = (int)reader["Id"],
                    Date = (DateTime)reader["Date"],
                    SimchaName = (string)reader["SimchaName"]
                };
                SetSimchaTotal(simcha);
                return simcha;
            }
        }

        public void AddContribution(Contribution contribution)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var cmd = sqlConnection.CreateCommand())
            {
                sqlConnection.Open();
                cmd.CommandText = "INSERT INTO Contributions " +
                                  "VALUES (@simchaId, @contributorId, @contributionAmount)";
                cmd.Parameters.AddWithValue("@simchaId", contribution.SimchaId);
                cmd.Parameters.AddWithValue("@contributorId", contribution.ContributorId);
                cmd.Parameters.AddWithValue("@contributionAmount", contribution.Amount);
                cmd.ExecuteNonQuery();
            }
        }

        public int UpdateContributions(int simchaId, IEnumerable<ContributorContribution> contributors)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Contributions " +
                                  "WHERE SimchaId = @simchaId";
                cmd.Parameters.AddWithValue("@simchaId", simchaId);

                connection.Open();
                cmd.ExecuteNonQuery();
                foreach (ContributorContribution contributor in contributors.Where(c => c.Contribute))
                {
                    AddContribution(new Contribution
                    {
                        SimchaId = simchaId,
                        ContributorId = contributor.Contributor.Id,
                        Amount = (decimal)contributor.ContributionAmount
                    });
                }
                return simchaId;
            }
        }
    }
}

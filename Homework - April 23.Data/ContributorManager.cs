using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework___April_23.Data
{
    public class ContributorManager
    {
        public string _connectionString;
        public ContributorManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int GetContributorCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) " +
                                  "FROM Contributors";
                connection.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public IEnumerable<Contributor> GetContributors()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Contributors ";
                connection.Open();
                var contributors = new List<Contributor>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var contributor = new Contributor
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        CellNumber = (string)reader["CellNumber"],
                        AlwaysInclude = (bool)reader["AlwaysInclude"],
                        Date = (DateTime)reader["Date"]
                    };
                    SetContributorBalance(contributor);
                    contributors.Add(contributor);
                }
                return contributors;
            }
        }

        public void SetContributorBalance(Contributor contributor)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                connection.Open();
                cmd.CommandText = "SELECT IsNull(SUM(DepositAmount),0) " +
                                  "FROM Deposits " +
                                  "WHERE ContributorId = @contributorId";
                cmd.Parameters.AddWithValue("@contributorId", contributor.Id);
                decimal depositTotal = (decimal)cmd.ExecuteScalar();
                cmd.Parameters.Clear();

                cmd.CommandText = "SELECT IsNull(SUM(ContributionAmount),0) " +
                                  "FROM Contributions " +
                                  "WHERE ContributorId = @contributorId";
                cmd.Parameters.AddWithValue("@contributorId", contributor.Id);
                decimal contributionsTotal = (decimal)cmd.ExecuteScalar();
                var x = depositTotal - contributionsTotal;
                contributor.Balance = depositTotal - contributionsTotal;
            }
        }

        public void AddContributor(Contributor contributor)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var cmd = sqlConnection.CreateCommand())
            {
                sqlConnection.Open();
                cmd.CommandText = "INSERT INTO Contributors " +
                                  "VALUES (@firstName, @lastName, @cellNumber, @alwaysInclude, @date)";
                cmd.Parameters.AddWithValue("@firstName", contributor.FirstName);
                cmd.Parameters.AddWithValue("@lastName", contributor.LastName);
                cmd.Parameters.AddWithValue("@cellNumber", contributor.CellNumber);
                cmd.Parameters.AddWithValue("@date", contributor.Date);
                cmd.Parameters.AddWithValue("@alwaysInclude", contributor.AlwaysInclude);
                cmd.ExecuteNonQuery();
            }
        }

        public void AddDeposit(Deposit deposit)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Deposits " +
                                  "VALUES (@depositAmount, @date, @contributorId)";
                cmd.Parameters.AddWithValue("@date", deposit.Date);
                cmd.Parameters.AddWithValue("@depositAmount", deposit.DepositAmount);
                cmd.Parameters.AddWithValue("@contributorId", deposit.ContributorId);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<Contributor> GetContributorsBySimchaId(int simchaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT c.FirstName, c.LastName " +
                                  "FROM Contributors c " +
                                  "JOIN Contributions cb ON cb.ContributorId = c.Id " +
                                  "WHERE cb.SimchaId = @simchaId " +
                                  "GROUP BY c.FirstName, c.LastName";
                cmd.Parameters.AddWithValue("@simchaId", simchaId);
                connection.Open();
                var contributors = new List<Contributor>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    contributors.Add(new Contributor
                    {
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"]
                    });
                }
                if (contributors == null)
                {
                    return null;
                }
                return contributors;
            }
        }

        public IEnumerable<Contribution> GetContributionsByContributorID(int contributorId)
        {
            var sManager = new SimchaManager(_connectionString);
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * from Contributions " +
                                  "WHERE ContributorId = @contributorId";
                cmd.Parameters.AddWithValue("@contributorId", contributorId);
                connection.Open();
                var contributions = new List<Contribution>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var contribution = new Contribution
                    {
                        SimchaId = (int)reader["SimchaId"],
                        ContributorId = (int)reader["ContributorId"],
                        Amount = (decimal)reader["ContributionAmount"]
                    };
                    var simcha = sManager.GetSimchaById(contribution.SimchaId);
                    contribution.SimchaName = simcha.SimchaName;
                    contribution.SimchaDate = simcha.Date;
                    contributions.Add(contribution);
                }
                return contributions;
            }
        }

        public IEnumerable<Deposit> GetDepositByContributorId(int contributorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * from Deposits " +
                                  "WHERE ContributorId = @contributorId";
                cmd.Parameters.AddWithValue("@contributorId", contributorId);
                connection.Open();
                var deposits = new List<Deposit>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    deposits.Add(new Deposit
                    {
                        Id = (int)reader["Id"],
                        DepositAmount = (decimal)reader["DepositAmount"],
                        Date = (DateTime)reader["Date"],
                        ContributorId = (int)reader["ContributorId"]
                    });
                }
                return deposits;
            }
        }

        public Contributor GetContributorById(int contributorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Contributors " +
                                  "WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", contributorId);
                connection.Open();
                var contributor = new Contributor();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    contributor.Id = (int)reader["Id"];
                    contributor.FirstName = (string)reader["FirstName"];
                    contributor.LastName = (string)reader["LastName"];
                    contributor.CellNumber = (string)reader["CellNumber"];
                    contributor.AlwaysInclude = (bool)reader["AlwaysInclude"];
                    contributor.Date = (DateTime)reader["Date"];
                }
                SetContributorBalance(contributor);
                return contributor;
            }
        }

        public IEnumerable<ContributorContribution> GetContributorContributions(int simchaId)
        {
            var contributors = GetContributors();
            var cc = new List<ContributorContribution>();
            foreach(Contributor contributor in contributors)
            {
                cc.Add(new ContributorContribution
                {
                    Contributor = contributor,
                    ContributionAmount = GetContributionAmount(simchaId, contributor.Id)
                });
            }
            return cc;
        }

        private decimal? GetContributionAmount(int simchaId, int contributorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT ContributionAmount " +
                                  "FROM Contributions " +
                                  "WHERE SimchaId = @simchaId and ContributorId = @contributorId";
                cmd.Parameters.AddWithValue("@simchaId", simchaId);
                cmd.Parameters.AddWithValue("@contributorId", contributorId);
                connection.Open();
                return (decimal?)cmd.ExecuteScalar();
            }
        }

        public void UpdateContributor(Contributor contributor)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var cmd = sqlConnection.CreateCommand())
            {
                sqlConnection.Open();
                cmd.CommandText = "UPDATE Contributors " +
                                  "SET FirstName = @firstName, LastName = @lastName, CellNumber = @cellNumber, AlwaysInclude = @alwaysInclude, Date = @date " +
                                  "WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", contributor.Id);
                cmd.Parameters.AddWithValue("@firstName", contributor.FirstName);
                cmd.Parameters.AddWithValue("@lastName", contributor.LastName);
                cmd.Parameters.AddWithValue("@cellNumber", contributor.CellNumber);
                cmd.Parameters.AddWithValue("@date", contributor.Date);
                cmd.Parameters.AddWithValue("@alwaysInclude", contributor.AlwaysInclude);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

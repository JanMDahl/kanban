using Dapper;
using KanbanAPI.Domain;
using MySql.Data.MySqlClient;

namespace KanbanAPI.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly string _connectionString;

        public TicketRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString"];
        }

        public async Task Upsert(Ticket ticket)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.ExecuteScalarAsync(@"
                REPLACE INTO kanban.Ticket
                SET
                Id = @Id,
                Title = @Title,
                Description = @Description,
                Status = @Status,
                Priority = @Priority,
                CreatedAt = @CreatedAt,
                UpdatedAt = @UpdatedAt,
                ClosedAt = @ClosedAt;
            ",
            new
            {
                ticket.Id,
                ticket.Title,
                ticket.Description,
                ticket.Status,
                ticket.Priority,
                ticket.CreatedAt,
                ticket.UpdatedAt,
                ticket.ClosedAt
            }
            );
        }

        public async Task<Ticket> GetById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Ticket>(@"
                SELECT *
                FROM kanban.Ticket
                WHERE Id = @id;
            ",
            new
            {
                id
            }
            );
        }

        public async Task<List<Ticket>> GetAll()
        {
            using var connection = new MySqlConnection(_connectionString);
            return (await connection.QueryAsync<Ticket>(@"
                SELECT *
                FROM kanban.Ticket;

            ")).ToList();
        }

        public async Task Delete(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.ExecuteScalarAsync(@"
                DELETE FROM kanban.Ticket
                WHERE Id = @Id
            ",
            new
            {
                Id = id
            });
        }
    }

    public interface ITicketRepository
    {
        Task Upsert(Ticket ticket);
        Task<Ticket> GetById(int id);
        Task<List<Ticket>> GetAll();
        Task Delete(int id);
    }
}

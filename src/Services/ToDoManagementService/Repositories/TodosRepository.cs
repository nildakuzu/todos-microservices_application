using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoManagementService.Api.Entities;
using ToDoManagementService.Api.Repositories.Interfaces;

namespace ToDoManagementService.Api.Repositories
{
    public class TodosRepository : ITodosRepository
    {
        private readonly IConfiguration _configuration;

        public TodosRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Create(Todo todo)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO todotbl (username, groupname,duedate,priority,description,done) VALUES (@username, @groupname,@duedate,@priority,@description,@done)",
                            new { username = todo.UserName, groupname = todo.GroupName, duedate = todo.DueDate, priority = todo.Priority, description = todo.Description, done = todo.Done });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM todotbl WHERE id = @id",
                new { id = id });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteUserTodos(string userName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM todotbl WHERE username = @username",
                new { username = userName });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<IEnumerable<Todo>> FilterTodos(Todo todo)
        {

            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var groupList = await connection.QueryAsync<Todo>
                ("SELECT * FROM todotbl WHERE (@username is null or username = @username) " +
                "and (@priority is null or priority = @priority) " +
                "and (@done is null or done = @done) " +
                "and (@description is null or description like @description) " +
                "and (@groupname is null or groupname = @groupname) " +
                "and (@duedate is null or duedate = @duedate) ",
                new
                {
                    username = todo.UserName,
                    priority = todo.Priority,
                    done = todo.Done,
                    description = todo.Description,
                    groupname = todo.GroupName,
                    duedate = todo.DueDate,
                });

            return groupList;
        }

        public async Task<Todo> Get(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var todo = await connection.QueryFirstOrDefaultAsync<Todo>
                ("SELECT * FROM todotbl WHERE id = @id", new { id = id });

            if (todo == null)
                return new Todo();

            return todo;
        }

        public async Task<IEnumerable<Todo>> GetUserTodos(string userName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var groupList = await connection.QueryAsync<Todo>
                ("SELECT * FROM todotbl WHERE username = @username", new { username = userName });

            return groupList;
        }

        public async Task<bool> Update(Todo todo)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                    ("UPDATE todotbl SET username=@username, groupname = @groupname,duedate = @duedate, priority = @priority,done = @done, description = @description WHERE Id = @Id",
                            new { username = todo.UserName, groupname = todo.GroupName, duedate = todo.DueDate, priority = todo.Priority, done = todo.Done, description = todo.Description, Id = todo.Id });

            if (affected == 0)
                return false;

            return true;
        }
    }
}

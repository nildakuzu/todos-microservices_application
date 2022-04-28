using Dapper;
using GroupManagementService.Api.Entities;
using GroupManagementService.Api.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupManagementService.Api.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IConfiguration _configuration;

        public GroupRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<Group>> GetUserGroups(string username)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var groupList = await connection.QueryAsync<Group>
                ("SELECT * FROM grouptbl WHERE username = @username", new { username = username });

            return groupList;
        }

        public async Task<bool> Create(Group group)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO grouptbl (username, groupname) VALUES (@username, @groupname)",
                            new { username = group.UserName, groupname = group.GroupName });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> Update(Group group)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                    ("UPDATE grouptbl SET username=@username, groupname = @groupname WHERE Id = @Id",
                            new { username = group.UserName, groupname = group.GroupName, Id = group.Id });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteUserGroup(string username)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM grouptbl WHERE username = @username",
                new { username = username });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> Delete(string groupname)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM grouptbl WHERE groupname = @groupname",
                new { groupname = groupname });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<Group> Get(string groupname)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var group = await connection.QueryFirstOrDefaultAsync<Group>
                ("SELECT * FROM grouptbl WHERE groupname = @groupname", new { groupname = groupname });

            if (group == null)
                return new Group { UserName = "No User Name", GroupName = "No Group Name" };

            return group;
        }
    }
}

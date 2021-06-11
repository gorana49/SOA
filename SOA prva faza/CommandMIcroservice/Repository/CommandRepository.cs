
using CommandMIcroservice.IRepository;
using CommandMIcroservice.Models;
using CommandMIcroservice.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CommandMIcroservice.Repository
{
    public class CommandRepository : ICommandRepository
    {
        private readonly ISensorContext _context;
        private readonly DataService _service;

        public CommandRepository(ISensorContext context, DataService serv)
        {
            _context = context;
            _service = serv;
        }
        public async Task PostData(SensorDataCommand sensorDataCommand)
        {
            await _context.SensorDataCommand.InsertOneAsync(sensorDataCommand);
        }
        public async Task<IEnumerable<SensorDataCommand>> GetData(string sensorType)
        {
            FilterDefinition<SensorDataCommand> filter = Builders<SensorDataCommand>.Filter.Eq(p => p.Sensor.SensorType, sensorType);
            return await _context
                                        .SensorDataCommand
                                        .Find(filter)
                                        .ToListAsync();
        }
    }
}

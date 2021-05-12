﻿using DataMicroservice.Model;
using DataMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DataController : ControllerBase
    {
        private readonly IDataService _db;

        public DataController(IDataService db)
        {
            this._db = db;
        }

        [HttpPost]
        public void Post([FromBody, Required] Dictionary<string, string> data)
        {
            foreach (KeyValuePair<string, string> kvp in data)
            {
                SensorData sensor = new SensorData(Double.Parse(kvp.Key), kvp.Value);
                this._db.SaveData(sensor);
            }
        }

        //[HttpGet("{sensorType}")]
        //[Route("getsensordata")]
        //async public Task<IActionResult> GetSensorData([FromQuery] string sensorType)
        //{
        //    Console.WriteLine($"sensorType: {sensorType}");
        //    string query = $"from(bucket: \"soa\") " +
        //        $"|> range(start: -50m) " +
        //        $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
        //        $"|> filter(fn: (r) => r._field == \"value\") " +
        //        $"|> filter(fn: (r) => r.sensor == \"{sensorType}\")";
        //    List<FluxTable> query_data = await _db.Query(query);
        //    //foreach (var data_point in query_data)
        //    //{
        //    //    Console.WriteLine(data_point);
        //    //}
        //    return Ok(query_data);
        //}

        //[HttpGet("{minutes}")]
        //[Route("getlastnminutesdata")]
        //async public Task<IActionResult> GetLastNMinutesData([FromQuery] int minutes)
        //{
        //    string query = $"from(bucket: \"soa\") " +
        //        $"|> range(start: -{minutes}m) " +
        //        $"|> filter(fn: (r) => r._measurement == \"SensorsData\") " +
        //        $"|> filter(fn: (r) => r._field == \"value\") ";
        //    List<FluxTable> query_data = await _db.Query(query);
        //    return Ok(query_data);
        //}

        // POST api/<DataController>


    }
}
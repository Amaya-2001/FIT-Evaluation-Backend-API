using Dapper;
using Evaluation_Backend_Amaya.Helper;
using Evaluation_Backend_Amaya.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace Evaluation_Backend_Amaya.Controllers
{
    [ApiController]
    [Route("/location")]
    public class LocationController : Controller
    {
        [HttpPost]
        public IActionResult saveLocation([FromBody] Location model)
        {
            try {
                using (IDbConnection db = new SqlConnection(DatabaseHelper.sDbConnectionString))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        db.Execute("[dbo].[spLocation_save]", new
                        {
                            sCode = model.code ?? "",
                            sName = model.name,
                        }, commandType: CommandType.StoredProcedure, transaction: transaction);
                        transaction.Commit();

                        return StatusCode(StatusCodes.Status200OK);
                    }
                }
            } catch (Exception ex) {
                { return StatusCode(StatusCodes.Status500InternalServerError); }
            } 
        }

        [HttpGet]
        public IActionResult getAllLocations()
        {
            using (IDbConnection db = new SqlConnection(DatabaseHelper.sDbConnectionString))
            {
                try
                {
                    var allLocations = db.Query<Location>("SELECT Code, Name FROM tblLocation".ToString());

                    return Json(allLocations);
                }
                catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError); }
            }
        }

        [HttpGet("autocomplete")]
        public IActionResult autoCompleteLocations([FromQuery] string? key)
        {
            using (IDbConnection db = new SqlConnection(DatabaseHelper.sDbConnectionString))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(key))
                    {
                        var autoCompleteLocations = db.Query<AutoCompleteLocations>
                            ("SELECT Code,'(' + CONVERT(VARCHAR,Code) + ') ' + Name label " +
                            "FROM tblLocation ORDER BY Code").ToList();

                        return Ok(autoCompleteLocations);
                    }
                    else
                    {
                        var autoCompleteLocations = db.Query<AutoCompleteLocations>
                           ("SELECT Code,'(' + CONVERT(VARCHAR,Code) + ') ' + Name label " +
                           "FROM tblLocation ORDER BY Code", new {key}).ToList();

                        return Ok(autoCompleteLocations);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new {message =  ex.Message});
                }
            }
        }

    }
}

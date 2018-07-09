using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace ExemploOneToOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Estado> Get(
            [FromServices]IConfiguration config)
        {
            using (SqlConnection conexao = new SqlConnection(
                config.GetConnectionString("ExemplosDapper")))
            {
                return conexao.Query<Estado, Regiao, Estado>(
                    "SELECT * " +
                    "FROM dbo.Estados E " +
                    "INNER JOIN dbo.Regioes R ON R.IdRegiao = E.IdRegiao " +
                    "ORDER BY E.NomeEstado",
                    map: (estado, regiao) =>
                    {
                        estado.DadosRegiao = regiao;
                        return estado;
                    },
                    splitOn: "SiglaEstado,IdRegiao");
            }
        }
    }
}
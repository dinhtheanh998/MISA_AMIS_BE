using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.NhanVien.API.Entities;
using MISA.AMIS.NhanVien.BL.BaseBL;
using MySqlConnector;

namespace MISA.AMIS.NhanVien.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController<Department>
    {
        public DepartmentsController(IBaseBL<Department> department) : base(department)
        {
        }
    }
}
